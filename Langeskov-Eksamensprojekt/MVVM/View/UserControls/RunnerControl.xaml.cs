using Infrastructure.Abstraction;
using Infrastructure.Model;
using Infrastructure.Persistence; // Added this line
using Microsoft.Extensions.Configuration;
using MVVM.ViewModel;
using System;
using System.Windows.Controls;

namespace MVVM.View.UserControls
{
    public partial class RunnerControl : UserControl
    {
        public RunnerControl()
        {
            InitializeComponent();
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("FEJL: Connection string 'DefaultConnection' blev ikke fundet i appsettings.json.");

            var runnerRepo = new SQLRunnerRepository(connectionString);
            var runnerGroupRepo = new SQLRunnerGroupRepository(connectionString);

            DataContext = new RunnerViewModel(runnerRepo, runnerGroupRepo);
        }


        //Event der er opkoblet til Datagridet, som er nødvendig til at sende et signal til en metode i viewmodellen.
        //Har at gøre med redigering af cellerne i datagridet.
        private void runnerDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction != DataGridEditAction.Commit)
                return;

            var grid = (DataGrid)sender;

            if (DataContext is RunnerViewModel vm && e.Row.Item is Runner runner)
            {
                grid.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (vm.UpdateRunnerCommand.CanExecute(runner))
                        vm.UpdateRunnerCommand.Execute(runner);
                }), System.Windows.Threading.DispatcherPriority.Background);
            }
        }
    }
}
