using System.Security;

/*
 * File:  TrackPointCsvFileParser.cs
 * Author: Bradley Crouch
 *
 * Description: Source code for TimeTableScheduler application suite.
 */

namespace Tracis.TimeTableScheduler;

public class TrackPointCsvFileParser : ITrackPointParser
{
    
    private enum eTrackOrder
    {
        eFromLocation,
        eToLocation,
        eDistance,
        eElectric,
        ePassengerUse,
        eLineCode,
        eEndField 
    }
    
    public List<ITrackPoint> ParseFile(string filePath)
    {
        List<ITrackPoint> trackPoints = new List<ITrackPoint>();
        bool header = true;
        int lineNumber = 0;
        try
        {
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {

                    var line = reader.ReadLine();
                    if(line == null) break;

                    lineNumber++;
                    
                    var values = line.Split(',');

                    if (header)
                    {
                        // todo: possibly check first line is a header
                        header = false;
                        continue;
                    }
                    
                    if (values.Length != (int) eTrackOrder.eEndField) // Ensure there is correct number of fields
                    {
                        throw new Exception($"Error on line {lineNumber}, Invalid CSV format. Expected 6 fields per line.");
                    }
                    
                    
                    TrackPoint trackPoint = new TrackPoint
                    {
                        FromLocation = values[(int) eTrackOrder.eFromLocation].Trim(),
                        ToLocation = values[(int) eTrackOrder.eToLocation].Trim(),
                        
                        Distance = ParseNullableInt(values[(int)eTrackOrder.eDistance]),
                        Electric  = ParseNullableBool(values[(int)eTrackOrder.eElectric]),
                        PassengerUse =  ParseNullableBool(values[(int)eTrackOrder.ePassengerUse]),
                        
                        LineCode = values[(int) eTrackOrder.eLineCode].Trim()
                    };

                    trackPoints.Add(trackPoint);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return trackPoints;
    }

    private static int? ParseNullableInt(string value)
    {
        if (int.TryParse(value, out int result))
        {
            return result;
        }
        return null;
    }

    private static bool? ParseNullableBool(string value)
    {
        if (value.ToUpper() == "Y")
            return true;
        if (value.ToUpper() == "N")
            return false;

        return null;
    }
}
