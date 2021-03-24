using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TennisScoreCalculationApi;
using TennisScoreCalculationApi.Model;

namespace TennisScoreCalculationUI.Services
{
    public class ScoreScreenService : BindableBase, IScoreScreenService
    {
        public async Task<string> StartNewGame()
        {
            return await ScoreCalculations.CreateNemGame();
        }

        public async Task<string> LoadGameById(int Id)
        {
            return await ScoreCalculations.GetGameByID(Id);
        }


        public async Task<string> IncreaseOnePoint(int gameId, ESidesInGame playerWins)
        {
            return await ScoreCalculations.PlayerWinningPoint(gameId, playerWins);
        }
    }
}
