using System.IO;
using System.Windows;
using Infrastructure.Model;
using Infrastructure;
using Infrastructure.Abstraction;
using Infrastructure.Repository;
using MVVM.ViewModel;
using Microsoft.Extensions.Configuration;


namespace MVVM
{
   public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("FEJL: Connection string 'DefaultConnection' blev ikke fundet i appsettings.json.");

            DatabaseInitializer.Initialize(connectionString);

            IMemberRepository memberRepository = new SQLMemberRepository(connectionString);
            IMemberGroupRepository memberGroupRepository = new SQLMemberGroupRepository(connectionString);


            // MainViewModel mainViewModel = new MainViewModel(memberRepository, memberGroupRepository); 
            // MainWindow mainWindow = new MainWindow { DataContext = mainViewModel };

            // mainWindow.Show();
        }
    }
}