using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RecipesApi.Models;

namespace RecipesApi.Repository
{
    /// <summary>
    /// This class is for connecting to MSSQL database and accessing the data.
    /// </summary>
    public class IngredientsDatabaseLink
    {
        private string conn = null;

        /// <summary>
        /// Class constructor 
        /// it sets the database connection string
        /// </summary>
        public IngredientsDatabaseLink()
        {
            conn = Startup.dbConnectionString;
        }

        /// <summary>
        /// Get all the ingredient data in the database to a list. In real-world applications, we might 
        /// need to use "caching" or load data asynchronously
        /// </summary>
        /// <returns></returns>
        public List<Ingredient> GetIngredients()
        {
            List<Ingredient> Ingredients = new List<Ingredient>();

            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand("Select * from Ingredients", con))
                    {
                        SqlDataReader rd = command.ExecuteReader();
                        while (rd.Read())
                        {
                            Ingredient newIngredient = new Ingredient();
                            newIngredient.IngID = Convert.ToInt32(rd["IngID"]);
                            newIngredient.Name = rd["Name"].ToString();
                            if (!(rd["Description"] is DBNull))
                                newIngredient.Description = rd["Description"].ToString();
                            if (!(rd["Notes"] is DBNull))
                                newIngredient.Notes = rd["Notes"].ToString();
                            Ingredients.Add(newIngredient);
                        }
                        rd.Dispose();
                    }
                }
                catch (Exception exp)
                {
                    Console.WriteLine("Query ingredient table failed.\n" + exp.Message);
                }
            }
            return Ingredients;
        }

        /// <summary>
        /// Get a single ingredient with a specified key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Ingredient GetIngredient(string key)
        {
            Ingredient newIngredient = null;

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("Select * from Ingredients where Name = '" + key + "'", con))
                    {
                        SqlDataReader rd = cmd.ExecuteReader();
                        if (rd.Read())
                        {
                            newIngredient = new Ingredient();
                            newIngredient.IngID = Convert.ToInt32(rd["IngID"]);
                            newIngredient.Name = rd["Name"].ToString();
                            if (!(rd["Description"] is DBNull))
                                newIngredient.Description = rd["Description"].ToString();
                            if (!(rd["Notes"] is DBNull))
                                newIngredient.Notes = rd["Notes"].ToString();
                        }
                        rd.Dispose();
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("Query ingredient failed.\n" + exp.Message);
            }
            return newIngredient;
        }

        /// <summary>
        /// Add a new ingredient to the data source
        /// </summary>
        /// <param name="item"></param>
        public void AddIngredient(Ingredient item)
        {
            string strInsertIngredient = "INSERT INTO Ingredients (Name, Description, Notes) VALUES ('{1}', '{2}' '{3}')";
            string cmdInsertIngredient = String.Format(strInsertIngredient, item.Name, item.Description, item.Notes);

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdInsertIngredient, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("Add ingredient to table failed.\n" + exp.Message);
            }
        }

        /// <summary>
        /// update a modified recipe
        /// </summary>
        /// <param name="item"></param>
        public void UpdateIngredient(Ingredient item)
        {
            string strUpdateIngredient = "UPDATE Ingredient SET Name='{0}', Description='{1}', Notes='{2}' WHERE RecipeID={3};";
            string cmdUpdateIngredient = String.Format(strUpdateIngredient, item.Name, item.Description, item.Notes, item.IngID);

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdUpdateIngredient, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("Update ingredient failed.\n" + exp.Message);
            }
        }

        /// <summary>
        /// remove a ingredient
        /// </summary>
        /// <param name="id"></param>
        public void RemoveIngredient(int id)
        {
            string strRemoveIngredient = "DELETE FROM Ingredient WHERE IngID={0};";

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(String.Format(strRemoveIngredient, id), con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("Delete ingredient failed.\n" + exp.Message);
            }
        }
    }
}

