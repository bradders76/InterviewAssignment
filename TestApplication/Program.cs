using Tracis.TimeTableScheduler;
using Microsoft.Extensions.Logging;

// Create an instance of ILogger for the Program class
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});
var logger = loggerFactory.CreateLogger<TrackPointCsvFileParser>();


/*
 * File:   Program.cs
 * Author: Bradley Crouch
 *
 * Description: Source code for TimeTableScheduler application suite.
 
 */
var parser = new  TrackPointCsvFileParser(logger);
var stationNetwork = new StationNetwork();

if(parser.ParseFile("Resources/Tracks.csv", out var trackPoints))

do
{
    stationNetwork.ParseTracks(trackPoints);

    // Ask for start and end locations
    Console.Write("Enter start location: ");
    string? startLocation = Console.ReadLine();
    
    Console.Write("Enter end location: ");
    string? endLocation = Console.ReadLine();

    if (startLocation == null || endLocation == null)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("Invalid location retry");
        Console.ForegroundColor = ConsoleColor.White;
        continue;
    }
    
    // Find shortest path

    try
    {
        var paths = stationNetwork.FindShortestPath(startLocation, endLocation);

        if (paths != null)
        {
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Green;


            stationNetwork.FindShortestPathStats(startLocation, endLocation, out var distanceKm, out var hops);

            int i = 0;
            foreach (var path in paths)
            {
                Console.WriteLine(
                    $"Path {++i}: From {path.FromLocation} to {path.ToLocation}, Distance: {path.Distance}, Passenger: {path.PassengerUse}, Electric: {path.Electric} ");

            }
            
            if (distanceKm.HasValue && hops.HasValue)
            {
                Console.WriteLine($"Shortest path distance: {distanceKm} km");
                Console.WriteLine($"Number of hops: {hops}");
            }
            else
            {
                Console.WriteLine("Invalid path.");
            }

            Console.ForegroundColor = ConsoleColor.White;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No path found between the specified locations.");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
    catch (Exception e)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Exception {e.ToString()}");
        Console.ForegroundColor = ConsoleColor.White;
    }
} while (true);


