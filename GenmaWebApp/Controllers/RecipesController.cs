using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GenmaWebApp.Data;
using GenmaWebApp.Models;
using GenmaWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml.Packaging.Ionic.Zip;

//identisk med master 20.05.2020
namespace GenmaWebApp.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecipesController(ApplicationDbContext context)
        {
            _context = context;
        }

        //indekserer i recipes for å finne searchstring
        public void Checktags(Searchclass searchy) 
        {
            List<RecipeTagModel> uwu = new List<RecipeTagModel>();
            if (searchy.gluten)
            {
                foreach (var rt in _context.RecipeTags)
                {
                    if (rt.Name == "Glutenfri")
                    {  
                        searchy.Searchtags.Add(rt.Id);
                    }
                }

             
            }

            if (searchy.vegan)
            {
                foreach (var rt in _context.RecipeTags)
                {
                    if (rt.Name == "Vegansk")
                    {
                        searchy.Searchtags.Add(rt.Id);
                    }
                }
            }
            if (searchy.vegetarian)
            {
                foreach (var rt in _context.RecipeTags)
                {
                    if (rt.Name == "Vegetariansk")
                    {
                        searchy.Searchtags.Add(rt.Id);
                    }
                }
            }
            if (searchy.dairy)
            {
                foreach (var rt in _context.RecipeTags)
                {
                    if (rt.Name == "Meieriprodukter")
                    {
                        searchy.Searchtags.Add(rt.Id);
                    }
                }
            }
            if (searchy.egg)
            {
                foreach (var rt in _context.RecipeTags)
                {
                    if (rt.Name == "Eggefri")
                    {
                        searchy.Searchtags.Add(rt.Id);
                    }
                }
            }
            if (searchy.nuts)
            {
                foreach (var rt in _context.RecipeTags)
                {
                    if (rt.Name == "Nøttefritt")
                    {
                        searchy.Searchtags.Add(rt.Id);
                    }
                }
            }
        }

        public async Task<IActionResult>
            Index(Searchclass searchy) //bytta til searchclass
        {
            List<RecipeModel> aha = new List<RecipeModel>();
            Checktags(searchy);
            List<RecipeModel> sorri = new List<RecipeModel>();
            aha = _context.Recipes.ToList();

            if (searchy.Searchtags.Count != 0)
            {aha.Clear();
                for (int i = 0; i < searchy.Searchtags.Count;)
                {
                    foreach (var tagRelation in _context.RecipeTagRelations)
                    {
                        if (tagRelation.RecipeTagId == searchy.Searchtags[i])
                        {
                            foreach (var ff in _context.Recipes)
                            {
                                if (ff.Id == tagRelation.RecipeId)
                                {
                                    aha.Add(ff);
                                }
                            }
                        }
                    }

                    i++;
                }
            }

            if (searchy.searchword != null)
            {
                foreach (var rp in aha)
                {
                    if (rp.Title.ToLower().Contains(searchy.searchword.ToLower()))
                    {
                        sorri.Add(rp);
                       
                    }
                }
                aha.Clear();
                aha.AddRange(sorri);
                sorri.Clear();
            }

            if (searchy.random)
            {
                Random r = new Random();
                int num = r.Next(aha.Count);
                if (aha.Count != 0 || num != 0)
                {
                    List<RecipeModel> ran = new List<RecipeModel> {aha[num]};
                    aha.Clear();
                    aha.AddRange(ran);
                    ran.Clear();
                }
            }

            return View(aha);

        }


        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeModel = await _context.Recipes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipeModel == null)
            {
                return NotFound();
            }

            return View(recipeModel);
        }
        [Authorize]

        // GET: Recipes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Procedure")] RecipeModel recipeModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipeModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(recipeModel);
        }
        [Authorize]
        // GET: Foods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeModel = await _context.Recipes
                .Include(m => m.RecipeTagRelationModels)
                .ThenInclude(m => m.RecipeTag)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipeModel == null)
            {
                return NotFound();
            }

            var fevm = new RecipeEditViewModel {RecipeModel = recipeModel};
            foreach (var tag in _context.RecipeTags)
            {
                fevm.SelectedTagStates.Add(new RecipeEditViewModel.TagSelectionState(tag, false));
            }

            foreach (var relation in recipeModel.RecipeTagRelationModels)
            {
                fevm.SelectedTagStates.First(t => t.RecipeTagModel.Id == relation.RecipeTagId).Selected = true;
            }

            return View(fevm);
        }
        [Authorize]
        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RecipeEditViewModel recipeEditViewModel)
        {
            if (id != recipeEditViewModel.RecipeModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(); // An error has occured if it arrives here
            }

            // Generates new FoodTagRelations while deleting non-relevant FoodTagRelations
            foreach (var tagState in recipeEditViewModel.SelectedTagStates)
            {
                var currentRecipeId = recipeEditViewModel.RecipeModel.Id;
                var currentRecipeTagId = tagState.RecipeTagModel.Id;
                if (tagState.Selected)
                {
                    // The current tag should be associated with current food
                    if (_context.RecipeTagRelations.Any(r =>
                        r.RecipeId == currentRecipeId && r.RecipeTagId == currentRecipeTagId)) continue;

                    // Current relation does NOT exist, we must create it. Otherwise, skip.
                    var relation = new RecipeTagRelationModel(currentRecipeId, currentRecipeTagId);
                    _context.Add(relation);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // The current tag should NOT associated with current food
                    if (!_context.RecipeTagRelations.Any(r =>
                        r.RecipeId == currentRecipeId && r.RecipeTagId == currentRecipeTagId)) continue;

                    var target = _context.RecipeTagRelations.Single(r =>
                        r.RecipeId == currentRecipeId && r.RecipeTagId == currentRecipeTagId);

                    _context.RecipeTagRelations.Remove(target);
                    await _context.SaveChangesAsync();
                }
            }

            // Save the edited FoodModel
            try
            {
                _context.Update(recipeEditViewModel.RecipeModel);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeModelExists(recipeEditViewModel.RecipeModel.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeModel = await _context.Recipes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipeModel == null)
            {
                return NotFound();
            }

            return View(recipeModel);
        }
        [Authorize]
        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipeModel = await _context.Recipes.FindAsync(id);
            _context.Recipes.Remove(recipeModel);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeModelExists(int id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }
    }
}