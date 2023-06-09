namespace HiveFS.WeatherService.Models;

public class WeatherForecast
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC * 9m / 5m);

    public string? Summary { get; set; }
}