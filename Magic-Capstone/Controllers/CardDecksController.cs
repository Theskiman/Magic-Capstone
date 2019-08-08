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
using Magic_Capstone.Models.DeckViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Magic_Capstone.Controllers
{
    public class CardDecksController : Controller
    {
        private readonly ApplicationDbContext _context;

        private Task<ApplicationUser> GetCurrentUserAsync() =>
            _userManager.GetUserAsync(HttpContext.User);

        private readonly UserManager<ApplicationUser> _userManager;

        public CardDecksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Gets all CardData instances in database and also gets all decks associated with the user
        [Authorize]
        public async Task<IActionResult> CardDeck()
        {
            var currentUser = await GetCurrentUserAsync();
           var userId = currentUser.Id;

            var viewModel = new CardDeckViewModel
            {
                AvailableDecks = await _context.decks.Where(d => d.UserId == userId).ToListAsync(),

            };
            
            viewModel.cardDatas = _context.cardDatas.ToList();
            
            return View(viewModel);
        }


      
        // POST: CardDecks/Create
       //Method to Save CardDatas to the CardDeck join table in order to create decks for the user method does a foreach 
       //with the Ids of the cards in order to allow the user to check multiple cards and save them all at once
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCard(CardDeck cardDeck, List<int> CardIds)
        {
            string referer = Request.Headers["Referer"].ToString();
            ModelState.Remove("CardDataId");
            if (ModelState.IsValid)
            {
                CardDeckViewModel viewModel = new CardDeckViewModel();

                cardDeck.DeckId = cardDeck.Deck.DeckId;
                
                cardDeck.Deck = null;
                cardDeck.CardData = null;

                viewModel.CardDataIds = CardIds;
                
                
                foreach(var id in CardIds)
                {
                    CardDeck newCard = new CardDeck
                    {
                        DeckId = cardDeck.DeckId,
                        CardDataId = id
                    };
                    _context.Add(newCard);
                };
                
                await _context.SaveChangesAsync();
                
            }
            
            return RedirectToAction("Index", "Decks");
        }


     




        // POST: CardDecks/Delete/5
        //Delete Method that deletes The join table item by its Id
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int CardDeckid)
        {
            string referer = Request.Headers["Referer"].ToString();
            var cardDeck = await _context.cardDecks.FindAsync(CardDeckid);
            _context.cardDecks.Remove(cardDeck);
            await _context.SaveChangesAsync();
            return Redirect(referer);
        }
        public  List<Deck> GetAllDecks()
        {
            List<Deck> decks =  _context.decks.ToList();
            return decks;
        }
        private bool CardDeckExists(int id)
        {
            return _context.cardDecks.Any(e => e.CardDeckId == id);
        }
    }
}
