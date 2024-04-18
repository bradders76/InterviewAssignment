namespace Tracis.TimeTableScheduler;
/*
 * File:   IStationNetwork.cs
 * Author: Bradley Crouch
 *
 * Description: Source code for TimeTableScheduler application suite.
 */

public interface IStationNetwork
{
    void ParseTracks(List<ITrackPoint> trackPoints);
    List<ITrackPoint>? FindShortestPath(string sourceStation, string destinationStation);
    void FindShortestPathStats(string sourceStation, string destinationStation, out double? distKm, out int? hops);
}