using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Click_IT
{
    public class PlayerStats
    {
        // Fields

        public string username { get; set; }
        public int age { get; set; }
        public string gender { get; set; }
        public int timesPlayed { get; set; }
        public decimal bestTime { get; set; }
        public decimal averageTime { get; set; }



        // Constructor

        /// <summary>
        /// Makes a new 'list' of stats for a player. 
        /// </summary>
        /// <param name="usernamePlayer"></param>
        /// <param name="agePlayer"></param>
        /// <param name="genderPlayer"></param>
        /// <param name="timesPlayedPlayer"></param>
        /// <param name="bestTimePlayer"></param>
        /// <param name="averageTimePlayer"></param>
        public PlayerStats(string usernamePlayer, int agePlayer, string genderPlayer, int timesPlayedPlayer, decimal bestTimePlayer, decimal averageTimePlayer)
        {
            username = usernamePlayer;
            age = agePlayer;
            gender = genderPlayer;
            timesPlayed = timesPlayedPlayer;
            bestTime = bestTimePlayer;
            averageTime = averageTimePlayer;
        }



        // Methods

        /// <summary>
        /// Return the 'list' of stats for a player.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}, {1}, {2}, {3}, {4}, {5}", username, age, gender, timesPlayed, bestTime, averageTime);

            return sb.ToString();
        }
    }
}
