using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisScoreCalculationApi.Model
{
    public class SingleSetInMatch
    {
        public int ScorePlayerA { get; set; }
        public int ScorePlayerB { get; set; }
        public ESidesInGame WinnerInSet { get; set; }
    }
}
