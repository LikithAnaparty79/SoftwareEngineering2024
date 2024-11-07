using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Dashboard.ViewModel;

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        private readonly MainPageViewModel _viewModel;

        public WelcomePage(MainPageViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            this.DataContext = _viewModel;

        }
        private void CreateSession_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = UserName.Text;
                string useremail = UserEmail.Text;
                string? response = _viewModel?.CreateSession(username,useremail);
                if (response == "success")
                {
                    this.NavigationService.Navigate(new ServerHomePage(_viewModel));
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void JoinSession_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_viewModel != null)
                {
                    string username = UserName.Text;
                    string useremail = UserEmail.Text;
                    string serverip = ServerIP.Text;
                    string serverport = ServerPort.Text;
                    string response = _viewModel.JoinSession(username, useremail, serverip, serverport);
                    if (response == "success")
                    {
                        this.NavigationService.Navigate(new ClientHomePage(_viewModel));
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }

}
