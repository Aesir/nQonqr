using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace nQonqr
{
	/// <summary>
	/// This class contains all operations possible on the Qonqr API
	/// </summary>
	public class Api
		: IDisposable
	{
		private const string API_KEY_HEADER = "ApiKey";
		private const string BASE_ADDRESS = @"https://api.qonqr.com/pub/";
		private const string SINGLE_ZONE_FORMAT_STRING = @"ZoneData/Status/{0}";
		private const string AREA_FORMAT_STRING = @"ZoneData/BoundingBoxStatus/{0}/{1}/{2}/{3}";

		private readonly HttpClient m_Client;

		[ContractInvariantMethod]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant()
		{
			Contract.Invariant(m_Client != null);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Api"/> class.
		/// </summary>
		/// <remarks>
		/// To obtain an API key see http://community.qonqr.com/index.php?/topic/3179-public-api-beta/
		/// </remarks>
		/// <param name="apiKey">The key you obtained for the Qonqr API.</param>
		public Api(string apiKey)
		{
			Contract.Requires<ArgumentNullException>(apiKey != null, "apiKey");
			Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(apiKey));

			m_Client = new HttpClient() { BaseAddress = new Uri(BASE_ADDRESS) };
			m_Client.DefaultRequestHeaders.Add(API_KEY_HEADER, apiKey);
		}

		/// <summary>
		/// Gets the data for a single zone.
		/// </summary>
		/// <param name="zoneId">The zone identifier, this can be obtained from your scope by viewing the zone
		/// summary and dropping the z prefix or by using the QueryRegion call.</param>
		/// <returns>An IObservable that will produce a single <see cref="IZone"/> before completing.</returns>
		public async Task<IZone> QueryZone(uint zoneId)
		{
			Contract.Ensures(Contract.Result<Task<IZone>>() != null);

			var uri = new Uri(string.Format(SINGLE_ZONE_FORMAT_STRING, zoneId), UriKind.Relative);
			var jsonResult = await m_Client.GetStringAsync(uri);
			var zone = JsonConvert.DeserializeObject<Zone>(jsonResult);

			return zone;
		}

		/// <summary>
		/// Queries a region for all zones in a given Lat/Long bounding box. Note that there is a limit on the
		/// number of zones that can be returned and the API will return the zones that were most recently
		/// deployed into if the limit is reached.
		/// </summary>
		/// <param name="topLat">The northern latitude of the bounding box.</param>
		/// <param name="leftLon">The western longitude of the bounding box.</param>
		/// <param name="bottomLat">The southern latitude of the bounding box.</param>
		/// <param name="rightLon">The eastern longitude of the bounding box.</param>
		/// <returns>An IObservable that will produce a single collection of <see cref="IZone"/> before completing.</returns>
		public async Task<IEnumerable<IZone>> QueryRegion(decimal topLat, decimal leftLon, decimal bottomLat, decimal rightLon)
		{
			Contract.Requires<ArgumentOutOfRangeException>(-90m <= topLat && topLat <= 90m);
			Contract.Requires<ArgumentOutOfRangeException>(-180m <= leftLon && leftLon <= 180m);
			Contract.Requires<ArgumentOutOfRangeException>(-90m <= bottomLat && bottomLat <= 90m);
			Contract.Requires<ArgumentOutOfRangeException>(-180m <= rightLon && rightLon <= 180m);
			Contract.Ensures(Contract.Result<Task<IEnumerable<IZone>>>() != null);

			var uri = new Uri(string.Format(AREA_FORMAT_STRING, topLat, leftLon, bottomLat, rightLon), UriKind.Relative);
			var jsonResult = await m_Client.GetStringAsync(uri);
			var zoneGroup = JsonConvert.DeserializeObject<ZoneGroup>(jsonResult);

			return zoneGroup.Zones;
		}

		#region IDisposable

		private bool m_AlreadyDisposed = false;

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (m_AlreadyDisposed)
				return;

			if (disposing)
			{
				m_Client.Dispose();
			}

			m_AlreadyDisposed = true;
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="Api"/> is reclaimed by garbage collection.
		/// </summary>
		~Api()
		{
			Dispose(false);
		}

		#endregion IDisposable
	}
}