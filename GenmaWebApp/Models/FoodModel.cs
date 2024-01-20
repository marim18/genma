using System.Collections.Generic;

namespace GenmaWebApp.Models
{
    public class FoodModel
    {
        public int Id {get; set;}
        public string Name {get; set;}
        public decimal Fat { get; set; }

        public List<FoodTagRelationModel> FoodTagRelationModels {get; set;}
        public FoodModel(){}

        public FoodModel(string name, decimal fat) //burde isteden ha String Name, og ta inn tag på et vis, må ha tag mattype og tag allergi eller trenger vi egentlig denne?
        {
            Name = name;
            Fat = fat;
            FoodTagRelationModels = new List<FoodTagRelationModel>();
        }

    }
} 
