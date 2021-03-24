using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisScoreCalculationApi.Model
{
    public class SingleMatch
    {
        public int MatchID { get; set; }
        public string NamePlayerA { get; set; }
        public string NamePlayerB { get; set; }
        public int ScorePlayerA { get; set; }
        public int ScorePlayerB { get; set; }
        public ESidesInGame AdvantageToPlayer { get; set; }
        public SingleSetInMatch Set1 { get; set; }
        public SingleSetInMatch Set2 { get; set; }
        public SingleSetInMatch Set3 { get; set; }
        public ESidesInGame WinnerInMatch { get; set; }
    }
}
