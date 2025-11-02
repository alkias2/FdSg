using Fd.Data.Domain;
using Fd.Data.Domain.StormGlass;

namespace Fd.Services.StormGlass;

public interface IStormGlassService
{
	
    SolunarDeserialize? GetSolunar(string timeStart, string timeEnd, Location? location);
    DeserializeTide? GetTides(string timeStart, string timeEnd, Location? location);
    DeserializeWeather? GetWeather(string timeStart, string timeEnd, Location? location);
}