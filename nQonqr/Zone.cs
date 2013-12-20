using System;

namespace nQonqr
{
	internal class Zone 
		: IZone
	{
		public uint ZoneId { get; set; }

		public string ZoneName { get; set; }

		public uint RegionId { get; set; }

		public string RegionName { get; set; }

		public string RegionCode { get; set; }

		public uint CountryId { get; set; }

		public string CountryName { get; set; }

		public string CountryCode { get; set; }

		public decimal Latitude { get; set; }

		public decimal Longitude { get; set; }

		public Faction ControlState { get; set; }

		public uint CapturedByPlayerId { get; set; }

		public string CapturedByCodename { get; set; }

		public DateTime? DateCapturedUTC { get; set; }

		public uint LeaderPlayerId { get; set; }

		public string LeaderCodename { get; set; }

		public DateTime? LeaderSinceDateUTC { get; set; }

		public uint LegionCount { get; set; }

		public uint SwarmCount { get; set; }

		public uint FacelessCount { get; set; }
	}
}