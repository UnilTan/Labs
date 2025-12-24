using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MilkProductsWPF.Models;

namespace MilkProductsWPF
{
    public partial class SaleWindow : Window
    {
        SalesContext db = new SalesContext(); // Переменная модели БД
        
        public SaleWindow()
        {
            InitializeComponent();
            LoadData();
            dpSaleDate.SelectedDate = DateTime.Now;
        }

        // Загрузка данных в DataGrid
        public void LoadData()
        {
            try
            {
                dataGrid.ItemsSource = db.Sales.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Добавление записи
        private void BtnIns_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCustomer.Text) || 
                    string.IsNullOrWhiteSpace(txtTotal.Text) ||
                    dpSaleDate.SelectedDate == null)
                {
                    MessageBox.Show("Заполните все поля", "Предупреждение", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Sale sale = new Sale();
                sale.SaleDate = dpSaleDate.SelectedDate.Value;
                sale.CustomerName = txtCustomer.Text;
                sale.TotalAmount = Convert.ToDecimal(txtTotal.Text);

                db.Sales.Add(sale);
                db.SaveChanges();
                LoadData();
                ClearFields();
                
                MessageBox.Show("Продажа успешно добавлена!", "Успех", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Сохранение изменений
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                db.SaveChanges();
                LoadData();
                MessageBox.Show("Изменения сохранены!", "Успех", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Удаление записи по ID
        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtId.Text))
                {
                    MessageBox.Show("Введите ID продажи для удаления", "Предупреждение", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int numDel = Convert.ToInt32(txtId.Text);
                var recDel = db.Sales.Where(rec => rec.SaleId == numDel).FirstOrDefault();
                
                if (recDel == null)
                {
                    MessageBox.Show("Продажа с указанным ID не найдена", "Предупреждение", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show($"Удалить продажу покупателя '{recDel.CustomerName}'?", 
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    db.Sales.Remove(recDel);
                    db.SaveChanges();
                    LoadData();
                    ClearFields();
                    
                    MessageBox.Show("Продажа удалена!", "Успех", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Сортировка
        private void BtnSort_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataGrid.ItemsSource = db.Sales.OrderByDescending(s => s.SaleDate).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сортировки: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Поиск
        private void BtnFind_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    LoadData();
                    return;
                }

                string searchText = txtSearch.Text.ToLower();
                var filteredSales = db.Sales
                    .Where(s => s.CustomerName.ToLower().Contains(searchText))
                    .ToList();
                
                dataGrid.ItemsSource = filteredSales;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Фильтр
        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Фильтр по сумме > 1000
                dataGrid.ItemsSource = db.Sales.Where(s => s.TotalAmount > 1000).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Выбор записи в DataGrid
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem is null) return;
            
            var recSelectSale = dataGrid.SelectedItem as Sale;
            if (recSelectSale != null)
            {
                txtId.Text = recSelectSale.SaleId.ToString();
                dpSaleDate.SelectedDate = recSelectSale.SaleDate;
                txtCustomer.Text = recSelectSale.CustomerName;
                txtTotal.Text = recSelectSale.TotalAmount.ToString();
            }
        }

        // Очистка полей
        private void ClearFields()
        {
            txtId.Clear();
            dpSaleDate.SelectedDate = DateTime.Now;
            txtCustomer.Clear();
            txtTotal.Clear();
        }

        // Показать все продажи
        private void Button7_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        // Продажи за сегодня
        private void Button8_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime today = DateTime.Today;
                dataGrid.ItemsSource = db.Sales
                    .Where(s => s.SaleDate.Date == today)
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Крупные продажи (сумма > 2000)
        private void Button9_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataGrid.ItemsSource = db.Sales.Where(s => s.TotalAmount > 2000).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Закрыть окно
        private void Button10_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            db?.Dispose();
            base.OnClosed(e);
        }
    }
}