using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Contoso.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ContosoWeb.Hubs
{
    [HubName("LiveStats")]
    public class LiveStatsHub : Hub<ILiveStatsClient>
    {
        public static LiveStat[] RetrieveLatestLiveStats(IContosoWebContext dbContext, int matchId)
        {
            Match match = dbContext.Matches
                                   .Include(x => x.HomeTeamStats)
                                   .Include(x => x.AwayTeamStats)
                                   .FirstOrDefault(m => m.MatchId == matchId);

            if (match == null)
            {
                return null;
            }

            return new[] {
                new LiveStat("Goals", match.HomeTeamStats.Goals, match.AwayTeamStats.Goals),
                new LiveStat("Shots", match.HomeTeamStats.Shots, match.AwayTeamStats.Shots),
                new LiveStat("Passes", match.HomeTeamStats.Passes, match.AwayTeamStats.Passes),
                new LiveStat("Fouls Lost", match.HomeTeamStats.FoulsLost, match.AwayTeamStats.FoulsLost),
                new LiveStat("Fouls Won", match.HomeTeamStats.FoulsWon, match.AwayTeamStats.FoulsWon),
                new LiveStat("Offside", match.HomeTeamStats.Offside, match.AwayTeamStats.Offside),
                new LiveStat("Corners", match.HomeTeamStats.Corners, match.AwayTeamStats.Corners)
            };
        }

        public GameInfo GetCurrentGameInfo()
        {
            try
            {
                using (var dbContext = new ContosoWebContext(StaticConfig.DbContext.WebConnectionStringName))
                {
                    var currentMatch = dbContext.Matches.Where(x => x.Progress == MatchProgress.InProgress)
                                                        .OrderBy(x => x.MatchDate)
                                                        .Select(x => new
                                                        {
                                                            x.MatchId,
                                                            x.MatchDate,
                                                            HomeTeamId = x.HomeTeam.TeamId,
                                                            HomeTeamName = x.HomeTeam.Name,
                                                            AwayTeamId = x.AwayTeam.TeamId,
                                                            AwayTeamName = x.AwayTeam.Name,
                                                        })
                                                        .FirstOrDefault();

                    if (currentMatch == null)
                    {
                        return null;
                    }

                    var previousMeetings = dbContext.Matches.Where(x => x.Progress == MatchProgress.Completed)
                                                            .OrderBy(x => x.MatchDate)
                                                            .Select(x => new PreviousMeetingInfo
                                                            {
                                                                HomeTeamAbv = x.HomeTeam.AbbreviatedName,
                                                                HomeTeamScore = x.HomeTeamScore,
                                                                AwayTeamAbv = x.AwayTeam.AbbreviatedName,
                                                                AwayTeamScore = x.AwayTeamScore,
                                                                MatchDate = x.MatchDate
                                                            })
                                                            .Take(10)
                                                            .ToList();

                    var homeTeamLeaders = dbContext.Teams.Where(x => x.TeamId == currentMatch.HomeTeamId)
                                                         .SelectMany(x => x.Players)
                                                         .OrderByDescending(x => x.GoalsScored)
                                                         .Select(x => new PlayerInfo { Name = x.Name, GoalsScored = x.GoalsScored })
                                                         .Take(2)
                                                         .ToList();

                    var awayTeamLeaders = dbContext.Teams.Where(x => x.TeamId == currentMatch.AwayTeamId)
                                                         .SelectMany(x => x.Players)
                                                         .OrderByDescending(x => x.GoalsScored)
                                                         .Select(x => new PlayerInfo { Name = x.Name, GoalsScored = x.GoalsScored })
                                                         .Take(2)
                                                         .ToList();

                    var liveStats = RetrieveLatestLiveStats(dbContext, currentMatch.MatchId);

                    return new GameInfo
                    {
                        MatchId = currentMatch.MatchId,
                        MatchDate = currentMatch.MatchDate,
                        MatchStats = liveStats,
                        PreviousMeetings = previousMeetings,
                        HomeTeamName = currentMatch.HomeTeamName,
                        HomeTeamSeasonGoalLeaders = homeTeamLeaders,
                        AwayTeamName = currentMatch.AwayTeamName,
                        AwayTeamSeasonGoalLeaders = awayTeamLeaders
                    };
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("GetCurrentGameInfo: {0}", ex);
                throw;
            }
        }

        public LiveStat[] GetLatestLiveStats(int matchId)
        {
            using (var dbContext = new ContosoWebContext(StaticConfig.DbContext.WebConnectionStringName))
            {
                return RetrieveLatestLiveStats(dbContext, matchId);
            }
        }

        public async Task RegisterForLiveStats(int matchId)
        {
            await Groups.Add(Context.ConnectionId, matchId.ToString());
        }
    }

    public interface ILiveStatsClient
    {
        void LiveStatsUpdated(int matchId, LiveStat[] stats);
    }

    public class GameInfo
    {
        public int MatchId { get; set; }
        public DateTimeOffset MatchDate { get; set; }
        public IList<LiveStat> MatchStats { get; set; }
        public IList<PreviousMeetingInfo> PreviousMeetings { get; set; }

        public string HomeTeamName { get; set; }
        public IList<PlayerInfo> HomeTeamSeasonGoalLeaders { get; set; }

        public string AwayTeamName { get; set; }
        public IList<PlayerInfo> AwayTeamSeasonGoalLeaders { get; set; }
    }

    public class PreviousMeetingInfo
    {
        public string HomeTeamAbv { get; set; }
        public int HomeTeamScore { get; set; }

        public string AwayTeamAbv { get; set; }
        public int AwayTeamScore { get; set; }

        public DateTimeOffset MatchDate { get; set; }
    }

    public class PlayerInfo
    {
        public string Name { get; set; }
        public int GoalsScored { get; set; }
    }

    public class LiveStat
    {
        public LiveStat(string name, int homeTeamValue, int awayTeamValue)
        {
            Name = name;
            HomeTeamValue = homeTeamValue;
            AwayTeamValue = awayTeamValue;
        }

        public string Name { get; set; }
        public int HomeTeamValue { get; set; }
        public int AwayTeamValue { get; set; }
    }
}