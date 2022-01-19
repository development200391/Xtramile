using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using Xtramile.Models;

namespace Xtramile.Controllers
{
    public class HomeController : Controller
    {


        private readonly ILogger<HomeController> _logger;
        private readonly IMemoryCache _memoryCache;
        public HomeController(ILogger<HomeController> logger, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            EntryData entryData = new EntryData();
            entryData.CityList = new List<SelectListItem>();

            entryData.CityList.Add(new SelectListItem { Value = "0", Text = "Select" });
            return View(entryData);
        }

        public async Task<JsonResult> selectCountry(string country)
        {
            List<SelectListItem> city = await PopulateCity(country);
            return Json(city);
        }

        public async Task<JsonResult> selectCity(string city)
        {
            County result = new County();
            APIByIDRs api = new APIByIDRs();

            if (city == null || city == "--Select City--")
            {
                return Json(result);
            }

            //with cache
            //bool isExist = _memoryCache.TryGetValue("Country", out api);
            //if (!isExist)
            //{
            //    return Json(result);
            //}

            try
            {

                //Hosted web API REST Service base url
                string Baseurl = "https://openweathermap.org/";
                using (var client = new HttpClient())
                {
                    //Passing service base url
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    //Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //Sending request to find web api REST service resource GetAllCountry using HttpClient
                    HttpResponseMessage Res = await client.GetAsync("data/2.5/weather?id=" + city + "&units=metric&appid=439d4b804bc8187953eb36d2a8c26a02&callback=jQuery34108673920292694877_1642314853068&_=1642314853069");
                    //Checking the response is successful or not which is sent using HttpClient
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api
                        var cityResponse = Res.Content.ReadAsStringAsync().Result;
                        cityResponse = cityResponse.Substring(41, cityResponse.Length - 42);
                        //Deserializing the response recieved from web api and storing into the Country list
                        api = JsonConvert.DeserializeObject<APIByIDRs>(cityResponse);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return Json(api);
        }
        private async Task<List<SelectListItem>> PopulateCity(string country)
        {
            //Hosted web API REST Service base url
            string Baseurl = "https://openweathermap.org/";
            APIRs CountryInfo = new APIRs();



            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllCountry using HttpClient
                HttpResponseMessage Res = await client.GetAsync("data/2.5/find?q=" + country + "&type=like&sort=population&cnt=30&appid=439d4b804bc8187953eb36d2a8c26a02");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Country list
                    CountryInfo = JsonConvert.DeserializeObject<APIRs>(EmpResponse);
                }
            }


            //setting up cache options
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(60));
            //set Cache
            _memoryCache.Set("Country", CountryInfo, cacheEntryOptions);

            List<SelectListItem> branches = new List<SelectListItem>();
            foreach (var i in CountryInfo.list)
            {
                branches.Add(new SelectListItem { Value = i.id.ToString(), Text = i.sys.country });
            }

            return branches;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}