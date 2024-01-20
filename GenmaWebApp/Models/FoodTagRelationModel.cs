using System.ComponentModel.DataAnnotations.Schema;

namespace GenmaWebApp.Models
{
    /**
     * Bridge table between entities Food and FoodTag.
     */
    public class FoodTagRelationModel
    {
        public int Id {get; set;}
        
        public int FoodId {get; set;}
        
        public FoodModel Food {get; set;}
        
        public int FoodTagId {get; set;}
        
        public FoodTagModel FoodTag {get; set;}

        public FoodTagRelationModel()
        {
            
        }
        
        public FoodTagRelationModel(FoodModel food, FoodTagModel foodTag)
        {
            Food = food;
            FoodTag = foodTag;
        }
        
        public FoodTagRelationModel(int foodId, int foodTagId)
        {
            FoodId = foodId;
            FoodTagId = foodTagId;
        }
    }
}