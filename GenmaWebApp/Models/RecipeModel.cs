using System;
using System.Collections.Generic;

namespace GenmaWebApp.Models
{
    // public class RecipeModel
    // {
    //     public int id { get; set; }
    //     public string title { get; set;}
    //     public string procedure { get; set; }
    //     public Tuple<ItemModel, double, string> amount; //eksempel: ingrediens[id],300,g?
    //     public List<Tuple<ItemModel,double, string>> ingrediens; //category isteden? Tilabke til versionen med 2 modeller for oppskrift?
    //     public List<CommentModel> commentsection;
    //     public RecipeModel(){}
    //
    //
    //     public RecipeModel(int id, string title,string procedure, List<Tuple<ItemModel,double, string>> ingrediens, List<CommentModel> commentsection)
    //     {
    //         this.id = id;
    //         this.title = title;
    //         this.procedure = procedure;
    //         this.ingrediens = ingrediens;
    //         this.commentsection = commentsection;
    //     }
    // }
    public class RecipeModel
    {
        public int Id { get; set; }
        public string Title { get; set;}
        public string Procedure { get; set; }

        public List<IngredientModel> IngredientList; //category isteden? Tilabke til versionen med 2 modeller for oppskrift?
        
        public List<CommentModel> CommentSection;
        public List<RecipeTagRelationModel> RecipeTagRelationModels {get; set;}

        public RecipeModel()
        {
            IngredientList = new List<IngredientModel>();
        }
        

        public RecipeModel(int id, string title,string procedure, List<IngredientModel> ingredientList, List<CommentModel> commentSection)
        {
            this.Id = id;
            this.Title = title;
            this.Procedure = procedure;
            this.IngredientList = ingredientList;
            this.CommentSection = commentSection;
        }

        public void AddIngredient(FoodModel food, double quantity, string unit)
        {
            // Todo: Determine if input should be a FoodModel or the FoodModel Id
            var measure = new IngredientModel(food.Id, this.Id, quantity, unit);
            IngredientList.Add(measure);
        }
    }
}