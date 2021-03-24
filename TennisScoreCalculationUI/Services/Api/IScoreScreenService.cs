using System.Threading.Tasks;
using TennisScoreCalculationApi.Model;

namespace TennisScoreCalculationUI.Services
{
    public interface IScoreScreenService
    {
        Task<string> IncreaseOnePoint(int gameId, ESidesInGame playerWins);
        Task<string> StartNewGame();
        Task<string> LoadGameById(int Id);
    }
}