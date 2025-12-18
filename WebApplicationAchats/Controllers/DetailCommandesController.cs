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
    public class DetailCommandesController : Controller
    {
        private readonly VenteContext _context;

        public DetailCommandesController(VenteContext context)
        {
            _context = context;
        }

        // GET: DetailCommandes
        public async Task<IActionResult> Index()
        {
            var venteContext = _context.DetailCommandes.Include(d => d.IdCommandeNavigation).Include(d => d.IdProduitNavigation);
            return View(await venteContext.ToListAsync());
        }

        // GET: DetailCommandes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailCommande = await _context.DetailCommandes
                .Include(d => d.IdCommandeNavigation)
                .Include(d => d.IdProduitNavigation)
                .FirstOrDefaultAsync(m => m.IdDetail == id);
            if (detailCommande == null)
            {
                return NotFound();
            }

            return View(detailCommande);
        }

        // GET: DetailCommandes/Create
        public IActionResult Create()
        {
            ViewData["IdCommande"] = new SelectList(_context.Commandes, "Numcom", "Numcom");
            ViewData["IdProduit"] = new SelectList(_context.Produits, "Id", "Id");
            return View();
        }

        // POST: DetailCommandes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDetail,IdCommande,IdProduit,Quantite")] DetailCommande detailCommande)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detailCommande);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCommande"] = new SelectList(_context.Commandes, "Numcom", "Numcom", detailCommande.IdCommande);
            ViewData["IdProduit"] = new SelectList(_context.Produits, "Id", "Id", detailCommande.IdProduit);
            return View(detailCommande);
        }

        // GET: DetailCommandes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailCommande = await _context.DetailCommandes.FindAsync(id);
            if (detailCommande == null)
            {
                return NotFound();
            }
            ViewData["IdCommande"] = new SelectList(_context.Commandes, "Numcom", "Numcom", detailCommande.IdCommande);
            ViewData["IdProduit"] = new SelectList(_context.Produits, "Id", "Id", detailCommande.IdProduit);
            return View(detailCommande);
        }

        // POST: DetailCommandes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDetail,IdCommande,IdProduit,Quantite")] DetailCommande detailCommande)
        {
            if (id != detailCommande.IdDetail)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detailCommande);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetailCommandeExists(detailCommande.IdDetail))
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
            ViewData["IdCommande"] = new SelectList(_context.Commandes, "Numcom", "Numcom", detailCommande.IdCommande);
            ViewData["IdProduit"] = new SelectList(_context.Produits, "Id", "Id", detailCommande.IdProduit);
            return View(detailCommande);
        }

        // GET: DetailCommandes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailCommande = await _context.DetailCommandes
                .Include(d => d.IdCommandeNavigation)
                .Include(d => d.IdProduitNavigation)
                .FirstOrDefaultAsync(m => m.IdDetail == id);
            if (detailCommande == null)
            {
                return NotFound();
            }

            return View(detailCommande);
        }

        // POST: DetailCommandes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detailCommande = await _context.DetailCommandes.FindAsync(id);
            if (detailCommande != null)
            {
                _context.DetailCommandes.Remove(detailCommande);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetailCommandeExists(int id)
        {
            return _context.DetailCommandes.Any(e => e.IdDetail == id);
        }
    }
}
