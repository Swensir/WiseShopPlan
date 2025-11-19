using SmartFoodPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartFoodPlanner.Services
{
    public class RecipeService
    {
        private readonly JsonDataService _dataService;
        private List<Product> _products;
        private List<Recipe> _recipes;

        public RecipeService(JsonDataService dataService)
        {
            _dataService = dataService;
            LoadData();
        }

        private void LoadData()
        {
            _products = _dataService.GetProducts();
            _recipes = _dataService.GetRecipes();
        }

        public List<Recipe> GetRecipesByAvailableProducts()
        {
            var availableRecipes = new List<Recipe>();

            foreach (var recipe in _recipes)
            {
                var canCook = true;

                foreach (var ingredient in recipe.Ingredients)
                {
                    var product = _products.FirstOrDefault(p => p.Id == ingredient.ProductId);
                    if (product == null || product.Quantity < ingredient.Quantity)
                    {
                        canCook = false;
                        break;
                    }
                }

                if (canCook)
                {
                    availableRecipes.Add(recipe);
                }
            }

            return availableRecipes;
        }

        public List<Recipe> GetRecipesByExpiringProducts()
        {
            var expiringProductIds = _products
                .Where(p => p.Status == ProductStatus.NeedToUse && p.Quantity > 0)
                .Select(p => p.Id)
                .ToList();

            var recipesWithExpiringProducts = _recipes
                .Where(recipe => recipe.Ingredients.Any(ingredient =>
                    expiringProductIds.Contains(ingredient.ProductId)))
                .OrderByDescending(recipe => recipe.Ingredients
                    .Count(ingredient => expiringProductIds.Contains(ingredient.ProductId)))
                .ToList();

            return recipesWithExpiringProducts;
        }

        public void UseRecipeIngredients(int recipeId)
        {
            var recipe = _recipes.FirstOrDefault(r => r.Id == recipeId);
            if (recipe == null) return;

            foreach (var ingredient in recipe.Ingredients)
            {
                var product = _products.FirstOrDefault(p => p.Id == ingredient.ProductId);
                if (product != null)
                {
                    product.Quantity -= ingredient.Quantity;
                    if (product.Quantity < 0) product.Quantity = 0;

                    // Update status based on new quantity
                    if (product.Quantity == 0)
                        product.Status = ProductStatus.InStock;
                    else if (product.Quantity < 0.3m * GetAverageUsage(product.Id))
                        product.Status = ProductStatus.Leftover;
                }
            }

            _dataService.SaveProducts(_products);
        }

        private decimal GetAverageUsage(int productId)
        {
            // Simplified - in real app would track historical usage
            return 1.0m;
        }
    }
}