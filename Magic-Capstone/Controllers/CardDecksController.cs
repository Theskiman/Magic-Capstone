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

        // GET: CardDecks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.cardDecks.Include(c => c.Card).Include(c => c.Deck);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CardDecks/Details/5
        public async Task<IActionResult> Details(int? id)
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
        }


        // POST: CardDecks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCard([Bind("CardDeckId,CardId,DeckId")] CardDeck cardDeck)
        {
            string referer = Request.Headers["Referer"].ToString();
            ViewData["UserId"] = GetCurrentUserAsync().Id;
            if (ModelState.IsValid)
            {
                _context.Add(cardDeck);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
         
            return Redirect(referer);
        }
      
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
            ViewData["CardId"] = new SelectList(_context.cards, "CardId", "CardId", cardDeck.CardId);
            ViewData["DeckId"] = new SelectList(_context.decks, "DeckId", "DeckId", cardDeck.DeckId);
            return View(cardDeck);
        }

        // POST: CardDecks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CardDeckId,CardId,DeckId")] CardDeck cardDeck)
        {
            if (id != cardDeck.CardDeckId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cardDeck);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardDeckExists(cardDeck.CardDeckId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CardId"] = new SelectList(_context.cards, "CardId", "CardId", cardDeck.CardId);
            ViewData["DeckId"] = new SelectList(_context.decks, "DeckId", "DeckId", cardDeck.DeckId);
            return View(cardDeck);
        }

        // GET: CardDecks/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
        }

        // POST: CardDecks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cardDeck = await _context.cardDecks.FindAsync(id);
            _context.cardDecks.Remove(cardDeck);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CardDeckExists(int id)
        {
            return _context.cardDecks.Any(e => e.CardDeckId == id);
        }
    }
}
