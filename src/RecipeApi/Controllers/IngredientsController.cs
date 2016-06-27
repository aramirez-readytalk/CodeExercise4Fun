using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RecipesApi.Models;
using RecipesApi.Repository;

namespace RecipesApi.Controllers
{
    /// <summary>
    /// Web API interface - ingredients
    /// </summary>
    [Route("api/[controller]")]
    public class IngredientsController : Controller
    {
        //*********************************************************************
        // We can connect to different Datalinks here for different backend data sources 
        //*********************************************************************
        //private IngredientsDatabaseLink mydatalink = new IngredientsDatabaseLink();
        private IngredientsDataLink mydatalink = new IngredientsDataLink();

        /// <summary>
        /// GET api/Ingredients  - get a list of all ingredients
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Ingredient> Get()
        {
            return mydatalink.GetIngredients();
        }

        /// <summary>
        /// GET api/Ingredients/key  - get a Ingredient by name
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("{key}")]
        public IActionResult GetIngredient(string key) 
        {
            var item = mydatalink.GetIngredient(key);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        /// <summary>
        /// POST api/Ingredients - add a new ingredient
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddIngredient([FromBody] Ingredient item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            mydatalink.AddIngredient(item);
            return GetIngredient(item.Name);
        }

        /// <summary>
        /// PUT api/Ingredients/key - update a modified ingredient
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(string key, [FromBody]Ingredient item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            var RecipeObj = mydatalink.GetIngredient(key);
            if (RecipeObj == null)
            {
                return NotFound();
            }
            mydatalink.UpdateIngredient(item);
            return new NoContentResult();
        }

        /// <summary>
        /// DELETE api/Ingredient/5 - delete a recipe by id
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            mydatalink.RemoveIngredient(id);
        }

    }
}
