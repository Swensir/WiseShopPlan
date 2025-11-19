using SmartFoodPlanner.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SmartFoodPlanner
{
    public class AddEditRecipeForm : Form
    {
        public Recipe RecipeValue { get; private set; }
        private List<Product> _availableProducts;
        private List<RecipeIngredient> _ingredients;
        private ListBox listIngredients;

        public AddEditRecipeForm(List<Product> availableProducts)
        {
            _availableProducts = availableProducts;
            _ingredients = new List<RecipeIngredient>();
            RecipeValue = new Recipe();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Добавить рецепт";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterParent;

            // Recipe Name
            var lblName = new Label { Text = "Название рецепта:", Location = new Point(10, 20), Width = 120 };
            var txtName = new TextBox { Location = new Point(140, 20), Width = 200 };

            // Cooking Time
            var lblTime = new Label { Text = "Время приготовления (мин):", Location = new Point(10, 60), Width = 120 };
            var numericTime = new NumericUpDown { Location = new Point(140, 60), Width = 100, Minimum = 1, Maximum = 480, Value = 30 };

            // Description
            var lblDesc = new Label { Text = "Описание:", Location = new Point(10, 100), Width = 120 };
            var txtDesc = new TextBox { Location = new Point(140, 100), Width = 200, Height = 60 };
            txtDesc.Multiline = true;
            txtDesc.Height = 40;

            // Instructions
            var lblInstructions = new Label { Text = "Инструкция:", Location = new Point(10, 150), Width = 120 };
            var txtInstructions = new TextBox { Location = new Point(140, 150), Width = 200, Height = 80 };
            txtInstructions.Multiline = true;
            txtInstructions.Height = 60;

            // Ingredients section
            var lblIngredients = new Label { Text = "Ингредиенты:", Location = new Point(350, 20), Width = 100 };

            var comboProducts = new ComboBox { Location = new Point(350, 50), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            comboProducts.Items.AddRange(_availableProducts.Select(p => $"{p.Name} ({p.Unit})").ToArray());
            if (comboProducts.Items.Count > 0) comboProducts.SelectedIndex = 0;

            var numericQty = new NumericUpDown { Location = new Point(510, 50), Width = 60, Minimum = 0.1m, Maximum = 100, Value = 1, DecimalPlaces = 2 };

            var btnAddIngredient = new Button { Text = "Добавить", Location = new Point(350, 80), Width = 80 };
            var btnRemoveIngredient = new Button { Text = "Удалить", Location = new Point(440, 80), Width = 80 };

            listIngredients = new ListBox { Location = new Point(350, 110), Width = 220, Height = 150 };

            // Buttons
            var btnOK = new Button { Text = "OK", Location = new Point(200, 400), Width = 80 };
            var btnCancel = new Button { Text = "Отмена", Location = new Point(300, 400), Width = 80 };

            // Events
            btnAddIngredient.Click += (s, e) =>
            {
                if (comboProducts.SelectedIndex == -1) return;

                var productName = comboProducts.Text.Split('(')[0].Trim();
                var product = _availableProducts.FirstOrDefault(p => p.Name == productName);

                if (product != null)
                {
                    var ingredient = new RecipeIngredient
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Quantity = numericQty.Value,
                        Unit = product.Unit
                    };

                    _ingredients.Add(ingredient);
                    listIngredients.Items.Add($"{ingredient.ProductName} - {ingredient.Quantity} {ingredient.Unit}");
                }
            };

            btnRemoveIngredient.Click += (s, e) =>
            {
                if (listIngredients.SelectedIndex != -1)
                {
                    _ingredients.RemoveAt(listIngredients.SelectedIndex);
                    listIngredients.Items.RemoveAt(listIngredients.SelectedIndex);
                }
            };

            btnOK.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Введите название рецепта!");
                    return;
                }

                if (_ingredients.Count == 0)
                {
                    MessageBox.Show("Добавьте ингредиенты!");
                    return;
                }

                RecipeValue.Name = txtName.Text;
                RecipeValue.Description = txtDesc.Text;
                RecipeValue.Instructions = txtInstructions.Text;
                RecipeValue.CookingTime = (int)numericTime.Value;
                RecipeValue.Ingredients = _ingredients;

                DialogResult = DialogResult.OK;
                Close();
            };

            btnCancel.Click += (s, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };

            this.Controls.AddRange(new Control[] {
                lblName, txtName, lblTime, numericTime, lblDesc, txtDesc,
                lblInstructions, txtInstructions, lblIngredients, comboProducts,
                numericQty, btnAddIngredient, btnRemoveIngredient, listIngredients,
                btnOK, btnCancel
            });

            this.ResumeLayout(false);
        }
    }
}