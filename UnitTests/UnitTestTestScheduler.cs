/*
 * File:  UnitTestTestScheduler.cs
 * Author: Bradley Crouch
 *
 * Description: Source code for TimeTableScheduler application suite.
 */

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Tracis.TimeTableScheduler;

namespace UnitTestScheduler;

[TestClass]
public class UnitTestTestScheduler
{
    private StationNetwork? _stationNetwork;
    private List<ITrackPoint>? _trackPoints;
    
    [TestInitialize]
    public void TestInitialize()
    {
        var parser = new TrackPointCsvFileParser();
        _trackPoints =  parser.ParseFile("Resources/Tracks.csv");
        
        _stationNetwork = new StationNetwork();
        _stationNetwork.ParseTracks(_trackPoints);
    }
      
    // TODO:: failing test cases, however appears invalid data.. 
    //        suggestion is to investigate with FindShortestPaths (see TestApplication)
    [TestMethod]
    [DataRow("BERKHMD", "TRING",    5.994,    1)]
    [DataRow("HYWRDSH", "KEYMERJ",  null,     null)]
    [DataRow("BERKHMD", "HEMLHMP",  5.553,    null)]
    [DataRow("BHAMNWS", "BHAMINT",  null,     null)]
    [DataRow("BERKHMD", "WATFDJ",   17.098,   7)]
    [DataRow("EUSTON",  "BERKHMD",  null,     null)]
    [DataRow("MNCRPIC", "CRDFCEN",  276.677,  64)]
    [DataRow("KNGX",    "EDINBUR",  null,     null)]
    [DataRow("THURSO",  "PENZNCE",  1457.246, 320)]
    [DataRow("PHBR",    "RYDP",     null,     null)]
    
    public void GivenScheduleLoaded_VerifyExpectedDistanceAndTrackCountBetweenStartAndEnd(string fromLocation, string toLocation, double? expDistance, int? expTrackCount)
    {
        Assert.IsNotNull(_stationNetwork);
        
        _stationNetwork.FindShortestPathStats(fromLocation, toLocation, out double? retDistance, out int? retTrackCount);
        
        Assert.AreEqual(expDistance, retDistance);
        Assert.AreEqual(expTrackCount, retTrackCount);
    }
    
    
    // TODO:: add test cases with faulty values
    [TestMethod]
    [DataRow(0, "ABCWM","FERNHIL",483,false,true,"U&D")]
    [DataRow(-1, "WMBYICD","_WLSDCL",1460,null,false,"DCL")]
    
    public void GivenScheduleLoaded_VerifyEntries(int index, string expFromLocation, string expToLocation, int? expDistance, bool? expElectric, bool? expPassengerUse, string? expLineCode)
    {
        Assert.IsNotNull(_trackPoints);
        
        // allows easy access to item from end
        if (index < 0) index = _trackPoints.Count() + index;
        
        Assert.AreEqual(expFromLocation,      _trackPoints[index].FromLocation);
        Assert.AreEqual(expToLocation,      _trackPoints[index].ToLocation);
        Assert.AreEqual(expDistance,      _trackPoints[index].Distance);
        Assert.AreEqual(expElectric,      _trackPoints[index].Electric);
        Assert.AreEqual(expPassengerUse,      _trackPoints[index].PassengerUse);
        Assert.AreEqual(expLineCode,      _trackPoints[index].LineCode);
    }
}
