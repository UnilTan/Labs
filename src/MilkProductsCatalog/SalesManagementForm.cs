using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MilkProductsCatalog.Models;

namespace MilkProductsCatalog
{
    public partial class SalesManagementForm : Form
    {
        SalesContext db = new SalesContext();
        
        // Элементы управления
        private DataGridView dataGridView1;
        private TextBox textBox1; // ID продажи
        private TextBox textBox2; // Имя покупателя
        private TextBox textBox3; // Общая сумма
        private TextBox textBoxSearch; // Поиск
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnSort;
        private Button btnSearch;
        private Button btnStats;
        private Button btnClear;
        private Button btnRefresh;
        private Button btnExport;
        private Button btnFilter;
        private GroupBox groupBoxControls;
        private GroupBox groupBoxSearch;
        private Label lblTitle;
        private Label lblId;
        private Label lblCustomer;
        private Label lblAmount;
        private Label lblSearch;
        private DateTimePicker dateTimePicker1;
        private Label lblDate;

        public SalesManagementForm()
        {
            InitializeControls();
            LoadData();
        }

        private void InitializeControls()
        {
            this.Text = "Управление продажами - Молочные продукты";
            this.Size = new Size(1200, 800);
            this.BackColor = Color.WhiteSmoke;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Заголовок
            lblTitle = new Label
            {
                Text = "Управление продажами молочных продуктов",
                Font = new Font("Microsoft Sans Serif", 18, FontStyle.Bold),
                Location = new Point(300, 20),
                Size = new Size(600, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.DarkBlue
            };

            // Группа полей ввода
            groupBoxControls = new GroupBox
            {
                Text = "Данные продажи",
                Location = new Point(30, 80),
                Size = new Size(400, 200),
                BackColor = Color.LightCyan,
                Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold)
            };

            // Поля ввода
            lblId = new Label
            {
                Text = "ID продажи:",
                Location = new Point(20, 30),
                Size = new Size(80, 23)
            };

            textBox1 = new TextBox
            {
                Location = new Point(110, 30),
                Size = new Size(120, 23),
                ReadOnly = true,
                BackColor = Color.LightGray
            };

            lblCustomer = new Label
            {
                Text = "Покупатель:",
                Location = new Point(20, 65),
                Size = new Size(80, 23)
            };

            textBox2 = new TextBox
            {
                Location = new Point(110, 65),
                Size = new Size(200, 23)
            };

            lblAmount = new Label
            {
                Text = "Сумма:",
                Location = new Point(20, 100),
                Size = new Size(80, 23)
            };

            textBox3 = new TextBox
            {
                Location = new Point(110, 100),
                Size = new Size(120, 23)
            };

            lblDate = new Label
            {
                Text = "Дата:",
                Location = new Point(20, 135),
                Size = new Size(80, 23)
            };

            dateTimePicker1 = new DateTimePicker
            {
                Location = new Point(110, 135),
                Size = new Size(200, 23),
                Format = DateTimePickerFormat.Short
            };

            // Добавляем элементы в группу
            groupBoxControls.Controls.AddRange(new Control[] {
                lblId, textBox1, lblCustomer, textBox2, 
                lblAmount, textBox3, lblDate, dateTimePicker1
            });

            // Группа поиска
            groupBoxSearch = new GroupBox
            {
                Text = "Поиск и фильтры",
                Location = new Point(450, 80),
                Size = new Size(300, 200),
                BackColor = Color.LightYellow,
                Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold)
            };

            lblSearch = new Label
            {
                Text = "Поиск по имени:",
                Location = new Point(20, 30),
                Size = new Size(100, 23)
            };

            textBoxSearch = new TextBox
            {
                Location = new Point(20, 55),
                Size = new Size(200, 23)
            };

            btnSearch = new Button
            {
                Text = "Найти",
                Location = new Point(230, 55),
                Size = new Size(60, 25),
                BackColor = Color.LightBlue
            };
            btnSearch.Click += BtnSearch_Click;

            btnFilter = new Button
            {
                Text = "Фильтр по дате",
                Location = new Point(20, 90),
                Size = new Size(120, 30),
                BackColor = Color.LightGreen
            };
            btnFilter.Click += BtnFilter_Click;

            btnStats = new Button
            {
                Text = "Статистика",
                Location = new Point(150, 90),
                Size = new Size(100, 30),
                BackColor = Color.LightCoral
            };
            btnStats.Click += BtnStats_Click;

            btnClear = new Button
            {
                Text = "Очистить",
                Location = new Point(20, 130),
                Size = new Size(80, 30),
                BackColor = Color.LightGray
            };
            btnClear.Click += BtnClear_Click;

