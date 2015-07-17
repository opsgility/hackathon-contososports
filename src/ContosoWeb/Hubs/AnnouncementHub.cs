using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ContosoWeb.Hubs
{
    [HubName("Announcement")]
    public class AnnouncementHub : Hub
    {
    }
}