using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Magic_Capstone.Data;
using Magic_Capstone.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Magic_Capstone.Controllers
{
    public class CardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private Task<ApplicationUser> GetCurrentUserAsync() =>
            _userManager.GetUserAsync(HttpContext.User);

        private readonly UserManager<ApplicationUser> _userManager;

        public CardsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

     
        //Simple Get all method getting all cardData instances in database to display to the user (based on user Id on card)
        // GET: Cards
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.cardDatas.Include(c => c.User);
            return View(await applicationDbContext.ToListAsync());
        }



   

        // POST: Cards/Create
        //Save method for when the user searches the API for cards and clicks on a card the associated card is saved as a CardData item to the Database
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CardId,ConditionId,name,manaCost,type,rarity,text,imageUrl,UserId")] CardData card)
        {
            string referer = Request.Headers["Referer"].ToString();
            ViewData["UserId"] = GetCurrentUserAsync().Id;
            if (ModelState.IsValid)
            {
               var user = await GetCurrentUserAsync();
                card.UserId = user.Id;

                _context.Add(card); 
                await _context.SaveChangesAsync();
                
                
            }
            return Redirect(referer);
            
        }
       

       

       

       
        //Delete method to delete a cardData item by its given ID
        // POST: Cards/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int CardDataid)
        {
            string referer = Request.Headers["Referer"].ToString();
            var card = await _context.cardDatas.FindAsync(CardDataid);
            _context.cardDatas.Remove(card);
            await _context.SaveChangesAsync();
            return Redirect(referer);
        }

        private bool CardExists(int id)
        {
            return _context.cards.Any(e => e.CardId == id);
        }
    }
}
