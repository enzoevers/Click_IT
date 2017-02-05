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
using System.Windows.Media.Converters;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Windows.Media.Animation;
using System.Media;

namespace Click_IT
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        // Fields 

        SoundPlayer countDown = new SoundPlayer(Properties.Resources.Race_start__mp3cut_net_);
        SoundPlayer backgroundMusic = new SoundPlayer(Properties.Resources._8_bit_loop);
        SoundPlayer inGameBackgroundMusic = new SoundPlayer(Properties.Resources._8_bit_loop_1);

        GameSetUp gameSU = new GameSetUp();
        PlayGame play;
        GameData GameDataInstance = new GameData();
        MainWindow main = new MainWindow();

        DateTime logOut = new DateTime(); 

        bool maximized = false;





        // Constructor 

        public Game()
        {
            InitializeComponent();

            addButton();
            foreach (Button btn in gameSU.Matrix)
            {
                btn.Click += new RoutedEventHandler(getButtonText);
            }
        }





        // Methods

        /// <summary>
        /// Adds all the buttons to the List<Button> Matrix.
        /// </summary>
        private void addButton()
        {
            gameSU.Matrix.Add(btn11);
            gameSU.Matrix.Add(btn12);
            gameSU.Matrix.Add(btn13);
            gameSU.Matrix.Add(btn14);
            gameSU.Matrix.Add(btn15);
            gameSU.Matrix.Add(btn16);
            gameSU.Matrix.Add(btn17);
            gameSU.Matrix.Add(btn18);

            gameSU.Matrix.Add(btn21);
            gameSU.Matrix.Add(btn22);
            gameSU.Matrix.Add(btn23);
            gameSU.Matrix.Add(btn24);
            gameSU.Matrix.Add(btn25);
            gameSU.Matrix.Add(btn26);
            gameSU.Matrix.Add(btn27);
            gameSU.Matrix.Add(btn28);

            gameSU.Matrix.Add(btn31);
            gameSU.Matrix.Add(btn32);
            gameSU.Matrix.Add(btn33);
            gameSU.Matrix.Add(btn34);
            gameSU.Matrix.Add(btn35);
            gameSU.Matrix.Add(btn36);
            gameSU.Matrix.Add(btn37);
            gameSU.Matrix.Add(btn38);

            gameSU.Matrix.Add(btn41);
            gameSU.Matrix.Add(btn42);
            gameSU.Matrix.Add(btn43);
            gameSU.Matrix.Add(btn44);
            gameSU.Matrix.Add(btn45);
            gameSU.Matrix.Add(btn46);
            gameSU.Matrix.Add(btn47);
            gameSU.Matrix.Add(btn48);

            gameSU.Matrix.Add(btn51);
            gameSU.Matrix.Add(btn52);
            gameSU.Matrix.Add(btn53);
            gameSU.Matrix.Add(btn54);
            gameSU.Matrix.Add(btn55);
            gameSU.Matrix.Add(btn56);
            gameSU.Matrix.Add(btn57);
            gameSU.Matrix.Add(btn58);

            gameSU.Matrix.Add(btn61);
            gameSU.Matrix.Add(btn62);
            gameSU.Matrix.Add(btn63);
            gameSU.Matrix.Add(btn64);
            gameSU.Matrix.Add(btn65);
            gameSU.Matrix.Add(btn66);
            gameSU.Matrix.Add(btn67);
            gameSU.Matrix.Add(btn68);

            gameSU.Matrix.Add(btn71);
            gameSU.Matrix.Add(btn72);
            gameSU.Matrix.Add(btn73);
            gameSU.Matrix.Add(btn74);
            gameSU.Matrix.Add(btn75);
            gameSU.Matrix.Add(btn76);
            gameSU.Matrix.Add(btn77);
            gameSU.Matrix.Add(btn78);

            gameSU.Matrix.Add(btn81);
            gameSU.Matrix.Add(btn82);
            gameSU.Matrix.Add(btn83);
            gameSU.Matrix.Add(btn84);
            gameSU.Matrix.Add(btn85);
            gameSU.Matrix.Add(btn86);
            gameSU.Matrix.Add(btn87);
            gameSU.Matrix.Add(btn88);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GameDataInstance.selectUserID(MainWindow.username);
            gameSU.fillScoreboardInGame();
            play = new PlayGame(gameSU.Matrix, gameSU.MatrixString);
            intUDAmountOfRounds.Value = 1;          
            lvUsernameAndBestTime.ItemsSource = gameSU.UsernameAndBestTime;
            lblBestPersonalReactionTime.Content = gameSU.BestAlloverTime.ToString();
            backgroundMusic.PlayLooping();
        }

        /// <summary>
        /// Detects the if the value of the intergerUpDown has changed.
        /// Sets the amount of rounds to the value in the intergerUpDown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void intUDAmountOfRounds_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            play.AmountOfRounds = Convert.ToInt32(e.NewValue); 
        }

        /// <summary>
        /// Gets the current gameID,
        /// hides the start button,
        /// disables changed of the intergerUpDown,
        /// starts the rigt audio file
        /// and starts the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            lblCurrentAverageTime.Content = "";

            play.GameID = GameDataInstance.getGameID();

            btnStart.Visibility = Visibility.Hidden;
            intUDAmountOfRounds.AllowSpin = false;
            play.GameEnded = false;
            backgroundMusic.Stop();
            countDown.PlaySync();
            inGameBackgroundMusic.PlayLooping();
            play.playGame();
            giveButtonText();
        }

        /// <summary>
        /// checks the pressed button and updates the best time and average time of the current game.
        /// If game ended the start button appears and the intergerUpDown can be changed.
        /// Than plays the background music.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void getButtonText(object sender, EventArgs e)
        {
            if(btnStart.Visibility == Visibility.Hidden)
            {
                play.RandomChosenButton.Content = "";
                play.getButtonText(sender, e);
                giveButtonText();
                lblCurrentAverageTime.Content = play.AverageTime;
                lblBestPersonalReactionTime.Content = play.BestTime;
            }
            
            if (play.GameEnded)
            {
                play.GameEnded = false;
                play.RandomChosenButton.Content = "";
                btnStart.Visibility = Visibility.Visible;
                intUDAmountOfRounds.AllowSpin = true;
                intUDAmountOfRounds.Value = 1;
                inGameBackgroundMusic.Stop();
                backgroundMusic.PlayLooping();
            }
        }

        private void giveButtonText()
        {
            play.RandomChosenButton.Content = play.RoundsLeft;
            play.RandomChosenButton.FontSize = 25;
            play.RandomChosenButton.FontFamily = new FontFamily("Kuro");
            play.RandomChosenButton.Foreground = Brushes.White;
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            backgroundMusic.Stop();
            logout();
        }

        private void logout()
        {
            logOut = DateTime.Now;
            writeLogOut();
            this.Close();
            main.Show();
        }

        /// <summary>
        /// Writes the logged in userID, login time and logout time to the database.
        /// </summary>
        private void writeLogOut()
        {
            string queryInsertPlayerID = "INSERT INTO LoginData (UserID, Login_date, Logout_date) VALUES (@UserID, @Login_date, @Logout_date)";     

            using (SqlCommand insertIntoLoginData = new SqlCommand(queryInsertPlayerID, MainWindow.conn))
            {
                MainWindow.conn.Open();
                insertIntoLoginData.Parameters.AddWithValue("@UserID", gameSU.UserID);
                insertIntoLoginData.Parameters.AddWithValue("@Login_date", MainWindow.login.LoginTime);
                insertIntoLoginData.Parameters.AddWithValue("@Logout_date", logOut);
                insertIntoLoginData.ExecuteNonQuery();
                MainWindow.conn.Close();
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            backgroundMusic.Stop();
            logOut = DateTime.Now;
            writeLogOut();
            Application.Current.Shutdown();
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

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void lblTitalBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.wC.dragWindow(e);
        }

        
    }
}
