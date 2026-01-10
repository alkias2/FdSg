using Fd.Data.Domain;
using Fd.Data.Domain.StormGlass;

namespace Fd.Web.Models {
	public class CatcheModel {
		public DateTime FishTime { get; set; }
		public Solunar? Solunar { get; set; }
		public Tide? Tide { get; set; }
		public Whether? Whether { get; set; }
		public string? Image { get; set; }
		public double? MoonPhase { get; set; }  // 0-1: New Moon (0) -> Full Moon (0.5) -> New Moon (1)
		
		// Location information
		public string? LocationName { get; set; }
		public string? District { get; set; }
		public double? Latitude { get; set; }
		public double? Longitude { get; set; }
	}
}
