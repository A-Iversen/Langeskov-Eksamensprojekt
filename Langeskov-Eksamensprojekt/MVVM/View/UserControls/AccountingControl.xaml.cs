using System;
using System.Windows.Controls;
using MVVM.ViewModel;
using Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Infrastructure.Abstraction;

namespace MVVM.View.UserControls
{
    public partial class AccountingControl : UserControl
    {
        public AccountingControl()
        {
            InitializeComponent();
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("FEJL: Connection string 'DefaultConnection' blev ikke fundet i appsettings.json.");

            IRunnerGroupRepository runnerGroupRepository = new SQLRunnerGroupRepository(connectionString);
            DataContext = new AccountingViewModel(runnerGroupRepository);
        }
    }
}
