namespace HiveFS.WeatherData.Dtos;

public class Forecast
{
    public string day { get; set; }
    public string temperature { get; set; }
    public string wind { get; set; }
}

public class WeatherForecastDto
{
    public string temperature { get; set; }
    public string wind { get; set; }
    public string description { get; set; }
    public IEnumerable<Forecast> forecast { get; set; }
}