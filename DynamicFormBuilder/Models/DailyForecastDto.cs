namespace DynamicFormBuilder.Models
{
    public class DailyForecastDto
    {
        public long Dt { get; set; }
        public Temp Temp { get; set; }
        public Weather[] Weather { get; set; }
    }

    public class Temp
    {
        public double Day { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
    }
}
