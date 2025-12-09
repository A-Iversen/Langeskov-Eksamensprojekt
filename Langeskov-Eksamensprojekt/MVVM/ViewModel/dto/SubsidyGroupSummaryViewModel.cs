namespace MVVM.ViewModel.dto
{
    // ViewModel for displaying a summary of a subsidy group, including its name and the number of runners.
    public class SubsidyGroupSummaryViewModel
    {
        public string? SubsidyGroupName { get; set; }
        public int RunnerCount { get; set; }
    }
}
