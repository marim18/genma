namespace GenmaWebApp.Models
{
    public class FoodTagModel
    {
        public int Id {get; set;}
        public string Name {get; set;}
        public FoodTagModel(){}

        public FoodTagModel(string name)
        {
            Name = name;
        }
        
    }
}