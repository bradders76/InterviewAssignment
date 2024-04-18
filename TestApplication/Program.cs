
using Tracis.TimeTableScheduler;

/*
 * File:   Program.cs
 * Author: Bradley Crouch
 *
 * Description: Source code for TimeTableScheduler application suite.
 
 */
var parser = new  TrackPointCsvFileParser();
var stationNetwork = new StationNetwork();


var trackPoints =  parser.ParseFile("Resources/Tracks.csv");

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

            for (int i = 0; i < paths.Count; i++)
            {
                Console.WriteLine(
                    $"Path {i + 1}: From {paths[i].FromLocation} to {paths[i].ToLocation}, Distance: {paths[i].Distance}");
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


