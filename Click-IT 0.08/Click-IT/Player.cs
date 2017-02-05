using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Data.SqlClient;
using System.Windows;

namespace Click_IT
{
    public class Player
    {
        // Fields

        private string username;
        private string password;
        private string firstName;
        private string surname;
        private string gender;
        private DateTime birthday;
        private string email;
        private string phoneNumber;
        private string country;
        private int playerAge;



        //Constructors

        /// <summary>
        /// Makes a new player.
        /// </summary>
        /// <param name="usernamePlayer"></param>
        /// <param name="passwordPlayer"></param>
        /// <param name="firstnamePlayer"></param>
        /// <param name="surnamePlayer"></param>
        /// <param name="birtdayPlayer"></param>
        /// <param name="genderPlayer"></param>
        /// <param name="phoneNumberPlayer"></param>
        /// <param name="emailPlayer"></param>
        /// <param name="countryPlayer"></param>
        public Player(string usernamePlayer, string passwordPlayer, string firstnamePlayer, string surnamePlayer, DateTime birtdayPlayer, string genderPlayer, string phoneNumberPlayer, string emailPlayer, string countryPlayer)
        {
            username = usernamePlayer;
            password = passwordPlayer;
            firstName = firstnamePlayer;
            surname = surnamePlayer;
            gender = genderPlayer;
            birthday = birtdayPlayer;
            email = emailPlayer;
            phoneNumber = phoneNumberPlayer;
            country = countryPlayer;
        }

        public Player()
        {

        }



        // Methods 

        /// <summary>
        /// Calculates the players age bases on the birthday in the database.
        /// </summary>
        /// <param name="BirthDay"></param>
        /// <returns></returns>
        public int calculateAge(DateTime BirthDay)
        {
            playerAge = (int)DateTime.Now.Subtract(BirthDay).TotalDays / 365;
            return playerAge;
        }

        /// <summary>
        /// Update the players profile. The existing row will be affected.
        /// </summary>
        /// <param name="UserID"></param>
        public void UpdateProfile(int UserID)
        {
            string queryUpdatePlayerPorifile = "UPDATE Player " +
                                              "SET Username = (@username), " +
                                              "Password = (@password), " +
                                              "First_name = (@first_name), " +
                                              "Surname = (@surname), " +
                                              "Birthday = (@birthday), " +
                                              "Gender_id = (@genderID), " +
                                              "Phone = (@phone), " +
                                              "Email = (@email), " +
                                              "Country_id = (@countryID) " +
                                              "WHERE PlayerID = (@id)";

            using (SqlCommand updatePlayerPorifile = new SqlCommand(queryUpdatePlayerPorifile, MainWindow.conn))
            {
                MainWindow.conn.Open();
                updatePlayerPorifile.Parameters.AddWithValue("@username", username);
                updatePlayerPorifile.Parameters.AddWithValue("@password", password);
                updatePlayerPorifile.Parameters.AddWithValue("@first_name", firstName);
                updatePlayerPorifile.Parameters.AddWithValue("@surname", surname);
                updatePlayerPorifile.Parameters.AddWithValue("@birthday", birthday);
                updatePlayerPorifile.Parameters.AddWithValue("@genderID", MakeProfile.genderID);
                updatePlayerPorifile.Parameters.AddWithValue("@phone", phoneNumber);
                updatePlayerPorifile.Parameters.AddWithValue("@email", email);
                updatePlayerPorifile.Parameters.AddWithValue("@countryID", MakeProfile.countryID);
                updatePlayerPorifile.Parameters.AddWithValue("@id", UserID);

                updatePlayerPorifile.ExecuteNonQuery();

                MainWindow.conn.Close();
            }
        }

        /// <summary>
        /// Makes new players in the database.
        /// </summary>
        public void insertProfileToDatabase()
        {
            string queryMakeProfile = "INSERT INTO Player (Username, Password, First_name, Surname, Birthday, Gender_id, Phone, Email, Country_id) VALUES (@Username, @Password, @First_name, @Surname, @Birthday, @GenderID, @Phone, @Email, @CountryID)";

            using (SqlCommand insertProfile = new SqlCommand(queryMakeProfile, MainWindow.conn))
            {
                MainWindow.conn.Open();
                insertProfile.Parameters.AddWithValue("@Username", username);
                insertProfile.Parameters.AddWithValue("@Password", password);
                insertProfile.Parameters.AddWithValue("@First_name", firstName);
                insertProfile.Parameters.AddWithValue("@Surname", surname);
                insertProfile.Parameters.AddWithValue("@Birthday", birthday);
                insertProfile.Parameters.AddWithValue("@GenderID", MakeProfile.genderID);
                insertProfile.Parameters.AddWithValue("@Phone", phoneNumber);
                insertProfile.Parameters.AddWithValue("@Email", email);
                insertProfile.Parameters.AddWithValue("@CountryID", MakeProfile.countryID);

                insertProfile.ExecuteNonQuery();

                MainWindow.conn.Close();
            }
        }
    }
}
