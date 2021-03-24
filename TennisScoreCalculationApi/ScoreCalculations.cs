using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TennisScoreCalculationApi.Model;

namespace TennisScoreCalculationApi
{
    public class ScoreCalculations
    {
        #region data members
        // Normaly, I would have created those paths in a settings file, or app settings,
        // or as a setting in a DB table. This is the wrong place for those paths and the
        // wrong way to define them, but it is only for this project.
        static string LocalPath = AppDomain.CurrentDomain.BaseDirectory;
        static string DbDirectory = $"{LocalPath}DB";
        #endregion

        #region public methods
        public static async Task<string> GetGameByID(int ID)
        {
            string gameData = string.Empty;

            await Task.Run(() =>
            {
                try
                {
                    string fullPath = $"{DbDirectory}\\{ID}.txt";

                    if (!Directory.Exists(DbDirectory))
                        Directory.CreateDirectory(DbDirectory);

                    // Return empty string if game file is not exist
                    gameData = File.Exists(fullPath) ? File.ReadAllText(fullPath) : string.Empty;
                }
                catch
                {
                    gameData = string.Empty;
                }
            });

            return gameData;
        }

        public static async Task<string> CreateNemGame()
        {
            string gameData = string.Empty;

            await Task.Run(() =>
            {
                try
                {
                    if (!Directory.Exists(DbDirectory))
                        Directory.CreateDirectory(DbDirectory);

                    int idIndex = 0;

                    // Get the highest index of DB file to increase it with 1 and create a new game file
                    FileInfo[] allDbFiles = new DirectoryInfo(DbDirectory).GetFiles();
                    foreach (var file in allDbFiles)
                    {
                        int currentFileIndex = int.Parse(file.Name.Replace(".txt", ""));
                        if (currentFileIndex > idIndex)
                            idIndex = currentFileIndex;
                    }
                    // Create a new empty game object with the new index of file
                    SingleMatch match = new SingleMatch()
                    {
                        MatchID = ++idIndex,
                        NamePlayerA = "Player A",
                        NamePlayerB = "Player B",
                        ScorePlayerA = 0,
                        ScorePlayerB = 0,
                        AdvantageToPlayer = ESidesInGame.None,
                        WinnerInMatch = ESidesInGame.None,
                        Set1 = new SingleSetInMatch()
                        {
                            ScorePlayerA = 0,
                            ScorePlayerB = 0,
                            WinnerInSet = ESidesInGame.None
                        },
                        Set2 = new SingleSetInMatch()
                        {
                            ScorePlayerA = 0,
                            ScorePlayerB = 0,
                            WinnerInSet = ESidesInGame.None
                        },
                        Set3 = new SingleSetInMatch()
                        {
                            ScorePlayerA = 0,
                            ScorePlayerB = 0,
                            WinnerInSet = ESidesInGame.None
                        }
                    };
                    gameData = Newtonsoft.Json.JsonConvert.SerializeObject(match);
                    File.WriteAllText($"{DbDirectory}\\{idIndex}.txt", gameData); // Write the data of the game to the game file
                }
                catch
                {
                    gameData = string.Empty;
                }
            });
            
            return gameData;
        }

