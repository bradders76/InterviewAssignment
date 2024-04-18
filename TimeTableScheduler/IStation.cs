namespace Tracis.TimeTableScheduler;
/*
 * File:   IStation.cs
 * Author: Bradley Crouch
 *
 * Description: Source code for TimeTableScheduler application suite.
 */

public interface IStation
{
    string Name { get; }
    void AddNeighbor(ITrackPoint trackPoint);
}