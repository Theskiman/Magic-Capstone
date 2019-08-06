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

     

        // GET: Cards
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.cardDatas.Include(c => c.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Cards/Details/5
     /*   public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.cards
              
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CardId == id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }*/

   

        // POST: Cards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", card.UserId);
            return View(card);
        }

       

        // POST: Cards/Delete/5
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
