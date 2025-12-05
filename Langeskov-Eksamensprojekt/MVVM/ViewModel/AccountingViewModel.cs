using System.Collections.Generic;
using System.Linq;
using Infrastructure.Abstraction;
using Infrastructure.Model;

namespace MVVM.ViewModel
{
    class AccountingViewModel
    {
        private readonly IRunnerGroupRepository _runnerGroupRepository;

        public IEnumerable<RunnerGroup> RunnerGroups { get; private set; }

        public AccountingViewModel(IRunnerGroupRepository runnerGroupRepository)
        {
            _runnerGroupRepository = runnerGroupRepository;
            RunnerGroups = new List<RunnerGroup>(); // Initialize with an empty list
            LoadMemberGroups();
        }

        private void LoadMemberGroups()
        {
            RunnerGroups = _runnerGroupRepository.GetAll();
        }
    }
}

