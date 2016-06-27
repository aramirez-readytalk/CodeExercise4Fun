using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RecipesApi.Models;
using RecipesApi.Repository;

namespace RecipesApi.Controllers
{
    /// <summary>
    /// Web API interface - recipes
    /// </summary>
    [Route("api/[controller]")]
    public class RecipesController : Controller
    {
        //*********************************************************************
        // We can connect to different Datalinks here for different backend data sources 
        //*********************************************************************
        private RecipeDatabaseLinks mydatalink = new RecipeDatabaseLinks();
        //private RecipeDataLinks mydatalink = new RecipeDataLinks();

        /// <summary>
        /// GET api/Recipes - get a list of all recipes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Recipe> GetRecipes()
        {
            return mydatalink.GetRecipes();
        }

        /// <summary>
        /// GET api/Recipes/key - get a recipe by name
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("{key}")]
        public IActionResult GetRecipe(string key)
        {
            var item = mydatalink.GetRecipe(key);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        /// <summary>
        /// POST api/Recipes - add a new recipe
        /// </summary>
        /// <param name="item">recipe to be added</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddRecipe([FromBody]Recipe item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            mydatalink.AddRecipe(item);
            return GetRecipe(item.Name);            
        }

        /// <summary>
        /// PUT api/Recipes/key - update a modified recipe
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPut("{key}")]
        public IActionResult Update(string key, [FromBody]Recipe item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            var RecipeObj = mydatalink.GetRecipe(key);
            if (RecipeObj == null)
            {
                return NotFound();
            }
            mydatalink.UpdateRecipe(item);
            return new NoContentResult();
        }

        /// <summary>
        /// DELETE api/Recipes/5 - delete a recipe by id
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            mydatalink.RemoveRecipe(id);
        }
    }
}
