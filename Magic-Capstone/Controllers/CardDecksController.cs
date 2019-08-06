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

        // GET: CardDecks all cardDatas to display and save cards to a new deck
   
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCard(CardDeck cardDeck)
        {
            string referer = Request.Headers["Referer"].ToString();
            ViewData["UserId"] = GetCurrentUserAsync().Id;
            if (ModelState.IsValid)
            {
                cardDeck.DeckId = cardDeck.Deck.DeckId;
                cardDeck.Deck = null;
                cardDeck.CardData = null;
                
                _context.Add(cardDeck);
                
                await _context.SaveChangesAsync();
                
            }
            
            return Redirect(referer);
        }
      

        // GET: CardDecks/Details/5
      /*  public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cardDeck = await _context.cardDecks
                .Include(c => c.Card)
                .Include(c => c.Deck)
                .FirstOrDefaultAsync(m => m.CardDeckId == id);
            if (cardDeck == null)
            {
                return NotFound();
            }

            return View(cardDeck);
        }*/


        // GET: CardDecks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cardDeck = await _context.cardDecks.FindAsync(id);
            if (cardDeck == null)
            {
                return NotFound();
            }
            ViewData["CardId"] = new SelectList(_context.cards, "CardId", "CardId", cardDeck.CardDataId);
            ViewData["DeckId"] = new SelectList(_context.decks, "DeckId", "DeckId", cardDeck.DeckId);
            return View(cardDeck);
        }

        // POST: CardDecks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
     

        // GET: CardDecks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cardDeck = await _context.cardDecks
                .Include(c => c.CardData)
                .Include(c => c.Deck)
                .FirstOrDefaultAsync(m => m.CardDeckId == id);
            if (cardDeck == null)
            {
                return NotFound();
            }

            return View(cardDeck);
        }

        // POST: CardDecks/Delete/5
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
