nQonqr
======

A .Net Portable Class Library implementation of the Qonqr API

```C#
var api = new Api("Your API key");

var zone = await api.QueryZone(2196724);
//Note you can also use .Result if you want to make a blocking call
//var zone = api.QueryZone(2196724).Result;

Console.WriteLine("{0}[{1}] // {2}[{3}] // {4}[{5}]",
	zone.ZoneName, zone.ZoneId,
	zone.RegionName, zone.RegionId,
	zone.CountryName, zone.CountryId);
Console.WriteLine("Held By {0} since {1:g}", zone.ControlState, zone.DateCapturedUTC);
Console.WriteLine();

Console.WriteLine("Bot counts:");
Console.WriteLine("\tFaceless {0:n0}", zone.FacelessCount);
Console.WriteLine("\tLegion {0:n0}", zone.LegionCount);
Console.WriteLine("\tSwarm {0:n0}", zone.SwarmCount);
Console.WriteLine();

Console.WriteLine("Captured By {0}[{1}]", zone.CapturedByCodename, zone.CapturedByPlayerId);
Console.WriteLine("Lead By {0}[{1}]", zone.LeaderCodename, zone.LeaderPlayerId);
Console.WriteLine();

Console.WriteLine("{0} Latitude, {1} Longitude", zone.Latitude, zone.Longitude);
Console.WriteLine();
```
