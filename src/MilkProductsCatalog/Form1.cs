using MilkProductsCatalog.Models;
using MilkProductsCatalog.Services;
using Microsoft.EntityFrameworkCore;

namespace MilkProductsCatalog
{
    public partial class Form1 : Form
    {
        private SalesContext db = new SalesContext();
        private readonly CatalogDataService dataService = new CatalogDataService("Server=.\\SQLEXPRESS;Database=Familia22i1L9;Integrated Security=true;TrustServerCertificate=true;Encrypt=false;");

        public Form1()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                // Проверяем подключение к базе данных
                if (!db.Database.CanConnect())
                {
                    // Пробуем найти рабочее подключение
                    var (success, workingConnection, error) = DatabaseHelper.TestConnections();
                    
                    if (success)
                    {
                        // Устанавливаем рабочую строку подключения
                        SalesContext.SetWorkingConnectionString(workingConnection);
                        
                        // Создаем новый контекст с рабочим подключением
                        db?.Dispose();
                        db = new SalesContext();
                        
                        // Пробуем снова
                        if (!db.Database.CanConnect())
                        {
                            ShowConnectionError();
                            return;
                        }
                    }
                    else
                    {
                        ShowConnectionError();
                        return;
                    }
                }

                var products = db.Products.ToList();
                dataGridView1.DataSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}\n\nВнутренняя ошибка: {ex.InnerException?.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowConnectionError()
        {
            MessageBox.Show(
                "Не удается подключиться к базе данных.\n\n" +
                "Проверьте:\n" +
                "1. Запущен ли SQL Server\n" +
                "2. Существует ли база данных Familia22i1L9\n" +
                "3. Правильность строки подключения в SalesContext.cs\n\n" +
                "Для SQL Server Express используйте: Server=.\\SQLEXPRESS;...\n\n" +
                "Нажмите 'Тест подключения' для диагностики.",
                "Ошибка подключения к БД",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private void btnPrice_Click(object sender, EventArgs e)
        {
            try
            {
                var products = db.Products.ToList();
                
                if (products.Any())
                {
                    var maxPrice = products.Max(p => p.Price);
                    var minPrice = products.Min(p => p.Price);
                    var avgPrice = products.Average(p => p.Price);

                    txtMaxPrice.Text = maxPrice.ToString("F2");
                    txtMinPrice.Text = minPrice.ToString("F2");
                    txtAvgPrice.Text = avgPrice.ToString("F2");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка вычисления цен: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSortAsc_Click(object sender, EventArgs e)
        {
            try
            {
                var sortedProducts = db.Products.OrderBy(p => p.Price).ToList();
                dataGridView1.DataSource = sortedProducts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сортировки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSortDesc_Click(object sender, EventArgs e)
        {
            try
            {
                var sortedProducts = db.Products.OrderByDescending(p => p.Price).ToList();
                dataGridView1.DataSource = sortedProducts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сортировки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string searchTerm = txtSearch.Text.Trim();
                if (string.IsNullOrEmpty(searchTerm))
                {
                    LoadProducts();
                    return;
                }

                var foundProducts = db.Products
                    .Where(p => p.ProductName.Contains(searchTerm))
                    .ToList();

                dataGridView1.DataSource = foundProducts;

                if (!foundProducts.Any())
                {
                    MessageBox.Show("Товары не найдены", "Поиск", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPartialSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string searchTerm = txtPartialSearch.Text.Trim();
                if (string.IsNullOrEmpty(searchTerm))
                {
                    LoadProducts();
                    return;
                }

                var foundProducts = db.Products
                    .Where(p => p.ProductName.StartsWith(searchTerm))
                    .ToList();

                dataGridView1.DataSource = foundProducts;

                if (!foundProducts.Any())
                {
                    MessageBox.Show("Товары не найдены", "Поиск", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            var salesForm = new SaleForm();
            salesForm.Show();
        }

        private void btnSaleDetails_Click(object sender, EventArgs e)
        {
            var saleDetailsForm = new SaleDetailsForm();
            saleDetailsForm.Show();
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            var testForm = new ConnectionTestForm();
            testForm.ShowDialog();
        }

        private void btnDataManagement_Click(object sender, EventArgs e)
        {
            var dataForm = new SimpleDataForm();
            dataForm.Show();
        }

        private void btnProductTable_Click(object sender, EventArgs e)
        {
            var tableForm = new ProductTableForm();
            tableForm.Show();
        }

        private void btnSalesManagement_Click(object sender, EventArgs e)
        {
            var salesManagementForm = new SalesManagementForm();
            salesManagementForm.Show();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            db?.Dispose();
            base.OnFormClosed(e);
        }
    }
}
