using System;
using System.Linq;
using System.Windows;

namespace Kislomolochnye_products
{
    public partial class MainWindow : Window
    {
        Familia22i1L9Entities db = new Familia22i1L9Entities();

        public MainWindow()
        {
            InitializeComponent();
            LoadProducts();
        }

        void LoadProducts()
        {
            ProductsDataGrid.ItemsSource = db.Product.ToList();
        }

        private void btnPrice_Click(object sender, RoutedEventArgs e)
        {
            var prices = db.Product.Select(p => p.Price);
            txtMaxPrice.Text = prices.Max().ToString("F2");
            txtMinPrice.Text = prices.Min().ToString("F2");
            txtAvgPrice.Text = prices.Average().ToString("F2");
        }

        private void btnSortAsc_Click(object sender, RoutedEventArgs e)
        {
            ProductsDataGrid.ItemsSource = db.Product.OrderBy(p => p.Price).ToList();
        }

        private void btnSortDesc_Click(object sender, RoutedEventArgs e)
        {
            ProductsDataGrid.ItemsSource = db.Product.OrderByDescending(p => p.Price).ToList();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string name = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(name))
            {
                var result = db.Product.Where(p => p.ProductName.Contains(name)).ToList();
                ProductsDataGrid.ItemsSource = result;
            }
            else
            {
                LoadProducts();
            }
        }

        private void btnPartialSearch_Click(object sender, RoutedEventArgs e)
        {
            string start = txtPartialSearch.Text.Trim();
            if (!string.IsNullOrEmpty(start))
            {
                var result = db.Product.Where(p => p.ProductName.StartsWith(start)).ToList();
                ProductsDataGrid.ItemsSource = result;
            }
            else
            {
                LoadProducts();
            }
        }

        private void btnSales_Click(object sender, RoutedEventArgs e)
        {
            var salesWindow = new SalesWindow();
            salesWindow.Show();
        }

        private void btnSaleDetails_Click(object sender, RoutedEventArgs e)
        {
            var saleDetailsWindow = new SaleDetailsWindow();
            saleDetailsWindow.Show();
        }

        private void btnTestConnection_Click(object sender, RoutedEventArgs e)
        {
            var testWindow = new ConnectionTestWindow();
            testWindow.ShowDialog();
        }
    }
}