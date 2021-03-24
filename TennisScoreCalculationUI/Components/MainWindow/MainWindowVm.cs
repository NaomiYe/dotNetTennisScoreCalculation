using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisScoreCalculationUI.Components.MainWindow
{
    public class MainWindowVm : BindableBase
    {
        #region data members
        private IRegionManager _regionManager;
        #endregion

        #region properties
        private Region _ScoreScreenContentRegion;
        public Region ScoreScreenContentRegion { get { return _ScoreScreenContentRegion; } set { SetProperty(ref _ScoreScreenContentRegion, value); } }

        private Region _RefereePanelContentRegion;
        public Region RefereePanelContentRegion { get { return _RefereePanelContentRegion; } set { SetProperty(ref _RefereePanelContentRegion, value); } }
        #endregion

        public MainWindowVm(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _regionManager.RequestNavigate("ScoreScreenContentRegion", "ScoreScreenView");
            _regionManager.RequestNavigate("RefereePanelContentRegion", "RefereePanelView");
        }
    }
}
