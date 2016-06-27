/// <summary>
/// Class model for ingredient
/// </summary>
namespace RecipesApi.Models
{
    /// <summary>
    /// Ingedients class
    /// </summary>
    public class Ingredient
    {
        /// <summary>
        /// Ingredient name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Ingredient ID
        /// </summary>
        public int IngID { get; set; }
        /// <summary>
        /// Ingredient description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Ingredient notes to hold "other" information
        /// </summary>
        public string Notes { get; set; }
    }
}
