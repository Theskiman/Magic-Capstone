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
    public class CardsController : Controller
    {
        private readonly ApplicationDbContext _context;
        

        private readonly UserManager<ApplicationUser> _userManager;

        public CardsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public CardsController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: Cards
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.cards.Include(c => c.Condition).Include(c => c.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Cards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.cards
                .Include(c => c.Condition)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CardId == id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        // GET: Cards/Create
        public IActionResult Create()
        {
            ViewData["ConditionId"] = new SelectList(_context.conditions, "ConditionId", "ConditionId");
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View();
        }

        // POST: Cards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CardId,ConditionId,name,manaCost,type,rarity,set,setName,text,imageUrl,UserId")] Card card)
        {
            if (ModelState.IsValid)
            {
                _context.Add(card);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConditionId"] = new SelectList(_context.conditions, "ConditionId", "ConditionId", card.ConditionId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", card.UserId);
            return View(card);
        }

        // GET: Cards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }
            ViewData["ConditionId"] = new SelectList(_context.conditions, "ConditionId", "ConditionId", card.ConditionId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", card.UserId);
            return View(card);
        }

        // POST: Cards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CardId,ConditionId,name,manaCost,type,rarity,set,setName,text,imageUrl,UserId")] Card card)
        {
            if (id != card.CardId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(card);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardExists(card.CardId))
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
            ViewData["ConditionId"] = new SelectList(_context.conditions, "ConditionId", "ConditionId", card.ConditionId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", card.UserId);
            return View(card);
        }

        // GET: Cards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.cards
                .Include(c => c.Condition)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CardId == id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        // POST: Cards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var card = await _context.cards.FindAsync(id);
            _context.cards.Remove(card);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CardExists(int id)
        {
            return _context.cards.Any(e => e.CardId == id);
        }
    }
}
