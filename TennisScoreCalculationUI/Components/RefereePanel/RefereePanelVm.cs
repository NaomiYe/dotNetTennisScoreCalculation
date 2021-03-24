using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TennisScoreCalculationUI.Languages;

namespace TennisScoreCalculationUI.Components.RefereePanel
{
    public class RefereePanelVm : BindableBase
    {
        #region data members
        private IEventAggregator _ea;
        #endregion

        #region properties
        private int _GameIdToLoad;
        public int GameIdToLoad { get { return _GameIdToLoad; } set { SetProperty(ref _GameIdToLoad, value); } }

        private bool _IsCurrentGameOver;
        public bool IsCurrentGameOver { get { return _IsCurrentGameOver; } set { SetProperty(ref _IsCurrentGameOver, value); } }
        #endregion

        #region commands
        public DelegateCommand StartNewGame { get; private set; }
        public DelegateCommand<object> GivePointToPlayer { get; private set; }
        public DelegateCommand LoadGameById { get; private set; }
        #endregion

        public RefereePanelVm(IEventAggregator ea, IEventAggregator eaSubscribe)
        {
            _ea = ea;
            eaSubscribe.GetEvent<IsCurrentGameOverEvent>().Subscribe(async val => await IsCurrentGameOverEventExecute(val));

            Task.Run(async () =>
            {
                await StartNewGameExecute();
            });

            StartNewGame = new DelegateCommand(async () => await StartNewGameExecute());
            GivePointToPlayer = new DelegateCommand<object>(async val => await GivePointToPlayerExecute(val), val => !IsCurrentGameOver).ObservesProperty(() => IsCurrentGameOver);
            LoadGameById = new DelegateCommand(async () => await LoadGameByIdExecute());
        }

        #region events execution
        private async Task IsCurrentGameOverEventExecute(bool param)
        {
            await Task.Run(() =>
            {
                IsCurrentGameOver = param;
            });
        }
        #endregion

        #region commands execution
        private async Task LoadGameByIdExecute()
        {
            await Task.Run(() =>
            {
                _ea.GetEvent<LoadGameEvent>().Publish(GameIdToLoad);
            });
        }

        private async Task GivePointToPlayerExecute(object param)
        {
            await Task.Run(() =>
            {
                if (param != null)
                {
                    switch (int.Parse(param.ToString()))
                    {
                        case 1:
                            _ea.GetEvent<IncreasePointToPlayerAEvent>().Publish();
                            break;

                        case 2:
                            _ea.GetEvent<IncreasePointToPlayerBEvent>().Publish();
                            break;
                    }
                }
            });
        }

        private async Task StartNewGameExecute()
        {
            await Task.Run(() =>
            {
                _ea.GetEvent<StartNewGameEvent>().Publish();
            });
        }
        #endregion
    }
}
