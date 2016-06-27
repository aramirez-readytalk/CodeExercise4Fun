using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RecipesApi.Models;

namespace RecipesApi.Repository
{
    /// <summary>
    /// This class is for connecting to MSSQL database and accessing the data.
    /// </summary>
    public class RecipeDatabaseLinks 
    {
        private string conn = null;

        /// <summary>
        /// Class constructor 
        /// it sets the database connection string
        /// </summary>
        public RecipeDatabaseLinks()
        {
            conn = Startup.dbConnectionString;
        }

        /// <summary>
        /// Get all the recipe data in the database to a list. In real-world applications, we might 
        /// need to use "caching" or load data asynchronously
        /// </summary>
        /// <returns></returns>
        public List<Recipe> GetRecipes()
        {
            List<Recipe> Recipes = new List<Recipe>();

            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand("Select * from Recipes", con))
                    {
                        SqlDataReader rd = command.ExecuteReader();
                        while (rd.Read())
                        {
                            Recipe newRecipe = new Recipe();
                            newRecipe.RecipeID = Convert.ToInt32(rd["RecipeID"]);
                            newRecipe.Name = rd["Name"].ToString();
                            if (!(rd["Description"] is DBNull))
                                newRecipe.Description = rd["Description"].ToString();
                            if (!(rd["Notes"] is DBNull))
                                newRecipe.Notes = rd["Notes"].ToString();
                            Recipes.Add(newRecipe);
                        }
                        rd.Dispose();
                    }
                }
                catch (Exception exp)
                {
                    Console.WriteLine("Query Recipes table failed.\n" + exp.Message );
                }
            }
            return Recipes;
        }

        /// <summary>
        /// Get a single recipe with a specified key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Recipe GetRecipe(string key)
        {
            Recipe newRecipe = null;

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("Select * from Recipes where Name = '" + key + "'", con))
                    {
                        SqlDataReader rd = cmd.ExecuteReader();
                        if (rd.Read())
                        {
                            newRecipe = new Recipe();
                            newRecipe.RecipeID = Convert.ToInt32(rd["RecipeID"]);
                            newRecipe.Name = rd["Name"].ToString();
                            if (!(rd["Description"] is DBNull))
                                newRecipe.Description = rd["Description"].ToString();
                            if (!(rd["Notes"] is DBNull))
                                newRecipe.Notes = rd["Notes"].ToString();

                            using (SqlCommand cmd2 = new SqlCommand("Select * from RecipeIngredients where RecipeID = '" + newRecipe.RecipeID + "'", con))
                            {
                                SqlDataReader rd2 = cmd2.ExecuteReader();
                                if (rd2.HasRows)
                                {
                                    newRecipe.RecipeIngredients = new List<RecipeIngredient>();
                                }
                                while (rd2.Read())
                                {
                                    RecipeIngredient newIng = new RecipeIngredient();
                                    newIng.IngID = Convert.ToInt32(rd2["IngID"]);
                                    newIng.Amount = Convert.ToDouble(rd2["Amount"]);
                                    newIng.OtherNotes = rd2["Notes"].ToString();
                                    newRecipe.RecipeIngredients.Add(newIng);
                                }
                            }
                        }
                        rd.Dispose();
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("Query Recipes table failed.\n" + exp.Message);
            }
            return newRecipe;
        }

        /// <summary>
        /// Add a new recipe to the data source
        /// </summary>
        /// <param name="item"></param>
        public void AddRecipe(Recipe item)
        {
            string strInsertRecipe = "INSERT INTO Recipes (Name, Description, Notes) output INSERTED.ID VALUES ('{0}','{1}','{2}')";
            string strInsertIngredient = "INSERT INTO RecipeIngredients (RecipeID, IngID, Amount, Notes) VALUES ({0}, {1}, {2} '{3}')";

            string cmdInsertRecipe = String.Format(strInsertRecipe, item.Name, item.Description, item.Notes);

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdInsertRecipe, con))
                    {
                        int newID = (int)cmd.ExecuteScalar();
                        if (newID > 0) // insert correctly
                        {
                            foreach (RecipeIngredient ring in item.RecipeIngredients)
                            {
                                string cmdInsertIngredient = String.Format(strInsertIngredient, newID, ring.IngID, ring.Amount, ring.OtherNotes);
                                using (SqlCommand cmd2 = new SqlCommand(cmdInsertIngredient, con))
                                {
                                    cmd2.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("Query Recipes table failed.\n" + exp.Message);
            }
        }

        /// <summary>
        /// update a modified recipe
        /// </summary>
        /// <param name="item"></param>
        public void UpdateRecipe(Recipe item)
        {
            string strUpdateRecipe = "UPDATE Recipe SET Name='{0}', Description='{1}', Notes='{2}' WHERE RecipeID={3};";
            string strRemoveIngredients = "DELETE * FROM RecipeIngredients where RecipeID={0};";
            string strInsertIngredient = "INSERT INTO RecipeIngredients (RecipeID, IngID, Amount, Notes) VALUES ({0}, {1}, {2} '{3}')";

            string cmdUpdateRecipe = String.Format(strUpdateRecipe, item.Name, item.Description, item.Notes, item.RecipeID);

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdUpdateRecipe, con))
                    {
                        cmd.ExecuteNonQuery();
                        using (SqlCommand cmd2 = new SqlCommand(String.Format(strRemoveIngredients, item.RecipeID), con))
                        {
                            cmd2.ExecuteNonQuery();
                        }

                        foreach (RecipeIngredient ring in item.RecipeIngredients)
                        {
                            string cmdInsertIngredient = String.Format(strInsertIngredient, item.RecipeID, ring.IngID, ring.Amount, ring.OtherNotes);
                            using (SqlCommand cmd2 = new SqlCommand(cmdInsertIngredient, con))
                            {
                                cmd2.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("Update recipe failed.\n" + exp.Message);
            }
        }

        /// <summary>
        /// remove a recipe
        /// </summary>
        /// <param name="id"></param>
        public void RemoveRecipe(int id)
        {
            string strRemoveRecipe = "DELETE FROM Recipes WHERE RecipeID={0};";
            string strRemoveIngredient = "DELETE FROM RecipeIngredients WHERE RecipeID={0};";

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(String.Format(strRemoveRecipe, id), con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    using (SqlCommand cmd2 = new SqlCommand(String.Format(strRemoveIngredient, id), con))
                    {
                        cmd2.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("Delete recipe failed.\n" + exp.Message);
            }
        }

    }
}
