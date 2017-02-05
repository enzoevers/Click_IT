using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Data.SqlClient;

namespace Click_IT
{
    public class Mail
    {
        // fields

        MailMessage mailToSend = new MailMessage();
        SmtpClient client = new SmtpClient();



        //propertys

        // contructor
        /// <summary>
        /// Setup the smtp client.
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <param name="MailFrom"></param>
        /// <param name="Port"></param>
        /// <param name="Host"></param>
        public Mail(string Username, string Password, string MailFrom, int Port, string Host)
        {
            ///Information about gmail smtp server.
            ///https://support.google.com/a/answer/176600?hl=nl
            ///
            client.Port = Port;
            client.Host = Host;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(Username, Password);
            mailToSend.From = new MailAddress(MailFrom);
        }



        // Methods     
           
        /// <summary>
        /// Setup to send a new mail.
        /// </summary>
        /// <param name="mailTo"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public void sendNewMail(string mailTo, string subject, string body)
        {
            mailToSend.To.Add(new MailAddress(mailTo));
            mailToSend.Subject = subject;
            mailToSend.Body = string.Format(body);
            client.Send(mailToSend);
        }

        /// <summary>
        /// General info messages.
        /// </summary>
        public void showInfo()
        {
            MessageBox.Show("An email will be send to the mailaddress" + "\n" + "bound to your account." + "\n" + "\n" +
                            "Enter the username of your account" + "\n" + "and the press send.", "Password recovery");
        }

        /// <summary>
        /// Make an email to recover a profiles password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="conn"></param>
        public void newPasswordMail(string username, SqlConnection conn)
        {
            string usernameRecovery = username;
            string mailToAddress = "";
            string password = "";

            if (!String.IsNullOrEmpty(usernameRecovery))
            {
                string queryGetMailAndPassword = "SELECT " +
                                                    "Email, " +
                                                    "Password " +
                                                 "FROM " +
                                                    "Player " +
                                                 "WHERE " +
                                                    "Username = (@username)";

                using (SqlCommand getMailAndPassword = new SqlCommand(queryGetMailAndPassword, conn))
                {
                    conn.Open();
                    getMailAndPassword.Parameters.AddWithValue("@username", usernameRecovery);
                    SqlDataReader emailAndPasswordReader = getMailAndPassword.ExecuteReader();
                    while (emailAndPasswordReader.Read())
                    {
                        mailToAddress = emailAndPasswordReader[0].ToString();
                        password = emailAndPasswordReader[1].ToString();
                    }
                    conn.Close();
                }
                string body = string.Format("Your password is: {0}.", password);
                try
                {
                    sendNewMail(mailToAddress, "Password recover", body);
                    MessageBox.Show("If you don't see the mail in your regulair email adress," + "\n" + "it probably is in your spam folder.", "Succes!");
                }
                catch (Exception)
                {
                    MessageBox.Show("Something went wrong." + "\n" + "Please try again", "Error");
                }
            }
            else if (String.IsNullOrEmpty(usernameRecovery))
            {
                MessageBox.Show("Please enter a username", "Username missing");
            }
        }


        /// <summary>
        /// Send a validation mail to determine if the email address is valid.
        /// </summary>
        /// <param name="FirstName"></param>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <param name="mailTo"></param>
        /// <returns></returns>
        public bool newValidationMail(string FirstName, string Username, string Password, string mailTo)
        {
            try
            {
                string body = string.Format("Welcome {0} to Click_IT. From now on you can log is with your username \"{1}\" and password \"{2}\".", FirstName, Username, Password);

                MainWindow.mail.sendNewMail(mailTo, "Confirmation", body);
                MessageBox.Show("The confirmation mail send correctly", "Welcome");

                return true;
            }
            catch (FormatException fe)
            {
                if (!String.IsNullOrEmpty(mailTo))
                {
                    MessageBox.Show(fe.ToString());
                }

                return false;
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong." + "\n" + "Please try again.", "Error");

                return false;
            }
        }
    }
}
