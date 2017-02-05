using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows;
using System.Media;
using System.Windows.Media;


namespace Click_IT
{
    class PlayGame
    {
        // Fields

        SoundPlayer coinSound = new SoundPlayer(Properties.Resources.Coin);
        SoundPlayer endSound = new SoundPlayer(Properties.Resources.Robot_end);

        GameSetUp gameSetUp = new GameSetUp();
        GameData GameDataInstance = new GameData();

        Stopwatch reactionTimer = new Stopwatch(); 
        TimeSpan roundTime;                       
        List<TimeSpan> reactionTimeList = new List<TimeSpan>();        

        Random randColor = new Random(); 
        int matrixIndex = 0;             //Hold the int that came out of randColor.

        int roundsPlayed = 0;
        private int amountOfRounds = 0;
        int roundsLeft;

        private bool gameEnded = false;

        private decimal averageTime;
        private decimal bestTime;
        private int gameID;



        // Properties

        public bool GameEnded
        {
            get
            {
                return this.gameEnded;
            }
            set
            {
                this.gameEnded = value;
            }
        }

        public int MatrixIndex
        {
            get
            {
                return this.matrixIndex;
            }
        }

        public int AmountOfRounds
        {
            set
            {
                this.amountOfRounds = value;
            }
        }

        public int RoundsLeft
        {
            get
            {
                return this.roundsLeft;
            }
        }

        public decimal AverageTime
        {
            get
            {
                return this.averageTime;
            }
        }

        public decimal BestTime
        {
            get
            {
                return this.bestTime;
            }
        }

        public int GameID
        {
            set
            {
                this.gameID = value;
            }
        }

        public Button RandomChosenButton
        {
            get
            {
                return this.gameSetUp.Matrix[matrixIndex];
            }
        }


        // Constructor

        public PlayGame(List<Button> ButtonList, List<string> ButtonAsStringList)
        {
            GameDataInstance.selectUserID(MainWindow.username);
            gameSetUp.Matrix = ButtonList;
            gameSetUp.MatrixString = ButtonAsStringList;
        }



        // Methods 

        /// <summary>
        /// Gets a integer between 0 and 63. This number refers to a button. The button gets a black background.
        /// </summary>
        /// <returns></returns>
        private int randomButton()
        {
            matrixIndex = randColor.Next(0, 64); // Generates a number between 0 and 63 (so 64 numbers a available. There are 64 button in the gameGrid).
            gameSetUp.Matrix[matrixIndex].Background = Brushes.Black;
            roundsLeft = amountOfRounds - roundsPlayed;

            ///
            /// Trying to give the current 'button to click' an pop-up effect
            /// 
            /*Point coordinat = matrix[matrixIndex].TransformToAncestor(this).Transform(new Point(0, 0));
            double y = Convert.Todouble(coordinat.Y + 10);

            DoubleAnimation da = new DoubleAnimation();
            da.From = Canvas.GetTop(matrix[matrixIndex]);
            da.To = Canvas.SetTop(matrix[matrixIndex], y);
            da.Duration = new Duration(TimeSpan.FromMilliseconds(50));*/

            return matrixIndex;
        }

        /// <summary>
        /// Writes the time current reaction time to a list and best reaction and average time of the current game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void getButtonText(object sender, EventArgs e)
        {
            string buttonName = (sender as Button).Name.ToString(); 

            if (buttonName == gameSetUp.MatrixString[matrixIndex]) 
            {
                reactionTimer.Stop();              
                roundTime = reactionTimer.Elapsed; 
                GameDataInstance.Times.Add(Convert.ToDecimal(roundTime.TotalMilliseconds));
                averageTime = GameDataInstance.Times.Average();

                roundsPlayed++;

                var converter = new BrushConverter();
                gameSetUp.Matrix[matrixIndex].Background = (Brush)converter.ConvertFromString("#506070");

                GameDataInstance.getCurrentBestReactionTime();      

                bestTime = GameDataInstance.BestCurrentReactionTime;                  

                if (roundsPlayed == amountOfRounds)
                {
                    //coinSound.PlaySync();
                    endSound.Play();
                    gameEnded = true;
                    GameDataInstance.insertReactionTime(gameID, DateTime.Now);
                    GameDataInstance.Times.Clear();
                    
                    roundsPlayed = 0;
                }
                else if (roundsPlayed < amountOfRounds)
                {
                    //coinSound.Play();         
                    gameEnded = false;
                    playGame();
                }
            }
        }

       /// <summary>
       /// Resets the timer and gets a random button.
       /// </summary>
        public void playGame()
        {
            reactionTimer.Reset();
            randomButton();        
            reactionTimer.Start(); 
        }
    }
}
