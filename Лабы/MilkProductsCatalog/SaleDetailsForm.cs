using MilkProductsCatalog.Models;
using Microsoft.EntityFrameworkCore;

namespace MilkProductsCatalog
{
    public partial class SaleDetailsForm : Form
    {
        private SalesContext db = new SalesContext();

        public SaleDetailsForm()
        {
            InitializeComponent();
            LoadSaleDetails();
        }

        private void LoadSaleDetails()
        {
            try
            {
                var saleDetails = db.SaleDetails
                    .Include(sd => sd.Product)
                    .Include(sd => sd.Sale)
                    .Select(sd => new
                    {
                        sd.SaleDetailId,
                        sd.SaleId,
                        ProductName = sd.Product.ProductName,
                        CustomerName = sd.Sale.CustomerName,
                        sd.Quantity,
                        sd.UnitPrice,
                        Total = sd.Quantity * sd.UnitPrice
                    })
                    .ToList();

                dataGridView1.DataSource = saleDetails;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSortByQuantity_Click(object sender, EventArgs e)
        {
            try
            {
                var sortedDetails = db.SaleDetails
                    .Include(sd => sd.Product)
                    .Include(sd => sd.Sale)
                    .OrderByDescending(sd => sd.Quantity)
                    .Select(sd => new
                    {
                        sd.SaleDetailId,
                        sd.SaleId,
                        ProductName = sd.Product.ProductName,
                        CustomerName = sd.Sale.CustomerName,
                        sd.Quantity,
                        sd.UnitPrice,
                        Total = sd.Quantity * sd.UnitPrice
                    })
                    .ToList();

                dataGridView1.DataSource = sortedDetails;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сортировки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearchProduct_Click(object sender, EventArgs e)
        {
            try
            {
                string searchTerm = txtSearchProduct.Text.Trim();
                if (string.IsNullOrEmpty(searchTerm))
                {
                    LoadSaleDetails();
                    return;
                }

                var foundDetails = db.SaleDetails
                    .Include(sd => sd.Product)
                    .Include(sd => sd.Sale)
                    .Where(sd => sd.Product.ProductName.Contains(searchTerm))
                    .Select(sd => new
                    {
                        sd.SaleDetailId,
                        sd.SaleId,
                        ProductName = sd.Product.ProductName,
                        CustomerName = sd.Sale.CustomerName,
                        sd.Quantity,
                        sd.UnitPrice,
                        Total = sd.Quantity * sd.UnitPrice
                    })
                    .ToList();

                dataGridView1.DataSource = foundDetails;

                if (!foundDetails.Any())
                {
                    MessageBox.Show("Записи не найдены", "Поиск", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                var details = db.SaleDetails.ToList();
                
                if (details.Any())
                {
                    var maxQuantity = details.Max(sd => sd.Quantity);
                    var minQuantity = details.Min(sd => sd.Quantity);
                    var avgQuantity = details.Average(sd => sd.Quantity);

                    txtMaxQuantity.Text = maxQuantity.ToString();
                    txtMinQuantity.Text = minQuantity.ToString();
                    txtAvgQuantity.Text = avgQuantity.ToString("F2");
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