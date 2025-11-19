using System.Collections.Generic;

namespace SmartFoodPlanner.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }
        public int CookingTime { get; set; }
        public List<RecipeIngredient> Ingredients { get; set; } = new List<RecipeIngredient>();
    }

    public class RecipeIngredient
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
    }
}