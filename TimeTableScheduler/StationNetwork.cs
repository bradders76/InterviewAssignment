using System.Diagnostics;

namespace Tracis.TimeTableScheduler;

/*
 * File:   StationNetwork.cs
 * Author: Bradley Crouch
 *
 * Description: Source code for TimeTableScheduler application suite.
 */

public class StationNetwork : IStationNetwork
{
    private List<Station>? _stations;
    private Dictionary<string, int>? _lookupValues;

    public void ParseTracks(List<ITrackPoint> trackPoints)
    {
        _lookupValues = new Dictionary<string, int>();
        _stations = new List<Station>();
        int items = 0;
        
        foreach (var trackPoint in trackPoints)
        {
            var toLocation = trackPoint.ToLocation;
            var fromLocation = trackPoint.FromLocation;
            var distiance = trackPoint.Distance;

            // must have valid (may wish to check for elec/passenger mode)
            // a) toLocation
            // b) fromLocation
            // c) distance (assuming m)

            if (toLocation == null) continue;
            if (fromLocation == null) continue;
            if (distiance == null) continue;

            // Add new station
            if (!_lookupValues.ContainsKey(fromLocation))
            {
                _lookupValues.Add(fromLocation, items++);
                _stations.Add(new Station(fromLocation));
            }

            var index = _lookupValues[fromLocation];
            _stations[index].AddNeighbor(trackPoint);
        }

        // set toLocationIds and fromLocationIds in trackPoints, will speed process
        foreach (var trackPoint in trackPoints)
        {
            var toLocation = trackPoint.ToLocation;
            var fromLocation = trackPoint.FromLocation;

            if (toLocation == null) continue;
            if (fromLocation == null) continue;

            if (_lookupValues.TryGetValue(toLocation, out int toLocationId))
            {
                trackPoint.ToLocationId = toLocationId;
            }
            else
            {
                trackPoint.ToLocationId = -1;
            }

            if (_lookupValues.TryGetValue(fromLocation, out int fromLocationId))
            {
                trackPoint.FromLocationId = fromLocationId;
            }
            else
            {
                trackPoint.FromLocationId = -1;
            }
        }
    }

    // Based on Dijkstra algorithm
    public List<ITrackPoint>? FindShortestPath(string sourceStation, string destinationStation)
    {
        if(_lookupValues == null)
            throw new ArgumentException("Invalid lookup values.");
        
        if(_stations== null)
            throw new ArgumentException("Invalid stations.");
        
        if (!_lookupValues.TryGetValue(sourceStation, out int sourceIndex))
            throw new ArgumentException("Invalid source station.");

        if (!_lookupValues.TryGetValue(destinationStation, out int destinationIndex))
            throw new ArgumentException("Invalid destination station.");
        
        if (sourceIndex < 0 || sourceIndex >= _stations.Count || destinationIndex < 0 || destinationIndex >= _stations.Count)
            throw new ArgumentException("Invalid source or destination index.");

        List<int> distance = Enumerable.Repeat(int.MaxValue, _stations.Count).ToList();
        List<int> previous = Enumerable.Repeat(-1, _stations.Count).ToList();
        
        HashSet<int> visited = new HashSet<int>();

        distance[sourceIndex] = 0;

        while (visited.Count < _stations.Count)
        {
            int currentStation = MinDistanceStation(distance, visited);
            if (currentStation == -1)
                break;

            visited.Add(currentStation);

            foreach (var trackPoint in _stations[currentStation].TrackPoints)
            {
                if(trackPoint.ToLocationId == null) continue;
                if(trackPoint.Distance == null) continue;
                
                int alt = distance[currentStation] + trackPoint.Distance.Value;
                if (alt < distance[trackPoint.ToLocationId.Value])
                {
                    distance[trackPoint.ToLocationId.Value] = alt;
                    previous[trackPoint.ToLocationId.Value] = currentStation;
                }
            }
        }

        // Reconstruct the path
        List<ITrackPoint> path = new List<ITrackPoint>();
        int current = destinationIndex;
        while (current != -1 && previous[current] != -1)
        {
            var trackPoint = _stations[previous[current]].TrackPoints.Find(tp => tp.ToLocationId == current);
            if (trackPoint != null)
                path.Add(trackPoint);

            current = previous[current];
        }
        
        // no path found
        if (path.Count == 0) return null;
        
        path.Reverse();
        return path;
    }

    //Todo:: Assuming distance is in km, this may or may not be correct
    public void FindShortestPathStats(string sourceStation, string destinationStation, out double ?distKm, out int? hops )
    {
        distKm = null;
        hops = null;
        
        var shortestPath = FindShortestPath(sourceStation, destinationStation);
        if (shortestPath == null)
            return;
        
        distKm = (double)( shortestPath.Sum(tp =>
        {
            Debug.Assert(tp.Distance != null, "tp.Distance != null");
            return tp.Distance.Value;
        }))/ 1000;
        
        hops = shortestPath.Count();
    }
    
    private int MinDistanceStation(List<int> distance, HashSet<int> visited)
    {
        int min = int.MaxValue;
        int minStation = -1;

        for (int i = 0; i < distance.Count; i++)
        {
            if (!visited.Contains(i) && distance[i] < min)
            {
                min = distance[i];
                minStation = i;
            }
        }
        
        return minStation;
    }
}