namespace Tracis.TimeTableScheduler;

/*
 * File:   Station.cs
 * Author: Bradley Crouch
 *
 * Description: Source code for TimeTableScheduler application suite.
 */

public class Station : IStation
{
    private object _lock = new object();
    public string Name { get; }
    public List<ITrackPoint> TrackPoints { get; } // List of neighbors and their distances
    
    public Station(string name)
    {
        Name = name;
        TrackPoints = new List<ITrackPoint>();
    }

    public void AddNeighbor(ITrackPoint trackPoint)
    {
        // ignore as not valid start location
        if (Name != trackPoint.FromLocation) return;

        // redundant, but to be safe if ValidStop is invalid
        if (trackPoint.ToLocation == null) return;

        if (!trackPoint.ValidStop()) return;


        lock (_lock)
        {
            string key = trackPoint.ToLocation;

            var currentTrackPoint = TrackPoints.FirstOrDefault(x => x.ToLocation == trackPoint.ToLocation);

            if (currentTrackPoint != null)
            {
                // ignore adding item as shorter distance exists, may wish to allow backup routes in future
                if (currentTrackPoint.Distance < trackPoint.Distance)
                {
                    return;
                }
                TrackPoints.Remove(currentTrackPoint);
            }

            TrackPoints.Add(trackPoint);
        }
    }
}
