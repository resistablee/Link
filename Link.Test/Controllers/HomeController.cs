using Figensoft.NET.Framework.Caching.Interfaces;
using Figensoft.NET.Framework.Configuration.Interface;
using Figensoft.NET.Framework.Enums;
using Link.Models;
using Link.Test.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Link.Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPGeolocation _IPGeolocation;

        public HomeController(ICaching caching, IConfig config)
        {
            _IPGeolocation = new IPGeolocation(caching, config);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ParsedUserAgent()
        {
            UserAgentModel userAgentInfo = UserAgent.Parse(Request.Headers["User-Agent"].ToString());
            return View(userAgentInfo);
        }

        public IActionResult IPGeolocationInfo()
        {
            string ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            ip = "88.238.56.6";

            var ipLocate = _IPGeolocation.Check(ip);
            if (!Figensoft.NET.Framework.Enums.StatusCode.SUCCEED.Equals(ipLocate.Status))
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

            return View(model: ipLocate.Result);
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