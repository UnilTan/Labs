using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MilkProductsCatalog.Models;

namespace MilkProductsCatalog
{
    public partial class ProductTableForm : Form
    {
        SalesContext db = new SalesContext();
        
        // Элементы управления
        private DataGridView dataGridView1;
        private TextBox textBox1; // ID
        private TextBox textBox2; // Название
        private TextBox textBox3; // Цена
        private TextBox textBoxFind; // Поиск
        private Button btnIns;
        private Button btnSave;
        private Button btnDel;
        private Button btnSort;
        private Button btnFind;
        private Button button6;
        private Button button7;
        private Button button8;
        private Button button9;
        private Button button10;
        private GroupBox groupBox1;
        private Label lblTitle;
        private Label lblFind;

        public ProductTableForm()
        {
            InitializeControls();
            LoadData();
        }

        private void InitializeControls()
        {
            this.Text = "Моя таблица - Молочные продукты";
            this.Size = new Size(1000, 700);
            this.BackColor = Color.LightGray;

            // Заголовок
            lblTitle = new Label
            {
                Text = "Моя таблица",
                Font = new Font("Microsoft Sans Serif", 16, FontStyle.Bold),
                Location = new Point(350, 20),
                Size = new Size(200, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Поля ввода сверху
            textBox1 = new TextBox
            {
                Location = new Point(50, 70),
                Size = new Size(120, 23)
            };

            textBox2 = new TextBox
            {
                Location = new Point(200, 70),
                Size = new Size(120, 23)
            };

            textBox3 = new TextBox
            {
                Location = new Point(350, 70),
                Size = new Size(120, 23)
            };

            // Основная таблица
            dataGridView1 = new DataGridView
            {
                Location = new Point(50, 120),
                Size = new Size(600, 300),
                BackgroundColor = Color.Gray,
                GridColor = Color.Black,
                BorderStyle = BorderStyle.FixedSingle,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            // Поле поиска
            lblFind = new Label
            {
                Text = "FIND",
                Location = new Point(300, 450),
                Size = new Size(50, 23),
                Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold)
            };

            textBoxFind = new TextBox
            {
                Location = new Point(360, 450),
                Size = new Size(120, 23)
            };

            // Группа кнопок справа (бирюзовая)
            groupBox1 = new GroupBox
            {
                Text = "groupBox1",
                Location = new Point(700, 50),
                Size = new Size(250, 450),
                BackColor = Color.Turquoise
            };

            // Кнопки в группе
            btnIns = new Button
            {
                Text = "Ins",
                Location = new Point(20, 30),
                Size = new Size(200, 40),
                BackColor = Color.LightGray
            };
            btnIns.Click += BtnIns_Click;

            btnSave = new Button
            {
                Text = "Save",
                Location = new Point(20, 80),
                Size = new Size(200, 40),
                BackColor = Color.LightGray
            };
            btnSave.Click += BtnSave_Click;

            btnDel = new Button
            {
                Text = "Del",
                Location = new Point(20, 130),
                Size = new Size(200, 40),
                BackColor = Color.LightGray
            };
            btnDel.Click += BtnDel_Click;

            btnSort = new Button
            {
                Text = "Sort",
                Location = new Point(20, 180),
                Size = new Size(200, 40),
                BackColor = Color.LightGray
            };
            btnSort.Click += BtnSort_Click;

            btnFind = new Button
            {
                Text = "Find",
                Location = new Point(20, 230),
                Size = new Size(200, 40),
                BackColor = Color.LightGray
            };
            btnFind.Click += BtnFind_Click;

            button6 = new Button
            {
                Text = "button6",
                Location = new Point(20, 280),
                Size = new Size(200, 40),
                BackColor = Color.LightGray
            };
            button6.Click += Button6_Click;

            // Кнопки внизу
            button7 = new Button
            {
                Text = "button7",
                Location = new Point(50, 520),
                Size = new Size(120, 40),
                BackColor = Color.LightGray
            };
            button7.Click += Button7_Click;

            button8 = new Button
            {
                Text = "button8",
                Location = new Point(200, 520),
                Size = new Size(120, 40),
                BackColor = Color.LightGray
            };
            button8.Click += Button8_Click;

            button9 = new Button
            {
                Text = "button9",
                Location = new Point(350, 520),
                Size = new Size(120, 40),
                BackColor = Color.LightGray
            };
            button9.Click += Button9_Click;

            button10 = new Button
            {
                Text = "button10",
                Location = new Point(500, 520),
                Size = new Size(120, 40),
                BackColor = Color.LightGray
            };
            button10.Click += Button10_Click;

            // Добавляем кнопки в группу
            groupBox1.Controls.AddRange(new Control[] {
                btnIns, btnSave, btnDel, btnSort, btnFind, button6
            });

            // Добавляем все элементы на форму
            this.Controls.AddRange(new Control[] {
                lblTitle, textBox1, textBox2, textBox3, dataGridView1,
                lblFind, textBoxFind, groupBox1,
                button7, button8, button9, button10
            });

            // Обработчик выбора строки в таблице
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        }

        // 1. Загрузка данных в таблицу
        public void LoadData()
        {
            try
            {
                dataGridView1.DataSource = db.Products.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }
        // 2. Добавление записи (Ins)
        private void BtnIns_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text))
                {
                    MessageBox.Show("Заполните название и цену!");
                    return;
                }

                Product product = new Product();
                product.ProductName = textBox2.Text;
                product.Price = Convert.ToDecimal(textBox3.Text);
                product.Category = "Молочные";
                product.ExpiryDays = 7;
                product.Description = "Добавлено через форму управления";

                db.Products.Add(product);
                db.SaveChanges();
                LoadData();
                ClearFields();
                MessageBox.Show("Товар добавлен!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления: {ex.Message}");
            }
        }

