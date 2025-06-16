global using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.Logging;


namespace sample_datagrid_many_data_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private async void OnStartup(object sender, StartupEventArgs e)
        {
            _host = Host.CreateDefaultBuilder(e.Args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddDebug();
                })
                .ConfigureServices(collection =>
                {
                    collection.AddHostedService<ApplicationHostService>();
                    collection.AddTransient<MainWindow>();
                    collection.AddTransient<MainWindowViewModel>();
                    collection.AddSingleton<DataItemObserver>();
                    collection.AddSingleton<IDataItemObserver>(d => d.GetRequiredService<DataItemObserver>());
                    collection.AddSingleton<IDataItemSubject>(d => d.GetRequiredService<DataItemObserver>());
                })
                .Build();

            await _host.StartAsync();
        }
        private IHost? _host;
    }

}
