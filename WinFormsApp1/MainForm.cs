using SmartFoodPlanner.Models;
using SmartFoodPlanner.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SmartFoodPlanner
{
    public partial class MainForm : Form
    {
        private JsonDataService _dataService;
        private RecipeService _recipeService;
        private List<Product> _products;
        private List<Recipe> _recipes;
        private List<MealPlan> _mealPlans;

        private TabControl tabControl;
        private DataGridView dataGridViewProducts;
        private DataGridView dataGridViewRecipes;
        private DataGridView dataGridViewMealPlan;
        private ListBox listBoxRecipeResults;
        private ListBox listBoxAvailableRecipes;
        private ListBox listBoxShoppingList;
        private DateTimePicker dateTimePickerPlanDate;

        public MainForm()
        {
            InitializeComponent();
            _dataService = new JsonDataService();
            _recipeService = new RecipeService(_dataService);
            LoadData();
            InitializeTabs();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Main Form
            this.Text = "Умный планировщик покупок еды";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Tab Control
            tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 0);
            tabControl.Size = new Size(1000, 600);

            // Create tabs
            CreateProductsTab();
            CreateRecipesTab();
            CreateMealPlannerTab();
            CreateShoppingListTab();

            this.Controls.Add(tabControl);
            this.ResumeLayout(false);
        }

        private void CreateProductsTab()
        {
            var tabPage = new TabPage("Мои продукты");

            // Data Grid View
            dataGridViewProducts = new DataGridView();
            dataGridViewProducts.Location = new Point(10, 10);
            dataGridViewProducts.Size = new Size(750, 400);
            dataGridViewProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Buttons
            var btnAdd = new Button();
            btnAdd.Text = "Добавить продукт";
            btnAdd.Location = new Point(10, 420);
            btnAdd.Size = new Size(120, 30);
            btnAdd.Click += BtnAddProduct_Click;

            var btnDelete = new Button();
            btnDelete.Text = "Удалить продукт";
            btnDelete.Location = new Point(140, 420);
            btnDelete.Size = new Size(120, 30);
            btnDelete.Click += BtnDeleteProduct_Click;

            tabPage.Controls.AddRange(new Control[] { dataGridViewProducts, btnAdd, btnDelete });
            tabControl.Controls.Add(tabPage);
        }

        private void CreateRecipesTab()
        {
            var tabPage = new TabPage("Рецепты");

            // Data Grid View
            dataGridViewRecipes = new DataGridView();
            dataGridViewRecipes.Location = new Point(10, 10);
            dataGridViewRecipes.Size = new Size(500, 200);
            dataGridViewRecipes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Buttons
            var btnAddRecipe = new Button();
            btnAddRecipe.Text = "Добавить рецепт";
            btnAddRecipe.Location = new Point(10, 220);
            btnAddRecipe.Size = new Size(120, 30);
            btnAddRecipe.Click += BtnAddRecipe_Click;

            var btnFindByProducts = new Button();
            btnFindByProducts.Text = "Найти по продуктам";
            btnFindByProducts.Location = new Point(140, 220);
            btnFindByProducts.Size = new Size(140, 30);
            btnFindByProducts.Click += BtnFindByProducts_Click;

            var btnFindByExpiring = new Button();
            btnFindByExpiring.Text = "Для скоропортящихся";
            btnFindByExpiring.Location = new Point(290, 220);
            btnFindByExpiring.Size = new Size(140, 30);
            btnFindByExpiring.Click += BtnFindByExpiring_Click;

            var btnUseRecipe = new Button();
            btnUseRecipe.Text = "Использовать рецепт";
            btnUseRecipe.Location = new Point(440, 220);
            btnUseRecipe.Size = new Size(120, 30);
            btnUseRecipe.Click += BtnUseRecipe_Click;

            // Results ListBox
            listBoxRecipeResults = new ListBox();
            listBoxRecipeResults.Location = new Point(10, 260);
            listBoxRecipeResults.Size = new Size(500, 200);

            tabPage.Controls.AddRange(new Control[] {
                dataGridViewRecipes, btnAddRecipe, btnFindByProducts,
                btnFindByExpiring, btnUseRecipe, listBoxRecipeResults
            });
            tabControl.Controls.Add(tabPage);
        }

        private void CreateMealPlannerTab()
        {
            var tabPage = new TabPage("Планировщик");

            // Date Picker
            dateTimePickerPlanDate = new DateTimePicker();
            dateTimePickerPlanDate.Location = new Point(10, 10);
            dateTimePickerPlanDate.Size = new Size(150, 20);
            dateTimePickerPlanDate.ValueChanged += DateTimePickerPlanDate_ValueChanged;

            // Available Recipes ListBox
            listBoxAvailableRecipes = new ListBox();
            listBoxAvailableRecipes.Location = new Point(10, 40);
            listBoxAvailableRecipes.Size = new Size(300, 200);

            // Add to Plan Button
            var btnAddToPlan = new Button();
            btnAddToPlan.Text = "Добавить в план";
            btnAddToPlan.Location = new Point(320, 40);
            btnAddToPlan.Size = new Size(120, 30);
            btnAddToPlan.Click += BtnAddToPlan_Click;

            // Meal Plan Data Grid
            dataGridViewMealPlan = new DataGridView();
            dataGridViewMealPlan.Location = new Point(10, 250);
            dataGridViewMealPlan.Size = new Size(500, 200);
            dataGridViewMealPlan.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            tabPage.Controls.AddRange(new Control[] {
                dateTimePickerPlanDate, listBoxAvailableRecipes,
                btnAddToPlan, dataGridViewMealPlan
            });
            tabControl.Controls.Add(tabPage);
        }

        private void CreateShoppingListTab()
        {
            var tabPage = new TabPage("Список покупок");

            listBoxShoppingList = new ListBox();
            listBoxShoppingList.Dock = DockStyle.Fill;
            listBoxShoppingList.Location = new Point(0, 0);
            listBoxShoppingList.Size = new Size(992, 572);

            tabPage.Controls.Add(listBoxShoppingList);
            tabControl.Controls.Add(tabPage);
        }

        private void LoadData()
        {
            _products = _dataService.GetProducts();
            _recipes = _dataService.GetRecipes();
            _mealPlans = _dataService.GetMealPlans();
        }

        private void InitializeTabs()
        {
            RefreshProductsGrid();
            RefreshRecipesGrid();
            RefreshMealPlanGrid();
            RefreshShoppingList();
            RefreshAvailableRecipesForPlan();
        }

        
        private void RefreshProductsGrid()
        {
            dataGridViewProducts.DataSource = null;
            dataGridViewProducts.DataSource = _products;
        }

        private void BtnAddProduct_Click(object sender, EventArgs e)
        {
            var form = new AddEditProductForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                var newId = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
                var product = new Product
                {
                    Id = newId,
                    Name = form.ProductNameValue,
                    Category = form.CategoryValue,
                    Unit = form.UnitValue,
                    Quantity = form.QuantityValue,
                    Status = form.StatusValue,
                    ExpiryDate = form.ExpiryDateValue
                };

                _products.Add(product);
                _dataService.SaveProducts(_products);
                RefreshProductsGrid();
            }
        }

        private void BtnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count > 0)
            {
                var productId = (int)dataGridViewProducts.SelectedRows[0].Cells["Id"].Value;
                _products.RemoveAll(p => p.Id == productId);
                _dataService.SaveProducts(_products);
                RefreshProductsGrid();
            }
        }

        
        private void RefreshRecipesGrid()
        {
            dataGridViewRecipes.DataSource = null;
            dataGridViewRecipes.DataSource = _recipes.Select(r => new
            {
                r.Id,
                r.Name,
                r.CookingTime,
                IngredientsCount = r.Ingredients.Count
            }).ToList();
        }

        private void BtnAddRecipe_Click(object sender, EventArgs e)
        {
            var form = new AddEditRecipeForm(_products);
            if (form.ShowDialog() == DialogResult.OK)
            {
                var newId = _recipes.Any() ? _recipes.Max(r => r.Id) + 1 : 1;
                form.RecipeValue.Id = newId;
                _recipes.Add(form.RecipeValue);
                _dataService.SaveRecipes(_recipes);
                RefreshRecipesGrid();
            }
        }

        private void BtnFindByProducts_Click(object sender, EventArgs e)
        {
            var availableRecipes = _recipeService.GetRecipesByAvailableProducts();
            ShowRecipesInListBox(availableRecipes, "Рецепты из доступных продуктов:");
        }

        private void BtnFindByExpiring_Click(object sender, EventArgs e)
        {
            var expiringRecipes = _recipeService.GetRecipesByExpiringProducts();
            ShowRecipesInListBox(expiringRecipes, "Рецепты для скоропортящихся продуктов:");
        }

        private void ShowRecipesInListBox(List<Recipe> recipes, string title)
        {
            listBoxRecipeResults.Items.Clear();
            listBoxRecipeResults.Items.Add(title);
            listBoxRecipeResults.Items.Add("");

            foreach (var recipe in recipes)
            {
                listBoxRecipeResults.Items.Add($"• {recipe.Name} ({recipe.CookingTime} мин)");
            }
        }

        private void BtnUseRecipe_Click(object sender, EventArgs e)
        {
            if (dataGridViewRecipes.SelectedRows.Count > 0)
            {
                var recipeId = (int)dataGridViewRecipes.SelectedRows[0].Cells["Id"].Value;
                _recipeService.UseRecipeIngredients(recipeId);
                LoadData();
                RefreshProductsGrid();
                MessageBox.Show("Ингредиенты списаны!");
            }
        }

       
        private void RefreshMealPlanGrid()
        {
            dataGridViewMealPlan.DataSource = null;
            dataGridViewMealPlan.DataSource = _mealPlans.Select(mp => new
            {
                mp.Date,
                mp.RecipeName
            }).ToList();
        }

        private void BtnAddToPlan_Click(object sender, EventArgs e)
        {
            if (dateTimePickerPlanDate.Value < DateTime.Today)
            {
                MessageBox.Show("Нельзя планировать на прошедшие даты!");
                return;
            }

            if (listBoxAvailableRecipes.SelectedItem == null)
            {
                MessageBox.Show("Выберите рецепт!");
                return;
            }

            var recipeName = listBoxAvailableRecipes.SelectedItem.ToString();
            var recipe = _recipes.FirstOrDefault(r => r.Name == recipeName);
            if (recipe != null)
            {
                var mealPlan = new MealPlan
                {
                    Date = dateTimePickerPlanDate.Value,
                    RecipeId = recipe.Id,
                    RecipeName = recipe.Name
                };

                _mealPlans.Add(mealPlan);
                _dataService.SaveMealPlans(_mealPlans);
                RefreshMealPlanGrid();
                RefreshShoppingList();
            }
        }

        private void DateTimePickerPlanDate_ValueChanged(object sender, EventArgs e)
        {
            RefreshAvailableRecipesForPlan();
        }

        private void RefreshAvailableRecipesForPlan()
        {
            listBoxAvailableRecipes.Items.Clear();
            var availableRecipes = _recipeService.GetRecipesByAvailableProducts();
            foreach (var recipe in availableRecipes)
            {
                listBoxAvailableRecipes.Items.Add(recipe.Name);
            }
        }

        
        private void RefreshShoppingList()
        {
            listBoxShoppingList.Items.Clear();

            var requiredIngredients = new Dictionary<string, (decimal Quantity, string Unit)>();

            foreach (var mealPlan in _mealPlans.Where(mp => mp.Date >= DateTime.Today && mp.Date <= DateTime.Today.AddDays(7)))
            {
                var recipe = _recipes.FirstOrDefault(r => r.Id == mealPlan.RecipeId);
                if (recipe != null)
                {
                    foreach (var ingredient in recipe.Ingredients)
                    {
                        var key = $"{ingredient.ProductName}|{ingredient.Unit}";
                        if (requiredIngredients.ContainsKey(key))
                        {
                            var current = requiredIngredients[key];
                            requiredIngredients[key] = (current.Quantity + ingredient.Quantity, current.Unit);
                        }
                        else
                        {
                            requiredIngredients[key] = (ingredient.Quantity, ingredient.Unit);
                        }
                    }
                }
            }

            foreach (var product in _products.Where(p => p.Quantity > 0))
            {
                var key = $"{product.Name}|{product.Unit}";
                if (requiredIngredients.ContainsKey(key))
                {
                    var required = requiredIngredients[key];
                    var newQuantity = required.Quantity - product.Quantity;
                    if (newQuantity > 0)
                    {
                        requiredIngredients[key] = (newQuantity, required.Unit);
                    }
                    else
                    {
                        requiredIngredients.Remove(key);
                    }
                }
            }

            listBoxShoppingList.Items.Add("=== СПИСОК ПОКУПОК ===");
            listBoxShoppingList.Items.Add("");

            foreach (var item in requiredIngredients)
            {
                var parts = item.Key.Split('|');
                var productName = parts[0];
                var unit = parts[1];
                listBoxShoppingList.Items.Add($"□ {productName} - {item.Value.Quantity} {item.Value.Unit}");
            }

            if (requiredIngredients.Count == 0)
            {
                listBoxShoppingList.Items.Add("Все необходимое уже есть дома!");
            }
        }
    }
}