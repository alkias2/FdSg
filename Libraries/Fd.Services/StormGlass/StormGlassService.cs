using Fd.Core;
using Fd.Data;
using Fd.Data.Domain;
using Fd.Data.Domain.StormGlass;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Fd.Services.StormGlass
{
    public class StormGlassService : IStormGlassService
	{
		//private const string AUTHORIZATION = "4e744e10-73f9-11ed-a654-0242ac130002-4e744e88-73f9-11ed-a654-0242ac130002"; //ftcalkias@gmail.com
		private const string AUTHORIZATION = "6703f5c2-7181-11ed-bce5-0242ac130002-6703f644-7181-11ed-bce5-0242ac130002"; //mamisma79@gmail.com
		//private const string AUTHORIZATION = "02219784-726d-11ee-8d52-0242ac130002-022197e8-726d-11ee-8d52-0242ac130002"; //alsotez@gmail.com


		private const string SGURL = "https://api.stormglass.io/v2/";
		private readonly ILogger<StormGlassService> _logger;
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public StormGlassService(ILogger<StormGlassService> logger, IServiceScopeFactory serviceScopeFactory) {
			_logger = logger;
			_serviceScopeFactory = serviceScopeFactory;
		}

		public DeserializeWeather? GetWeather(string timeStart, string timeEnd, Location? location) {
			if(location == null) 
				return null;
			List<string> parameters = new List<string>() {
				"airTemperature",
				"airTemperature80m",
				"pressure",
				"cloudCover",
				"humidity",
				"currentDirection",
				"currentSpeed",
				"gust",
				"seaLevel",
				"swellDirection",
				"swellHeight",
				"swellPeriod",
				"visibility",
				"waterTemperature",
				"waveDirection",
				"waveHeight",
				"wavePeriod",
				"windWaveDirection",
				"windDirection",
				"windSpeed",
			};
			var weatherToken =
				$"{SGURL}weather/point?lat={location.Lat.ToDotValue()}&lng={location.Lng.ToDotValue()}&params={string.Join(",",parameters)}&start={timeStart}&end={timeEnd}";

			var response = GetFromStormGlass(weatherToken);
			if (response != string.Empty) {
				DeserializeWeather? weather = JsonConvert.DeserializeObject<DeserializeWeather>(response);
				
				using (var scope = _serviceScopeFactory.CreateScope())
				{
					var context = scope.ServiceProvider.GetRequiredService<DataContext>();
					context.SgData.Add(new SgData
					{
						StartTime = timeStart.UnixToDtDateTime(),
						EndTime = timeEnd.UnixToDtDateTime(),
						Name = "Whether",
						RowData = response,
						LocationId = location.Id,
					});
					context.SaveChanges();
				}
				return weather;
			}
			return null;
		}

		public SolunarDeserialize? GetSolunar(string timeStart, string timeEnd, Location? location) {

			if (location == null)
				return null;

			var astronomicalToken = $"{SGURL}astronomy/point?lat={location.Lat.ToDotValue()}&lng={location.Lng.ToDotValue()}&start={timeStart}&end{timeEnd}";

			var response = GetFromStormGlass(astronomicalToken);
			if (response != string.Empty) {
				SolunarDeserialize? solunar = JsonConvert.DeserializeObject<SolunarDeserialize>(response);

				using (var scope = _serviceScopeFactory.CreateScope())
				{
					var context = scope.ServiceProvider.GetRequiredService<DataContext>();
					context.SgData.Add(new SgData
					{
						StartTime = timeStart.UnixToDtDateTime(),
						EndTime = timeEnd.UnixToDtDateTime(),
						Name = "Astronomy",
						RowData = response,
						LocationId = location.Id,
					});
					context.SaveChanges();
				}

				return solunar;
			}
			return null;
		}

		public DeserializeTide? GetTides(string timeStart, string timeEnd, Location? location) {
			if (location == null)
				return null;

			var tidesToken = $"{SGURL}tide/extremes/point?lat={location.Lat.ToDotValue()}&lng={location.Lng.ToDotValue()}&start={timeStart}&end={timeEnd}";
			var response = GetFromStormGlass(tidesToken);

			if (response != string.Empty) {
				DeserializeTide? dataTide = JsonConvert.DeserializeObject<DeserializeTide>(response);

				using (var scope = _serviceScopeFactory.CreateScope())
				{
					var context = scope.ServiceProvider.GetRequiredService<DataContext>();
					context.SgData.Add(new SgData
					{
						StartTime = timeStart.UnixToDtDateTime(),
						EndTime = timeEnd.UnixToDtDateTime(),
						Name = "tide",
						RowData = response,
						LocationId = location.Id,
					});
					context.SaveChanges();
				}

				return dataTide;
			}

			return null;
		}


		private string GetFromStormGlass(string token)
		{
			try {
				using var httpClient = new HttpClient();

				var request = new HttpRequestMessage(HttpMethod.Get, token);

				request.Headers.Add("Accept", "application/json");
				request.Headers.Add("Authorization", AUTHORIZATION);
				httpClient.Timeout = new TimeSpan(0, 0, 0, 24);

				var response = httpClient.Send(request);
				using var reader = new StreamReader(response.Content.ReadAsStream());
				var responseBody = reader.ReadToEnd();

				return responseBody;
			}
			catch (Exception ex) {
				_logger.LogError(ex.Message);
				return string.Empty;
			}
			
		}

    }
}