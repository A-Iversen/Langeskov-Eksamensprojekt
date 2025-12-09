using System;
using System.Windows.Controls;
using MVVM.ViewModel;
using Infrastructure.Repository;
using Microsoft.Extensions.Configuration;

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
    }
}
