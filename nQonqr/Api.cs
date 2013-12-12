using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace nQonqr
{
	/// <summary>
	/// This class contains all operations possible on the Qonqr API
	/// </summary>
	public class Api
	{
		private const string API_KEY_HEADER = "ApiKey";
		private const string SINGLE_ZONE_FORMAT_STRING = @"https://api.qonqr.com/pub/ZoneData/Status/{0}";
		private const string AREA_FORMAT_STRING = @"https://api.qonqr.com/pub/ZoneData/BoundingBoxStatus/{0}/{1}/{2}/{3}";

		private readonly string m_ApiKey;

		[ContractInvariantMethod]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant()
		{
			Contract.Invariant(!string.IsNullOrWhiteSpace(m_ApiKey));
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

			m_ApiKey = apiKey;
		}

		private HttpWebRequest CreateApiRequest(Uri uri)
		{
			Contract.Requires(uri != null);
			Contract.Ensures(Contract.Result<WebRequest>() != null);

			var request = WebRequest.CreateHttp(uri);
			request.Headers[API_KEY_HEADER] = m_ApiKey;

			return request;
		}

		private void HandleResponse<T>(WebResponse x, IObserver<T> responseWatcher)
		{
			var response = x as HttpWebResponse;
			if (response == null)
			{
				responseWatcher.OnError(new Exception("Unknown web response"));
				return;
			}

			if (response.StatusCode != HttpStatusCode.OK)
			{
				responseWatcher.OnError(new Exception(string.Format("Server returned bad status code: {0}", response.StatusDescription)));
				return;
			}

			try
			{
				using (var rs = response.GetResponseStream())
				{
					using (var sr = new StreamReader(response.GetResponseStream()))
					{
						using (var reader = new JsonTextReader(sr))
						{
							JsonSerializer serializer = new JsonSerializer();
							var data = serializer.Deserialize<T>(reader);

							responseWatcher.OnNext(data);
							responseWatcher.OnCompleted();
						}
					}
				}
			}
			catch (Exception ex)
			{
				responseWatcher.OnError(new Exception("Error parsing response", ex));
				return;
			}
		}

		/// <summary>
		/// Gets the data for a single zone.
		/// </summary>
		/// <param name="zoneId">The zone identifier, this can be obtained from your scope by viewing the zone 
		/// summary and dropping the z prefix or by using the QueryRegion call.</param>
		/// <returns>An IObservable that will produce a single <see cref="IZone"/> before completing.</returns>
		public IObservable<IZone> QueryZone(uint zoneId)
		{
			Contract.Ensures(Contract.Result<IObservable<IZone>>() != null);

			var uri = new Uri(string.Format(SINGLE_ZONE_FORMAT_STRING, zoneId));
			var request = CreateApiRequest(uri);

			var asyncServiceCallWrapper = new AsyncSubject<Zone>();

			Task<WebResponse>
				.Factory
				.FromAsync(request.BeginGetResponse, request.EndGetResponse, TaskCreationOptions.None)
				.ToObservable()
				.Subscribe(
					x =>
					{
						HandleResponse(x, asyncServiceCallWrapper);
					},
					asyncServiceCallWrapper.OnError
				);

			return asyncServiceCallWrapper.AsObservable<IZone>();
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
		public IObservable<IEnumerable<IZone>> QueryRegion(decimal topLat, decimal leftLon, decimal bottomLat, decimal rightLon)
		{
			Contract.Requires<ArgumentOutOfRangeException>(-90m <= topLat && topLat <= 90m);
			Contract.Requires<ArgumentOutOfRangeException>(-180m <= leftLon && leftLon <= 180m);
			Contract.Requires<ArgumentOutOfRangeException>(-90m <= bottomLat && bottomLat <= 90m);
			Contract.Requires<ArgumentOutOfRangeException>(-180m <= rightLon && rightLon <= 180m);
			Contract.Ensures(Contract.Result<IObservable<IEnumerable<IZone>>>() != null);

			var uri = new Uri(string.Format(AREA_FORMAT_STRING, topLat, leftLon, bottomLat, rightLon));
			var request = CreateApiRequest(uri);

			var asyncServiceCallWrapper = new AsyncSubject<ZoneGroup>();

			Task<WebResponse>
				.Factory
				.FromAsync(request.BeginGetResponse, request.EndGetResponse, TaskCreationOptions.None)
				.ToObservable()
				.Subscribe(
					x =>
					{
						HandleResponse(x, asyncServiceCallWrapper);
					},
					asyncServiceCallWrapper.OnError
				);

			return asyncServiceCallWrapper
				.Select(zg => zg.Zones)
				.AsObservable();
		}
	}
}