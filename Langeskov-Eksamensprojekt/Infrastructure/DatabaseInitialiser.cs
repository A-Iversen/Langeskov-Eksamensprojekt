using Microsoft.Data.SqlClient;
using System.IO;
using System.Text;
using System.Reflection;
using System;

namespace Infrastructure
{
    public static class DatabaseInitializer
    {
        private const string DatabaseSetupFileName = "DatabaseSetup.sql";


        // Connection string til databasen fra appsettings.json
        public static void Initialize(string connectionString)
        {
            if (IsDatabaseEmpty(connectionString))
            {
                Console.WriteLine("Databasen ser tom ud. Opretter tabeller og indsætter Seed Data...");
                ExecuteSetupScript(connectionString);
                Console.WriteLine("Databaseopsætning fuldført.");
            }
            else
            {
                Console.WriteLine("Databasen er allerede opsat. Springer opsætning over.");
            }
        }

        private static bool IsDatabaseEmpty(string connectionString)
        {
            try
            {
                // Tjekker om hovedtabellen 'Member' findes
                string query = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Member'";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    int tableCount = (int)command.ExecuteScalar();

                    // Hvis Member-tabellen ikke findes, er databasen tom/ny
                    return tableCount == 0;
                }
            }
            catch (SqlException ex)
            {
                // Hvis den giver en fejl antager vi at den er tom eller forkert.
                Console.WriteLine($"ADVARSEL: Kunne ikke tjekke databasestatus. Fejl: {ex.Message}");
                // Vi returnerer true for at forsøge at køre opsætningen
                return true;
            }
        }

        private static void ExecuteSetupScript(string connectionString)
        {
            string sqlScript = File.ReadAllText(DatabaseSetupFileName, Encoding.UTF8);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlScript, connection);
                command.ExecuteNonQuery();
            }
        }
    }
}