        // 3. Сохранение изменений (Save)
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                db.SaveChanges();
                LoadData();
                MessageBox.Show("Изменения сохранены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        // 4. Удаление записи (Del)
        private void BtnDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("Введите ID товара для удаления!");
                    return;
                }

                int numDel = Convert.ToInt32(textBox1.Text);
                var recDel = db.Products.Where(rec => rec.ProductId == numDel).FirstOrDefault();
                
                if (recDel == null)
                {
                    MessageBox.Show("Запись не найдена!");
                    return;
                }

                db.Products.Remove(recDel);
                db.SaveChanges();
                LoadData();
                ClearFields();
                MessageBox.Show("Товар удален!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}");
            }
        }

        // 5. Сортировка (Sort)
        private void BtnSort_Click(object sender, EventArgs e)
        {
            try
            {
                var sortedProducts = db.Products.OrderBy(p => p.Price).ToList();
                dataGridView1.DataSource = sortedProducts;
                MessageBox.Show("Данные отсортированы по цене!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сортировки: {ex.Message}");
            }
        }

        // 6. Поиск (Find)
        private void BtnFind_Click(object sender, EventArgs e)
        {
            try
            {
                string searchTerm = textBoxFind.Text.Trim();
                if (string.IsNullOrEmpty(searchTerm))
                {
                    LoadData();
                    return;
                }

                var foundProducts = db.Products
                    .Where(p => p.ProductName.Contains(searchTerm))
                    .ToList();

                dataGridView1.DataSource = foundProducts;
                
                if (!foundProducts.Any())
                {
                    MessageBox.Show("Товары не найдены!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска: {ex.Message}");
            }
        }

        // 7. Дополнительная функция (button6)
        private void Button6_Click(object sender, EventArgs e)
        {
            try
            {
                // Показать статистику
                var products = db.Products.ToList();
                if (!products.Any())
                {
                    MessageBox.Show("Нет данных для статистики!");
                    return;
                }

                decimal maxPrice = products.Max(p => p.Price);
                decimal minPrice = products.Min(p => p.Price);
                decimal avgPrice = products.Average(p => p.Price);
                int totalCount = products.Count;

                string stats = $"Статистика по товарам:\n" +
                              $"Всего товаров: {totalCount}\n" +
                              $"Максимальная цена: {maxPrice:F2}\n" +
                              $"Минимальная цена: {minPrice:F2}\n" +
                              $"Средняя цена: {avgPrice:F2}";

                MessageBox.Show(stats, "Статистика");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка расчета статистики: {ex.Message}");
            }
        }

        // 8. Заполнение полей при выборе записи
        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0) return;

                var selectedProduct = dataGridView1.SelectedRows[0].DataBoundItem as Product;
                if (selectedProduct == null) return;

                textBox1.Text = selectedProduct.ProductId.ToString();
                textBox2.Text = selectedProduct.ProductName;
                textBox3.Text = selectedProduct.Price.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выбора записи: {ex.Message}");
            }
        }

        // Дополнительные кнопки (button7-10)
        private void Button7_Click(object sender, EventArgs e)
        {
            // Фильтр по категории "Молоко"
            try
            {
                var milkProducts = db.Products.Where(p => p.Category == "Молоко").ToList();
                dataGridView1.DataSource = milkProducts;
                MessageBox.Show($"Найдено товаров категории 'Молоко': {milkProducts.Count}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации: {ex.Message}");
            }
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            // Фильтр по категории "Творог"
            try
            {
                var cottageProducts = db.Products.Where(p => p.Category == "Творог").ToList();
                dataGridView1.DataSource = cottageProducts;
                MessageBox.Show($"Найдено товаров категории 'Творог': {cottageProducts.Count}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации: {ex.Message}");
            }
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            // Показать товары с истекающим сроком годности
            try
            {
                var expiringProducts = db.Products.Where(p => p.ExpiryDays <= 5).ToList();
                dataGridView1.DataSource = expiringProducts;
                MessageBox.Show($"Товаров с коротким сроком годности: {expiringProducts.Count}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации: {ex.Message}");
            }
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            // Обновить все данные
            LoadData();
            ClearFields();
            MessageBox.Show("Данные обновлены!");
        }

        // Очистка полей
        private void ClearFields()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBoxFind.Text = "";
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            db?.Dispose();
            base.OnFormClosed(e);
        }
    }
}