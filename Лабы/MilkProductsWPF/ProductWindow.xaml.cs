using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MilkProductsWPF.Models;
using Microsoft.EntityFrameworkCore;

namespace MilkProductsWPF
{
    public partial class ProductWindow : Window
    {
        SalesContext db = new SalesContext(); // Переменная модели БД
        
        public ProductWindow()
        {
            InitializeComponent();
            LoadData();
            LoadCities();
        }

        // Загрузка данных в DataGrid
        public void LoadData()
        {
            try
            {
                // Проверяем подключение к базе данных
                if (!db.Database.CanConnect())
                {
                    MessageBox.Show("Не удается подключиться к базе данных. Проверьте строку подключения.", 
                        "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Проверяем, существует ли таблица City
                try
                {
                    var testCity = db.Cities.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Таблица City не найдена. Выполните скрипт создания таблиц.\n\nОшибка: {ex.Message}", 
                        "Ошибка структуры БД", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                dataGrid.ItemsSource = db.Products.Include(p => p.City).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}\n\nДетали: {ex.InnerException?.Message}", 
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Загрузка городов в ComboBox
        private void LoadCities()
        {
            try
            {
                // Проверяем подключение к базе данных
                if (!db.Database.CanConnect())
                {
                    MessageBox.Show("Не удается подключиться к базе данных для загрузки городов.", 
                        "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                cmbCity.ItemsSource = db.Cities.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки городов: {ex.Message}\n\nВозможно, таблица City не создана. Выполните скрипт создания таблиц.", 
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Добавление записи
        private void BtnIns_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text) || 
                    string.IsNullOrWhiteSpace(txtPrice.Text) ||
                    string.IsNullOrWhiteSpace(txtCategory.Text))
                {
                    MessageBox.Show("Заполните обязательные поля: Название, Цена, Категория", 
                        "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Product product = new Product();
                product.ProductName = txtName.Text;
                product.Price = Convert.ToDecimal(txtPrice.Text);
                product.Category = txtCategory.Text;
                product.ExpiryDays = string.IsNullOrWhiteSpace(txtExpiryDays.Text) ? 0 : Convert.ToInt32(txtExpiryDays.Text);
                product.Description = ""; // Пустое описание по умолчанию
                product.CityId = cmbCity.SelectedValue as int?;

                db.Products.Add(product);
                db.SaveChanges();
                LoadData();
                ClearFields();
                
                MessageBox.Show("Продукт успешно добавлен!", "Успех", 
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
                    MessageBox.Show("Введите ID продукта для удаления", "Предупреждение", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int numDel = Convert.ToInt32(txtId.Text);
                var recDel = db.Products.Where(rec => rec.ProductId == numDel).FirstOrDefault();
                
                if (recDel == null)
                {
                    MessageBox.Show("Продукт с указанным ID не найден", "Предупреждение", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show($"Удалить продукт '{recDel.ProductName}'?", 
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    db.Products.Remove(recDel);
                    db.SaveChanges();
                    LoadData();
                    ClearFields();
                    
                    MessageBox.Show("Продукт удален!", "Успех", 
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
                dataGrid.ItemsSource = db.Products.OrderBy(p => p.ProductName).ToList();
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
                var filteredProducts = db.Products
                    .Where(p => p.ProductName.ToLower().Contains(searchText) ||
                               p.Category.ToLower().Contains(searchText))
                    .ToList();
                
                dataGrid.ItemsSource = filteredProducts;
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
                // Фильтр по цене > 100
                dataGrid.ItemsSource = db.Products.Where(p => p.Price > 100).ToList();
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
            
            var recSelectProduct = dataGrid.SelectedItem as Product;
            if (recSelectProduct != null)
            {
                txtId.Text = recSelectProduct.ProductId.ToString();
                txtName.Text = recSelectProduct.ProductName;
                txtPrice.Text = recSelectProduct.Price.ToString();
                txtCategory.Text = recSelectProduct.Category;
                txtExpiryDays.Text = recSelectProduct.ExpiryDays.ToString();
                cmbCity.SelectedValue = recSelectProduct.CityId;
            }
        }

        // Очистка полей
        private void ClearFields()
        {
            txtId.Clear();
            txtName.Clear();
            txtPrice.Clear();
            txtCategory.Clear();
            txtExpiryDays.Clear();
            cmbCity.SelectedIndex = -1;
        }

        // Показать все записи
        private void Button7_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        // Фильтр по категориям
        private void Button8_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataGrid.ItemsSource = db.Products.OrderBy(p => p.Category).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Дорогие товары (цена > 200)
        private void Button9_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataGrid.ItemsSource = db.Products.Where(p => p.Price > 200).ToList();
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