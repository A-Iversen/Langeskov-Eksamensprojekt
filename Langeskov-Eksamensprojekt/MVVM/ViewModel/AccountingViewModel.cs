using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Infrastructure.Model;
using Infrastructure.Repository;
using MVVM.Core;
using MVVM.ViewModel.dto;

namespace MVVM.ViewModel
{
    // ViewModel for the accounting section of the application.
    // Handles logic and data related to runner groups and subsidy groups for display in the UI.
    class AccountingViewModel : ViewModelBase
    {
        private readonly IRunnerGroupRepository _runnerGroupRepository;
        private readonly IRunnerRepository _runnerRepository;
        private readonly ISubsidyGroupRepository _subsidyGroupRepository;

        // Gets a collection of all runner groups.
        public IEnumerable<RunnerGroup> RunnerGroups { get; private set; }
        // Gets a list summarizing the number of runners in each subsidy group.
        public List<SubsidyGroupSummaryViewModel> SubsidyGroupSummary { get; private set; }

        private Visibility _runnerGroupsVisibility;
        public Visibility RunnerGroupsVisibility
        {
            get { return _runnerGroupsVisibility; }
            set
            {
                _runnerGroupsVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _subsidySummaryVisibility;
        public Visibility SubsidySummaryVisibility
        {
            get { return _subsidySummaryVisibility; }
            set
            {
                _subsidySummaryVisibility = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowRunnerGroupsCommand { get; }
        public ICommand ShowSubsidySummaryCommand { get; }

        // Initializes a new instance of the AccountingViewModel class.
        // <param name="runnerGroupRepository">The repository for runner group data.</param>
        // <param name="runnerRepository">The repository for runner data.</param>
        // <param name="subsidyGroupRepository">The repository for subsidy group data.</param>
        public AccountingViewModel(IRunnerGroupRepository runnerGroupRepository, IRunnerRepository runnerRepository, ISubsidyGroupRepository subsidyGroupRepository)
        {
            _runnerGroupRepository = runnerGroupRepository;
            _runnerRepository = runnerRepository;
            _subsidyGroupRepository = subsidyGroupRepository;

            RunnerGroups = new List<RunnerGroup>();
            SubsidyGroupSummary = new List<SubsidyGroupSummaryViewModel>();

            LoadMemberGroups();
            LoadSubsidyGroupSummary();

            ShowRunnerGroupsCommand = new RelayCommand(ShowRunnerGroups);
            ShowSubsidySummaryCommand = new RelayCommand(ShowSubsidySummary);

            // Set initial visibility
            RunnerGroupsVisibility = Visibility.Visible;
            SubsidySummaryVisibility = Visibility.Collapsed;
        }

        private void ShowRunnerGroups(object? obj)
        {
            RunnerGroupsVisibility = Visibility.Visible;
            SubsidySummaryVisibility = Visibility.Collapsed;
        }

        private void ShowSubsidySummary(object? obj)
        {
            RunnerGroupsVisibility = Visibility.Collapsed;
            SubsidySummaryVisibility = Visibility.Visible;
        }

        // Loads all runner groups from the repository.
        private void LoadMemberGroups()
        {
            RunnerGroups = _runnerGroupRepository.GetAll();
        }

        // Calculates and loads the summary of runners per subsidy group.
        // This method iterates through each subsidy group and counts the number of runners belonging to it.
        private void LoadSubsidyGroupSummary()
        {
            var runners = _runnerRepository.GetAll();
            var subsidyGroups = _subsidyGroupRepository.GetAll();
            var summary = new List<SubsidyGroupSummaryViewModel>();

            foreach (var sg in subsidyGroups)
            {
                // For each subsidy group, count how many runners have a matching SubsidyGroupID.
                int count = runners.Count(r => r.SubsidyGroupID == sg.SubsidyGroupID);
                summary.Add(new SubsidyGroupSummaryViewModel
                {
                    SubsidyGroupName = sg.SubsidyGroupNameText,
                    RunnerCount = count
                });
            }

            SubsidyGroupSummary = summary;
        }
    }
}

