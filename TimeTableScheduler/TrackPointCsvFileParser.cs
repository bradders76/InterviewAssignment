/*
 * File:  TrackPointCsvFileParser.cs
 * Author: Bradley Crouch
 *
 * Description: Source code for TimeTableScheduler application suite.
 */

using Microsoft.Extensions.Logging;

namespace Tracis.TimeTableScheduler;

public class TrackPointCsvFileParser : ITrackPointParser
{
    private ILogger<TrackPointCsvFileParser>? _logger;
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
    
    public string? ErrorMessage { get; private set; }
    
    public TrackPointCsvFileParser(ILogger<TrackPointCsvFileParser>? logger = null)
    {
        _logger = logger;
    } 
    
    public bool  ParseFile(string filePath, out List<ITrackPoint> trackPoints)
    {
        _logger?.LogDebug($"Parsing CSV File {filePath} started");
     
        trackPoints = new List<ITrackPoint>();
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
                        string errorMessage = $"Error on line {lineNumber}, Invalid CSV format. Expected 6 fields per line.";
                        
                        _logger?.LogError(errorMessage);
                        ErrorMessage = errorMessage;
                        
                            
                        // error, so fail (may wish to have error list
                        return false;
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
            string errorMessage = ex.ToString();

            _logger?.LogError(ex, ex.ToString());
            
            // invalid file, so igore
            trackPoints.Clear();
            return false;
        }

        _logger?.LogDebug($"Parsing CSV File {filePath} ended successfully");

        return true;
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
