using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TennisScoreCalculationApi.Model;
using TennisScoreCalculationUI.Services;

namespace TennisScoreCalculationUI.Components.ScoreScreen
{
    public class ScoreScreenVm : BindableBase
    {
        #region data members
        private IScoreScreenService _scoreScreenService;
        private IEventAggregator _ea;
        #endregion

        #region Properties
        private int _GameId;
        public int GameId { get { return _GameId; } set { SetProperty(ref _GameId, value); } }

        private string _PlayerA_Name;
        public string PlayerA_Name { get { return _PlayerA_Name; } set { SetProperty(ref _PlayerA_Name, value); } }

        private string _PlayerB_Name;
        public string PlayerB_Name { get { return _PlayerB_Name; } set { SetProperty(ref _PlayerB_Name, value); } }

        private string _WinnerName;
        public string WinnerName { get { return _WinnerName; } set { SetProperty(ref _WinnerName, value); } }

        private int _PlayerA_Set1_Wins;
        public int PlayerA_Set1_Wins { get { return _PlayerA_Set1_Wins; } set { SetProperty(ref _PlayerA_Set1_Wins, value); } }

        private int _PlayerB_Set1_Wins;
        public int PlayerB_Set1_Wins { get { return _PlayerB_Set1_Wins; } set { SetProperty(ref _PlayerB_Set1_Wins, value); } }

        private int _PlayerA_Set2_Wins;
        public int PlayerA_Set2_Wins { get { return _PlayerA_Set2_Wins; } set { SetProperty(ref _PlayerA_Set2_Wins, value); } }

        private int _PlayerB_Set2_Wins;
        public int PlayerB_Set2_Wins { get { return _PlayerB_Set2_Wins; } set { SetProperty(ref _PlayerB_Set2_Wins, value); } }

        private int _PlayerA_Set3_Wins;
        public int PlayerA_Set3_Wins { get { return _PlayerA_Set3_Wins; } set { SetProperty(ref _PlayerA_Set3_Wins, value); } }

        private int _PlayerB_Set3_Wins;
        public int PlayerB_Set3_Wins { get { return _PlayerB_Set3_Wins; } set { SetProperty(ref _PlayerB_Set3_Wins, value); } }

        private int _PlayerA_Score;
        public int PlayerA_Score { get { return _PlayerA_Score; } set { SetProperty(ref _PlayerA_Score, value); } }

        private int _PlayerB_Score;
        public int PlayerB_Score { get { return _PlayerB_Score; } set { SetProperty(ref _PlayerB_Score, value); } }

        private ESidesInGame _AdvantageToPlayer;
        public ESidesInGame AdvantageToPlayer { get { return _AdvantageToPlayer; } set { SetProperty(ref _AdvantageToPlayer, value); } }

        private bool _IsGameOver;
        public bool IsGameOver { get { return _IsGameOver; } set { SetProperty(ref _IsGameOver, value); } }
        #endregion

        public ScoreScreenVm(IEventAggregator ea, IEventAggregator eaSubscribe, IScoreScreenService scoreScreenService)
        {
            _scoreScreenService = scoreScreenService;
            _ea = ea;

            eaSubscribe.GetEvent<StartNewGameEvent>().Subscribe(async () => await StartNewGameEventExecute());
            eaSubscribe.GetEvent<IncreasePointToPlayerAEvent>().Subscribe(async () => await IncreasePointToPlayerAEventExecute());
            eaSubscribe.GetEvent<IncreasePointToPlayerBEvent>().Subscribe(async () => await IncreasePointToPlayerBEventExecute());
            eaSubscribe.GetEvent<LoadGameEvent>().Subscribe(async val => await LoadGameEventExecute(val));
        }

        #region events execution
        private async Task LoadGameEventExecute(int param)
        {
            var gameData = await _scoreScreenService.LoadGameById(param);
            await SetGameDataToView(gameData);
        }

        private async Task IncreasePointToPlayerAEventExecute()
        {
            var gameData = await _scoreScreenService.IncreaseOnePoint(GameId, ESidesInGame.PlayerA);
            await SetGameDataToView(gameData);
        }

        private async Task IncreasePointToPlayerBEventExecute()
        {
            var gameData = await _scoreScreenService.IncreaseOnePoint(GameId, ESidesInGame.PlayerB);
            await SetGameDataToView(gameData);
        }

        private async Task StartNewGameEventExecute()
        {
            var gameData = await _scoreScreenService.StartNewGame();
            await SetGameDataToView(gameData);
        }
        #endregion

        // Set match object data to view properties
        private async Task SetGameDataToView(string gameData)
        {
            await Task.Run(() =>
            {
                var match = Newtonsoft.Json.JsonConvert.DeserializeObject<SingleMatch>(gameData);

                if (match != null)
                {
                    GameId = match.MatchID;

                    PlayerA_Name = match.NamePlayerA;
                    PlayerB_Name = match.NamePlayerB;

                    PlayerA_Set1_Wins = match.Set1.ScorePlayerA;
                    PlayerB_Set1_Wins = match.Set1.ScorePlayerB;
                    PlayerA_Set2_Wins = match.Set2.ScorePlayerA;
                    PlayerB_Set2_Wins = match.Set2.ScorePlayerB;
                    PlayerA_Set3_Wins = match.Set3.ScorePlayerA;
                    PlayerB_Set3_Wins = match.Set3.ScorePlayerB;

                    PlayerA_Score = match.ScorePlayerA;
                    PlayerB_Score = match.ScorePlayerB;

                    AdvantageToPlayer = match.AdvantageToPlayer;

                    // If loaded game was over
                    IsGameOver = match.WinnerInMatch != ESidesInGame.None ? true : false;
                    _ea.GetEvent<IsCurrentGameOverEvent>().Publish(IsGameOver);

                    // Update the winner
                    if (IsGameOver)
                        WinnerName = match.WinnerInMatch == ESidesInGame.PlayerA ? PlayerA_Name : PlayerB_Name;
                }
            });
        }
    }
}
