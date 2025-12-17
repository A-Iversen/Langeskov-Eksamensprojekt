using Infrastructure.Abstraction;
using Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Unittests
{
    public class MockRunnerRepository : IRunnerRepository
    {
        private readonly List<Runner> _runners;

        public MockRunnerRepository(List<Runner>? initialRunners = null)
        {
            _runners = initialRunners ?? new List<Runner>();
        }

        public IEnumerable<Runner> GetAll()
        {
            throw new NotImplementedException();
        }

        public Runner? GetById(int id)
        {
            return _runners.FirstOrDefault(r => r.RunnerID == id);
        }

        public Runner Add(Runner entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Runner entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool RunnerExists(string name, DateTime dateOfBirth)
        {
            return _runners.Any(r => r.Name == name && r.DateOfBirth.Date == dateOfBirth.Date);
        }
    }
}
