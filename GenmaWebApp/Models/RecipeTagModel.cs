namespace GenmaWebApp.Models
{
    public class RecipeTagModel
    {
        public int Id {get; set;}
        public string Name {get; set;}
        public RecipeTagModel(){}

        public RecipeTagModel(string name)
        {
            Name = name;
        }
        
    }
}