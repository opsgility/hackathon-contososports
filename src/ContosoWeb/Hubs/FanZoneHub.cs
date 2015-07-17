using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ContosoWeb.Hubs
{
    [HubName("FanZone")]
    public class FanZoneHub : Hub
    {
    }
    
    public interface IFanZoneClient
    {
        void FanZoneVolumeUpdated(FanZoneVolume[] volumeData);
    }

    public class FanZoneVolume
    {
        public int Decibels { get; set; }

        public int XCoord { get; set; }
        public int YCoord { get; set; }

        public string HexColor { get; set; }
    }
}