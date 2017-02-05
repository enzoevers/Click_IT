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
using System.Windows.Shapes;
using System.Data.SqlClient;


namespace Click_IT
{
    /// <summary>
    /// Interaction logic for MakeProfile.xaml
    /// </summary>
    public partial class MakeProfile : Window
    {
        // Fields

        public static Player p1; 

        Stats stats = new Stats(); 
        GameData gameData = new GameData();

        public bool proceed = false; 
        MakeProfileChecker mpc;

        bool maximized = false; 

        public static int countryID;
        string countryName = "";

        public static int genderID;
        string genderCharacter = "";

        private bool updatePlayer = false;
        private bool makeNewPlayer = false;


        // Properties

        public bool UpdatePlayer
        {
            get
            {
                return this.updatePlayer;
            }
            set
            {
                this.updatePlayer = value;
            }
        }

        public bool MakeNewPlayer
        {
            get
            {
                return this.makeNewPlayer;
            }
            set
            {
                this.makeNewPlayer = value;
            }
        }



        // Constructor

        public MakeProfile()
        {
            InitializeComponent();


            string queryGetCountries = "SELECT COUNTRY_NAME FROM Country"; 

            using (SqlCommand getCountries = new SqlCommand(queryGetCountries, MainWindow.conn))
            {
                MainWindow.conn.Open();
                SqlDataReader countryName = getCountries.ExecuteReader();
                while (countryName.Read())
                {
                    cbMakeProfileCountry.Items.Add(countryName[0].ToString()); 
                }
                MainWindow.conn.Close();
            }


            string queryGetGenders = "SELECT Gender FROM Gender"; 

            using (SqlCommand getGender = new SqlCommand(queryGetGenders, MainWindow.conn))
            {
                MainWindow.conn.Open();
                SqlDataReader GenderCharacter = getGender.ExecuteReader();
                while (GenderCharacter.Read())
                {
                    cbMakeProfileGender.Items.Add(GenderCharacter[0].ToString()); 
                }
                MainWindow.conn.Close();
            }
        }


        // Methods

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gameData.selectUserID(MainWindow.username);
            insertPlayerProfileData();
        }

        /// <summary>
        /// Gets all the data of the logged in user if he wants to update his/her profile.
        /// </summary>
        private void insertPlayerProfileData()
        {
            if (updatePlayer)
            {
                string queryGetAndInsetPlayerData = "SELECT * FROM Player WHERE username = (@currentUsername)";

                using(SqlCommand getAndInsetPlayerData = new SqlCommand(queryGetAndInsetPlayerData, MainWindow.conn))
                {
                    MainWindow.conn.Open();
                    getAndInsetPlayerData.Parameters.AddWithValue("@currentUsername", MainWindow.username);
                    SqlDataReader readProfileData = getAndInsetPlayerData.ExecuteReader();
                    while (readProfileData.Read())
                    {
                        tbMakeProfileFirstName.Text = readProfileData[3].ToString();
                        tbMakeProfileSurname.Text = readProfileData[4].ToString();
                        tbMakeProfileUsername.Text = readProfileData[1].ToString();
                        tbMakeProfilePassword.Password = readProfileData[2].ToString();
                        dpMakeProfileAge.SelectedDate = Convert.ToDateTime(readProfileData[5]).Date;
                        cbMakeProfileCountry.SelectedIndex = Convert.ToInt32(readProfileData[9]);
                        tbMakeProfilePhoneNumber.Text = readProfileData[7].ToString();
                        cbMakeProfileGender.SelectedIndex = Convert.ToInt32(readProfileData[6]);   
                        tbMakeProfileEmail.Text = readProfileData[8].ToString();                
                    }
                    MainWindow.conn.Close();
                }
            }
        }


        private void btnCreateProfile_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow(); // Make a new object of the MainWindow Form.
                                                // By doing this, the MainWindow can be shown later on.

            lookUpCountryID();
            lookUpGenderID();

            if (UpdatePlayer)
            {
                string queryEmptyUsernameAndPassword = "UPDATE Player SET Username = '', Password = '' WHERE Username = (@username)";

                using (SqlCommand emptyUsernameAndPassword = new SqlCommand(queryEmptyUsernameAndPassword, MainWindow.conn))
                {
                    MainWindow.conn.Open();
                    emptyUsernameAndPassword.Parameters.AddWithValue("@username", MainWindow.username);
                    emptyUsernameAndPassword.ExecuteNonQuery();
                    MainWindow.conn.Close();
                }
            }
            mpc = new MakeProfileChecker(this); // Checks wheter the given usename and password already exists or not.   
                                            // Fills, if needed, both error string and shows them.
                                            // If everything is ok, the bool "proceed" is set to true.

