using System.Collections.Generic;
using System.Linq;
using RecipesApi.Models;

namespace RecipesApi.Repository
{
    /// <summary>
    /// This class is a dummy data source for Web API functionality testing. We can develope
    /// different classes to access different data sources with similar class/function structures
    /// </summary>
    public class RecipeDataLink
    {
        private List<Recipe> RecipeList = new List<Recipe>();

        /// <summary>
        /// Class constructor to create data for testing
        /// </summary>
        public RecipeDataLink()
        {
            RecipeList.Clear();

            for (int i = 0; i < 5; i++)
            {
                Recipe thisRecipe = new Recipe();
                thisRecipe.RecipeID = i;
                thisRecipe.Name = "MyRecipe" + i.ToString();

                thisRecipe.RecipeIngredients = new List<RecipeIngredient>();
                for (int j = 0; j < 3; i++)
                {
                    RecipeIngredient ing = new RecipeIngredient();
                    ing.IngID = j;
                    ing.Amount = j * 10;
                    thisRecipe.RecipeIngredients.Add(ing);
                }
                RecipeList.Add(thisRecipe);
            }
        }

        /// <summary>
        /// Get all the recipe data in a list
        /// </summary>
        /// <returns></returns>
        public List<Recipe> GetRecipes()
        {
            return RecipeList;
        }

        /// <summary>
        /// Get a single recipe with a specified key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Recipe GetRecipe(string key)
        {
            return RecipeList.Where(e => e.Name.Equals(key)).SingleOrDefault();
        }

        /// <summary>
        /// Add a new recipe to the data source
        /// </summary>
        /// <param name="item"></param>
        public void AddRecipe(Recipe item)
        {
            RecipeList.Add(item);
        }

        /// <summary>
        /// update a modified recipe
        /// </summary>
        /// <param name="item"></param>
        public void UpdateRecipe(Recipe item)
        {
            var toUpdate = RecipeList.SingleOrDefault(r => r.RecipeID == item.RecipeID);
            if (toUpdate != null)
            {
                toUpdate.RecipeID = item.RecipeID;
                toUpdate.Name = item.Name;
                toUpdate.Description = item.Description;
                toUpdate.Notes = item.Notes;
                if (item.RecipeIngredients != null)
                {
                    if (toUpdate.RecipeIngredients == null)
                    {
                        toUpdate.RecipeIngredients = new List<RecipeIngredient>();
                    }
                    else
                    {
                        toUpdate.RecipeIngredients.Clear();
                    }
                    foreach (RecipeIngredient ing in item.RecipeIngredients)
                    {
                        RecipeIngredient newIng = new RecipeIngredient();
                        newIng.IngID = ing.IngID;
                        newIng.Amount = ing.Amount;
                        newIng.OtherNotes = ing.OtherNotes;
                        toUpdate.RecipeIngredients.Add(newIng);
                    }
                }
                else
                {
                    toUpdate.RecipeIngredients = null;
                }
            }
        }

        /// <summary>
        /// remove a recipe
        /// </summary>
        /// <param name="id"></param>
        public void RemoveRecipe(int id)
        {
            var toRemove = RecipeList.SingleOrDefault(r => r.RecipeID == id);
            if (toRemove != null)
            {
                RecipeList.Remove(toRemove);
            }
        }
    }
}
