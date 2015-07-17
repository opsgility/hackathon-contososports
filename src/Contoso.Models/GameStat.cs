using System.ComponentModel.DataAnnotations;

namespace Contoso.Models
{
    public class GameStat
    {
        [ScaffoldColumn(false)]
        public int GameStatId { get; set; }
        public Team Team { get; set; }
        public int Goals { get; set; }
        public int Shots { get; set; }
        public int Passes { get; set; }
        public int FoulsLost { get; set; }
        public int FoulsWon { get; set; }
        public int Offside { get; set; }
        public int Corners { get; set; }
    }
}
