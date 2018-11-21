using System.Windows;
using System.Windows.Controls;

namespace DataProtector.App
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((MainWindowViewModel)DataContext).Password = ((PasswordBox)sender).SecurePassword;
            }
        }
    }
}