using System.IO;
using System.Windows;
using Infrastructure;
using MVVM.ViewModel;
using Microsoft.Extensions.Configuration;
using MVVM.View;


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

            MainViewModel mainViewModel = new MainViewModel(); 
            MainWindow mainWindow = new MainWindow { DataContext = mainViewModel };

            mainWindow.Show();
        }
    }
}