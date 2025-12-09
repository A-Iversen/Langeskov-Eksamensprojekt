using System;
using System.Windows.Controls;
using MVVM.ViewModel;
using Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Infrastructure.Persistence;

namespace MVVM.View.UserControls
{
    // Interaction logic for AccountingControl.xaml
    public partial class AccountingControl : UserControl
    {
        public AccountingControl()
        {
            InitializeComponent();

            // Set up configuration to read appsettings.json
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Retrieve the connection string from configuration
            string connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("FEJL: Connection string 'DefaultConnection' blev ikke fundet i appsettings.json.");

            // Initialize repositories with the connection string (or mock data for SubsidyGroupRepository)
            IRunnerRepository runnerRepository = new SQLRunnerRepository(connectionString);
            ISubsidyGroupRepository subsidyGroupRepository = new SQLSubsidyGroupRepository(); // Using mock data for now
            IRunnerGroupRepository runnerGroupRepository = new SQLRunnerGroupRepository(connectionString);
            
            // Set the DataContext to an instance of AccountingViewModel, injecting the required repositories
            DataContext = new AccountingViewModel(runnerGroupRepository, runnerRepository, subsidyGroupRepository);
        }
    }
}
