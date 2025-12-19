using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Model;
using Infrastructure.Abstraction;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Persistence
{
    public class SQLSubsidyGroupRepository : ISubsidyGroupRepository
    {
        private readonly string _connectionString;

        public SQLSubsidyGroupRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<SubsidyGroup> GetAll()
        {
            var subsidyGroups = new List<SubsidyGroup>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT SubsidyGroupID, SubsidyGroupNameText, AgeRange FROM SubsidyGroup";
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    subsidyGroups.Add(new SubsidyGroup(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2)
                    ));
                }
                connection.Close();
            }
            return subsidyGroups;
        }

        public SubsidyGroup? GetById(int id)
        {
            SubsidyGroup? subsidyGroup = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT SubsidyGroupID, SubsidyGroupNameText, AgeRange FROM SubsidyGroup WHERE SubsidyGroupID = @SubsidyGroupID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@SubsidyGroupID", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    subsidyGroup = new SubsidyGroup(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2)
                    );
                }
                connection.Close();
            }
            return subsidyGroup;
        }

        public SubsidyGroup Add(SubsidyGroup entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "INSERT INTO SubsidyGroup (SubsidyGroupNameText, AgeRange) VALUES (@SubsidyGroupNameText, @AgeRange); SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@SubsidyGroupNameText", entity.SubsidyGroupNameText);
                command.Parameters.AddWithValue("@AgeRange", entity.AgeRange);
                connection.Open();
                int newId = Convert.ToInt32(command.ExecuteScalar());
                entity.SubsidyGroupID = newId;
                connection.Close();
            }
            return entity;
        }

        public void Update(SubsidyGroup entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "UPDATE SubsidyGroup SET SubsidyGroupNameText = @SubsidyGroupNameText, AgeRange = @AgeRange WHERE SubsidyGroupID = @SubsidyGroupID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@SubsidyGroupNameText", entity.SubsidyGroupNameText);
                command.Parameters.AddWithValue("@AgeRange", entity.AgeRange);
                command.Parameters.AddWithValue("@SubsidyGroupID", entity.SubsidyGroupID);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "DELETE FROM SubsidyGroup WHERE SubsidyGroupID = @SubsidyGroupID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@SubsidyGroupID", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