            btnRefresh = new Button
            {
                Text = "Обновить",
                Location = new Point(110, 130),
                Size = new Size(80, 30),
                BackColor = Color.LightSteelBlue
            };
            btnRefresh.Click += BtnRefresh_Click;

            // Добавляем элементы в группу поиска
            groupBoxSearch.Controls.AddRange(new Control[] {
                lblSearch, textBoxSearch, btnSearch, btnFilter, 
                btnStats, btnClear, btnRefresh
            });

            // Основная таблица
            dataGridView1 = new DataGridView
            {
                Location = new Point(30, 300),
                Size = new Size(720, 350),
                BackgroundColor = Color.White,
                GridColor = Color.Gray,
                BorderStyle = BorderStyle.FixedSingle,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Кнопки управления данными (справа от таблицы)
            btnAdd = new Button
            {
                Text = "Добавить",
                Location = new Point(780, 300),
                Size = new Size(120, 40),
                BackColor = Color.LightGreen,
                Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold)
            };
            btnAdd.Click += BtnAdd_Click;

            btnUpdate = new Button
            {
                Text = "Изменить",
                Location = new Point(780, 350),
                Size = new Size(120, 40),
                BackColor = Color.LightBlue,
                Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold)
            };
            btnUpdate.Click += BtnUpdate_Click;

