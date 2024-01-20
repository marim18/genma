using System.Linq;
using System.Threading.Tasks;
using GenmaWebApp.Data;
using GenmaWebApp.Models;
using GenmaWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GenmaWebApp.Controllers
{
    [Authorize]
    public class FoodsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FoodsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Foods
        public async Task<IActionResult> Index()
        {
            return View(await _context.Foods.ToListAsync());
        }

        // GET: Foods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodModel = await _context.Foods
                .Include(m => m.FoodTagRelationModels)
                .ThenInclude(m => m.FoodTag)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (foodModel == null)
            {
                return NotFound();
            }

            return View(foodModel);
        }

        // GET: Foods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Foods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Fat")] FoodModel foodModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(foodModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(foodModel);
        }

        // GET: Foods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodModel = await _context.Foods
                .Include(m => m.FoodTagRelationModels)
                .ThenInclude(m => m.FoodTag)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (foodModel == null)
            {
                return NotFound();
            }

            var fevm = new FoodEditViewModel {FoodModel = foodModel};
            foreach (var tag in _context.FoodTags)
            {
                fevm.SelectedTagStates.Add(new FoodEditViewModel.TagSelectionState(tag, false));
            }

            foreach (var relation in foodModel.FoodTagRelationModels)
            {
                fevm.SelectedTagStates.First(t => t.FoodTagModel.Id == relation.FoodTagId).Selected = true;
            }

            return View(fevm);
        }

        // POST: Foods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FoodEditViewModel foodEditViewModel)
        {
            if (id != foodEditViewModel.FoodModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(); // An error has occured if it arrives here
            }

            // Generates new FoodTagRelations while deleting non-relevant FoodTagRelations
            foreach (var tagState in foodEditViewModel.SelectedTagStates)
            {
                var currentFoodId = foodEditViewModel.FoodModel.Id;
                var currentFoodTagId = tagState.FoodTagModel.Id;
                if (tagState.Selected)
                {
                    // The current tag should be associated with current food
                    if (_context.FoodTagRelations.Any(r =>
                        r.FoodId == currentFoodId && r.FoodTagId == currentFoodTagId)) continue;

                    // Current relation does NOT exist, we must create it. Otherwise, skip.
                    var relation = new FoodTagRelationModel(currentFoodId, currentFoodTagId);
                    _context.Add(relation);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // The current tag should NOT associated with current food
                    if (!_context.FoodTagRelations.Any(r =>
                        r.FoodId == currentFoodId && r.FoodTagId == currentFoodTagId)) continue;

                    var target = _context.FoodTagRelations.Single(r =>
                        r.FoodId == currentFoodId && r.FoodTagId == currentFoodTagId);

                    _context.FoodTagRelations.Remove(target);
                    await _context.SaveChangesAsync();
                }
            }
            
            // Save the edited FoodModel
            try
            {
                _context.Update(foodEditViewModel.FoodModel);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodModelExists(foodEditViewModel.FoodModel.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Foods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodModel = await _context.Foods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foodModel == null)
            {
                return NotFound();
            }

            return View(foodModel);
        }

        // POST: Foods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var foodModel = await _context.Foods.FindAsync(id);
            _context.Foods.Remove(foodModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodModelExists(int id)
        {
            return _context.Foods.Any(e => e.Id == id);
        }
    }
}