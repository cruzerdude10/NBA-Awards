using System;
using System.IO;

namespace NBA_Awards
{
    public struct PlayerInfo
    {
        public string name;
        public string team;
        public bool rookie;
        public double rating;
        public int gamesPlayed;
        public double minutesPerGame;
        public double pointsPerGame;
        public double reboundsPerGame;
        public double assistsPerGame;
        public double shotPercentage;
        public double freethrowPercentage;
        public double pointsPerMinute;
        public double freethrowShotsPerShot;
        public double gameTime;
    }
   
    class Program
    {
        static void Main(string[] args)
        {
            string fileLocation = @"C:\Users\Alex Cruz\Desktop\progam text files\NBA_DATA.csv";
            PlayerInfo[] playerData = ReadPlayerInfo(fileLocation);

            string[] priusAndGasGuzzler = PointsPerMinute(playerData);
            string priusWinner = priusAndGasGuzzler[0];
            string gasGuzzlerWinner = priusAndGasGuzzler[1];
            Console.WriteLine($"Prius Award : {priusWinner}");
            Console.WriteLine($"Gas Guzzler Award : {gasGuzzlerWinner}");

            string foulTargetWinner = FoulTargetAward(playerData);
            Console.WriteLine($"Foul Target Award: {foulTargetWinner}");
            
            
            int[,] achieverWinners = AvgRating(playerData);
            for (int i = 0; i < achieverWinners.GetLength(0); i++)
            {
                string[] awardTitles = { "Overachiver Awards", "On the Fence Awards", "Underachiver Awards" };
                Console.WriteLine(awardTitles[i]);
                for (int index = 0; index < achieverWinners.GetLength(1); index++)
                {
                    Console.Write(playerData[achieverWinners[i,index]].name + "    ");
                }
                Console.WriteLine();
                Console.WriteLine();
            }


            string bangForYouBuckAward = RookieGameTime(playerData);
            Console.WriteLine("Bang For Your Buck Award: " + bangForYouBuckAward);

            string[] gordoGekko = regionPoints(playerData);
            Console.WriteLine($"Gordon Gekko Award: {gordoGekko[0]} region- {gordoGekko[1]:F2} pts");

            string[] gamesPlayedAward = GamesPlayedHiLo(playerData);
            string[] minsPGAward = MinPerGameHiLo(playerData);
            string[] ppgAward = PointsPerGameHiLo(playerData);
            string[] reboundsPG = ReboundsPerGameHiLo(playerData);
            string[] assistsPG = AssistsPerGameHiLo(playerData);
            string[] shotPerc = ShotPercentHiLo(playerData);
            string[] ftPerc = FreethrowPercentHiLo(playerData);
            Console.WriteLine("Charlie Brown Awards");
            Console.WriteLine($"Least games played: {gamesPlayedAward[1]}");
            Console.WriteLine($"Least mins per game: {minsPGAward[1]}");
            Console.WriteLine($"Least points per game: {ppgAward[1]}");
            Console.WriteLine($"Least rebounds per game: {reboundsPG[1]}");
            Console.WriteLine($"Least assists per game: {assistsPG[1]}");
            Console.WriteLine($"Lowest shot percentage: {shotPerc[1]}");
            Console.WriteLine($"Lowest freethrow percentage: {ftPerc[1]}");

            Console.WriteLine("Tiger Uppercut Awards");
            Console.WriteLine($"Most games played: {gamesPlayedAward[0]}");
            Console.WriteLine($"Most mins per game: {minsPGAward[0]}");
            Console.WriteLine($"Most points per game: {ppgAward[0]}");
            Console.WriteLine($"Most rebounds per game: {reboundsPG[0]}");
            Console.WriteLine($"Most assists per game: {assistsPG[0]}");
            Console.WriteLine($"Highest shot percentage: {shotPerc[0]}");
            Console.WriteLine($"Highest freethrow percentage: {ftPerc[0]}");
        }
        static PlayerInfo[] ReadPlayerInfo(string fileLocation)
        {
            StreamReader readFile = new(fileLocation);
            PlayerInfo[] players = new PlayerInfo[NumberOfPlayers(fileLocation)];
            readFile.ReadLine();
            for (int index = 0; index < players.Length; index++)
            {
                string playerRecord = readFile.ReadLine();
                string[] fieldsArray = playerRecord.Split(",");
                players[index].name                = fieldsArray[0];
                players[index].team                = fieldsArray[1];
                players[index].rookie              = isPlayerRookie(fieldsArray[2]);
                players[index].rating              = double.Parse(fieldsArray[3]);
                players[index].gamesPlayed         = int.Parse(fieldsArray[4]);
                players[index].minutesPerGame      = double.Parse(fieldsArray[5]);
                players[index].pointsPerGame       = double.Parse(fieldsArray[6]);
                players[index].reboundsPerGame     = double.Parse(fieldsArray[7]);
                players[index].assistsPerGame      = double.Parse(fieldsArray[8]);
                players[index].shotPercentage      = double.Parse(fieldsArray[9]);
                players[index].freethrowPercentage = double.Parse(fieldsArray[10]);

                players[index].pointsPerMinute = (players[index].pointsPerGame / players[index].minutesPerGame) * players[index].gamesPlayed;
                players[index].freethrowShotsPerShot = players[index].freethrowPercentage / players[index].shotPercentage;
                players[index].gameTime = players[index].gamesPlayed * players[index].minutesPerGame;
                
            }
            return players;
            
        }
        static bool isPlayerRookie(string rookieValue)
        {
            if (rookieValue == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static int NumberOfPlayers(string fileLocation)
        {
            StreamReader readfile = new(fileLocation);
            int playerCount = 0;
            readfile.ReadLine();
            while (!readfile.EndOfStream)
            {
                playerCount++;
                readfile.ReadLine();
            }
            readfile.Close();
            return playerCount;
        }
        static void displayArray(PlayerInfo[] playerData)
        {
            for (int i = 0; i < playerData.Length; i++)
            {
                Console.Write(playerData[i].name);
                Console.Write(playerData[i].team);
                Console.Write(playerData[i].rating);
                Console.Write(playerData[i].rookie);
                Console.Write(playerData[i].gamesPlayed);
                Console.Write(playerData[i].minutesPerGame);
                Console.Write(playerData[i].pointsPerGame);
                Console.Write(playerData[i].reboundsPerGame);
                Console.Write(playerData[i].assistsPerGame);
                Console.Write(playerData[i].shotPercentage);
                Console.Write(playerData[i].freethrowPercentage);

                Console.WriteLine();
            }
        } 

        //prius & gas guzzler
        static string[] PointsPerMinute(PlayerInfo[] players)
        {
            int greatestPlayer = 0;
            int leastPlayer = 0;
            for (int index = 0; index < players.Length; index++)
            {
                if (players[greatestPlayer].pointsPerGame < players[index].pointsPerGame)
                {
                    greatestPlayer = index;
                }
                if (players[leastPlayer].pointsPerGame > players[index].pointsPerGame)
                {
                    leastPlayer = index;
                }
            }
            string[] winners = { players[greatestPlayer].name, players[leastPlayer].name };
            return winners;
        }

        //Best and Worst of Each Field
        #region Best / Worst Catagories
        static string[] GamesPlayedHiLo(PlayerInfo[] players)
        {
            int highestPlayer = 0;
            int lowestPlayer = 0;
            for (int index = 0; index < players.Length; index++)
            {
                if (players[highestPlayer].gamesPlayed < players[index].gamesPlayed)
                {
                    highestPlayer = index;
                }
                if (players[lowestPlayer].gamesPlayed > players[index].gamesPlayed)
                {
                    lowestPlayer = index;
                }
            }
            string[] winners = { players[highestPlayer].name, players[lowestPlayer].name };
            return winners;
        }
        static string[] MinPerGameHiLo(PlayerInfo[] players)
        {
            int highestPlayer = 0;
            int lowestPlayer = 0;
            for (int index = 0; index < players.Length; index++)
            {
                if (players[highestPlayer].minutesPerGame < players[index].minutesPerGame)
                {
                    highestPlayer = index;
                }
                if (players[lowestPlayer].minutesPerGame > players[index].minutesPerGame)
                {
                    lowestPlayer = index;
                }
            }
            string[] winners = { players[highestPlayer].name, players[lowestPlayer].name };
            return winners;
        }
        static string[] PointsPerGameHiLo(PlayerInfo[] players)
        {
            int highestPlayer = 0;
            int lowestPlayer = 0;
            for (int index = 0; index < players.Length; index++)
            {
                if (players[highestPlayer].pointsPerGame < players[index].pointsPerGame)
                {
                    highestPlayer = index;
                }
                if (players[lowestPlayer].pointsPerGame > players[index].pointsPerGame)
                {
                    lowestPlayer = index;
                }
            }
            string[] winners = { players[highestPlayer].name, players[lowestPlayer].name };
            return winners;
        }
        static string[] ReboundsPerGameHiLo(PlayerInfo[] players)
        {
            int highestPlayer = 0;
            int lowestPlayer = 0;
            for (int index = 0; index < players.Length; index++)
            {
                if (players[highestPlayer].reboundsPerGame < players[index].reboundsPerGame)
                {
                    highestPlayer = index;
                }
                if (players[lowestPlayer].reboundsPerGame > players[index].reboundsPerGame)
                {
                    lowestPlayer = index;
                }
            }
            string[] winners = { players[highestPlayer].name, players[lowestPlayer].name };
            return winners;
        }
        static string[] AssistsPerGameHiLo(PlayerInfo[] players)
        {
            int highestPlayer = 0;
            int lowestPlayer = 0;
            for (int index = 0; index < players.Length; index++)
            {
                if (players[highestPlayer].assistsPerGame < players[index].assistsPerGame)
                {
                    highestPlayer = index;
                }
                if (players[lowestPlayer].assistsPerGame > players[index].assistsPerGame)
                {
                    lowestPlayer = index;
                }
            }
            string[] winners = { players[highestPlayer].name, players[lowestPlayer].name };
            return winners;
        }
        static string[] ShotPercentHiLo(PlayerInfo[] players)
        {
            int highestPlayer = 0;
            int lowestPlayer = 0;
            for (int index = 0; index < players.Length; index++)
            {
                if (players[highestPlayer].shotPercentage < players[index].shotPercentage)
                {
                    highestPlayer = index;
                }
                if (players[lowestPlayer].shotPercentage > players[index].shotPercentage)
                {
                    lowestPlayer = index;
                }
            }
            string[] winners = { players[highestPlayer].name, players[lowestPlayer].name };
            return winners;
        }
        static string[] FreethrowPercentHiLo(PlayerInfo[] players)
        {
            int highestPlayer = 0;
            int lowestPlayer = 0;
            for (int index = 0; index < players.Length; index++)
            {
                if (players[highestPlayer].freethrowPercentage < players[index].freethrowPercentage)
                {
                    highestPlayer = index;
                }
                if (players[lowestPlayer].freethrowPercentage > players[index].freethrowPercentage)
                {
                    lowestPlayer = index;
                }
            }
            string[] winners = { players[highestPlayer].name, players[lowestPlayer].name };
            return winners;
        }
        #endregion

        //Region with most points
        static string[] regionPoints(PlayerInfo[] players)
        { 
            string[] northEastern = { "BKN", "BOS", "CHI", "CLE", "DET", "IND", "NY", "PHI", "TOR" };
            string[] northWestern = { "MIL", "MIN", "POR", "UTA", "WAS" };
            string[] southEastern = { "ATL", "CHA", "MEM", "MIA", "NO", "ORL" };
            string[] southWestern = { "DAL", "DEN", "GS", "HOU", "LAC", "LAL", "SAC", "OKC", "PHO", "SA" };

            double NEPoints = 0;
            double NWPoints = 0;
            double SEPoints = 0;
            double SWPoints = 0;

            for (int i = 0; i < players.Length; i++)
            {
                string playerCity = players[i].team;
                double playerPoints = players[i].pointsPerGame * players[i].gamesPlayed;
                
                
                if (IsStringInArray(playerCity, northEastern))
                {
                    NEPoints += playerPoints;
                }
                else if (IsStringInArray(playerCity, northWestern))
                {
                    NWPoints += playerPoints;
                }
                else if (IsStringInArray(playerCity, southEastern))
                {
                    SEPoints += playerPoints;
                }
                else if (IsStringInArray(playerCity, southWestern))
                {
                    SWPoints += playerPoints;
                }
                else
                {
                    Console.WriteLine("Team Not Found");
                }
            }
            string[] winningRegion = new string[2];
            if (NEPoints > NWPoints && NEPoints > SEPoints && NEPoints > SWPoints)
            {
                winningRegion[0] = "North East";
                winningRegion[1] = NEPoints.ToString();
            }
            else if (NWPoints > NEPoints && NWPoints > SEPoints && NWPoints > SWPoints)
            {
                winningRegion[0] = "North West";
                winningRegion[1] = NWPoints.ToString();
            }
            else if (SEPoints > NEPoints && SEPoints > NWPoints && SEPoints > SWPoints)
            {
                winningRegion[0] = "South East";
                winningRegion[1] = SEPoints.ToString();
            }
            else if (SWPoints > NEPoints && SWPoints > NWPoints && SWPoints > SEPoints)
            {
                winningRegion[0] = "South West";
                winningRegion[1] = SWPoints.ToString();
            }

            return winningRegion;
        }
        static bool IsStringInArray(string city, string[] region)
        {
            for (int i = 0; i < region.Length; i++)
            {
                if (region[i] == city)
                {
                    return true;
                }
            }
            return false;
        }
        static bool IsIntInArray(int num, int[,] array)
        {
            foreach (var item in array)
            {
                if (item == num)
                {
                    return true;
                }
            }
            return false;
        }

        //Bang for Buck Player
        static string RookieGameTime(PlayerInfo[] players)
        {
            double highestRookieTime = 0;
            int rookie = -1;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].rookie)
                {
                    double playerGameTime = players[i].gameTime;
                    if (playerGameTime > highestRookieTime)
                    {
                        highestRookieTime = playerGameTime;
                        rookie = i;
                    }
                }
            }
            return players[rookie].name;
        }

