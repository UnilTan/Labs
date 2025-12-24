using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MilkProductsWPF.Models;
using Microsoft.EntityFrameworkCore;

namespace MilkProductsWPF
{
    public partial class SaleDetailsWindow : Window
    {
        SalesContext db = new SalesContext(); // Переменная модели БД
        
        public SaleDetailsWindow()
        {
            InitializeComponent();
            LoadData();
            LoadComboBoxes();
        }

        // Загрузка данных в DataGrid
        public void LoadData()
        {
            try
            {
                dataGrid.ItemsSource = db.SaleDetails
                    .Include(sd => sd.Product)
                    .Include(sd => sd.Sale)
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Загрузка данных в ComboBox
        private void LoadComboBoxes()
        {
            try
            {
                // Загрузка продуктов
                cmbProduct.ItemsSource = db.Products.ToList();
                
                // Загрузка продаж с отображаемым текстом
                var sales = db.Sales.Select(s => new 
                { 
                    SaleId = s.SaleId,
                    DisplayText = $"{s.SaleId} - {s.CustomerName} ({s.SaleDate:dd.MM.yyyy})"
                }).ToList();
                cmbSale.ItemsSource = sales;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки справочников: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Добавление записи
        private void BtnIns_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbSale.SelectedValue == null || 
                    cmbProduct.SelectedValue == null ||
                    string.IsNullOrWhiteSpace(txtQuantity.Text) ||
                    string.IsNullOrWhiteSpace(txtUnitPrice.Text))
                {
                    MessageBox.Show("Заполните все поля", "Предупреждение", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                SaleDetails saleDetail = new SaleDetails();
                saleDetail.SaleId = (int)cmbSale.SelectedValue;
                saleDetail.ProductId = (int)cmbProduct.SelectedValue;
                saleDetail.Quantity = Convert.ToInt32(txtQuantity.Text);
                saleDetail.UnitPrice = Convert.ToDecimal(txtUnitPrice.Text);

                db.SaleDetails.Add(saleDetail);
                db.SaveChanges();
                LoadData();
                ClearFields();
                
                MessageBox.Show("Деталь продажи успешно добавлена!", "Успех", 
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
                    MessageBox.Show("Введите ID детали для удаления", "Предупреждение", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int numDel = Convert.ToInt32(txtId.Text);
                var recDel = db.SaleDetails.Where(rec => rec.SaleDetailId == numDel).FirstOrDefault();
                
                if (recDel == null)
                {
                    MessageBox.Show("Деталь с указанным ID не найдена", "Предупреждение", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show("Удалить выбранную деталь продажи?", 
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    db.SaleDetails.Remove(recDel);
                    db.SaveChanges();
                    LoadData();
                    ClearFields();
                    
                    MessageBox.Show("Деталь удалена!", "Успех", 
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
                dataGrid.ItemsSource = db.SaleDetails
                    .Include(sd => sd.Product)
                    .Include(sd => sd.Sale)
                    .OrderBy(sd => sd.Product.ProductName)
                    .ToList();
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
                var filteredDetails = db.SaleDetails
                    .Include(sd => sd.Product)
                    .Include(sd => sd.Sale)
                    .Where(sd => sd.Product.ProductName.ToLower().Contains(searchText) ||
                                sd.Sale.CustomerName.ToLower().Contains(searchText))
                    .ToList();
                
                dataGrid.ItemsSource = filteredDetails;
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
                // Фильтр по количеству > 5
                dataGrid.ItemsSource = db.SaleDetails
                    .Include(sd => sd.Product)
                    .Include(sd => sd.Sale)
                    .Where(sd => sd.Quantity > 5)
                    .ToList();
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
            
            var recSelectDetail = dataGrid.SelectedItem as SaleDetails;
            if (recSelectDetail != null)
            {
                txtId.Text = recSelectDetail.SaleDetailId.ToString();
                cmbSale.SelectedValue = recSelectDetail.SaleId;
                cmbProduct.SelectedValue = recSelectDetail.ProductId;
                txtQuantity.Text = recSelectDetail.Quantity.ToString();
                txtUnitPrice.Text = recSelectDetail.UnitPrice.ToString();
            }
        }

        // Очистка полей
        private void ClearFields()
        {
            txtId.Clear();
            cmbSale.SelectedIndex = -1;
            cmbProduct.SelectedIndex = -1;
            txtQuantity.Clear();
            txtUnitPrice.Clear();
        }

        // Показать все детали
        private void Button7_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        // Группировка по продуктам
        private void Button8_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataGrid.ItemsSource = db.SaleDetails
                    .Include(sd => sd.Product)
                    .Include(sd => sd.Sale)
                    .OrderBy(sd => sd.Product.ProductName)
                    .ThenBy(sd => sd.Sale.SaleDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Большие объемы (количество > 10)
        private void Button9_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataGrid.ItemsSource = db.SaleDetails
                    .Include(sd => sd.Product)
                    .Include(sd => sd.Sale)
                    .Where(sd => sd.Quantity > 10)
                    .ToList();
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