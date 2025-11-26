using Microsoft.Data.SqlClient;
using Infrastructure.Model;

namespace Infrastructure.Repository
{
   public class SQLMemberRepository : IMemberRepository
   {
       private readonly string _connectionString;

       public SQLMemberRepository(string connectionString)
       {
           _connectionString = connectionString;
       }

       private Member MapMember(SqlDataReader reader)
       {
           var member = new Member
           {
               MemberID = (int)reader["MemberID"],
               Name = (string)reader["Name"],
               Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : (string)reader["Email"],
               Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : (string)reader["Address"],
               PostalCode = reader.IsDBNull(reader.GetOrdinal("PostalCode")) ? null : (string)reader["PostalCode"],
               PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? null : (string)reader["PhoneNumber"],
               Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : (string)reader["Gender"],
               DateOfBirth = (DateTime)reader["DateOfBirth"]
           };
           
           // Set private properties using methods
           member.SetSubsidyGroup((int)reader["SubsidyGroupID"]);
           member.SetMemberGroupID((int)reader["MemberGroupID"]);
           
           return member;
       }

       // SQL: SELECT * FROM MEMBER.
       public IEnumerable<Member> GetAll()
       {
           var members = new List<Member>();
           string query = "SELECT MemberID, Name, Email, Address, PostalCode, PhoneNumber, Gender, DateOfBirth, SubsidyGroupID, MemberGroupID FROM Member";

           using (SqlConnection connection = new SqlConnection(_connectionString))
           {
               SqlCommand command = new SqlCommand(query, connection);
               connection.Open();

               using (SqlDataReader reader = command.ExecuteReader())
               {
                   while (reader.Read())
                   {
                       members.Add(MapMember(reader));
                   }
               }
           }
           return members;
       }

       // SQL: SELECT * FROM MEMBER WHERE MemberID = @ID.
       public Member? GetById(int id)
       {
           Member? member = null;
           string query = "SELECT MemberID, Name, Email, Address, PostalCode, PhoneNumber, Gender, DateOfBirth, SubsidyGroupID, MemberGroupID FROM Member WHERE MemberID = @MemberID";

           using (SqlConnection connection = new SqlConnection(_connectionString))
           {
               SqlCommand command = new SqlCommand(query, connection);
               command.Parameters.AddWithValue("@MemberID", id);
               connection.Open();

               using (SqlDataReader reader = command.ExecuteReader())
               {
                   if (reader.Read())
                   {
                       member = MapMember(reader);
                   }
               }
           }
           return member;
       }
       
       // SQL: INSERT INTO MEMBER ...; SELECT SCOPE_IDENTITY()
       public Member Add(Member entity)
       {
           // alle felter undtagen MemberID, da den auto-genereres.
           string query = @"
           INSERT INTO Member (Name, Email, Address, PostalCode, PhoneNumber, Gender, DateOfBirth, SubsidyGroupID, MemberGroupID) 
           VALUES (@Name, @Email, @Address, @PostalCode, @PhoneNumber, @Gender, @DateOfBirth, @SubsidyGroupID, @MemberGroupID);
           SELECT SCOPE_IDENTITY();";

           using (SqlConnection connection = new SqlConnection(_connectionString))
           {
               SqlCommand command = new SqlCommand(query, connection);
               command.Parameters.AddWithValue("@Name", entity.Name);
               command.Parameters.AddWithValue("@Email", (object)entity.Email ?? DBNull.Value); // Håndterer null
               command.Parameters.AddWithValue("@Address", (object)entity.Address ?? DBNull.Value);
               command.Parameters.AddWithValue("@PostalCode", (object)entity.PostalCode ?? DBNull.Value);
               command.Parameters.AddWithValue("@PhoneNumber", (object)entity.PhoneNumber ?? DBNull.Value);
               command.Parameters.AddWithValue("@Gender", (object)entity.Gender ?? DBNull.Value);
               command.Parameters.AddWithValue("@DateOfBirth", entity.DateOfBirth);
               command.Parameters.AddWithValue("@SubsidyGroupID", entity.SubsidyGroupID);
               command.Parameters.AddWithValue("@MemberGroupID", entity.MemberGroupID);

               connection.Open();

               // Kører command og henter det nye ID
               object result = command.ExecuteScalar();
               if (result != null)
               {
                   entity.MemberID = Convert.ToInt32(result);
               }
           }
           // Returnerer entiteten med det tildelte ID.
           return entity;
       }

       // SQL: UPDATE MEMBER SET ... WHERE MemberID = @ID.

       public void Update(Member entity)
       {
           string query = @"
           UPDATE Member SET 
               Name = @Name, 
               Email = @Email, 
               Address = @Address, 
               PostalCode = @PostalCode, 
               PhoneNumber = @PhoneNumber, 
               Gender = @Gender, 
               DateOfBirth = @DateOfBirth, 
               SubsidyGroupID = @SubsidyGroupID,
               MemberGroupID = @MemberGroupID
           WHERE MemberID = @MemberID";

           using (SqlConnection connection = new SqlConnection(_connectionString))
           {
               SqlCommand command = new SqlCommand(query, connection);
               command.Parameters.AddWithValue("@MemberID", entity.MemberID);
               command.Parameters.AddWithValue("@Name", entity.Name);
               command.Parameters.AddWithValue("@Email", (object)entity.Email ?? DBNull.Value);
               command.Parameters.AddWithValue("@Address", (object)entity.Address ?? DBNull.Value);
               command.Parameters.AddWithValue("@PostalCode", (object)entity.PostalCode ?? DBNull.Value);
               command.Parameters.AddWithValue("@PhoneNumber", (object)entity.PhoneNumber ?? DBNull.Value);
               command.Parameters.AddWithValue("@Gender", (object)entity.Gender ?? DBNull.Value);
               command.Parameters.AddWithValue("@DateOfBirth", entity.DateOfBirth);
               command.Parameters.AddWithValue("@SubsidyGroupID", entity.SubsidyGroupID);
               command.Parameters.AddWithValue("@MemberGroupID", entity.MemberGroupID);

               connection.Open();
               command.ExecuteNonQuery();
           }
       }

       public void Delete(int id)
       {
           string query = "DELETE FROM Member WHERE MemberID = @MemberID";

           using (SqlConnection connection = new SqlConnection(_connectionString))
           {
               SqlCommand command = new SqlCommand(query, connection);
               command.Parameters.AddWithValue("@MemberID", id);
               connection.Open();
               command.ExecuteNonQuery();
           }
       }

       // MemberExists() fra interface
       public bool MemberExists(string name, DateTime dateOfBirth)
       {
           string query = "SELECT COUNT(1) FROM Member WHERE Name = @Name AND DateOfBirth = @DateOfBirth";
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
