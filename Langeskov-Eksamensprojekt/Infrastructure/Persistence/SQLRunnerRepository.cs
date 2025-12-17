using Microsoft.Data.SqlClient;
using Infrastructure.Model;
using Infrastructure.Abstraction; // Added this line

namespace Infrastructure.Persistence
{
   public class SQLRunnerRepository : IRunnerRepository
   {
       private readonly string _connectionString;

       public SQLRunnerRepository(string connectionString)
       {
           _connectionString = connectionString;
       }

       private Runner MapRunner(SqlDataReader reader)
       {
           var runner = new Runner
           {
               RunnerID = reader.GetInt32(reader.GetOrdinal("RunnerID")),
               Name = reader.GetString(reader.GetOrdinal("Name")),
               Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
               Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
               PostalCode = reader.IsDBNull(reader.GetOrdinal("PostalCode")) ? null : reader.GetString(reader.GetOrdinal("PostalCode")),
               PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? null : reader.GetString(reader.GetOrdinal("PhoneNumber")),
               Gender = (Gender)Enum.Parse(typeof(Gender), reader.GetString(reader.GetOrdinal("Gender"))),
               DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth"))
           };
           
           // Set private properties using methods
           runner.SetSubsidyGroup(reader.GetInt32(reader.GetOrdinal("SubsidyGroupID")));
           runner.SetRunnerGroupID(reader.GetInt32(reader.GetOrdinal("RunnerGroupID")));
           
           return runner;
       }

       // SQL: SELECT * FROM RUNNER.
       public IEnumerable<Runner> GetAll()
       {
           var runners = new List<Runner>();
           string query = "SELECT RunnerID, Name, Email, Address, PostalCode, PhoneNumber, Gender, DateOfBirth, SubsidyGroupID, RunnerGroupID FROM Runner";

           using (SqlConnection connection = new SqlConnection(_connectionString))
           {
               SqlCommand command = new SqlCommand(query, connection);
               connection.Open();

               using (SqlDataReader reader = command.ExecuteReader())
               {
                   while (reader.Read())
                   {
                       runners.Add(MapRunner(reader));
                   }
               }
           }
           return runners;
       }

       // Tidligere - SQL: SELECT * FROM RUNNER WHERE RunnerID = @ID.
       
       // Nu som Stored Procedure - SQL: EXEC sp_GetRunnerById @RunnerID

        public Runner? GetById(int id)
       {
           Runner? runner = null;

           using (SqlConnection connection = new SqlConnection(_connectionString))
           {
               SqlCommand command = new SqlCommand("sp_GetRunnerById", connection);
               // Fortæller ADO.NET at det er en stored procedure
               command.CommandType = System.Data.CommandType.StoredProcedure;
               
               command.Parameters.AddWithValue("@RunnerID", id);
               connection.Open();

               using (SqlDataReader reader = command.ExecuteReader())
               {
                   if (reader.Read())
                   {
                       runner = MapRunner(reader);
                   }
               }
           }
           return runner;
       }
       
