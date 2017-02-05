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
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Net.Mail;

namespace Click_IT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Fields

        /// Doesn't work for some reason.
        //public static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + System.AppDomain.CurrentDomain.BaseDirectory + "playerDatabase.mdf;Integrated Security=True"; 

        public const string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=I:\Visual Studio\School\Periode 2\FUN12\xaml\Click-IT 0.08\Click-IT\playerDatabase.mdf;Integrated Security=True";
        public static SqlConnection conn = new SqlConnection(connectionString);

        public static string username = "";

        Stats stats = new Stats();
        public static MakeProfile mP = new MakeProfile(); 
        public static DateTime loginTime = new DateTime();      
        public static Mail mail;
        public static WindowControle wC;
        public static Login login;       

        List<PlayerStats> playerStatsList = new List<PlayerStats>();


        bool maximized = false;

        

        // Constructor

        public MainWindow()
        {
            InitializeComponent();         
        }



        // Methodes

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbUsernameRecovery.Visibility = Visibility.Hidden;
            btnSendEmail.Visibility = Visibility.Hidden;

            mail = new Mail("gameclickit@gmail.com", "Click1234", "gameclickit@gmail.com", 587, "smtp.gmail.com");
            wC = new WindowControle(this.WindowState, this);

            stats.getStatsFromPlayers(conn);
            lvScoreList.ItemsSource = stats.PlayerStatList;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvScoreList.ItemsSource);
            view.Filter = filter;
        }


        private bool filter(object item)
        {
            if (String.IsNullOrEmpty(tbSearchPlayer.Text)) // If the search box is empty all items in the listView are shown.
                return true;
            else
                return ((item as PlayerStats).username.IndexOf(tbSearchPlayer.Text, StringComparison.Ordinal) >= 0);
        }


        private void tbSearchPlayer_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lvScoreList.ItemsSource).Refresh(); 
        }


        private void btnCreateProfile_Click(object sender, RoutedEventArgs e)
        {
            mP.UpdatePlayer= false;
            mP.MakeNewPlayer = true;
            mP.Show();
            this.Close();
        }
        

        private void btnUdateProfile_Click(object sender, RoutedEventArgs e)
        {
            username = tbUserName.Text;
            login = new Login(conn, tbUserName.Text, tbPassword.Password, this);
            login.checkLogin();          

            if (login.CountUsernameAndPassword == 1)
            {
                mP.UpdatePlayer = true;
                mP.MakeNewPlayer = false;
                mP.Show();
                this.Close();
            }
        }


        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            username = tbUserName.Text;
            login = new Login(conn, tbUserName.Text, tbPassword.Password, this);
            login.checkLogin();
            login.openGame();
        }


        private void btnForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            tbUsernameRecovery.Visibility = Visibility.Visible;
            btnSendEmail.Visibility = Visibility.Visible;
            mail.showInfo();
        }


        private void btnSendEmail_Click(object sender, RoutedEventArgs e)
        {
            mail.newPasswordMail(tbUsernameRecovery.Text, conn);
        }     


        private void lblTitalBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            wC.dragWindow(e);
        }


        private void btnMinimize_Click_1(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }


        private void btnMaximaize_Click(object sender, RoutedEventArgs e)
        {
            if (!maximized)
            {
                WindowState = WindowState.Maximized;
                maximized = true;
            }
            else if (maximized)
            {
                WindowState = WindowState.Normal;
                maximized = false;
            }
        }


        private void btnExit_Click_1(object sender, RoutedEventArgs e)
        {
            wC.Exit();
        }
    }
}

