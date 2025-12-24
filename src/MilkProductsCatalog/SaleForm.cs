using MilkProductsCatalog.Models;

namespace MilkProductsCatalog
{
    public partial class SaleForm : Form
    {
        private SalesContext db = new SalesContext();

        public SaleForm()
        {
            InitializeComponent();
            LoadSales();
        }

        private void LoadSales()
        {
            try
            {
                var sales = db.Sales.ToList();
                dataGridView1.DataSource = sales;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSortByDate_Click(object sender, EventArgs e)
        {
            try
            {
                var sortedSales = db.Sales.OrderBy(s => s.SaleDate).ToList();
                dataGridView1.DataSource = sortedSales;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сортировки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSortByAmount_Click(object sender, EventArgs e)
        {
            try
            {
                var sortedSales = db.Sales.OrderByDescending(s => s.TotalAmount).ToList();
                dataGridView1.DataSource = sortedSales;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сортировки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                string searchTerm = txtSearchCustomer.Text.Trim();
                if (string.IsNullOrEmpty(searchTerm))
                {
                    LoadSales();
                    return;
                }

                var foundSales = db.Sales
                    .Where(s => s.CustomerName.Contains(searchTerm))
                    .ToList();

                dataGridView1.DataSource = foundSales;

                if (!foundSales.Any())
                {
                    MessageBox.Show("Продажи не найдены", "Поиск", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                var sales = db.Sales.ToList();
                
                if (sales.Any())
                {
                    var maxAmount = sales.Max(s => s.TotalAmount);
                    var minAmount = sales.Min(s => s.TotalAmount);
                    var avgAmount = sales.Average(s => s.TotalAmount);

                    txtMaxAmount.Text = maxAmount.ToString("F2");
                    txtMinAmount.Text = minAmount.ToString("F2");
                    txtAvgAmount.Text = avgAmount.ToString("F2");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка вычисления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            db?.Dispose();
            base.OnFormClosed(e);
        }
    }
}