            btnDelete = new Button
            {
                Text = "Удалить",
                Location = new Point(780, 400),
                Size = new Size(120, 40),
                BackColor = Color.LightCoral,
                Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold)
            };
            btnDelete.Click += BtnDelete_Click;

            btnSort = new Button
            {
                Text = "Сортировать",
                Location = new Point(780, 450),
                Size = new Size(120, 40),
                BackColor = Color.LightYellow,
                Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold)
            };
            btnSort.Click += BtnSort_Click;

            btnExport = new Button
            {
                Text = "Экспорт",
                Location = new Point(780, 500),
                Size = new Size(120, 40),
                BackColor = Color.LightSalmon,
                Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold)
            };
            btnExport.Click += BtnExport_Click;

            // Добавляем все элементы на форму
            this.Controls.AddRange(new Control[] {
                lblTitle, groupBoxControls, groupBoxSearch, dataGridView1,
                btnAdd, btnUpdate, btnDelete, btnSort, btnExport
            });

            // Обработчик выбора строки в таблице
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        }

        // 1. Загрузка данных в таблицу
        public void LoadData()
        {
            try
            {
                var sales = db.Sales.OrderByDescending(s => s.SaleDate).ToList();
                dataGridView1.DataSource = sales;
                
                // Настройка заголовков колонок
                if (dataGridView1.Columns.Count > 0)
                {
                    dataGridView1.Columns["SaleId"].HeaderText = "ID";
                    dataGridView1.Columns["CustomerName"].HeaderText = "Покупатель";
                    dataGridView1.Columns["SaleDate"].HeaderText = "Дата продажи";
                    dataGridView1.Columns["TotalAmount"].HeaderText = "Сумма";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // 2. Добавление записи
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text))
                {
                    MessageBox.Show("Заполните имя покупателя и сумму!", "Внимание", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Sale sale = new Sale();
                sale.CustomerName = textBox2.Text.Trim();
                sale.TotalAmount = Convert.ToDecimal(textBox3.Text);
                sale.SaleDate = dateTimePicker1.Value;

                db.Sales.Add(sale);
                db.SaveChanges();
                LoadData();
                ClearFields();
                MessageBox.Show("Продажа добавлена успешно!", "Успех", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 3. Изменение записи
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("Выберите продажу для изменения!", "Внимание", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int saleId = Convert.ToInt32(textBox1.Text);
                var sale = db.Sales.FirstOrDefault(s => s.SaleId == saleId);
                
                if (sale == null)
                {
                    MessageBox.Show("Продажа не найдена!", "Ошибка", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                sale.CustomerName = textBox2.Text.Trim();
                sale.TotalAmount = Convert.ToDecimal(textBox3.Text);
                sale.SaleDate = dateTimePicker1.Value;

                db.SaveChanges();
                LoadData();
                MessageBox.Show("Продажа обновлена успешно!", "Успех", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 4. Удаление записи
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("Выберите продажу для удаления!", "Внимание", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show("Вы уверены, что хотите удалить эту продажу?", 
                    "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result != DialogResult.Yes) return;

                int saleId = Convert.ToInt32(textBox1.Text);
                var sale = db.Sales.FirstOrDefault(s => s.SaleId == saleId);
                
                if (sale == null)
                {
                    MessageBox.Show("Продажа не найдена!", "Ошибка", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                db.Sales.Remove(sale);
                db.SaveChanges();
                LoadData();
                ClearFields();
                MessageBox.Show("Продажа удалена успешно!", "Успех", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 5. Сортировка
        private void BtnSort_Click(object sender, EventArgs e)
        {
            try
            {
                var sortedSales = db.Sales.OrderBy(s => s.TotalAmount).ToList();
                dataGridView1.DataSource = sortedSales;
                MessageBox.Show("Данные отсортированы по сумме!", "Информация", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сортировки: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 6. Поиск
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string searchTerm = textBoxSearch.Text.Trim();
                if (string.IsNullOrEmpty(searchTerm))
                {
                    LoadData();
                    return;
                }

                var foundSales = db.Sales
                    .Where(s => s.CustomerName.Contains(searchTerm))
                    .OrderByDescending(s => s.SaleDate)
                    .ToList();

                dataGridView1.DataSource = foundSales;
                
                if (!foundSales.Any())
                {
                    MessageBox.Show("Продажи не найдены!", "Поиск", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Найдено продаж: {foundSales.Count}", "Поиск", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 7. Фильтр по дате
        private void BtnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime selectedDate = dateTimePicker1.Value.Date;
                var filteredSales = db.Sales
                    .Where(s => s.SaleDate.Date == selectedDate)
                    .OrderByDescending(s => s.SaleDate)
                    .ToList();

                dataGridView1.DataSource = filteredSales;
                
                MessageBox.Show($"Найдено продаж за {selectedDate.ToShortDateString()}: {filteredSales.Count}", 
                    "Фильтр", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 8. Статистика
        private void BtnStats_Click(object sender, EventArgs e)
        {
            try
            {
                var sales = db.Sales.ToList();
                if (!sales.Any())
                {
                    MessageBox.Show("Нет данных для статистики!", "Статистика", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                decimal totalAmount = sales.Sum(s => s.TotalAmount);
                decimal avgAmount = sales.Average(s => s.TotalAmount);
                decimal maxAmount = sales.Max(s => s.TotalAmount);
                decimal minAmount = sales.Min(s => s.TotalAmount);
                int totalCount = sales.Count;

                var today = DateTime.Today;
                var todaySales = sales.Where(s => s.SaleDate.Date == today).ToList();
                decimal todayAmount = todaySales.Sum(s => s.TotalAmount);

                string stats = $"Статистика продаж:\n\n" +
                              $"Всего продаж: {totalCount}\n" +
                              $"Общая сумма: {totalAmount:F2} руб.\n" +
                              $"Средняя сумма: {avgAmount:F2} руб.\n" +
                              $"Максимальная сумма: {maxAmount:F2} руб.\n" +
                              $"Минимальная сумма: {minAmount:F2} руб.\n\n" +
                              $"За сегодня:\n" +
                              $"Продаж: {todaySales.Count}\n" +
                              $"Сумма: {todayAmount:F2} руб.";

                MessageBox.Show(stats, "Статистика продаж", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка расчета статистики: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 9. Очистка полей
        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        // 10. Обновление данных
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            ClearFields();
            MessageBox.Show("Данные обновлены!", "Информация", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 11. Экспорт данных
        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                var sales = db.Sales.OrderByDescending(s => s.SaleDate).ToList();
                if (!sales.Any())
                {
                    MessageBox.Show("Нет данных для экспорта!", "Экспорт", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string exportData = "Отчет по продажам молочных продуктов\n";
                exportData += $"Дата создания: {DateTime.Now}\n\n";
                exportData += "ID\tПокупатель\t\tДата\t\tСумма\n";
                exportData += new string('-', 60) + "\n";

                foreach (var sale in sales)
                {
                    exportData += $"{sale.SaleId}\t{sale.CustomerName}\t\t{sale.SaleDate.ToShortDateString()}\t\t{sale.TotalAmount:F2}\n";
                }

                exportData += new string('-', 60) + "\n";
                exportData += $"Всего продаж: {sales.Count}\n";
                exportData += $"Общая сумма: {sales.Sum(s => s.TotalAmount):F2} руб.\n";

                // Сохранение в файл
                string fileName = $"Sales_Report_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                System.IO.File.WriteAllText(fileName, exportData);

                MessageBox.Show($"Отчет сохранен в файл: {fileName}", "Экспорт", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка экспорта: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 12. Заполнение полей при выборе записи
        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0) return;

                var selectedSale = dataGridView1.SelectedRows[0].DataBoundItem as Sale;
                if (selectedSale == null) return;

                textBox1.Text = selectedSale.SaleId.ToString();
                textBox2.Text = selectedSale.CustomerName;
                textBox3.Text = selectedSale.TotalAmount.ToString("F2");
                dateTimePicker1.Value = selectedSale.SaleDate;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выбора записи: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Очистка полей
        private void ClearFields()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBoxSearch.Text = "";
            dateTimePicker1.Value = DateTime.Now;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            db?.Dispose();
            base.OnFormClosed(e);
        }
    }
}