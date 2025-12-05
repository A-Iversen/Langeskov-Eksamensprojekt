using System.Collections.Generic;
using System.Linq;
using Infrastructure.Abstraction;
using Infrastructure.Model;

namespace MVVM.ViewModel
{
    class AccountingViewModel
    {
        private readonly IRunnerGroupRepository _memberGroupRepository;

        public IEnumerable<RunnerGroup> MemberGroups { get; private set; }

        public AccountingViewModel(IRunnerGroupRepository memberGroupRepository)
        {
            _memberGroupRepository = memberGroupRepository;
            MemberGroups = new List<RunnerGroup>(); // Initialize with an empty list
            LoadMemberGroups();
        }

        private void LoadMemberGroups()
        {
            MemberGroups = _memberGroupRepository.GetAll();
        }
    }
}

