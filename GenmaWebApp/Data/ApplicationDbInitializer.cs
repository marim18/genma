using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GenmaWebApp.Models;
using Microsoft.AspNetCore.Identity;
using OfficeOpenXml;

namespace GenmaWebApp.Data
{
    public class ApplicationDbInitializer
    {
        public static void Initialize(ApplicationDbContext db, UserManager<ApplicationUser> um,
            RoleManager<IdentityRole> rm)
        {
            // Get the db context
            // var db = service.GetRequiredService<ApplicationDbContext>();

            // Recreate database
            db.Database.EnsureDeleted(); //complaint
            db.Database.EnsureCreated();

            // var um = service.GetRequiredService<UserManager<ApplicationUser>>();
            // var rm = service.GetRequiredService<RoleManager<IdentityRole>>();

            // Create admin role
            var adminRole = new IdentityRole("Admin");

            // Add the admin role
            rm.CreateAsync(adminRole).Wait();

            // Add a regular user (no extra roles)
            var user = new ApplicationUser
            {
                UserName = "user@uia.no",
                Email = "user@uia.no",
                Street = "Street",
                EmailConfirmed = true
            };

            um.CreateAsync(user, "Password1.").Wait();

            var admin = new ApplicationUser
            {
                UserName = "Admin@",
                Email = "Admin@uia.no",
                Street = "Ole",
                EmailConfirmed = true
            };

            um.CreateAsync(admin, "Password1.").Wait();
            um.AddToRoleAsync(admin, adminRole.Name).Wait();

            // ========================================
            //adding starter objects

            // Load the ingredients dataset
            loadDataset(db);
            
            // Todo: Remove these example entities.
            // Example ItemModels
            var saus = db.Foods.First(i => i.Id == 5);
            var eple = db.Foods.First(i => i.Id == 25);

            var beef_minced_meat = new FoodModel("Minced Meat (Beef)", 100);
            db.Add(beef_minced_meat);
            
            // Example FoodTags Todo: Create Tags elsewhere in later iterations
            var foodTag1 = new FoodTagModel("Beef");
            var foodTag2 = new FoodTagModel("Minced Meat");
            var foodTag3 = new FoodTagModel("Meat");
            db.Add(foodTag1);
            db.Add(foodTag2);
            db.Add(foodTag3);
            
            // Example FoodTagRelations Todo: Create Food Tag relations elsewhere in later iterations
            var foodTagRelation1 = new FoodTagRelationModel(beef_minced_meat, foodTag1);
            var foodTagRelation2 = new FoodTagRelationModel(beef_minced_meat, foodTag2);
            var foodTagRelation3 = new FoodTagRelationModel(beef_minced_meat, foodTag3);
            db.Add(foodTagRelation1);
            db.Add(foodTagRelation2);
            db.Add(foodTagRelation3);
            
            //recipe tags
            // Example recipeTags 
            var RecepieTag1 = new RecipeTagModel("Glutenfri");
            var RecepieTag2 = new RecipeTagModel("Vegansk");
            var RecepieTag3 = new RecipeTagModel("Meieriprodukter");
            var Vegetarian = new RecipeTagModel("Vegetariansk");
            var favorites = new RecipeTagModel("Favoritter");
            var nuts = new RecipeTagModel("Nøttefritt");
            var egg = new RecipeTagModel("Eggefri");
            db.Add(RecepieTag1);
            db.Add(RecepieTag2);
            db.Add(RecepieTag3);
            db.Add(Vegetarian);
            db.Add(favorites);
            db.Add(nuts);
            db.Add(egg);
            
            // Example recipeTagRelations 
            
            var Salad = new RecipeModel();
            Salad.Title = "salad";
            var Milkshake = new RecipeModel();
            Milkshake.Title = "milkshake";
            var bread = new RecipeModel();
            bread.Title = "bread";
            var RecepieTagRelation1 = new RecipeTagRelationModel(Salad, RecepieTag1);
            var RecepieTagRelation2 = new RecipeTagRelationModel(Milkshake, RecepieTag1);
            var RecepieTagRelation3 = new RecipeTagRelationModel(bread, RecepieTag2);
            db.Add(RecepieTagRelation1);
            db.Add(RecepieTagRelation2);
            db.Add(RecepieTagRelation3);


            // recipe
            var appleSauce = new RecipeModel();
            appleSauce.Title = "Eplesaus test recipe";
            appleSauce.AddIngredient(saus, 1, "l");
            appleSauce.AddIngredient(eple, 5, "whole");
            appleSauce.Procedure = "First you fry apple, then you eat bon apetit";
            appleSauce.CommentSection = new List<CommentModel>();
            var RecipeTagRelation4 = new RecipeTagRelationModel(appleSauce,RecepieTag1);
            
            // commment
            var hangry = new CommentModel();
            hangry.username = user.UserName;
            hangry.message = "IM HANGRY";
            appleSauce.CommentSection.Add(hangry);
            //
            //
            db.Add(RecipeTagRelation4);
            db.Add(appleSauce);
            db.Add(hangry);
            db.SaveChanges();

        }


