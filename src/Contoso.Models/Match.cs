using System;
using System.ComponentModel.DataAnnotations;

namespace Contoso.Models
{
    public class Match
    {
        [ScaffoldColumn(false)]
        public int MatchId { get; set; }
        public Team HomeTeam { get; set; }
        public int HomeTeamScore { get; set; }
        public Team AwayTeam { get; set; }
        public int AwayTeamScore { get; set; }
        public DateTimeOffset MatchDate { get; set; }
        public MatchProgress Progress { get; set; }
        public GameStat HomeTeamStats { get; set; }
        public GameStat AwayTeamStats { get; set; }

        public int? WinningTeamId
        {
            get
            {
                if (HomeTeamScore > AwayTeamScore)
                    return HomeTeam.TeamId;

                if (AwayTeamScore > HomeTeamScore)
                    return AwayTeam.TeamId;

                return null;
            }
        }
    }
}
