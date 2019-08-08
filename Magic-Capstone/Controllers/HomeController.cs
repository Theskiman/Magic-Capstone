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
using Microsoft.AspNetCore.Authorization;

namespace Magic_Capstone.Controllers
{
    [Authorize]
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

       //Method that takes the string passed into the input field and pings the external api for type based on the string
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
        //Method that takes the string passed into the input field and pings the external api for Name based on the string
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
        //Method that takes the string passed into the input field and pings the external api for color based on the string
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

        //Method to get the last 20 cards added to the database and displays them on the home page under the search bars
        public async Task<IActionResult> Index()
        {

            var currentuser = await GetCurrentUserAsync();
           
                var userId = currentuser.Id;

            var applicationDbContext = _context.cardDatas
                    .Where(cd => cd.UserId == userId)
                    .Take(20);

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
