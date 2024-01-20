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
    [Authorize]
    public class FoodTagsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FoodTagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FoodTags
        public async Task<IActionResult> Index()
        {
            return View(await _context.FoodTags.ToListAsync());
        }

        // GET: FoodTags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodTagModel = await _context.FoodTags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foodTagModel == null)
            {
                return NotFound();
            }

            return View(foodTagModel);
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
        public async Task<IActionResult> Create([Bind("Id,Name")] FoodTagModel foodTagModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(foodTagModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(foodTagModel);
        }

        // GET: FoodTags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodTagModel = await _context.FoodTags.FindAsync(id);
            if (foodTagModel == null)
            {
                return NotFound();
            }
            return View(foodTagModel);
        }

        // POST: FoodTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] FoodTagModel foodTagModel)
        {
            if (id != foodTagModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(foodTagModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodTagModelExists(foodTagModel.Id))
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
            return View(foodTagModel);
        }

        // GET: FoodTags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodTagModel = await _context.FoodTags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foodTagModel == null)
            {
                return NotFound();
            }

            return View(foodTagModel);
        }

        // POST: FoodTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var foodTagModel = await _context.FoodTags.FindAsync(id);
            _context.FoodTags.Remove(foodTagModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodTagModelExists(int id)
        {
            return _context.FoodTags.Any(e => e.Id == id);
        }
    }
}
