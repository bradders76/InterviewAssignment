namespace Tracis.TimeTableScheduler;

/*
 * File:  TrackPoint.cs
 * Author: Bradley Crouch
 *
 * Description: Source code for TimeTableScheduler application suite.
 */

public class TrackPoint : ITrackPoint
{
    public string? FromLocation { get; set; }
    public string? ToLocation { get; set; }
    public int? Distance { get; set; }
    public bool? Electric { get; set; }
    public bool? PassengerUse { get; set; }
    public string? LineCode { get; set; }
    public int? FromLocationId { get; set; }
    public int? ToLocationId { get; set; }
    
    
    // TODO: Possibly allow additional criteria, such as passenger and must be electric
    public bool ValidStop()
    {
        if (FromLocation != null && ToLocation != null && Distance != null)
        {
            return true;
        }
        
        return false;
    }
}