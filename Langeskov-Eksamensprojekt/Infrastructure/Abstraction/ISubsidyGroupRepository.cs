using Infrastructure.Model;

namespace Infrastructure.Repository
{
    // Defines the contract for a repository handling SubsidyGroup entities,
    // extending the generic IRepository with SubsidyGroup-specific operations.
    public interface ISubsidyGroupRepository : IRepository<SubsidyGroup>
    {
    }
}