            if (proceed)
            {
                getCountryName(countryID);
                getGenderCharacter(genderID);
                assignPlayerValues();      // A new new object of Player (p1) is made.

                if (makeNewPlayer)
                {
                    p1.insertProfileToDatabase(); // Inserts the player on a new row in the Players table.
                }
                else if (updatePlayer)
                {
                    p1.UpdateProfile(gameData.ValidUserID);
                } 

                this.Close(); 
                main.Show();  
            }
        }

        
        /// <summary>
        /// Gets the ID of the selected country.
        /// </summary>
        private void lookUpCountryID()
        {
            string queryGetCountryID = "SELECT id FROM Country WHERE COUNTRY_NAME = (@country)";

            using (SqlCommand getCountryID = new SqlCommand(queryGetCountryID, MainWindow.conn))
            {
                MainWindow.conn.Open();
                getCountryID.Parameters.AddWithValue("@country", cbMakeProfileCountry.SelectedItem.ToString());
                SqlDataReader idReader = getCountryID.ExecuteReader();
                while (idReader.Read())
                {
                    countryID = Convert.ToInt32(idReader[0]);
                }
                MainWindow.conn.Close();
            }
        }


        /// <summary>
        /// Gets the ID of the selected gender.
        /// </summary>
        private void lookUpGenderID()
        {
            string queryGetGenderID = "SELECT id FROM Gender WHERE Gender = (@genderCharacter)";

            using (SqlCommand getGenderID = new SqlCommand(queryGetGenderID, MainWindow.conn))
            {
                MainWindow.conn.Open();
                getGenderID.Parameters.AddWithValue("@genderCharacter", cbMakeProfileGender.SelectedItem.ToString());
                SqlDataReader idReader = getGenderID.ExecuteReader();
                while (idReader.Read())
                {
                    genderID = Convert.ToInt32(idReader[0]);
                }
                MainWindow.conn.Close();
            }
        }

        /// <summary>
        /// Gets the country name of a certain countryID.
        /// </summary>
        /// <param name="ID"></param>
        private void getCountryName(int ID)
        {
            string queryGetCountryName = "SELECT COUNTRY_NAME FROM Country WHERE id = (@countryID)";

            using (SqlCommand getCountryName = new SqlCommand(queryGetCountryName, MainWindow.conn))
            {
                MainWindow.conn.Open();
                getCountryName.Parameters.AddWithValue("countryID", ID);
                SqlDataReader countryNameReader = getCountryName.ExecuteReader();
                while (countryNameReader.Read())
                {
                    countryName = countryNameReader[0].ToString();
                }
                MainWindow.conn.Close();
            }
        }


        /// <summary>
        /// Getst the gender abbreviation of a certain genderID
        /// </summary>
        /// <param name="ID"></param>
        private void getGenderCharacter(int ID)
        {
            string querygetGenderCharacter = "SELECT Gender FROM Gender WHERE id = (@genderID)";

            using (SqlCommand getGenderCharacter = new SqlCommand(querygetGenderCharacter, MainWindow.conn))
            {
                MainWindow.conn.Open();
                getGenderCharacter.Parameters.AddWithValue("genderID", ID);
                SqlDataReader genderCharacterReader = getGenderCharacter.ExecuteReader();
                while (genderCharacterReader.Read())
                {
                    genderCharacter = genderCharacterReader[0].ToString();
                }
                MainWindow.conn.Close();
            }
        }


        /// <summary>
        /// Assigns the values if the filled in data to the a new player.
        /// </summary>
        private void assignPlayerValues()
        {
            p1 = new Player(
                tbMakeProfileUsername.Text,
                tbMakeProfilePassword.Password,
                tbMakeProfileFirstName.Text,
                tbMakeProfileSurname.Text,
                dpMakeProfileAge.SelectedDate.Value,
                genderCharacter,
                tbMakeProfilePhoneNumber.Text,
                tbMakeProfileEmail.Text,
                countryName
                );
        }


        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.wC.Exit();
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

  
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            this.Close();
            main.Show();
        }
    }
}
