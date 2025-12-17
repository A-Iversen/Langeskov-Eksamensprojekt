using Infrastructure.Model;

namespace Infrastructure.Abstraction
{
    public interface IRunnerRepository : IRepository<Runner>
    {
        bool RunnerExists(string name, DateTime dateOfBirth);
    }
}
