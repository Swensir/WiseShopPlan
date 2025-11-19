using SmartFoodPlanner.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SmartFoodPlanner
{
    public class AddEditProductForm : Form
    {
        public string ProductNameValue { get; private set; }
        public string CategoryValue { get; private set; }
        public string UnitValue { get; private set; }
        public decimal QuantityValue { get; private set; }
        public ProductStatus StatusValue { get; private set; }
        public DateTime? ExpiryDateValue { get; private set; }

        private TextBox txtName;
        private TextBox txtCategory;
        private TextBox txtUnit;
        private NumericUpDown numericQuantity;
        private ComboBox comboStatus;
        private CheckBox checkBoxExpiry;
        private DateTimePicker dateTimePickerExpiry;

        public AddEditProductForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form settings
            this.Text = "Добавить продукт";
            this.Size = new Size(300, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Controls
            var lblName = new Label { Text = "Название:", Location = new Point(10, 20), Width = 100 };
            txtName = new TextBox { Location = new Point(120, 20), Width = 150 };

            var lblCategory = new Label { Text = "Категория:", Location = new Point(10, 60), Width = 100 };
            txtCategory = new TextBox { Location = new Point(120, 60), Width = 150 };

            var lblUnit = new Label { Text = "Единица измерения:", Location = new Point(10, 100), Width = 100 };
            txtUnit = new TextBox { Location = new Point(120, 100), Width = 150, Text = "шт" };

            var lblQuantity = new Label { Text = "Количество:", Location = new Point(10, 140), Width = 100 };
            numericQuantity = new NumericUpDown
            {
                Location = new Point(120, 140),
                Width = 150,
                Minimum = 0,
                Maximum = 1000,
                DecimalPlaces = 2,
                Value = 1
            };

            var lblStatus = new Label { Text = "Статус:", Location = new Point(10, 180), Width = 100 };
            comboStatus = new ComboBox { Location = new Point(120, 180), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            comboStatus.Items.AddRange(Enum.GetNames(typeof(ProductStatus)));
            comboStatus.SelectedIndex = 0;

            checkBoxExpiry = new CheckBox { Text = "Срок годности", Location = new Point(10, 220), Width = 120 };
            dateTimePickerExpiry = new DateTimePicker
            {
                Location = new Point(120, 220),
                Width = 150,
                Enabled = false,
                MinDate = DateTime.Today,
                Value = DateTime.Today.AddDays(7)
            };
            checkBoxExpiry.CheckedChanged += (s, e) => dateTimePickerExpiry.Enabled = checkBoxExpiry.Checked;

            var btnOK = new Button { Text = "OK", Location = new Point(50, 270), Width = 80 };
            btnOK.Click += BtnOK_Click;

            var btnCancel = new Button { Text = "Отмена", Location = new Point(150, 270), Width = 80 };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                lblName, txtName, lblCategory, txtCategory, lblUnit, txtUnit,
                lblQuantity, numericQuantity, lblStatus, comboStatus,
                checkBoxExpiry, dateTimePickerExpiry, btnOK, btnCancel
            });

            this.ResumeLayout(false);
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название продукта!");
                return;
            }

            ProductNameValue = txtName.Text;
            CategoryValue = txtCategory.Text;
            UnitValue = txtUnit.Text;
            QuantityValue = numericQuantity.Value;
            StatusValue = (ProductStatus)comboStatus.SelectedIndex;
            ExpiryDateValue = checkBoxExpiry.Checked ? dateTimePickerExpiry.Value : (DateTime?)null;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}