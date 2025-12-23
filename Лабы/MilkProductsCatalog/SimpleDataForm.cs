using System;
using System.Linq;
using System.Windows.Forms;
using MilkProductsCatalog.Models;

namespace MilkProductsCatalog
{
    public partial class SimpleDataForm : Form
    {
        SalesContext db = new SalesContext();
        
        // Элементы управления
        private DataGridView dataGridView1;
        private TextBox textBox1; // Название
        private TextBox textBox2; // Цена
        private TextBox textBox3; // Категория
        private TextBox textBox4; // Поиск
        private Button btnAdd;
        private Button btnDelete;
        private Button btnSearch;
        private Label label1, label2, label3, label4;

        public SimpleDataForm()
        {
            InitializeControls();
            LoadProducts();
        }

        private void InitializeControls()
        {
            this.Text = "Управление товарами";
            this.Size = new System.Drawing.Size(800, 600);

            // DataGridView
            dataGridView1 = new DataGridView
            {
                Location = new System.Drawing.Point(12, 12),
                Size = new System.Drawing.Size(760, 300),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            // Labels
            label1 = new Label { Location = new System.Drawing.Point(12, 330), Text = "Название:", Size = new System.Drawing.Size(80, 23) };
            label2 = new Label { Location = new System.Drawing.Point(12, 360), Text = "Цена:", Size = new System.Drawing.Size(80, 23) };
            label3 = new Label { Location = new System.Drawing.Point(12, 390), Text = "Категория:", Size = new System.Drawing.Size(80, 23) };
            label4 = new Label { Location = new System.Drawing.Point(12, 450), Text = "Поиск:", Size = new System.Drawing.Size(80, 23) };

            // TextBoxes
            textBox1 = new TextBox { Location = new System.Drawing.Point(100, 330), Size = new System.Drawing.Size(200, 23) };
            textBox2 = new TextBox { Location = new System.Drawing.Point(100, 360), Size = new System.Drawing.Size(200, 23) };
            textBox3 = new TextBox { Location = new System.Drawing.Point(100, 390), Size = new System.Drawing.Size(200, 23) };
            textBox4 = new TextBox { Location = new System.Drawing.Point(100, 450), Size = new System.Drawing.Size(200, 23) };

            // Buttons
            btnAdd = new Button
            {
                Location = new System.Drawing.Point(320, 330),
                Size = new System.Drawing.Size(100, 30),
                Text = "Добавить"
            };
            btnAdd.Click += btnAdd_Click;

            btnDelete = new Button
            {
                Location = new System.Drawing.Point(320, 370),
                Size = new System.Drawing.Size(100, 30),
                Text = "Удалить"
            };
            btnDelete.Click += btnDelete_Click;

            btnSearch = new Button
            {
                Location = new System.Drawing.Point(320, 450),
                Size = new System.Drawing.Size(100, 30),
                Text = "Найти"
            };
            btnSearch.Click += btnSearch_Click;

            // Добавляем элементы на форму
            this.Controls.AddRange(new Control[] {
                dataGridView1, label1, label2, label3, label4,
                textBox1, textBox2, textBox3, textBox4,
                btnAdd, btnDelete, btnSearch
            });
        }

        // Загрузка данных
        public void LoadProducts()
        {
            try
            {
                dataGridView1.DataSource = db.Products.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
        }

        // Добавление товара
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
                {
                    MessageBox.Show("Заполните название и цену!");
                    return;
                }

                Product product = new Product();
                product.ProductName = textBox1.Text;
                product.Price = Convert.ToDecimal(textBox2.Text);
                product.Category = textBox3.Text ?? "Молочные";
                product.ExpiryDays = 7;
                product.Description = "Добавлено через форму управления";

                db.Products.Add(product);
                db.SaveChanges();
                LoadProducts();
                ClearFields();
                MessageBox.Show("Товар добавлен!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления: {ex.Message}");
            }
        }

        // Удаление товара
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите товар для удаления!");
                    return;
                }

                var selectedProduct = dataGridView1.SelectedRows[0].DataBoundItem as Product;
                if (selectedProduct != null)
                {
                    db.Products.Remove(selectedProduct);
                    db.SaveChanges();
                    LoadProducts();
                    MessageBox.Show("Товар удален!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}");
            }
        }

        // Поиск товара
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string searchTerm = textBox4.Text.Trim();
                if (string.IsNullOrEmpty(searchTerm))
                {
                    LoadProducts();
                    return;
                }

                var foundProducts = db.Products
                    .Where(p => p.ProductName.Contains(searchTerm))
                    .ToList();

                dataGridView1.DataSource = foundProducts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска: {ex.Message}");
            }
        }

        // Очистка полей
        private void ClearFields()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            db?.Dispose();
            base.OnFormClosed(e);
        }
    }
}