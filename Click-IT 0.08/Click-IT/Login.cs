using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Media;

namespace Click_IT
{
    public class Login
    {
        // Fields

        SoundPlayer deepHit = new SoundPlayer(Properties.Resources.Deep_hit);

        MakeProfile mP = new MakeProfile();
        private SqlConnection conn;
        private string username;
        private string password;

        private int countUsername;
        private int countPassword;
        private int countUsernameAndPassword;
        private Window window;

        private string loginError;

        private DateTime loginTime;



        //Poperties

        /// <summary>
        /// Return the time of login.
        /// </summary>
        public DateTime LoginTime
        {
            get
            {
                return this.loginTime;
            }
        }

        /// <summary>
        /// Return how many times the combination of username and password exists in the database.
        /// </summary>
        public int CountUsernameAndPassword
        {
            get
            {
                return this.countUsernameAndPassword;
            }
        }

        // Constructor

        public Login(SqlConnection Connection, string Username, string Password, Window Window)
        {
            conn = Connection;
            username = Username;
            password = Password;
            window = Window;
        }

        // Methods

        /// <summary>
        /// Gets the amount of times the username, password and the combination exists in the database.
        /// </summary>
        public void checkLogin()
        {
            string queryCheckUsername = "SELECT " +                     // Counts how many usernames there are in the database similar to
                                            "COUNT(*) " +               // the entered username.
                                        "FROM " +
                                            "Player " +
                                        "WHERE " +
                                            "Username LIKE @Username";

            string queryCheckPassword = "SELECT " +                     // Counts how many passwords there are in the database similar to
                                            "COUNT(*) " +               // the entered password.
                                        "FROM " +
                                            "Player " +
                                        "WHERE " +
                                            "Password LIKE @Password";

            string queryCheckUsernameAndPassword = "SELECT " +                          // Counts how many 
                                                        "COUNT(*) " +                    // combination
                                                    "FROM " +                            // there are in the database
                                                        "Player " +                      // similar to
                                                    "WHERE " +                           // the entered username
                                                        "Username LIKE @Username AND " + // and password.
                                                        "Password LIKE @Password";






            using (SqlCommand checkUsername = new SqlCommand(queryCheckUsername, conn))
            {
                conn.Open();
                checkUsername.Parameters.AddWithValue("@Username", username);
                countUsername = (int)checkUsername.ExecuteScalar(); // Gives the countUsername the amount of found usernames similair to the entered username.
                conn.Close();
            }

            using (SqlCommand checkUsername = new SqlCommand(queryCheckPassword, conn))
            {
                conn.Open();
                checkUsername.Parameters.AddWithValue("@Password", password);
                countPassword = (int)checkUsername.ExecuteScalar(); // Gives the countPassword the amount of found passwords similair to the entered password.
                conn.Close();
            }

            using (SqlCommand checkUsername = new SqlCommand(queryCheckUsernameAndPassword, conn))
            {
                conn.Open();
                checkUsername.Parameters.AddWithValue("@Username", username);
                checkUsername.Parameters.AddWithValue("@Password", password);
                countUsernameAndPassword = (int)checkUsername.ExecuteScalar(); // Gives the countUsernameAndPassword the 
                                                                               // amount of found combinations similair to the entered username and password.
                conn.Close();
            }
            sendResult();
        }


        /// <summary>
        /// Adds errors if needed.
        /// </summary>
        public void sendResult()
        {
            if (password == "")
            {
                loginError += "Please enter a password." + "\n";
            }
            if (String.IsNullOrEmpty(username))
            {
                loginError += "Please enter a username." + "\n";
            }

            if (countUsername == 0 && !String.IsNullOrEmpty(username))
            {
                loginError += "The given username doesn't exist" + "\n";
            }

            if (countPassword == 0 && password != "")
            {
                loginError += "The given password doesn't exist" + "\n";
            }

            if (countUsernameAndPassword == 0)
            {
                if (String.IsNullOrEmpty(loginError))
                {
                    loginError += "The combination of the username and password doesn't exist" + "\n";
                }
            }

            if (countUsername == 0 || countPassword == 0 || countUsernameAndPassword == 0)
            {
                MessageBox.Show(loginError, "Login errors");
            }

            loginError = "";
        }

        /// <summary>
        /// If bot logged in to update the profile opens the game.
        /// </summary>
        public void openGame()
        {
            if (!mP.UpdatePlayer)
            {
                if (countUsernameAndPassword == 1)
                {
                    loginTime = DateTime.Now; 
                    //deepHit.PlaySync();

                    Game game = new Game();
                    window.Close();
                    game.Show();
                }
            }
        }
    }
}
