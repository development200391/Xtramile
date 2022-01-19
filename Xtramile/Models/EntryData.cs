using Microsoft.AspNetCore.Mvc.Rendering;

namespace Xtramile.Models
{
    public class EntryData
    {
        public string? Country { get; set; }
        public string? City { get; set; }
        public List<SelectListItem>? CityList { get; set; }
        public APIRs apirs { get; set; }
    }
}
