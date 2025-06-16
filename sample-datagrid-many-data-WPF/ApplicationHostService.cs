using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace sample_datagrid_many_data_WPF
{
    public class ApplicationHostService(IServiceProvider _serviceProvider) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            _MainWindow.Show();
            await Task.CompletedTask;
        }

        private Window? _MainWindow;

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