        // Increase 1 point to the player sent, calculate the match new state, save and return it
        public static async Task<string> PlayerWinningPoint(int gameId, ESidesInGame playerWinningPoint)
        {
            string gameData = string.Empty;

            try
            {
                gameData = await GetGameByID(gameId);
                var gameObject = Newtonsoft.Json.JsonConvert.DeserializeObject<SingleMatch>(gameData);
                switch (playerWinningPoint)
                {
                    case ESidesInGame.PlayerA:
                        // Didn't reach to 40 points yet
                        if (gameObject.ScorePlayerA < 3)
                        {
                            gameObject.ScorePlayerA++;
                            break;
                        }
                        // Has 40 points with advantage - increase a point (to win)
                        if (gameObject.AdvantageToPlayer == ESidesInGame.PlayerA)
                        {
                            gameObject.ScorePlayerA++;
                            break;
                        }
                        // Has 40 points with advantage to opponent - cancel advantage and back to 40:40
                        if (gameObject.AdvantageToPlayer == ESidesInGame.PlayerB)
                        {
                            gameObject.AdvantageToPlayer = ESidesInGame.None;
                            break;
                        }
                        // Has 40 points without advantage - gain advantage
                        if (gameObject.ScorePlayerB == 3)
                        {
                            gameObject.AdvantageToPlayer = ESidesInGame.PlayerA;
                            break;
                        }
                        // If none of the above it true - gain a point
                        gameObject.ScorePlayerA++;
                        break;

                    case ESidesInGame.PlayerB:
                        // Didn't reach to 40 points yet
                        if (gameObject.ScorePlayerB < 3)
                        {
                            gameObject.ScorePlayerB++;
                            break;
                        }
                        // Has 40 points with advantage - increase a point (to win)
                        if (gameObject.AdvantageToPlayer == ESidesInGame.PlayerB)
                        {
                            gameObject.ScorePlayerB++;
                            break;
                        }
                        // Has 40 points with advantage to opponent - cancel advantage and back to 40:40
                        if (gameObject.AdvantageToPlayer == ESidesInGame.PlayerA)
                        {
                            gameObject.AdvantageToPlayer = ESidesInGame.None;
                            break;
                        }
                        // Has 40 points without advantage - gain advantage
                        if (gameObject.ScorePlayerA == 3)
                        {
                            gameObject.AdvantageToPlayer = ESidesInGame.PlayerB;
                            break;
                        }
                        // If none of the above it true - gain a point
                        gameObject.ScorePlayerB++;
                        break;
                }
                // Check if the current game was won by one of the players or none
                var playerWins = await WinningInGame(gameObject);
                if (playerWins != ESidesInGame.None) // The game was won
                {
                    switch (playerWins)
                    {
                        // Player A won
                        case ESidesInGame.PlayerA:
                            // If Set 1 is not over yet
                            if (gameObject.Set1.WinnerInSet == ESidesInGame.None)
                            {
                                gameObject.Set1.ScorePlayerA++; // Give a win to Player A
                                await WinningInSet(gameObject.Set1); // Check if Set 1 is over and update
                                break;
                            }
                            // If Set 2 is not over yet
                            if (gameObject.Set2.WinnerInSet == ESidesInGame.None)
                            {
                                gameObject.Set2.ScorePlayerA++; // Give a win to Player A
                                await WinningInSet(gameObject.Set2); // Check if Set 2 is over and update
                                await WinningInMatch(gameObject); // Check if Match is over and update
                                break;
                            }
                            // If Set 3 is not over yet
                            if (gameObject.Set3.WinnerInSet == ESidesInGame.None)
                            {
                                gameObject.Set3.ScorePlayerA++; // Give a win to Player A
                                await WinningInSet(gameObject.Set3); // Check if Set 3 is over and update
                                await WinningInMatch(gameObject); // Check if Match is over and update
                                break;
                            }
                            break;

                        // Player B won
                        case ESidesInGame.PlayerB:
                            // If Set 1 is not over yet
                            if (gameObject.Set1.WinnerInSet == ESidesInGame.None)
                            {
                                gameObject.Set1.ScorePlayerB++; // Give a win to Player B
                                await WinningInSet(gameObject.Set1); // Check if Set 1 is over and update
                                break;
                            }
                            // If Set 2 is not over yet
                            if (gameObject.Set2.WinnerInSet == ESidesInGame.None)
                            {
                                gameObject.Set2.ScorePlayerB++; // Give a win to Player B
                                await WinningInSet(gameObject.Set2); // Check if Set 2 is over and update
                                await WinningInMatch(gameObject); // Check if Match is over and update
                                break;
                            }
                            // If Set 3 is not over yet
                            if (gameObject.Set3.WinnerInSet == ESidesInGame.None)
                            {
                                gameObject.Set3.ScorePlayerB++; // Give a win to Player B
                                await WinningInSet(gameObject.Set3); // Check if Set 3 is over and update
                                await WinningInMatch(gameObject); // Check if Match is over and update
                                break;
                            }
                            break;
                    }

                    // The Set score updated - reset points and advantage for a new game
                    gameObject.ScorePlayerA = 0;
                    gameObject.ScorePlayerB = 0;
                    gameObject.AdvantageToPlayer = ESidesInGame.None;
                }

                gameData = Newtonsoft.Json.JsonConvert.SerializeObject(gameObject);
                File.WriteAllText($"{DbDirectory}\\{gameObject.MatchID}.txt", gameData); // Write the updated data of the game to the game file
            }
            catch
            {
                gameData = string.Empty;
            }
            
            return gameData;
        }
        #endregion

