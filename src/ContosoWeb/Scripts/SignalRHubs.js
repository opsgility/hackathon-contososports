
function startLiveStatsFeed(matchId) {
    $.connection.hub.logging = true;

    //configure Announcement hub
    var announcementsHub = $.connection.Announcement;
    announcementsHub.client.announcement = function (item) {
        var newArrivals = $('a#NewArrivalsPanel');
        newArrivals.attr("href", item.Url); //Set the URL
        newArrivals.text(item.Title); //Set the title
    };

    //configure LiveStats hub
    var liveStatsHub = $.connection.LiveStats;
    liveStatsHub.client.liveStatsUpdated = updateLiveStats;

    $.connection.hub.start().done(function () {
        console.log('hub connection open');

        //Get the latest live stats...
        liveStatsHub.server.getLatestLiveStats(matchId).done(function (stats) {
            //Update and show the stats table
            updateLiveStats(matchId, stats);
            $("#liveStatsTable").show();

            //And register for ongoing updates
            liveStatsHub.server.registerForLiveStats(matchId);
        });
    });

    function updateLiveStats(matchId, stats) {
        //expects stats object as array of arrays in format of {<HomeTeamStatValue>, <StatName>, <AwayTeamStatValue>}
        $("#liveStatsTable tbody").html("");
        for (var i = 0; i < stats.length; i++) {
            $("#liveStatsTable tbody").append("<tr><td>" + stats[i].HomeTeamValue + "</td><td>" + stats[i].Name + "</td><td>" + stats[i].AwayTeamValue + "</td></tr>");
        }
    }
}