        static void loadDataset(ApplicationDbContext db)
        {
            // Reading .xlsx Dataset
            using var package =
                new ExcelPackage(new FileInfo("Data/Dataset/The-Norwegian-Food-Composition-Table-2019.xlsx"));
            var sheetFoods = package.Workbook.Worksheets["Foods"];

            // Console.WriteLine("READING IN DATASET DATA:\n");

            // Column indices
            const int columnId = 1;
            const int columnName = 2;
            const int columnFat = 11;

            // Regexs
            var regexFoodId = new Regex(@"\d{2}\.\d{3}"); // Food Id format (e.g. '01.123')

            // Loop through each row
            for (var row = 6; row <= 1928; row++)
            {
                var currentFoodId = sheetFoods.Cells[row, columnId].Text; // Get current row's Food Id

                if (string.IsNullOrEmpty(currentFoodId))
                    continue;

                if (regexFoodId.IsMatch(currentFoodId))
                {
                    // Current row is a Food Item
                    // Console.WriteLine($"FOOD : {sheetFoods.Cells[row, columnName].Text}");

                    // Creates a new item

                    var item = new FoodModel();


                    // Name
                    item.Name = sheetFoods.Cells[row, columnName].Text;

                    // Category
                    // TODO: Add correct category ID !
                    // item.CategoryId = -1;


                    decimal sheet = Convert.ToDecimal(sheetFoods.Cells[row, columnFat].Text);
                    // var fatDecimal = sheet , new CultureInfo("no-NO"));
                    item.Fat = decimal.Round(sheet, 2);

                    db.Add(item);
                }
                else
                {
                    // Current row is a category

                    var idSections = currentFoodId.Split('.');
                    var numSections = idSections.Length;

                    if (numSections == 1)
                    {
                        // Current row is a TOP category
                        // Console.WriteLine(
                        // $"TOP CATEGORY {sheetFoods.Cells[row, columnId].Text}: {sheetFoods.Cells[row, columnName].Text}");

                        // TODO: Create category model 
                        // TODO: Select only Id section as Primary Key
                        // TODO: Set ParentCategoryKey as Null
                        // TODO: Retrieve name and store it
                    }
                    else
                    {
                        // Current row is a LESSER category
                        // Console.WriteLine(
                        // $"LESSER CATEGORY {sheetFoods.Cells[row, columnId].Text}: {sheetFoods.Cells[row, columnName].Text}");

                        // TODO: Create category model 
                        // TODO: Select last Id section of ID as Primary Key
                        // TODO: Select second last section of ID as ParentCategoryKey (Foreign Key)
                        // TODO: Retrieve name and store it
                    }
                }
            }

            db.SaveChanges();
        }
    }
}