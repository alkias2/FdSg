using System.Diagnostics;
using Fd.Core;
using Fd.Core.Infrastructure;
using Fd.Data;
using Fd.Data.Domain;
using Fd.Data.Domain.StormGlass;
using Fd.Services.StormGlass;
using Fd.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Fd.Web.Controllers {
	public class HomeController : Controller {
		private readonly ILogger<HomeController> _logger;
		private readonly IStormGlassService _stormGlassService;
		private readonly DataContext _dataContext;
		private readonly IServiceScopeFactory _serviceScopeFactory;

		/// <summary>
		/// Initializes a new instance of the <see cref="HomeController"/> class.
		/// </summary>
		/// <param name="logger">The logger instance.</param>
		/// <param name="stormGlassService">The StormGlass service for weather and marine data.</param>
		/// <param name="dataContext">The database context.</param>
		public HomeController(
			ILogger<HomeController> logger, 
			IStormGlassService stormGlassService, 
			DataContext dataContext,
			IServiceScopeFactory serviceScopeFactory) {
			_logger = logger;
			_stormGlassService = stormGlassService;
			_dataContext = dataContext;
			_serviceScopeFactory = serviceScopeFactory;
		}

		/// <summary>
		/// Main action that displays fishing catch data with associated weather, tide, and solunar information.
		/// Retrieves and updates weather data for specified dates and shows historical catch information.
		/// </summary>
		/// <returns>A view containing fishing catch data with associated environmental conditions.</returns>
		public async Task<IActionResult>  Index()
		{
			//return View();

			var locs = await _dataContext.Location.FindAsync((long)1);

			//assigns year, month, day
			var startDate = new DateTime(2023, 11, 13);
			var endDate = new DateTime(2023, 11,14);
			
			try {
				await Task.WhenAll(
					Task.Run(() => GetWetherData(startDate, endDate, locs)),
					Task.Run(() => GetTidesData(startDate, endDate, locs)),
					Task.Run(() => GetSolunarData(startDate, endDate, locs))
				);
			}
			catch (Exception ex) {
				_logger.LogError(ex, "Error fetching weather data");
				// Consider how you want to handle the error - maybe show a warning to the user
				// For now we'll continue to show any data we have in the database
			}

			var catcheDates = new List<DateTime> {
				new DateTime(2021, 10, 3, 9, 9, 44),
				new DateTime(2021, 10, 17, 20, 33, 53),
				new DateTime(2021, 10, 30, 14, 10, 48),
				new DateTime(2021, 11, 14, 12, 5, 55),
				new DateTime(2021, 11, 14, 12, 5, 55),
				new DateTime(2021, 11, 21, 9, 52, 31),
				new DateTime(2022, 11, 20, 15, 54, 29),
				new DateTime(2023, 10, 6, 8, 26, 36),
				new DateTime(2023, 10, 6, 9, 18, 1),
				new DateTime(2023, 10, 13, 18, 38, 54),
				new DateTime(2023, 10, 21, 7, 26, 13),
			};

			var cImg = new List<string>() {
				"20211003_090944.jpg",
				"20211017_203353.jpg",
				"20211030_141048.jpg",
				"20211114_120156.jpg",
				"20211121_095232.jpg",
				"20221120_155429.jpg",
				"20230618_173411.jpg",
				"20231006_082636.jpg",
				"20231006_091801.jpg",
				"20231013_183854.jpg",
				"20231021_072613.jpg",
			};

			var catches = new List<CatcheModel>();

			foreach (var catche in catcheDates) {
				var solunar = SolunarByDay(catche);
				var location = locs; // Currently using the same location for all catches
				catches.Add(new CatcheModel {
					FishTime = catche,
					Solunar = solunar,
					Tide = TideByDay(catche),
					Whether = WhetherByDay(catche),
					MoonPhase = solunar?.MoonFraction,
					LocationName = location?.Name,
					District = location?.District,
					Latitude = location?.Lat,
					Longitude = location?.Lng
				});
			}

			for (var i = 0; i < catches.Count; i++) {
				catches[i].Image = cImg[i];
			}

			//var fishingDate = new DateTime(2021, 10, 17, 20, 33, 53);
			//var catchWhether = WhetherByDay(fishingDate);
			//var catchTide = TideByDay(fishingDate);
			//var catchSolunar = SolunarByDay(fishingDate);

			return View(catches);
		}

		/// <summary>
		/// Retrieves the solunar data for a specific fishing date by finding the closest matching record.
		/// </summary>
		/// <param name="fishingDate">The date to find solunar data for.</param>
		/// <returns>The closest matching solunar data record, or null if none found.</returns>
		private Solunar? SolunarByDay(DateTime fishingDate)
		{
			using (var scope = _serviceScopeFactory.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
				var solDates = dbContext.Solunar.Select(x => x.Date!.Value).ToList();
				if (!solDates.Any()) return null;

				var solClosest = solDates.ArgMin(iTime => Math.Abs((iTime - fishingDate).Ticks));
				return dbContext.Solunar.FirstOrDefault(x => x.Date == solClosest);
			}
		}

		/// <summary>
		/// Retrieves the tide data for a specific fishing date by finding the closest matching record.
		/// </summary>
		/// <param name="fishingDate">The date to find tide data for.</param>
		/// <returns>The closest matching tide data record, or null if none found.</returns>
		private Tide? TideByDay(DateTime fishingDate)
		{
			using (var scope = _serviceScopeFactory.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
				var tideDates = dbContext.Tide.Select(x => x.Date!.Value).ToList();
				if (!tideDates.Any()) return null;

				var closestTimeDate = tideDates.ArgMin(iTime => Math.Abs((iTime - fishingDate).Ticks));
				return dbContext.Tide.FirstOrDefault(x => x.Date == closestTimeDate);
			}
		}

		/// <summary>
		/// Retrieves the weather data for a specific fishing date by finding the closest matching record.
		/// </summary>
		/// <param name="fishingDate">The date to find weather data for.</param>
		/// <returns>The closest matching weather data record, or null if none found.</returns>
		private Whether? WhetherByDay(DateTime fishingDate) {
			using (var scope = _serviceScopeFactory.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
				var weatherDates = dbContext.Whether.Select(x => x.Date).ToList();
				if (!weatherDates.Any()) return null;

				var closestWeatherDate = weatherDates.ArgMin(iTime => Math.Abs((iTime - fishingDate).Ticks));
				return dbContext.Whether.FirstOrDefault(x => x.Date == closestWeatherDate);
			}
		}

		/// <summary>
		/// Retrieves and updates solunar data from the StormGlass API for a given location and date range.
		/// </summary>
		/// <param name="startDate">The start date of the period to fetch data for.</param>
		/// <param name="endDate">The end date of the period to fetch data for.</param>
		/// <param name="location">The location to fetch data for.</param>
		/// <returns>The deserialized solunar data response, or null if location is null.</returns>
		private SolunarDeserialize? GetSolunarData(DateTime startDate, DateTime endDate, Location? location) {
            if (location == null) return null;

            var solunar = _stormGlassService.GetSolunar(startDate.Floor().ToUniversalTime().ToUnix(), endDate.Ceil().ToUniversalTime().ToUnix(), location);
			if (solunar?.data != null) {
				using (var scope = _serviceScopeFactory.CreateScope())
				{
					var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
					foreach (var sol in solunar.data) {
						if (sol?.time == null) continue;

						var s = new Solunar {
							Date = sol.time,
							SunRise = sol?.sunrise,
							SunSet = sol?.sunset,
							MoonRise = sol?.moonrise,
							MoonSet = sol?.moonset,
							MoonFraction = sol?.moonFraction,
							CivilDawn = sol?.civilDawn,
							CivilDusk = sol?.civilDusk,
							MoonClosestName = sol?.moonPhase?.closest?.text,
							MoonClosestTime = sol?.moonPhase?.closest?.time,
							MoonClosestValue = sol?.moonPhase?.closest?.value,
							MoonCurrentName = sol?.moonPhase?.current?.text,
							MoonCurrentTime = sol?.moonPhase?.current?.time,
							MoonCurrenttValue = sol?.moonPhase?.current?.value,
							LocationId = location.Id
						};

						var exists = sol?.time != null ? dbContext.Solunar.FirstOrDefault(s => 
							s.Date == sol.time && s.LocationId == location.Id) : null;

						if (exists != null) {
							exists.SunRise = sol?.sunrise;
							exists.SunSet = sol?.sunset;
							exists.MoonRise = sol?.moonrise;
							exists.MoonSet = sol?.moonset;
							exists.MoonFraction = sol?.moonFraction;
							exists.CivilDawn = sol?.civilDawn;
							exists.CivilDusk = sol?.civilDusk;
							exists.MoonClosestName = sol?.moonPhase?.closest?.text;
							exists.MoonClosestTime = sol?.moonPhase?.closest?.time;
							exists.MoonClosestValue = sol?.moonPhase?.closest?.value;
							exists.MoonCurrentName = sol?.moonPhase?.current?.text;
							exists.MoonCurrentTime = sol?.moonPhase?.current?.time;
							exists.MoonCurrenttValue = sol?.moonPhase?.current?.value;
						}
						else {
							dbContext.Solunar.Add(s);
						}
					}
					dbContext.SaveChanges();
				}
			}
			return solunar;
		}

		/// <summary>
		/// Retrieves and updates tide data from the StormGlass API for a given location and date range.
		/// </summary>
		/// <param name="startDate">The start date of the period to fetch data for.</param>
		/// <param name="endDate">The end date of the period to fetch data for.</param>
		/// <param name="location">The location to fetch data for.</param>
		/// <returns>The deserialized tide data response, or null if location is null.</returns>
		private DeserializeTide? GetTidesData(DateTime startDate, DateTime endDate, Location? location) {
            if (location == null) return null;
            
            var tides = _stormGlassService.GetTides(startDate.Floor().ToUniversalTime().ToUnix(), endDate.Ceil().ToUniversalTime().ToUnix(), location);
			if (tides?.data != null && tides.data.Any()) {
				using (var scope = _serviceScopeFactory.CreateScope())
				{
					var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
					foreach (var tide in tides.data) {
						var t = new Tide {
							Height = tide?.height,
							Date = tide?.time,
							Type = tide?.type,
							LocationId = location.Id
						};
						var exists = dbContext.Tide
							.Where(x => x.Date == tide!.time && x.LocationId == location.Id)
							.OrderBy(x=>x.Id)
							.LastOrDefault();

						if (exists != null) {
							exists.Height = tide?.height;
							exists.Date = tide?.time;
							exists.Type = tide?.type;
							exists.LocationId = location.Id;
						}
						else {
							dbContext.Tide.Add(t);
						}
					}
					dbContext.SaveChanges();
				}
			}
			return tides;
		}

		/// <summary>
		/// Retrieves and updates weather data from the StormGlass API for a given location and date range.
		/// </summary>
		/// <param name="startDate">The start date of the period to fetch data for.</param>
		/// <param name="endDate">The end date of the period to fetch data for.</param>
		/// <param name="location">The location to fetch data for.</param>
		/// <returns>The deserialized weather data response, or null if location is null.</returns>
		private DeserializeWeather? GetWetherData(DateTime startDate, DateTime endDate, Location? location) {
			if(location==null) return null;

			var weather = _stormGlassService.GetWeather(startDate.Floor().ToUniversalTime().ToUnix(), endDate.Ceil().ToUniversalTime().ToUnix(), location);
			if (weather?.hours != null && weather.hours.Any()) {
				using (var scope = _serviceScopeFactory.CreateScope())
				{
					var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
					foreach (var hour in weather.hours) {
						if (hour?.time == null) continue;

						var w = new Whether {
							Date = hour.time,
							AirTemperature = hour?.airTemperature?.sg,
							Pressure = hour?.pressure?.sg,
							CloudCover = hour?.cloudCover?.sg,
							CurrentDirection = hour?.currentDirection?.sg,
							CurrentSpeed = hour?.currentSpeed?.sg,
							Gust = hour?.gust?.sg,
							Humidity = hour?.humidity?.sg,
							SeaLevel = hour?.seaLevel?.sg,
							SwellDirection = hour?.swellDirection?.sg,
							SwellHeight = hour?.swellHeight?.sg,
							SwellPeriod = hour?.swellPeriod?.sg,
							waterTemperature = hour?.waterTemperature?.sg,
							waveDirection = hour?.waveDirection?.sg,
							waveHeight = hour?.waveHeight?.sg,
							wavePeriod = hour?.wavePeriod?.sg,
							windDirection = hour?.windDirection?.sg,
							windSpeed = hour?.windSpeed?.sg,
							LocationId = location.Id,
						};

						var exists = hour?.time != null ? dbContext.Whether.FirstOrDefault(x => x.Date == hour.time && x.LocationId == location.Id) : null;
						if (exists != null) {
							exists.AirTemperature = hour?.airTemperature?.sg;
							exists.Pressure = hour?.pressure?.sg;
							exists.CloudCover = hour?.cloudCover?.sg;
							exists.CurrentDirection = hour?.currentDirection?.sg;
							exists.CurrentSpeed = hour?.currentSpeed?.sg;
							exists.Gust = hour?.gust?.sg;
							exists.Humidity = hour?.humidity?.sg;
							exists.SeaLevel = hour?.seaLevel?.sg;
							exists.SwellDirection = hour?.swellDirection?.sg;
							exists.SwellHeight = hour?.swellHeight?.sg;
							exists.SwellPeriod = hour?.swellPeriod?.sg;
							exists.waterTemperature = hour?.waterTemperature?.sg;
							exists.waveDirection = hour?.waveDirection?.sg;
							exists.waveHeight = hour?.waveHeight?.sg;
							exists.wavePeriod = hour?.wavePeriod?.sg;
							exists.windDirection = hour?.windDirection?.sg;
							exists.windSpeed = hour?.windSpeed?.sg;
						}
						else {
							dbContext.Whether.Add(w);
						}
					}
					dbContext.SaveChanges();
				}
			}
			return weather;
		}

		/// <summary>
		/// Displays the privacy policy page.
		/// </summary>
		/// <returns>The privacy policy view.</returns>
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}