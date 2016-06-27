using System.Collections.Generic;
using System.Linq;
using RecipesApi.Models;

namespace RecipesApi.Repository
{
    /// <summary>
    /// This class is a dummy data source for Web API functionality testing. We can develope
    /// different classes to access different data sources with similar class/function structures
    /// </summary>
    public class IngredientsDataLink
    {
        private List<Ingredient> IngredientList = new List<Ingredient>();

        /// <summary>
        /// Class constructor to create data for testing
        /// </summary>
        public IngredientsDataLink()
        {
            IngredientList.Clear();

            for (int i = 0; i < 5; i++)
            {
                Ingredient thisIngredient = new Ingredient();
                thisIngredient.IngID = i;
                thisIngredient.Name = "MyIngredient" + i.ToString();
                thisIngredient.Description = "";
                IngredientList.Add(thisIngredient);
            }
        }

        /// <summary>
        /// Get all the ingredient data in a list
        /// </summary>
        /// <returns></returns>
        public List<Ingredient> GetIngredients()
        {
            return IngredientList;
        }

        /// <summary>
        /// Get a single ingredient with a specified key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Ingredient GetIngredient(string key)
        {
            return IngredientList.Where(e => e.Name.Equals(key)).SingleOrDefault();
        }

        /// <summary>
        /// Add a new ingredient to the data source
        /// </summary>
        /// <param name="item"></param>
        public void AddIngredient(Ingredient item)
        {
            IngredientList.Add(item);
        }

        /// <summary>
        /// update a modified ingredient
        /// </summary>
        /// <param name="item"></param>
        public void UpdateIngredient(Ingredient item)
        {
            var toUpdate = IngredientList.SingleOrDefault(r => r.IngID == item.IngID);
            if (toUpdate != null)
            {
                toUpdate.IngID = item.IngID;
                toUpdate.Name = item.Name;
                toUpdate.Description = item.Description;
                toUpdate.Notes = item.Notes;
            }
        }

        /// <summary>
        /// remove a ingredient
        /// </summary>
        /// <param name="id"></param>
        public void RemoveIngredient(int id)
        {
            var toRemove = IngredientList.SingleOrDefault(r => r.IngID == id);
            if (toRemove != null)
            {
                IngredientList.Remove(toRemove);
            }
        }
    }
}
