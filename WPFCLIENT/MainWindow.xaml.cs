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
using System.Xaml;
using System.Drawing;
using MySql.Data.MySqlClient;

namespace WPFCLIENT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Window_Loaded);
            sql.Open();
        }
        public MySqlConnection sql = new MySqlConnection(SQL_Explorer.connectionString());
        private void logSubmit(object sender, RoutedEventArgs e)
        {
            ErrorExplorer EE = new ErrorExplorer(lblErrorExpLOG, sql); EE.activateLOG(logNickName.Text.ToString(), logPass.Password.ToString());
            if (ErrorExplorer.valide_key == true) { Login.IsEnabled = false; }
        }

        private void Go_Registration(object sender, RoutedEventArgs e)
        {
            Login.IsEnabled = false;
            Registration.IsEnabled = true;
            Registration.Visibility = Visibility.Visible;
            ErrorExplorer.CleanToRes();
        }

        private void Go_Login(object sender, RoutedEventArgs e)
        {
            Login.Visibility = Visibility.Visible;
            Registration.IsEnabled = false;
            Login.IsEnabled = true;
            ErrorExplorer.CleanToRes();
        }

        private void regSubmit(object sender, RoutedEventArgs e)
        {
            ErrorExplorer EE = new ErrorExplorer(lblErrorExpREG, sql); EE.activateREG(regNickName.Text.ToString(),regEmail.Text.ToString(), regName.Text.ToString(), regSurname.Text.ToString(), regPass.Password.ToString(), regRePass.Password.ToString());
            if (ErrorExplorer.valide_key == true) { Registration.IsEnabled = false; }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
          /*  BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(@"/source.gif", UriKind.RelativeOrAbsolute);
            bi.EndInit();
            MainBackground.ImageSource = bi; */
        }
    }
}
