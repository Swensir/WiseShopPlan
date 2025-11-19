using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using SmartFoodPlanner.Models;

namespace SmartFoodPlanner.Services
{
    public class JsonDataService
    {
        private readonly string _dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        private readonly string _productsFile;
        private readonly string _recipesFile;
        private readonly string _mealPlansFile;

        public JsonDataService()
        {
            if (!Directory.Exists(_dataFolder))
                Directory.CreateDirectory(_dataFolder);

            _productsFile = Path.Combine(_dataFolder, "products.json");
            _recipesFile = Path.Combine(_dataFolder, "recipes.json");
            _mealPlansFile = Path.Combine(_dataFolder, "mealplans.json");
        }

        // Products
        public List<Product> GetProducts()
        {
            if (!File.Exists(_productsFile)) return new List<Product>();
            var json = File.ReadAllText(_productsFile);
            return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
        }

        public void SaveProducts(List<Product> products)
        {
            var json = JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_productsFile, json);
        }

        // Recipes
        public List<Recipe> GetRecipes()
        {
            if (!File.Exists(_recipesFile)) return new List<Recipe>();
            var json = File.ReadAllText(_recipesFile);
            return JsonSerializer.Deserialize<List<Recipe>>(json) ?? new List<Recipe>();
        }

        public void SaveRecipes(List<Recipe> recipes)
        {
            var json = JsonSerializer.Serialize(recipes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_recipesFile, json);
        }

        // Meal Plans
        public List<MealPlan> GetMealPlans()
        {
            if (!File.Exists(_mealPlansFile)) return new List<MealPlan>();
            var json = File.ReadAllText(_mealPlansFile);
            return JsonSerializer.Deserialize<List<MealPlan>>(json) ?? new List<MealPlan>();
        }

        public void SaveMealPlans(List<MealPlan> mealPlans)
        {
            var json = JsonSerializer.Serialize(mealPlans, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_mealPlansFile, json);
        }
    }
}