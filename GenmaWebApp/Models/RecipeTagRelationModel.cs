using System.ComponentModel.DataAnnotations.Schema;

namespace GenmaWebApp.Models
{
    /**
     * Bridge table between entities recipe and recipeTag.
     */
    public class RecipeTagRelationModel
    {
        public int Id {get; set;}
        
        public int RecipeId {get; set;}
        
        public RecipeModel Recipe {get; set;}
        
        public int RecipeTagId {get; set;}
        
        public RecipeTagModel RecipeTag {get; set;}

        public RecipeTagRelationModel()
        {
            
        }
        
        public RecipeTagRelationModel(RecipeModel recipe, RecipeTagModel recipeTag)
        {
            Recipe = recipe;
            RecipeTag = recipeTag;
        }
        
        public RecipeTagRelationModel(int recipeId, int recipeTagId)
        {
            RecipeId = recipeId;
            RecipeTagId = recipeTagId;
        }
    }
}