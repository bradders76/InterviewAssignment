namespace Tracis.TimeTableScheduler;

/*
 * File:   ITrackPointParser.cs
 * Author: Bradley Crouch
 *
 * Description: Source code for TimeTableScheduler application suite.
 */
public interface ITrackPointParser
{

    public string? ErrorMessage { get; }
    public bool ParseFile(string filePath, out List<ITrackPoint> trackPoints);
}