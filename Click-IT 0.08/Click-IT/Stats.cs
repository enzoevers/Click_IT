using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;

namespace Click_IT
{
    public class Stats
    {
        // Fields

        public decimal averageReactionTime;
        public decimal averageGameReactionTime;
        public decimal bestTime;
        private PlayerStats ps;
        private List<PlayerStats> playerStatList = new List<PlayerStats>();
        private Player p = new Player();
        private GameData gd = new GameData();



        // Property

        /// <summary>
        /// A list with stats from all players.
        /// </summary>
        public List<PlayerStats> PlayerStatList
        {
            get
            {
                return this.playerStatList;
            }
        }



        // Methods

        /// <summary>
        /// Selects the best reaction time of a player.
        /// </summary>
        /// <param name="id"></param>
        public void getPersonalBestReactionTime(int id)
        {
            string queryGetPersonalBestReactionTime = "SELECT " + 
                                                        "MIN(Reaction_time) " +
                                                      "FROM " +
                                                        "GameData " +
                                                      "WHERE " +
                                                        "UserID = (@userID)";

            using(SqlCommand bestReactionTime = new SqlCommand(queryGetPersonalBestReactionTime, MainWindow.conn))
            {
                MainWindow.conn.Open();
                bestReactionTime.Parameters.AddWithValue("@userID", id);
                SqlDataReader bestTimeReader = bestReactionTime.ExecuteReader();
                while (bestTimeReader.Read())
                {
                    try
                    {
                        bestTime = Convert.ToDecimal(bestTimeReader[0]);
                    }
                    catch (Exception)
                    {
                        bestTime = 0;
                    }
                    
                }
                MainWindow.conn.Close();
            }
        }

        /// <summary>
        /// Gets all data needed from the database to make a new object of the PlayerStats clas.
        /// </summary>
        /// <param name="connection"></param>
        public void getStatsFromPlayers(SqlConnection connection)
        {
            playerStatList.Clear();

            string queryGePlayerStats = "SELECT " +
                                            "p.Username, " +
                                            "p.birthday, " +
                                            "g.Gender, " +
                                            "(SELECT " +
                                                "MAX(gd.GameID) " +
                                            "FROM " +
                                                 "GameData gd " +
                                            "WHERE " +
                                                "p.PlayerID = gd.UserID " +
                                            "HAVING " +
                                                "MAX(gd.Reaction_time) > 0), " +

                                            "(SELECT " +
                                                "MIN(gd.Reaction_time) " +
                                            "FROM " +
                                                 "GameData gd " +
                                            "WHERE " +
                                                "p.PlayerID = gd.UserID " +
                                            "HAVING " +
                                                "MIN(gd.Reaction_time) > 0), " +

                                            "(SELECT " +
                                                "AVG(gd.Reaction_time) " +
                                            "FROM " +
                                                 "GameData gd " +
                                            "WHERE " +
                                                "p.PlayerID = gd.UserID " +
                                            "HAVING " +
                                                "AVG(gd.Reaction_time) > 0) " +
                                        "FROM " +
                                            "Player p " +
                                        "INNER JOIN " +
                                            "Gender g ON g.Id = p.Gender_id " +
                                        "WHERE " +
                                             "(SELECT " +
                                                "MAX(gd.GameID) " +
                                            "FROM " +
                                                 "GameData gd " +
                                            "WHERE " +
                                                "p.PlayerID = gd.UserID) > 0 " +
                                       "ORDER BY " +
                                            "(SELECT " +
                                                "MIN(gd.Reaction_time) " +
                                            "FROM " +
                                                "GameData gd " +
                                            "WHERE " +
                                                "p.PlayerID = gd.UserID) ASC";

            using (SqlCommand makePlayerObject = new SqlCommand(queryGePlayerStats, connection))
            {
                connection.Open();
                SqlDataReader playerStatsInfoReader = makePlayerObject.ExecuteReader();
                while (playerStatsInfoReader.Read())
                {
                    // Makes a new PlayerStats object using the constructor.
                    ps = new PlayerStats(
                        playerStatsInfoReader[0].ToString(),
                        p.calculateAge(Convert.ToDateTime(playerStatsInfoReader[1])),
                        playerStatsInfoReader[2].ToString(),
                        Convert.ToInt32(playerStatsInfoReader[3]),
                        Convert.ToDecimal(playerStatsInfoReader[4]),
                        Convert.ToDecimal(playerStatsInfoReader[5])
                        );

                    playerStatList.Add(ps); 
                }
                connection.Close();
            }
        }
    }
}
