namespace Tracis.TimeTableScheduler;

/*
 * File:   ITrackPointParser.cs
 * Author: Bradley Crouch
 *
 * Description: Source code for TimeTableScheduler application suite.
 */
public interface ITrackPointParser
{
    List<ITrackPoint> ParseFile(string filePath);
}