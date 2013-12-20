using System;

namespace nQonqr
{
	/// <summary>
	/// Implementers of this interface contain the data the Qonqr API returns about a zone
	/// </summary>
	public interface IZone
	{
		/// <summary>
		/// Gets the zone identifier/primary key, useful for single-zone API calls and for zone data keys.
		/// </summary>
		uint ZoneId { get; }

		/// <summary>
		/// Gets the name of the zone itself, on a scope the zone name displays as {ZoneName} // {RegionName} // {CountryName}.
		/// </summary>
		string ZoneName { get; }

		/// <summary>
		/// Gets the primary key for the region the zone is located in.
		/// </summary>
		uint RegionId { get; }

		/// <summary>
		/// Gets the a short-code for the region, e.g. MN for Minnesota.
		/// </summary>
		string RegionCode { get; }

		/// <summary>
		/// Gets the name of the region the zone is in, on a scope the zone name displays as {ZoneName} // {RegionName} // {CountryName}.
		/// </summary>
		string RegionName { get; }

		/// <summary>
		/// Gets the primary key for the country the zone is located in.
		/// </summary>
		uint CountryId { get; }

		/// <summary>
		/// Gets the short-code for the country, e.g. US.
		/// </summary>
		string CountryCode { get; }

		/// <summary>
		/// Gets the name of the country the zone is in, on a scope the zone name displays as {ZoneName} // {RegionName} // {CountryName}.
		/// </summary>
		string CountryName { get; }

		/// <summary>
		/// Gets the latitude where the zone is located in degrees.
		/// </summary>
		decimal Latitude { get; }

		/// <summary>
		/// Gets the longitude where the zone is located in degrees.
		/// </summary>
		decimal Longitude { get; }

		/// <summary>
		/// Gets the Faction that currently controls the zone.
		/// </summary>
		Faction ControlState { get; }

		/// <summary>
		/// Gets the identifier of the player that captured the zone or 0 if not captured/captured by a member of the non-controlling faction.
		/// </summary>
		uint CapturedByPlayerId { get; }

		/// <summary>
		/// Gets the code name of the player that captured the zone or the empty string if not captured/captured by a member of the non-controlling faction.
		/// </summary>
		string CapturedByCodename { get; }

		/// <summary>
		/// Gets the UTC date and time when the zone was captured by the currently controlling faction.
		/// </summary>
		DateTime? DateCapturedUTC { get; }

		/// <summary>
		/// Gets the identifier of the player that has the most bots in the zone for the controlling faction or 0 if not controlled.
		/// </summary>
		uint LeaderPlayerId { get; }

		/// <summary>
		/// Gets the code name of the player that has the most bots in the zone for the controlling faction or the empty string if not controlled
		/// </summary>
		string LeaderCodename { get; }

		/// <summary>
		/// Gets UTC date and time when the zone leader took the lead in the zone
		/// </summary>
		DateTime? LeaderSinceDateUTC { get; }

		/// <summary>
		/// Gets the total number of Faceless bots in the zone.
		/// </summary>
		uint FacelessCount { get; }

		/// <summary>
		/// Gets the total number of Legion bots in the zone.
		/// </summary>
		uint LegionCount { get; }

		/// <summary>
		/// Gets the total number of Swarm bots in the zone.
		/// </summary>
		uint SwarmCount { get; }
	}
}