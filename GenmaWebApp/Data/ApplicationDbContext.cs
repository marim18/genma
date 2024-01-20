using System;
using System.Collections.Generic;
using System.Text;
using GenmaWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GenmaWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<FoodModel> Foods { get; set; }
        public DbSet<RecipeModel> Recipes { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
        public DbSet<FoodTagModel> FoodTags { get; set; }
        public DbSet<FoodTagRelationModel> FoodTagRelations { get; set; }

        public DbSet<IngredientModel> Ingredients { get; set; }
        public DbSet<RecipeTagModel> RecipeTags { get; set; }
        public DbSet<RecipeTagRelationModel> RecipeTagRelations { get; set; }
    }
}