using System.Collections.Generic;
using System.Linq;
using Infrastructure.Abstraction;
using Infrastructure.Model;

namespace MVVM.ViewModel
{
    class AccountingViewModel
    {
        private readonly IMemberGroupRepository _memberGroupRepository;

        public IEnumerable<MemberGroup> MemberGroups { get; private set; }

        public AccountingViewModel(IMemberGroupRepository memberGroupRepository)
        {
            _memberGroupRepository = memberGroupRepository;
            MemberGroups = new List<MemberGroup>(); // Initialize with an empty list
            LoadMemberGroups();
        }

        private void LoadMemberGroups()
        {
            MemberGroups = _memberGroupRepository.GetAll();
        }
    }
}

