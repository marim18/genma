namespace GenmaWebApp.Models
{
    // Ingredient is the bridge table between Food and Recipe.
    public class IngredientModel
    {
        
        public int Id { get; set; }
        
        public int FoodModelId { get; set; }

        public FoodModel FoodModel { get; set; }

        public int RecipeModelId { get; set; }

        public double Quantity { get; set; }

        public string Unit { get; set; }

        public IngredientModel()
        {
            
        }

        public IngredientModel(int foodModelId, int recipeModelId, double quantity, string unit)
        {
            FoodModelId = foodModelId;
            RecipeModelId = recipeModelId;
            Quantity = quantity;
            Unit = unit;
        }

    }
}