       // SQL: INSERT INTO RUNNER ...; SELECT SCOPE_IDENTITY()
       public Runner Add(Runner entity)
       {
           // alle felter undtagen RunnerID, da den auto-genereres.
           string query = @"
           INSERT INTO Runner (Name, Email, Address, PostalCode, PhoneNumber, Gender, DateOfBirth, SubsidyGroupID, RunnerGroupID) 
           VALUES (@Name, @Email, @Address, @PostalCode, @PhoneNumber, @Gender, @DateOfBirth, @SubsidyGroupID, @RunnerGroupID);
           SELECT SCOPE_IDENTITY();";

           using (SqlConnection connection = new SqlConnection(_connectionString))
           {
               SqlCommand command = new SqlCommand(query, connection);
               command.Parameters.AddWithValue("@Name", entity.Name);
               command.Parameters.AddWithValue("@Email", (object?)entity.Email ?? DBNull.Value); // Håndterer null
               command.Parameters.AddWithValue("@Address", (object?)entity.Address ?? DBNull.Value);
               command.Parameters.AddWithValue("@PostalCode", (object?)entity.PostalCode ?? DBNull.Value);
               command.Parameters.AddWithValue("@PhoneNumber", (object?)entity.PhoneNumber ?? DBNull.Value);
               command.Parameters.AddWithValue("@Gender", entity.Gender.ToString());
               command.Parameters.AddWithValue("@DateOfBirth", entity.DateOfBirth);
               command.Parameters.AddWithValue("@SubsidyGroupID", entity.SubsidyGroupID);
               command.Parameters.AddWithValue("@RunnerGroupID", entity.RunnerGroupID);

               connection.Open();

               // Kører command og henter det nye ID
               object result = command.ExecuteScalar();
               if (result != null)
               {
                   entity.RunnerID = Convert.ToInt32(result);
               }
           }
           // Returnerer entiteten med det tildelte ID.
           return entity;
       }

       // SQL: UPDATE RUNNER SET ... WHERE RunnerID = @ID.

       public void Update(Runner entity)
       {
           string query = @"
           UPDATE Runner SET 
               Name = @Name, 
               Email = @Email, 
               Address = @Address, 
               PostalCode = @PostalCode, 
               PhoneNumber = @PhoneNumber, 
               Gender = @Gender, 
               DateOfBirth = @DateOfBirth, 
               SubsidyGroupID = @SubsidyGroupID,
               RunnerGroupID = @RunnerGroupID
           WHERE RunnerID = @RunnerID";

           using (SqlConnection connection = new SqlConnection(_connectionString))
           {
               SqlCommand command = new SqlCommand(query, connection);
               command.Parameters.AddWithValue("@RunnerID", entity.RunnerID);
               command.Parameters.AddWithValue("@Name", entity.Name);
               command.Parameters.AddWithValue("@Email", (object?)entity.Email ?? DBNull.Value);
               command.Parameters.AddWithValue("@Address", (object?)entity.Address ?? DBNull.Value);
               command.Parameters.AddWithValue("@PostalCode", (object?)entity.PostalCode ?? DBNull.Value);
               command.Parameters.AddWithValue("@PhoneNumber", (object?)entity.PhoneNumber ?? DBNull.Value);
               command.Parameters.AddWithValue("@Gender", (object?)entity.Gender ?? DBNull.Value);
               command.Parameters.AddWithValue("@DateOfBirth", entity.DateOfBirth);
               command.Parameters.AddWithValue("@SubsidyGroupID", entity.SubsidyGroupID);
               command.Parameters.AddWithValue("@RunnerGroupID", entity.RunnerGroupID);

               connection.Open();
               command.ExecuteNonQuery();
           }
       }

       public void Delete(int id)
       {
           string query = "DELETE FROM Runner WHERE RunnerID = @RunnerID";

           using (SqlConnection connection = new SqlConnection(_connectionString))
           {
               SqlCommand command = new SqlCommand(query, connection);
               command.Parameters.AddWithValue("@RunnerID", id);
               connection.Open();
               command.ExecuteNonQuery();
           }
       }

       // RunnerExists() fra interface
       public bool RunnerExists(string name, DateTime dateOfBirth)
       {
           string query = "SELECT COUNT(1) FROM Runner WHERE Name = @Name AND DateOfBirth = @DateOfBirth";
           int count = 0;

           using (SqlConnection connection = new SqlConnection(_connectionString))
           {
               SqlCommand command = new SqlCommand(query, connection);
               command.Parameters.AddWithValue("@Name", name);
               command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth.Date); // Sammenlign kun dato

               connection.Open();

               // Udfører kommandoen og henter antallet af rækker
               object result = command.ExecuteScalar();
               if (result != null)
               {
                   count = Convert.ToInt32(result);
               }
           }

           return count > 0;
       }
   }
}
