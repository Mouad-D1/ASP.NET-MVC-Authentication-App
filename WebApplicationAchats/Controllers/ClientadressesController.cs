using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationAchats.Models;

namespace WebApplicationAchats.Controllers
{
    public class ClientadressesController : Controller
    {
        private readonly VenteContext _context;

        public ClientadressesController(VenteContext context)
        {
            _context = context;
        }

        // GET: Clientadresses
        public async Task<IActionResult> Index()
        {
            var venteContext = _context.Clientadresses.Include(c => c.NumcliNavigation);
            return View(await venteContext.ToListAsync());
        }

        // GET: Clientadresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientadress = await _context.Clientadresses
                .Include(c => c.NumcliNavigation)
                .FirstOrDefaultAsync(m => m.Numcli == id);
            if (clientadress == null)
            {
                return NotFound();
            }

            return View(clientadress);
        }

        // GET: Clientadresses/Create
        public IActionResult Create()
        {
            ViewData["Numcli"] = new SelectList(_context.Clients, "Numcli", "Numcli");
            return View();
        }

        // POST: Clientadresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Numcli,Address1,State")] Clientadress clientadress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clientadress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Numcli"] = new SelectList(_context.Clients, "Numcli", "Numcli", clientadress.Numcli);
            return View(clientadress);
        }

        // GET: Clientadresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientadress = await _context.Clientadresses.FindAsync(id);
            if (clientadress == null)
            {
                return NotFound();
            }
            ViewData["Numcli"] = new SelectList(_context.Clients, "Numcli", "Numcli", clientadress.Numcli);
            return View(clientadress);
        }

        // POST: Clientadresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Numcli,Address1,State")] Clientadress clientadress)
        {
            if (id != clientadress.Numcli)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clientadress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientadressExists(clientadress.Numcli))
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
            ViewData["Numcli"] = new SelectList(_context.Clients, "Numcli", "Numcli", clientadress.Numcli);
            return View(clientadress);
        }

        // GET: Clientadresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientadress = await _context.Clientadresses
                .Include(c => c.NumcliNavigation)
                .FirstOrDefaultAsync(m => m.Numcli == id);
            if (clientadress == null)
            {
                return NotFound();
            }

            return View(clientadress);
        }

        // POST: Clientadresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clientadress = await _context.Clientadresses.FindAsync(id);
            if (clientadress != null)
            {
                _context.Clientadresses.Remove(clientadress);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientadressExists(int id)
        {
            return _context.Clientadresses.Any(e => e.Numcli == id);
        }
    }
}
