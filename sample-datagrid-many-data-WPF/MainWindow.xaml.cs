using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sample_datagrid_many_data_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel m_ViewModel;

        public MainWindow(MainWindowViewModel viewModel)
        {
            m_ViewModel = viewModel;
            DataContext = viewModel;
            InitializeComponent();
        }


        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            m_ViewModel.LoadedCommand.Execute(null);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            m_ViewModel.UnloadedCommand.Execute(null);
        }
    }
}