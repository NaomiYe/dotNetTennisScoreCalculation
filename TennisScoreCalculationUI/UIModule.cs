using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using Prism.Regions;
using Prism.Modularity;
using TennisScoreCalculationUI.Components.ScoreScreen;
using TennisScoreCalculationUI.Components.RefereePanel;

namespace TennisScoreCalculationUI
{
    public class UIModule : IModule
    {
        private IRegionManager _regionManager;

        public UIModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion("ScoreScreenContentRegion", typeof(ScoreScreenView));
            _regionManager.RegisterViewWithRegion("RefereePanelContentRegion", typeof(RefereePanelView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ScoreScreenView>();
            containerRegistry.RegisterForNavigation<RefereePanelView>();
        }
    }
}
