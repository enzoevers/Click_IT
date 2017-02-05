using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace Click_IT
{
    public class GameData
    {
        int validUserID;
        decimal bestCurrentReactionTime;
        List<decimal> times = new List<decimal>();

        public decimal BestCurrentReactionTime
        {
            get
            {
                return this.bestCurrentReactionTime;
            }
        }

        public int ValidUserID
        {
            get
            {
                return this.validUserID;
            }
        }

        /// <summary>
        /// Add a reaction time to the 'times' list.
        /// </summary>
        public List<decimal> Times
        {
            get
            {
                return this.times;
            }
            set
            {
                this.times.Add(Convert.ToDecimal(value));
            }
        }


        /// <summary>
        /// Looks for the lower value in the 'times' list.
        /// </summary>
        public void getCurrentBestReactionTime()
        {
            bestCurrentReactionTime = times.Min();
        }

        /// <summary>
        /// Selects the ID of the logged in player.
        /// </summary>
        /// <param name="username"></param>
        public void selectUserID(string username)
        {
            string queryGetUserID = "SELECT PlayerID FROM Player WHERE Username = (@Username)";

            using (SqlCommand getUserID = new SqlCommand(queryGetUserID, MainWindow.conn))
            {
                MainWindow.conn.Open();
                getUserID.Parameters.AddWithValue("@Username", username);
                SqlDataReader reader = getUserID.ExecuteReader();
                while (reader.Read())
                {
                    validUserID = Convert.ToInt32(reader[0]);
                }
                MainWindow.conn.Close();
            } 
        }

        /// <summary>
        /// Inserts every reaction in the 'times' list into the database. 
        /// </summary>
        /// <param name="gameID"></param>
        /// <param name="date"></param>
        public void insertReactionTime(int gameID, DateTime date)
        {   
            string queryWriteTime = "INSERT INTO GameData (GameID, Date, UserID, Reaction_time) VALUES (@GameID, @date, @userID, @Reaction_time)";

            foreach (decimal dec in times)
            {
                using (SqlCommand writeTime = new SqlCommand(queryWriteTime, MainWindow.conn))
                {
                    MainWindow.conn.Open();
                    writeTime.Parameters.AddWithValue("@GameID", gameID);
                    writeTime.Parameters.AddWithValue("@date", date);
                    writeTime.Parameters.AddWithValue("@userID", validUserID);
                    writeTime.Parameters.AddWithValue("@Reaction_time", dec);
                    writeTime.ExecuteNonQuery();
                    MainWindow.conn.Close();
                }
            }       
        }

        /// <summary>
        /// Selects the highest GameID (last game) and adds one to that.
        /// This increased GameID is later used to bind the reaction times to te right game in the database. 
        /// </summary>
        public int getGameID()
        {
            int gameID = 0;
            string queryGetGameID = "SELECT " +
                                        "MAX(gd.GameID) " +
                                    "FROM " +
                                        "GameData gd " +
                                    "WHERE " +
                                        "gd.UserID = (@userID) " +
                                    "HAVING " +
                                        "MAX(gd.Reaction_time) > 0";

            using (SqlCommand getGameID = new SqlCommand(queryGetGameID, MainWindow.conn))
            {
                MainWindow.conn.Open();
                getGameID.Parameters.AddWithValue("@userID", validUserID);
                SqlDataReader readGameID = getGameID.ExecuteReader();
                while (readGameID.Read())
                {
                    gameID = Convert.ToInt32(readGameID[0]);                   
                }
                MainWindow.conn.Close();
            }

            return gameID += 1;
        }
    }
}
