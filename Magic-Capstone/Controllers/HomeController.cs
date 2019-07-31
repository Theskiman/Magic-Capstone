using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Magic_Capstone.Models;
using Magic_Capstone.Data;
using Microsoft.AspNetCore.Identity;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace Magic_Capstone.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

       public async Task<IActionResult> FindType(string type)
        {
            ApiHelper.InitializeClient();
            string url = $"https://api.magicthegathering.io/v1/cards?type={type}";
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var card = await response.Content.ReadAsAsync<Rootobject>();

                    return View(card);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

        }
        public async Task<IActionResult> FindName(string name)
        {
            ApiHelper.InitializeClient();
            string url = $"https://api.magicthegathering.io/v1/cards?name={name}";
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var card = await response.Content.ReadAsAsync<Rootobject>();
              
                    return View(card);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

        }
        public async Task<IActionResult> FindColor(string color)
        {
            ApiHelper.InitializeClient();
            string url = $"https://api.magicthegathering.io/v1/cards?colors={color}";
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var card = await response.Content.ReadAsAsync<Rootobject>();

                    return View(card);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.cardDatas.Take(20);
            return View(await applicationDbContext.ToListAsync());
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
