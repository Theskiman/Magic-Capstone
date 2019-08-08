using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Magic_Capstone.Data;
using Magic_Capstone.Models;
using Magic_Capstone.Models.DeckViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Magic_Capstone.Controllers
{
    [Authorize]
    public class DecksController : Controller
    {
        private readonly ApplicationDbContext _context;

        private Task<ApplicationUser> GetCurrentUserAsync() =>
            _userManager.GetUserAsync(HttpContext.User);

        private readonly UserManager<ApplicationUser> _userManager;

        public DecksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Decks
        //Simple method to get all decks associated with the current logged in user
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            var userid = user.Id;

            var applicationDbContext = _context.decks.Include(d => d.User)

                .Where(d => d.UserId == userid);
               
                
          
                
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Decks/Details/5
        //Method to show the details of its deck including all of the cards associated with the deck 
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deck = await _context.decks           
                .Include(c => c.User)
                .Include(c => c.cardDecks)
                .ThenInclude(cd => cd.CardData)
                .Where(c => c.DeckId == id)
                .FirstOrDefaultAsync(m => m.DeckId == id);
            
            var cards = _context.cardDatas.ToList();
            
            foreach (CardDeck item in _context.cardDecks)
            {
                if (item.DeckId == deck.DeckId)
                {
                    deck.cardDecks.Add(item);
                }
                
            }
                        
            if (deck == null)
            {
                return NotFound();
            }
            
            return View(deck);
        }


        // GET: Decks/Create
        //Method to create a deck then take you to the page that allows you to add cards to the deck
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View();
        }

        // POST: Decks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeckName,Description")] Deck deck)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await GetCurrentUserAsync();
                deck.UserId = currentUser.Id;
                _context.Add(deck);
                await _context.SaveChangesAsync();
                return RedirectToAction("CardDeck", "CardDecks");
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", deck.UserId);
            return View(deck);
        }

        // GET: Decks/Edit/5
        //Simple method allowing users to edit the decks Name and its Description
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deck = await _context.decks.FindAsync(id);
            if (deck == null)
            {
                return NotFound();
            }
            
           
            return View(deck);
        }

        // POST: Decks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DeckId,DeckName,Description,UserId")] Deck deck)
        {
            var currentUser = await GetCurrentUserAsync();
           
            
            if (id != deck.DeckId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    _context.Update(deck);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeckExists(deck.DeckId))
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
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", deck.UserId);
            return View(deck);
        }

        // GET: Decks/Delete/5
      //Delete method that allows users to delete a deck along with every cardDeck join table item associated with it based on the decks ID
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var currentuser = await GetCurrentUserAsync();

            var deck = await _context.decks
                .Include(d => d.User)
                .Include(d => d.cardDecks)
                .ThenInclude(cd => cd.CardData)
                .Where(d => d.DeckId == id)
                .FirstOrDefaultAsync(m => m.DeckId == id);
            if (deck == null)
            {
                return NotFound();
            }

            return View(deck);
        }

        // POST: Decks/Delete/5
       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await GetCurrentUserAsync();
            var userid = user.Id;
            var deck = await _context.decks.FindAsync(id);
            var cardDatas = _context.cardDatas;
            var cardDecks = _context.cardDecks;


            foreach (CardDeck item in cardDecks)
            {
                if (item.DeckId == deck.DeckId && userid == deck.UserId)
                {
                    cardDecks.Remove(item);
                }
            }
            if(userid == deck.UserId)
            {
                _context.decks.Remove(deck);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Decks");
        }
        //Method to get all CardDeck items associated with a deck based on the deckId
        public Deck DecksCards(Deck deck)
        {
            foreach (CardDeck item in _context.cardDecks)
            {
                if (item.DeckId == deck.DeckId)
                {
                    deck.cardDecks.Add(item);
                }
            }
            return (deck);
        }
        private bool DeckExists(int id)
        {
            return _context.decks.Any(e => e.DeckId == id);
        }
    }
}
