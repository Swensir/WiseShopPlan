using System;

namespace SmartFoodPlanner.Models
{
    public class MealPlan
    {
        public DateTime Date { get; set; }
        public int RecipeId { get; set; }
        public string RecipeName { get; set; }
    }
}