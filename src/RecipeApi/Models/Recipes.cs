using System.Collections.Generic;

/// <summary>
/// Class model for recipe 
/// </summary>
namespace RecipesApi.Models
{
    /// <summary>
    /// Ingredient class for ingredient entry in recipe
    /// </summary>
    public class RecipeIngredient
    {
        /// <summary>
        /// Ingredient ID to link to a recipe
        /// </summary>
        public int IngID { get; set; }
        /// <summary>
        /// Ingredient amount in the recipe
        /// </summary>
        public double Amount { get; set; }
        /// <summary>
        /// Extra field to hold other information if necessary
        /// </summary>
        public string OtherNotes { get; set; }
    }

    /// <summary>
    /// Recipe class
    /// </summary>
    public class Recipe
    {
        /// <summary>
        /// Recipe name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Recipe ID
        /// </summary>
        public int RecipeID { get; set; }
        /// <summary>
        /// Recipe description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Recipe notes
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// The recipe ingredients list
        /// </summary>
        public List<RecipeIngredient> RecipeIngredients { get; set; }
    }
}
