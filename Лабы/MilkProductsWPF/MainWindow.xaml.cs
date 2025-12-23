using System;
using System.Windows;

namespace MilkProductsWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                
                // Проверяем и создаем таблицы при запуске
                if (!DatabaseHelper.CheckAndCreateTables())
                {
                    MessageBox.Show("Не удалось инициализировать базу данных. Некоторые функции могут не работать.", 
                        "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnProducts_Click(object sender, RoutedEventArgs e)
        {
            ProductWindow productWindow = new ProductWindow();
            productWindow.Show();
        }

        private void BtnSales_Click(object sender, RoutedEventArgs e)
        {
            SaleWindow saleWindow = new SaleWindow();
            saleWindow.Show();
        }

        private void BtnSaleDetails_Click(object sender, RoutedEventArgs e)
        {
            SaleDetailsWindow saleDetailsWindow = new SaleDetailsWindow();
            saleDetailsWindow.Show();
        }

        private void BtnCities_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CityWindow cityWindow = new CityWindow();
                cityWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия окна городов: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}