using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationAchats.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebApplicationAchats.Controllers
{
    [Authorize] // Changé : Authentification obligatoire pour tous
    public class ProduitsController : Controller
    {
        private readonly VenteContext _context;

        public ProduitsController(VenteContext context)
        {
            _context = context;
        }

        // GET: Produits
        [Authorize(Roles = "Admin,Manager,User")]
        public async Task<IActionResult> Index(string? searchString, string? filter)
        {
            var produits = _context.Produits
                .Include(p => p.Categorie)
                .Include(p => p.Marque)
                .AsQueryable();

            // Filtre par recherche (existant)
            if (!string.IsNullOrEmpty(searchString))
            {
                produits = produits.Where(p => p.Nom.Contains(searchString));
            }

            // Filtre par disponibilité (NOUVEAU)
            if (filter == "disponible")
            {
                produits = produits.Where(p => p.Disponible == true);
            }
            else if (filter == "indisponible")
            {
                produits = produits.Where(p => p.Disponible == false);
            }

            ViewData["CurrentFilter"] = searchString;
            ViewData["FilterDisponibilite"] = filter;
            // Compteur de produits disponibles
            var produitsDisponibles = await _context.Produits.Where(p => p.Disponible == true).CountAsync();
            ViewBag.NombreDisponibles = produitsDisponibles;
            // Statistiques par catégorie
            ViewBag.StatsParCategorie = await _context.Produits
        .Include(p => p.Categorie)
        .GroupBy(p => p.Categorie.Nom)
        .Select(g => new
        {
            Categorie = g.Key,
            Total = g.Count()
        })
        .ToListAsync();
            // Jointure LINQ
            ViewBag.JoinProduitsCategories =
        (from p in _context.Produits
         join c in _context.Categories
         on p.CategorieId equals c.CategorieId
         select new
         {
             Produit = p.Nom,
             Prix = p.Prix,
             Categorie = c.Nom
         }).ToList();

            return View(await produits.ToListAsync());
        }

        // GET: Produits/Details/5 - Accessible par tous
        [Authorize(Roles = "Admin,Manager,User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produit = await _context.Produits
                .Include(p => p.Categorie)
                .Include(p => p.Marque)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        // GET: Produits/Create - Admin uniquement
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CategorieId"] = new SelectList(_context.Categories, "CategorieId", "Nom");
            ViewData["MarqueId"] = new SelectList(_context.Marques, "MarqueId", "Nom");
            return View();
        }

        // POST: Produits/Create - Admin uniquement
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Nom,Description,Prix,Quantite,DateAjout,Disponible,CategorieId,MarqueId")] Produit produit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategorieId"] = new SelectList(_context.Categories, "CategorieId", "Nom", produit.CategorieId);
            ViewData["MarqueId"] = new SelectList(_context.Marques, "MarqueId", "Nom", produit.MarqueId);
            return View(produit);
        }

        // GET: Produits/Edit/5 - Admin et Manager
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produit = await _context.Produits.FindAsync(id);
            if (produit == null)
            {
                return NotFound();
            }
            ViewData["CategorieId"] = new SelectList(_context.Categories, "CategorieId", "Nom", produit.CategorieId);
            ViewData["MarqueId"] = new SelectList(_context.Marques, "MarqueId", "Nom", produit.MarqueId);
            return View(produit);
        }

        // POST: Produits/Edit/5 - Admin et Manager
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Description,Prix,Quantite,DateAjout,Disponible,CategorieId,MarqueId")] Produit produit)
        {
            if (id != produit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProduitExists(produit.Id))
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
            ViewData["CategorieId"] = new SelectList(_context.Categories, "CategorieId", "Nom", produit.CategorieId);
            ViewData["MarqueId"] = new SelectList(_context.Marques, "MarqueId", "Nom", produit.MarqueId);
            return View(produit);
        }

        // GET: Produits/Delete/5 - Admin uniquement
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produit = await _context.Produits
                .Include(p => p.Categorie)
                .Include(p => p.Marque)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        // POST: Produits/Delete/5 - Admin uniquement
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produit = await _context.Produits.FindAsync(id);
            if (produit != null)
            {
                _context.Produits.Remove(produit);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProduitExists(int id)
        {
            return _context.Produits.Any(e => e.Id == id);
        }
    }
}