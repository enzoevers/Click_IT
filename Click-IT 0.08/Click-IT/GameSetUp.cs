using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows.Media;

namespace Click_IT
{
    class GameSetUp
    {
        // Fields 

        private int userID; 

        private List<string> matrixString = new List<string>();
        string column = "";        
        string row = "";          
        private List<Button> matrix = new List<Button>();   

        public GameData GameDataInstance = new GameData(); 

        Stats playerStats = new Stats(); 

        private List<PlayerStats> usernameAndBestTime = new List<PlayerStats>(); 
        private decimal bestAlloverTime;



        // Properties

        public int UserID
        {
            get
            {
                return this.userID;
            }
        }

        public List<string> MatrixString
        {
            get
            {
                return this.matrixString;
            }
            set
            {
                this.matrixString = value;
            }
        }

        public List<Button> Matrix
        {
            get
            {
                return this.matrix;
            }
            set
            {
                this.matrix = value;
            }
        }

        public List<PlayerStats> UsernameAndBestTime
        {
            get
            {
                return this.usernameAndBestTime;
            }
        }

        public decimal BestAlloverTime
        {
            get
            {
                return this.bestAlloverTime;
            }
            set
            {
                this.bestAlloverTime = value;
            }
        }


        
        // Methods

        /// <summary>
        /// Gets the username and best reaction time from the database of all 
        /// </summary>
        public void fillScoreboardInGame()
        {
            GameDataInstance.selectUserID(MainWindow.username);
            userID = GameDataInstance.ValidUserID;              

            //addButton();           
            addButtonAsString();   
            //addButtonClickEvent();  
            setButtonColor();      


            playerStats.getPersonalBestReactionTime(userID);                       
            bestAlloverTime = playerStats.bestTime;


            string queryGetUsernameAndBestTime = "SELECT p.Username, min(gd.Reaction_time) " +
                                                 "FROM GameData gd " +
                                                 "INNER JOIN Player p " +
                                                 "ON gd.UserID = p.PlayerID " +                              
                                                 "GROUP BY p.Username " +
                                                 "ORDER BY min(gd.Reaction_time)";             
            using (SqlCommand GetUsernameAndBestTime = new SqlCommand(queryGetUsernameAndBestTime, MainWindow.conn))
            {
                MainWindow.conn.Open();
                SqlDataReader usernameAndTimeReader = GetUsernameAndBestTime.ExecuteReader();
                while (usernameAndTimeReader.Read())
                {
                    usernameAndBestTime.Add(
                        new PlayerStats(
                        usernameAndTimeReader[0].ToString(),
                        Convert.ToInt32(null),
                        null,
                        Convert.ToInt32(null),
                        Convert.ToDecimal(usernameAndTimeReader[1]),
                        Convert.ToDecimal(null)
                        )
                    );
                }
                MainWindow.conn.Close();
            }
        }

        /// <summary>
        /// Adds all buttons ass string to the List<string> matrixString.
        /// </summary>
        private void addButtonAsString()
        {
            for (int c = 1; c < 9; c++)
            {
                column = c.ToString();
                for (int r = 1; r < 9; r++)
                {
                    row = r.ToString();
                    matrixString.Add("btn" + column + row);
                }
            }
        }


        /// <summary>
        /// Gives all buttons a backcolor with RGB values (R)50 (G)60 (B)70
        /// </summary>
        private void setButtonColor()
        {
            var converter = new BrushConverter();
            foreach (Button btn in matrix)
            {
                btn.Background = (Brush)converter.ConvertFromString("#506070");
            }
        }
    }
}
