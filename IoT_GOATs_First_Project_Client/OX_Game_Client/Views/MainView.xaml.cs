using MahApps.Metro.Controls;
using OX_Game_Client.ViewModels;
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

namespace OX_Game_Client.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : MetroWindow
    {
        public MainView()
        {
            InitializeComponent();
            var viewModel = new MainViewModel();
            this.DataContext = viewModel;

            viewModel.RequestNavigateToEmptyPage += NavigateToEmptyPage;
        }
        private void NavigateToEmptyPage()
        {
            //MainFrame.Visibility = Visibility.Collapsed;
            //MainFrame.Navigate(new Empty());
            //MainFrame.Visibility = Visibility.Visible;
            Empty emptyPage = new Empty();
            emptyPage.Show();
            this.Close();
        }

    }
}