        //Foul Target
        static string FoulTargetAward(PlayerInfo[] players)
        {
            double highest = 0;
            int playerID = -1;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].freethrowShotsPerShot > highest)
                {
                    highest = players[i].freethrowShotsPerShot;
                    playerID = i;
                }
            }
            return players[playerID].name;
        }
        static double SumRatings(PlayerInfo[] players)
        {
            double sumRatings = 0;
            for (int i = 0; i < players.Length; i++)
            {
                sumRatings += players[i].rating;
            }
            return sumRatings;
        }
        static int[,] AvgRating(PlayerInfo[] players)
        {
            double avg = SumRatings(players) / players.Length;
            int[,] winners = new int[3, 3];//0,0 best player, 1,0 mediumest player 2,0 worst Player
            for (int index = 0; index < winners.GetLength(0); index++)
            {
                double currentDiff = 0;
                int highest = 0;
                int lowest = 0;
                int median = 0;
                double tempDiff;

                for (int i = 1; i < players.Length; i++)
                {
                    if (!IsIntInArray(i, winners))
                    {
                        if (players[i].rating > players[highest].rating)
                        {
                            highest = i;
                        }
                        if (players[i].rating < players[lowest].rating)
                        {
                            lowest = i;
                        }
                        currentDiff = players[i].rating - avg;
                        tempDiff = players[median].rating - avg;

                        if (players[i].rating > avg)
                        {
                            if (tempDiff < 0)
                            {
                                tempDiff *= -1;
                            }
                            if (currentDiff < tempDiff)
                            {
                                median = i;
                            }
                        }
                        if (players[i].rating < avg)
                        {
                            if (tempDiff > 0)
                            {
                                tempDiff *= -1;
                            }
                            if (currentDiff > tempDiff)
                            {
                                median = i;
                            }
                        }
                    }//end if

                }//end for
                winners[0, index] = highest;
                winners[1, index] = median;
                winners[2, index] = lowest;
            }//end for

            return winners;
        }//end function
    }
}
