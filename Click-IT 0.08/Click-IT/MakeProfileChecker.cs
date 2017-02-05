using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace Click_IT
{
    class MakeProfileChecker
    {
        // Fields

        MakeProfile mp;
        int countUsername;
        int countPassword;


        // Constructor

        /// <summary>
        /// Gets if the chosen username and password exists fi the database.
        /// </summary>
        public MakeProfileChecker(MakeProfile makePlayerClass)
        {
            mp = makePlayerClass;
            countUsernameAndPassword();
        }



        // Methods 

        /// <summary>
        /// Gets the amount of usernames and passwords the from the database that are the samen as the filled in username and password.
        /// </summary>
        private void countUsernameAndPassword()
        {
            string queryCheckUsername = "SELECT COUNT(*) FROM Player WHERE Username LIKE @Username";
            string queryCheckPassword = "SELECT COUNT(*) FROM Player WHERE Password LIKE @Password";

            using (SqlCommand checkUsername = new SqlCommand(queryCheckUsername, MainWindow.conn))
            {
                MainWindow.conn.Open();
                checkUsername.Parameters.AddWithValue("@Username", mp.tbMakeProfileUsername.Text);
                countUsername = (int)checkUsername.ExecuteScalar(); // Gives countUsename the amount if string found 
                                                                    // in column "Username" form the Players table equal to the given username.
                                                                    // This is used in the sendResult method.
                MainWindow.conn.Close();
            }

            using (SqlCommand checkPassword = new SqlCommand(queryCheckPassword, MainWindow.conn))
            {
                MainWindow.conn.Open();
                checkPassword.Parameters.AddWithValue("@Password", mp.tbMakeProfilePassword.Password);
                countPassword = (int)checkPassword.ExecuteScalar(); // Gives countPassword the amount if string found 
                                                                    // in column "Password" form the Players table equal to the given password.
                                                                    // This is used in the sendResult method.
                MainWindow.conn.Close();
            }
            sendResult();
        }


        /// <summary>
        /// Adds errors if needed and sends a validation mail.
        /// </summary>
        private void sendResult()
        {
            string validationError = "";
            string emptyTextBoxError = "";


            // Fill error string for existing login data.
            if (countUsername == 1)
            {
                validationError += "The given username already exists." + "\n";
            }

            if (countPassword == 1)
            {
                validationError += "The given password already exists." + "\n";
            }

            // Fill error string for empty textboxes.
            if (mp.tbMakeProfileUsername.Text == "")
            {
                emptyTextBoxError += "Please enter an username." + "\n";
                mp.tbMakeProfileUsername.BorderBrush = Brushes.Red;
            }

            if (mp.tbMakeProfilePassword.Password == "")
            {
                emptyTextBoxError += "Please enter a password." + "\n";
                mp.tbMakeProfilePassword.BorderBrush = Brushes.Red;
            }

            if (mp.tbMakeProfileFirstName.Text == "")
            {
                emptyTextBoxError += "Please enter a fist name." + "\n";
                mp.tbMakeProfileFirstName.BorderBrush = Brushes.Red;
            }

            if (mp.tbMakeProfileSurname.Text == "")
            {
                emptyTextBoxError += "Please enter a surname." + "\n";
                mp.tbMakeProfileSurname.BorderBrush = Brushes.Red;
            }

            if (mp.dpMakeProfileAge.SelectedDate == null)
            {
                emptyTextBoxError += "Please enter a birthday." + "\n";
                mp.dpMakeProfileAge.BorderBrush = Brushes.Red;
            }

            if (mp.cbMakeProfileGender.SelectedIndex == 0)
            {
                emptyTextBoxError += "Please enter a gender." + "\n";
                mp.cbMakeProfileGender.BorderBrush = Brushes.Red;
            }

            if (mp.tbMakeProfilePhoneNumber.Text == "")
            {
                emptyTextBoxError += "Please enter a phone number." + "\n";
                mp.tbMakeProfilePhoneNumber.BorderBrush = Brushes.Red;
            }

            if (mp.cbMakeProfileCountry.SelectedIndex == 0)
            {
                emptyTextBoxError += "Please enter a country." + "\n";
                mp.cbMakeProfileCountry.BorderBrush = Brushes.Red;
            }

            if (String.IsNullOrEmpty(mp.tbMakeProfileEmail.Text))
            {
                emptyTextBoxError += "Please enter an email address." + "\n";
                mp.tbMakeProfileEmail.BorderBrush = Brushes.Red;
            }

            // Show the emptyTextBoxError if not empty.
            if (emptyTextBoxError != "")
            {
                MessageBox.Show(emptyTextBoxError);
            }

            if (countUsername == 1 || countPassword == 1)
            {
                MessageBox.Show(validationError, "Errors");
            }

            try
            {
                if (countUsername == 0 && countPassword == 0 &&
                    mp.tbMakeProfileUsername.Text != "" &&
                    mp.tbMakeProfilePassword.Password != "" &&
                    mp.tbMakeProfileFirstName.Text != "" &&
                    mp.tbMakeProfileSurname.Text != "" &&
                    mp.dpMakeProfileAge.SelectedDate.Value != null &&
                    mp.cbMakeProfileGender.SelectedItem.ToString() != "Select gender..." &&
                    mp.tbMakeProfilePhoneNumber.Text != "" &&
                    mp.tbMakeProfileEmail.Text != "" &&
                    mp.cbMakeProfileCountry.SelectedItem.ToString() != "Choose your country...")
                {
                    bool emailIsValid = MainWindow.mail.newValidationMail(mp.tbMakeProfileFirstName.Text, mp.tbMakeProfileUsername.Text, mp.tbMakeProfilePassword.Password, mp.tbMakeProfileEmail.Text);

                    if (!emailIsValid)
                    {
                        emptyTextBoxError += "Please enter a valid email." + "\n";
                        mp.tbMakeProfileEmail.BorderBrush = Brushes.Red;
                    }
                    else if (emailIsValid)
                    {
                        mp.proceed = true;
                    }
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Please enter a valid birthday.");
            }
            catch (Exception)
            {
                MessageBox.Show("Sorry, it seems something went wrong." + "\n" + "Please try again.");
            }
        }
    }
}
