using Microsoft.Data.SqlClient;
using System.IO;
using System.Text;
using System.Reflection;
using System;
using System.Diagnostics; // Added for Debug.WriteLine

namespace Infrastructure
{
    public static class DatabaseInitializer
    {
        private const string DatabaseSetupFileName = "DatabaseSetup.sql";


        // Connection string til databasen fra appsettings.json
        public static void Initialize(string connectionString)
        {
            try
            {
                Debug.WriteLine("Starter database initialisering...");
                var builder = new SqlConnectionStringBuilder(connectionString);
                var databaseName = builder.InitialCatalog;
                builder.InitialCatalog = "master";
                var masterConnectionString = builder.ConnectionString;

                Debug.WriteLine($"Checker om databasen '{databaseName}' eksisterer...");
                using (var connection = new SqlConnection(masterConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = $"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{databaseName}') CREATE DATABASE [{databaseName}]";
                        command.ExecuteNonQuery();
                    }
                    Debug.WriteLine($"Databasen '{databaseName}' er oprettet eller eksisterer allerede.");
                }

                if (IsDatabaseEmpty(connectionString))
                {
                    Debug.WriteLine("Databasen ser tom ud. Opretter tabeller og indsætter Seed Data...");
                    ExecuteSetupScript(connectionString);
                    Debug.WriteLine("Databaseopsætning fuldført.");
                }
                else
                {
                    Debug.WriteLine("Databasen er allerede opsat. Springer opsætning over.");
                }
                Debug.WriteLine("Database initialisering fuldført.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"FEJL under database initialisering: {ex}");
            }
        }

        private static bool IsDatabaseEmpty(string connectionString)
        {
            try
            {
                // Tjekker om hovedtabellen 'Runner' findes
                string query = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Runner'";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    int tableCount = (int)command.ExecuteScalar();

                    // Hvis Runner-tabellen ikke findes, er databasen tom/ny
                    return tableCount == 0;
                }
            }
            catch (SqlException ex)
            {
                // Hvis den giver en fejl antager vi at den er tom eller forkert.
                Debug.WriteLine($"ADVARSEL: Kunne ikke tjekke databasestatus. Fejl: {ex.Message}");
                // Vi returnerer true for at forsøge at køre opsætningen
                return true;
            }
        }

        private static void ExecuteSetupScript(string connectionString)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string sqlScript;

            // Get the directory of the executing assembly
            string? assemblyDir = Path.GetDirectoryName(assembly.Location);

            if (assemblyDir == null)
            {
                throw new InvalidOperationException("Kunne ikke finde stien til den eksekverende assembly.");
            }

            // Construct the full path to the SQL file
            string sqlFilePath = Path.Combine(assemblyDir, DatabaseSetupFileName);
            
            try
            {
                // Read the SQL script from the file
                sqlScript = File.ReadAllText(sqlFilePath, Encoding.UTF8);

            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine($"FEJL: Opsætningsfilen '{DatabaseSetupFileName}' blev ikke fundet i stien: '{sqlFilePath}'. Sørg for at filen er sat til 'Copy to Output Directory' i dine projektindstillinger.");
                throw;
            }
            
            // Split the script into batches using "GO" as a separator
            var scriptBatches = sqlScript.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                foreach (var batch in scriptBatches)
                {
                    if (string.IsNullOrWhiteSpace(batch)) continue;
                    
                    using (SqlCommand command = new SqlCommand(batch, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}