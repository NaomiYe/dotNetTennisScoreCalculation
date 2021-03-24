using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TennisScoreCalculationApi;
using TennisScoreCalculationApi.Model;
using System.Threading.Tasks;

namespace TennisScoreCalculationTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestFailCreateNewGame()
        {
            // Arrange
            SingleMatch match = new SingleMatch();
            match.MatchID = int.MaxValue;
            var gameData = Newtonsoft.Json.JsonConvert.SerializeObject(match);

            // Act
            string actual = await ScoreCalculations.CreateNemGame();

            // Assert
            Assert.AreNotEqual(gameData, actual);
        }

        [TestMethod]
        public async Task TestSuccessCreateNewGame()
        {
            // Arrange
            SingleMatch match = new SingleMatch()
            {
                MatchID = 8,
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
            var gameData = Newtonsoft.Json.JsonConvert.SerializeObject(match);

            // Act
            string matchStr = await ScoreCalculations.CreateNemGame();
            SingleMatch matchObj = Newtonsoft.Json.JsonConvert.DeserializeObject<SingleMatch>(matchStr);
            matchObj.MatchID = 8;
            var actual = Newtonsoft.Json.JsonConvert.SerializeObject(matchObj);

            // Assert
            Assert.AreEqual(gameData, actual);
        }

        [TestMethod]
        public async Task TestLoadGameById()
        {
            // Arrange
            string str = string.Empty;

            // Act
            string actual = await ScoreCalculations.GetGameByID(int.MaxValue);

            // Assert
            Assert.AreEqual(str, actual);
        }

        // Score 40:40 with advantage to PlayerA
        [TestMethod]
        public async Task TestAddPointsToPlayers1()
        {
            // Arrange
            SingleMatch match = new SingleMatch()
            {
                MatchID = 8,
                NamePlayerA = "Player A",
                NamePlayerB = "Player B",
                ScorePlayerA = 3,
                ScorePlayerB = 3,
                AdvantageToPlayer = ESidesInGame.PlayerA,
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
            var gameData = Newtonsoft.Json.JsonConvert.SerializeObject(match);

            // Act
            string matchStr = await ScoreCalculations.CreateNemGame();
            SingleMatch matchObj = Newtonsoft.Json.JsonConvert.DeserializeObject<SingleMatch>(matchStr);
            int currentGameId = matchObj.MatchID;
            for (int i = 0; i < 3; i++)
            {
                matchStr = await ScoreCalculations.PlayerWinningPoint(currentGameId, ESidesInGame.PlayerA);
            }
            for (int i = 0; i < 3; i++)
            {
                matchStr = await ScoreCalculations.PlayerWinningPoint(currentGameId, ESidesInGame.PlayerB);
            }
            matchStr = await ScoreCalculations.PlayerWinningPoint(currentGameId, ESidesInGame.PlayerA);
            matchObj = Newtonsoft.Json.JsonConvert.DeserializeObject<SingleMatch>(matchStr);
            matchObj.MatchID = 8;
            var actual = Newtonsoft.Json.JsonConvert.SerializeObject(matchObj);

            // Assert
            Assert.AreEqual(gameData, actual);
        }

        // Score 40:40 with advantage to PlayerB and 7:6 to PlayerA on Set1
        [TestMethod]
        public async Task TestAddPointsToPlayers2()
        {
            // Arrange
            SingleMatch match = new SingleMatch()
            {
                MatchID = 8,
                NamePlayerA = "Player A",
                NamePlayerB = "Player B",
                ScorePlayerA = 3,
                ScorePlayerB = 3,
                AdvantageToPlayer = ESidesInGame.PlayerB,
                WinnerInMatch = ESidesInGame.None,
                Set1 = new SingleSetInMatch()
                {
                    ScorePlayerA = 7,
                    ScorePlayerB = 6,
                    WinnerInSet = ESidesInGame.PlayerA
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
            var gameData = Newtonsoft.Json.JsonConvert.SerializeObject(match);

            // Act
            string matchStr = await ScoreCalculations.CreateNemGame();
            SingleMatch matchObj = Newtonsoft.Json.JsonConvert.DeserializeObject<SingleMatch>(matchStr);
            int currentGameId = matchObj.MatchID;
            for (int i = 0; i < 4 * (5 * 1); i++)
            {
                matchStr = await ScoreCalculations.PlayerWinningPoint(currentGameId, ESidesInGame.PlayerA);
            }
            for (int i = 0; i < 4 * (6 * 1); i++)
            {
                matchStr = await ScoreCalculations.PlayerWinningPoint(currentGameId, ESidesInGame.PlayerB);
            }
            for (int i = 0; i < 4 * (2 * 1); i++)
            {
                matchStr = await ScoreCalculations.PlayerWinningPoint(currentGameId, ESidesInGame.PlayerA);
            }
            for (int i = 0; i < 3; i++)
            {
                matchStr = await ScoreCalculations.PlayerWinningPoint(currentGameId, ESidesInGame.PlayerA);
            }
            for (int i = 0; i < 3; i++)
            {
                matchStr = await ScoreCalculations.PlayerWinningPoint(currentGameId, ESidesInGame.PlayerB);
            }
            matchStr = await ScoreCalculations.PlayerWinningPoint(currentGameId, ESidesInGame.PlayerB);
            matchObj = Newtonsoft.Json.JsonConvert.DeserializeObject<SingleMatch>(matchStr);
            matchObj.MatchID = 8;
            var actual = Newtonsoft.Json.JsonConvert.SerializeObject(matchObj);

            // Assert
            Assert.AreEqual(gameData, actual);
        }

        [TestMethod]
        public async Task TestPlayerA_WinningInMatch()
        {
            // Arrange
            SingleMatch match = new SingleMatch()
            {
                MatchID = 8,
                NamePlayerA = "Player A",
                NamePlayerB = "Player B",
                ScorePlayerA = 0,
                ScorePlayerB = 0,
                AdvantageToPlayer = ESidesInGame.None,
                WinnerInMatch = ESidesInGame.PlayerA,
                Set1 = new SingleSetInMatch()
                {
                    ScorePlayerA = 6,
                    ScorePlayerB = 0,
                    WinnerInSet = ESidesInGame.PlayerA
                },
                Set2 = new SingleSetInMatch()
                {
                    ScorePlayerA = 6,
                    ScorePlayerB = 0,
                    WinnerInSet = ESidesInGame.PlayerA
                },
                Set3 = new SingleSetInMatch()
                {
                    ScorePlayerA = 0,
                    ScorePlayerB = 0,
                    WinnerInSet = ESidesInGame.None
                }
            };
            var gameData = Newtonsoft.Json.JsonConvert.SerializeObject(match);

            // Act
            string matchStr = await ScoreCalculations.CreateNemGame();
            SingleMatch matchObj = Newtonsoft.Json.JsonConvert.DeserializeObject<SingleMatch>(matchStr);
            int currentGameId = matchObj.MatchID;
            for (int i = 0; i < 4 * (6 * 2); i++)
            {
                matchStr = await ScoreCalculations.PlayerWinningPoint(currentGameId, ESidesInGame.PlayerA);
            }
            matchObj = Newtonsoft.Json.JsonConvert.DeserializeObject<SingleMatch>(matchStr);
            matchObj.MatchID = 8;
            var actual = Newtonsoft.Json.JsonConvert.SerializeObject(matchObj);

            // Assert
            Assert.AreEqual(gameData, actual);
        }

        [TestMethod]
        public async Task TestPlayerB_WinningInMatch()
        {
            // Arrange
            SingleMatch match = new SingleMatch()
            {
                MatchID = 8,
                NamePlayerA = "Player A",
                NamePlayerB = "Player B",
                ScorePlayerA = 0,
                ScorePlayerB = 0,
                AdvantageToPlayer = ESidesInGame.None,
                WinnerInMatch = ESidesInGame.PlayerB,
                Set1 = new SingleSetInMatch()
                {
                    ScorePlayerA = 6,
                    ScorePlayerB = 0,
                    WinnerInSet = ESidesInGame.PlayerA
                },
                Set2 = new SingleSetInMatch()
                {
                    ScorePlayerA = 0,
                    ScorePlayerB = 6,
                    WinnerInSet = ESidesInGame.PlayerB
                },
                Set3 = new SingleSetInMatch()
                {
                    ScorePlayerA = 0,
                    ScorePlayerB = 6,
                    WinnerInSet = ESidesInGame.PlayerB
                }
            };
            var gameData = Newtonsoft.Json.JsonConvert.SerializeObject(match);

            // Act
            string matchStr = await ScoreCalculations.CreateNemGame();
            SingleMatch matchObj = Newtonsoft.Json.JsonConvert.DeserializeObject<SingleMatch>(matchStr);
            int currentGameId = matchObj.MatchID;
            for (int i = 0; i < 4 * (6 * 1); i++)
            {
                matchStr = await ScoreCalculations.PlayerWinningPoint(currentGameId, ESidesInGame.PlayerA);
            }
            for (int i = 0; i < 4 * (6 * 2); i++)
            {
                matchStr = await ScoreCalculations.PlayerWinningPoint(currentGameId, ESidesInGame.PlayerB);
            }
            matchObj = Newtonsoft.Json.JsonConvert.DeserializeObject<SingleMatch>(matchStr);
            matchObj.MatchID = 8;
            var actual = Newtonsoft.Json.JsonConvert.SerializeObject(matchObj);

            // Assert
            Assert.AreEqual(gameData, actual);
        }
    }
}
