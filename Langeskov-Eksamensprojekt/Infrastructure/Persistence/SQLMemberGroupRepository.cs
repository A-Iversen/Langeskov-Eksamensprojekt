using Infrastructure.Abstraction;
using Infrastructure.Model;

namespace Infrastructure.Repository
{
    public class SQLMemberGroupRepository : IMemberGroupRepository
    {
        private readonly string _connectionString;

        public SQLMemberGroupRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private static List<MemberGroup> _groupData = new List<MemberGroup>
        {
            new MemberGroup(1, "Single", 159.00m),
            new MemberGroup(2, "SingleGratis", 0.00m),
            new MemberGroup(3, "Family", 350.00m),
            new MemberGroup(4, "FamilyGratis", 0.00m)
        };

        public IEnumerable<MemberGroup> GetAll()
        {
            // SQL: SELECT * FROM MEMBERGROUP
            return _groupData;
        }

        public MemberGroup? GetById(int id)
        {
            // SQL: SELECT * FROM MEMBERGROUP WHERE MemberGroupID = @ID
            return _groupData.FirstOrDefault(g => g.MemberGroupID == id);
        }

        public MemberGroup Add(MemberGroup entity)
        {
            // SQL: INSERT INTO MEMBERGROUP (MemberGroupName, SubscriptionFee) VALUES (@Name, @Fee); SELECT SCOPE_IDENTITY();
            if (!_groupData.Any(g => g.MemberGroupID == entity.MemberGroupID))
            {
                _groupData.Add(entity);
            }
            return entity;
        }

        public void Update(MemberGroup entity)
        {
            // SQL: UPDATE MEMBERGROUP SET SubscriptionFee = @Fee, MemberGroupName = @Name WHERE MemberGroupID = @ID
            var existing = _groupData.FirstOrDefault(g => g.MemberGroupID == entity.MemberGroupID);
            if (existing != null)
            {
                existing.SubscriptionFee = entity.SubscriptionFee;
                existing.MemberGroupName = entity.MemberGroupName;
            }
        }
        public void Delete(int id)
        {
            // SQL: DELETE FROM MEMBERGROUP WHERE MemberGroupID = @ID
            var groupToRemove = _groupData.FirstOrDefault(g => g.MemberGroupID == id);
            if (groupToRemove != null)
            {
                _groupData.Remove(groupToRemove);
            }
        }
    }
}