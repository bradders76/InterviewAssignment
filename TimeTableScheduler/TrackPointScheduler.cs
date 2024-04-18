using System.Collections.Frozen;

namespace Tracis.TimeTableScheduler;

public class TrackPointScheduler
{

    public static Dictionary<string, Station> ParseTrackPoint(List<ITrackPoint> trackPoints)
    {
        var returnList = new Dictionary<string, Station>();

        foreach (var trackPoint in trackPoints)
        {
            if (trackPoint.FromLocation == null) continue;

            // Check if the key exists in the dictionary
            if (!returnList.ContainsKey(trackPoint.FromLocation))
            {
                // If the key doesn't exist, create a new entry with an empty station
                returnList.Add(trackPoint.FromLocation, new Station(trackPoint.FromLocation));
            }


            returnList[trackPoint.FromLocation].AddNeighbor(trackPoint);
        }

        return returnList;
    }

   
}