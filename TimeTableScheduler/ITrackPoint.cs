namespace Tracis.TimeTableScheduler;

/*
 * File:   ITrackPoint.cs
 * Author: Bradley Crouch
 *
 * Description: Source code for TimeTableScheduler application suite.
 */

public interface ITrackPoint
{
    string? FromLocation { get; set; }
    string? ToLocation { get; set; }
    int? Distance { get; set; }
    bool? Electric { get; set; }
    bool? PassengerUse { get; set; }
    string? LineCode { get; set; }
    
    int? FromLocationId { get; set; }
    int? ToLocationId { get; set; }


    bool ValidStop();
}