        #region private methods
        // Return if one of the players or none won the game
        private static async Task<ESidesInGame> WinningInGame(SingleMatch gameObject)
        {
            ESidesInGame returnResult = ESidesInGame.None;

            await Task.Run(() =>
            {
                try
                {
                    // If Player A reached to 40 points
                    if (gameObject.ScorePlayerA == 4)
                        returnResult = ESidesInGame.PlayerA;
                    // If Player B reached to 40 points
                    else if (gameObject.ScorePlayerB == 4)
                        returnResult = ESidesInGame.PlayerB;
                    // None of the players won the game
                    else
                        returnResult = ESidesInGame.None;
                }
                catch
                {
                    returnResult = ESidesInGame.None;
                }
            });

            return returnResult;
        }

        // Check and update if one of the players or none won the set
        private static async Task WinningInSet(SingleSetInMatch set)
        {
            await Task.Run(() =>
            {
                // If 7:X or 6:below 5 sets
                if (set.ScorePlayerA == 7 || (set.ScorePlayerA == 6 && set.ScorePlayerB < 5))
                {
                    // Player A won the set
                    set.WinnerInSet = ESidesInGame.PlayerA;
                }
                // If X:7 or below 5 sets:6
                else if (set.ScorePlayerB == 7 || (set.ScorePlayerB == 6 && set.ScorePlayerA < 5))
                {
                    // Player B won the set
                    set.WinnerInSet = ESidesInGame.PlayerB;
                }
            });
        }

        // Check and update if one of the players or none won the match
        private static async Task WinningInMatch(SingleMatch match)
        {
            await Task.Run(() =>
            {
                // If the same player won the 2 first sets - no need for the 3rd, the result won't be changed
                if (match.Set1.WinnerInSet != ESidesInGame.None && match.Set2.WinnerInSet != ESidesInGame.None && match.Set1.WinnerInSet == match.Set2.WinnerInSet)
                {
                    // Update the winner
                    match.WinnerInMatch = match.Set1.WinnerInSet == ESidesInGame.PlayerA ? ESidesInGame.PlayerA : ESidesInGame.PlayerB;
                }
                // If all 3 sets were over
                else if (match.Set1.WinnerInSet != ESidesInGame.None && match.Set2.WinnerInSet != ESidesInGame.None && match.Set3.WinnerInSet != ESidesInGame.None)
                {
                    int playerA_Winnings = 0;
                    int playerB_Winnings = 0;

                    // Count winnings of the players
                    if (match.Set1.WinnerInSet == ESidesInGame.PlayerA)
                        playerA_Winnings++;
                    else
                        playerB_Winnings++;

                    if (match.Set2.WinnerInSet == ESidesInGame.PlayerA)
                        playerA_Winnings++;
                    else
                        playerB_Winnings++;

                    if (match.Set3.WinnerInSet == ESidesInGame.PlayerA)
                        playerA_Winnings++;
                    else
                        playerB_Winnings++;

                    // Update the winner
                    match.WinnerInMatch = playerA_Winnings > playerB_Winnings ? ESidesInGame.PlayerA : ESidesInGame.PlayerB;
                }
            });
        }
        #endregion
    }
}
