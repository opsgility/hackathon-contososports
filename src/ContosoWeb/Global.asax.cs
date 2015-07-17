using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Contoso.Models;
using ContosoWeb.Hubs;
using ContosoWeb.Utils;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Transports;
using Microsoft.Practices.Unity;
using Unity.Mvc4;

namespace ContosoWeb
{
    public class Global : HttpApplication
    {
        internal static IUnityContainer UnityContainer;

        private static HubConnection _lobHubConnection;
        private static IDisposable _updatesDisposer;

        protected void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            
            Database.SetInitializer(new ContosoWebDbInitializer());

            UnityContainer = UnityConfig.BuildContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(UnityContainer));

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            WebApiConfig.RegisterWebApi(GlobalConfiguration.Configuration, UnityContainer);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            StartGameDataListener();
            StartFanZoneUpdates();
        }

        protected void Application_Stop(object sender, EventArgs e)
        {
            if (_updatesDisposer != null)
            {
                _updatesDisposer.Dispose();
            }

            if (_lobHubConnection != null)
            {
                _lobHubConnection.Stop();
            }
        }

        #region [ Game Data ]

        /// <summary>
        /// Starts a SignalR client which listens for change notifications from the LOB app.
        /// Whenever match data is changed update notifications are automatically pushed to clients of this website.
        /// </summary>
        private static void StartGameDataListener()
        {
            Task.Run(() => {
                try
                {
                    Trace.TraceInformation("Connecting to Game Data notifications");

                    var serverUrl = ConfigurationHelpers.GetString("Contoso.Lob.Endpoint");

                    _lobHubConnection = new HubConnection(serverUrl);
                    _updatesDisposer = _lobHubConnection.CreateHubProxy("UpdatesHub")
                                                        .On<int>("NotifyGameDataUpdated", SendGameUpdateNotifications);

                    _lobHubConnection.Start().Wait();

                    Trace.TraceInformation("Connected");
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Error in game data poller: {0}", ex);
                }
            });
        }

        private static void SendGameUpdateNotifications(int matchId)
        {
            Trace.TraceInformation("Match updated ({0}), sending notifications", matchId);

            using (var dbContext = new ContosoWebContext(StaticConfig.DbContext.WebConnectionStringName))
            {
                var stats = LiveStatsHub.RetrieveLatestLiveStats(dbContext, matchId);
                if (stats != null)
                {
                    var liveStatsHub = GlobalHost.ConnectionManager.GetHubContext<LiveStatsHub, ILiveStatsClient>();

                    liveStatsHub.Clients.Group(matchId.ToString()).LiveStatsUpdated(matchId, stats);
                }
            }
        }
        #endregion

        #region [ Fan Zone Updates ]

        /// <summary>
        /// Starts a thread to continuously send random "noise" data to fan zone clients
        /// </summary>
        private static void StartFanZoneUpdates()
        {
            Task.Run(() => {
                var timer = Stopwatch.StartNew();

                try
                {
                    while (true)
                    {
                        Thread.Sleep(500);
                        SendMockFanZoneVolumeSignalRData(timer.Elapsed);
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Error in fan zone data provider: {0}", ex);
                }
            });
        }

        private static int CalculateOscillationValue(int min, int max, double timeInSeconds)
        {
            const double TWO_PI = 2 * Math.PI;

            //            |-----range------|
            // 0---------min---------------^---------------max
            //                           start

            int range = (max - min) / 2;
            int start = min + range;

            // offset varies between -{range} and {range} depending on the value of {time}
            // timeInSeconds is scaled so that a full oscillation occurs approx every 20 seconds
            var offset = (Math.Sin(TWO_PI * timeInSeconds * 0.05) * range);

            return (int) (start + offset);
        }

        private static void SendMockFanZoneVolumeSignalRData(TimeSpan elapsed)
        {
            double time = elapsed.TotalSeconds;

            var volumeData = new [] {
                new FanZoneVolume {
                    Decibels = CalculateOscillationValue(min: 100, max: 120, timeInSeconds: time),
                    XCoord = 100,
                    YCoord = 100,
                    HexColor = "#BF16CD"
                },
                new FanZoneVolume {
                    Decibels = CalculateOscillationValue(min: 60, max: 100, timeInSeconds: time),
                    XCoord = 350,
                    YCoord = 200,
                    HexColor = "#029B39"
                },
                new FanZoneVolume {
                    Decibels = CalculateOscillationValue(min: 85, max: 110, timeInSeconds: time),
                    XCoord = 50,
                    YCoord = 300,
                    HexColor = "#C05911"
                },
                new FanZoneVolume {
                    Decibels = CalculateOscillationValue(min: 95, max: 115, timeInSeconds: time),
                    XCoord = 400,
                    YCoord = 400,
                    HexColor = "#D8B201"
                }
            };

            var fanZoneHub = GlobalHost.ConnectionManager.GetHubContext<FanZoneHub, IFanZoneClient>();

            fanZoneHub.Clients.All.FanZoneVolumeUpdated(volumeData);
        }
        #endregion
    }
}
