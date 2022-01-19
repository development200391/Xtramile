namespace Xtramile.Models
{
    public class APIRs
    {
        public string? message { get; set; }
        public string? cod { get; set; }
        public string? count { get; set; }
        public List<County> list { get; set; }
    }

    public class County
    {
        public int id { get; set; }
        public string? name { get; set; }
        public Coord? coord { get; set; }
        public Main? main { get; set; }
        public int dt { get; set; }
        public Wind wind { get; set; }
        public Sys sys { get; set; }
        public string? rain { get; set; }
        public string? snow { get; set; }
        public Clouds? clouds { get; set; }
        public List<Weather>? weather { get; set; }

    }
    public class Coord
    {
        public decimal lat { get; set; }
        public decimal lon { get; set; }

    }

    public class Main
    {
        public decimal temp { get; set; }
        public decimal feels_like { get; set; }
        public decimal temp_min { get; set; }
        public decimal temp_max { get; set; }
        public decimal pressure { get; set; }
        public decimal humidity { get; set; }
        public decimal sea_level { get; set; }
        public decimal grnd_level { get; set; }

    }

    public class Wind
    {
        public decimal speed { get; set; }
        public decimal deg { get; set; }
    }

    public class Sys
    {
        public string? country { get; set; }
    }

    public class Clouds
    {
        public int all { get; set; }
    }
    public class Weather
    {
        public int id { get; set; }
        public string? main { get; set; }
        public string? description { get; set; }
        public string? icon { get; set; }
    }

}
