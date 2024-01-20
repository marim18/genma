using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GenmaWebApp.Data;
using GenmaWebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace GenmaWebApp.Controllers
{
    public class RecipeTagsController : Controller
    {
        
        private readonly ApplicationDbContext _context;

        public RecipeTagsController(ApplicationDbContext context)
        {
            _context = context;
        }
[Authorize]
        // GET: FoodTags
        public async Task<IActionResult> Index()
        {
            return View(await _context.RecipeTags.ToListAsync());
        }

        // GET: FoodTags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var RecipeTagModel = await _context.RecipeTags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (RecipeTagModel == null)
            {
                return NotFound();
            }

            return View(RecipeTagModel);
        }

        // GET: FoodTags/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FoodTags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] RecipeTagModel RecipeTagModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(RecipeTagModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(RecipeTagModel);
        }

        // GET: FoodTags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var RecipeTagModel = await _context.RecipeTags.FindAsync(id);
            if (RecipeTagModel == null)
            {
                return NotFound();
            }
            return View(RecipeTagModel);
        }

        // POST: FoodTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] RecipeTagModel recipeTagModel)
        {
            if (id != recipeTagModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipeTagModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodTagModelExists(recipeTagModel.Id))
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
            return View(recipeTagModel);
        }

        // GET: FoodTags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeTagModel = await _context.RecipeTags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipeTagModel == null)
            {
                return NotFound();
            }

            return View(recipeTagModel);
        }

        // POST: FoodTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipeTagModel = await _context.FoodTags.FindAsync(id);
            _context.FoodTags.Remove(recipeTagModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodTagModelExists(int id)
        {
            return _context.FoodTags.Any(e => e.Id == id);
        }
    }
}
