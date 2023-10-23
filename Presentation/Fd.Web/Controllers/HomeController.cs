﻿using System.Diagnostics;
using Fd.Core;
using Fd.Core.Infrastructure;
using Fd.Data;
using Fd.Data.Domain;
using Fd.Data.Domain.StormGlass;
using Fd.Data.StormGlass;
using Fd.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fd.Web.Controllers {
	public class HomeController : Controller {
		private readonly ILogger<HomeController> _logger;
		private readonly IStormGlassData _stormGlassData;
		private readonly DataContext _dataContext;

		public HomeController(ILogger<HomeController> logger, IStormGlassData stormGlassData, DataContext dataContext) {
			_logger = logger;
			_stormGlassData = stormGlassData;
			_dataContext = dataContext;
		}

		public IActionResult Index() {

			//return View();

			var locs = _dataContext.Location.ToList();

			//assigns year, month, day
			var startDate = new DateTime(2021, 10, 30);
			var endDate = new DateTime(2021, 10, 30);

			//var startDate = new DateTime(2023, 10, 23);
			//var endDate = new DateTime(2023, 10, 23);

			var weather = GetWetherData(startDate, endDate, locs);

			var tides = GetTidesData(startDate, endDate, locs);

			var solunar = GetSolunarData(startDate, endDate, locs);

			return View();
		}

		private SolunarDeserialize? GetSolunarData(DateTime startDate, DateTime endDate, List<Location> locs) {
			var solunar = _stormGlassData.GetSolunar(startDate.Floor().ToUnix(), endDate.Ceil().ToUnix(), locs.First());
			if (solunar != null) {
				foreach (var sol in solunar.data) {
					var s = new Solunar {
						Date = sol?.time,
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
						LocationId = locs.First().Id
					};
					_dataContext.Solunar.Add(s);
				}

				_dataContext.SaveChanges();
			}

			return solunar;
		}

		private DeserializeTide? GetTidesData(DateTime startDate, DateTime endDate, List<Location> locs) {
			var tides = _stormGlassData.GetTides(startDate.Floor().ToUnix(), endDate.Ceil().ToUnix(), locs.First());
			if (tides != null && tides.data.Any()) {
				foreach (var tide in tides.data) {
					var t = new Tide {
						Height = tide?.height,
						Date = tide?.time,
						Type = tide?.type,
						LocationId = locs.First().Id
					};
					_dataContext.Tide.Add(t);
				}

				_dataContext.SaveChanges();
			}

			return tides;
		}

		private DeserializeWeather? GetWetherData(DateTime startDate, DateTime endDate, List<Location> locs) {
			var weather = _stormGlassData.GetWeather(startDate.Floor().ToUnix(), endDate.Ceil().ToUnix(), locs.First());
			if (weather != null && weather.hours.Any()) {
				foreach (var hour in weather.hours) {
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
						LocationId = locs.First().Id,
					};
					_dataContext.Whether.Add(w);
				}

				_dataContext.SaveChanges();
			}

			return weather;
		}

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