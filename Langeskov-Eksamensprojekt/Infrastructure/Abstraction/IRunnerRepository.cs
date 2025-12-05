using Infrastructure.Model;

namespace Infrastructure.Repository
{
    public interface IRunnerRepository : IRepository<Runner>
    {
        bool RunnerExists(string name, DateTime dateOfBirth);
    }
}
