using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using Infrastructure.Model;
using Infrastructure.Abstraction;

namespace Infrastructure.Persistence
{
    public class SQLRunnerGroupRepository : IRunnerGroupRepository
    {
        private readonly string _connectionString;

        public SQLRunnerGroupRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<RunnerGroup> GetAll()
        {
            var runnerGroups = new List<RunnerGroup>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT RunnerGroupID, RunnerGroupName, SubscriptionFee FROM RunnerGroup";
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    runnerGroups.Add(new RunnerGroup(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetDecimal(2)
                    ));
                }
                connection.Close();
            }
            return runnerGroups;
        }

        public RunnerGroup? GetById(int id)
        {
            RunnerGroup? runnerGroup = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT RunnerGroupID, RunnerGroupName, SubscriptionFee FROM RunnerGroup WHERE RunnerGroupID = @RunnerGroupID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@RunnerGroupID", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    runnerGroup = new RunnerGroup(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetDecimal(2)
                    );
                }
                connection.Close();
            }
            return runnerGroup;
        }

        public RunnerGroup Add(RunnerGroup entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "INSERT INTO RunnerGroup (RunnerGroupName, SubscriptionFee) VALUES (@RunnerGroupName, @SubscriptionFee); SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@RunnerGroupName", entity.RunnerGroupName);
                command.Parameters.AddWithValue("@SubscriptionFee", entity.SubscriptionFee);
                connection.Open();
                int newId = Convert.ToInt32(command.ExecuteScalar());
                entity.RunnerGroupID = newId;
                connection.Close();
            }
            return entity;
        }

        public void Update(RunnerGroup entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "UPDATE RunnerGroup SET RunnerGroupName = @RunnerGroupName, SubscriptionFee = @SubscriptionFee WHERE RunnerGroupID = @RunnerGroupID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@RunnerGroupName", entity.RunnerGroupName);
                command.Parameters.AddWithValue("@SubscriptionFee", entity.SubscriptionFee);
                command.Parameters.AddWithValue("@RunnerGroupID", entity.RunnerGroupID);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "DELETE FROM RunnerGroup WHERE RunnerGroupID = @RunnerGroupID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@RunnerGroupID", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}