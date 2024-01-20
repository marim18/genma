using System.Collections.Generic;

namespace GenmaWebApp.Models.ViewModels
{
    /**
     * A ViewModel to facilitate the Edit page of Foods.
     * Used to make checkboxes easier to implement in frontend.
     */
    public class RecipeEditViewModel
    {
        
        /**
         * Helper-class used with the IList in the outside class to imitate how dictionaries work.
         * Note: Dictionaries does NOT work well with model binding within DOTNET CORE! Use Lists instead.
         */
        public class TagSelectionState
        {
            public RecipeTagModel RecipeTagModel { get; set; }
            public bool Selected { get; set; }
            
            public TagSelectionState(){}

            public TagSelectionState(RecipeTagModel recipeTagModel, bool selected)
            {
                RecipeTagModel = recipeTagModel;
                Selected = selected;
            }
        }

        public RecipeModel RecipeModel { get; set; }
        
        public IList<TagSelectionState> SelectedTagStates { get; set; }

        public RecipeEditViewModel()
        {
            SelectedTagStates = new List<TagSelectionState>();
        }
        
    }
}