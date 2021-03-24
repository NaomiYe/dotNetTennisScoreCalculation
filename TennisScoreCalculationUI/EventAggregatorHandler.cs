using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisScoreCalculationUI
{
    public class StartNewGameEvent : PubSubEvent { }

    public class LoadGameEvent : PubSubEvent<int> { }

    public class IncreasePointToPlayerAEvent : PubSubEvent { }

    public class IncreasePointToPlayerBEvent : PubSubEvent { }

    public class IsCurrentGameOverEvent : PubSubEvent<bool> { }
}
