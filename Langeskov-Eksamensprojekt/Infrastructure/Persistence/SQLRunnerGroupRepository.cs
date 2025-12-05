using Infrastructure.Abstraction;
using Infrastructure.Model;

namespace Infrastructure.Repository
{
    public class SQLRunnerGroupRepository : IRunnerGroupRepository
    {
        private readonly string _connectionString;

        public SQLRunnerGroupRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private static List<RunnerGroup> _groupData = new List<RunnerGroup>
        {
            new RunnerGroup(1, "Single", 159.00m),
            new RunnerGroup(2, "SingleGratis", 0.00m),
            new RunnerGroup(3, "Family", 350.00m),
            new RunnerGroup(4, "FamilyGratis", 0.00m)
        };

        public IEnumerable<RunnerGroup> GetAll()
        {
            // SQL: SELECT * FROM RUNNERGROUP
            return _groupData;
        }

        public RunnerGroup? GetById(int id)
        {
            // SQL: SELECT * FROM RUNNERGROUP WHERE RunnerGroupID = @ID
            return _groupData.FirstOrDefault(g => g.RunnerGroupID == id);
        }

        public RunnerGroup Add(RunnerGroup entity)
        {
            // SQL: INSERT INTO RUNNERGROUP (RunnerGroupName, SubscriptionFee) VALUES (@Name, @Fee); SELECT SCOPE_IDENTITY();
            if (!_groupData.Any(g => g.RunnerGroupID == entity.RunnerGroupID))
            {
                _groupData.Add(entity);
            }
            return entity;
        }

        public void Update(RunnerGroup entity)
        {
            // SQL: UPDATE RUNNERGROUP SET SubscriptionFee = @Fee, RunnerGroupName = @Name WHERE RunnerGroupID = @ID
            var existing = _groupData.FirstOrDefault(g => g.RunnerGroupID == entity.RunnerGroupID);
            if (existing != null)
            {
                existing.SubscriptionFee = entity.SubscriptionFee;
                existing.RunnerGroupName = entity.RunnerGroupName;
            }
        }
        public void Delete(int id)
        {
            // SQL: DELETE FROM RUNNERGROUP WHERE RunnerGroupID = @ID
            var groupToRemove = _groupData.FirstOrDefault(g => g.RunnerGroupID == id);
            if (groupToRemove != null)
            {
                _groupData.Remove(groupToRemove);
            }
        }
    }
}