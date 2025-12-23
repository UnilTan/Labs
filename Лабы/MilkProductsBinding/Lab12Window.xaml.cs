using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MilkProductsBinding.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MilkProductsBinding
{
    public partial class Lab12Window : Window
    {
        private SalesContext db = new SalesContext();

        public Lab12Window()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
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

                // Загружаем данные
                LoadData();

                statusText.Text = "ЛР-12: Данные загружены успешно";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                statusText.Text = "Ошибка инициализации";
            }
        }

        #region 1. Работа с DataGrid - Продукты

        /// <summary>
        /// Загружает данные во все элементы управления
        /// </summary>
        private void LoadData_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // 1. DataGrid с продуктами - простая привязка
                dtProduct.ItemsSource = db.Product.ToList();

                // 2. DataGrid с деталями продаж - сложная привязка с ComboBox
                // Источник для ComboBox-столбца
                cmbIdPrSale.ItemsSource = db.Product.ToList();
                
                // Источник для DataGrid с включением связанных данных
                dtDetailSale.ItemsSource = db.DetailSale.Include(d => d.Product).ToList();

                // 3. ListView - каталог товаров в виде плиток
                RefreshListView();

                // 4. Заполняем фильтр категорий
                var categories = db.Product.Select(p => p.Category).Distinct().ToList();
                categories.Insert(0, "Все категории");
                cmbCategoryFilter.ItemsSource = categories;
                cmbCategoryFilter.SelectedIndex = 0;

                statusText.Text = $"Загружено: {db.Product.Count()} продуктов, {db.DetailSale.Count()} деталей продаж";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Сохраняет изменения в таблице продуктов
        /// </summary>
        private void SaveProductChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int changesCount = db.SaveChanges();
                
                if (changesCount > 0)
                {
                    MessageBox.Show($"Сохранено {changesCount} изменений в таблице продуктов!", 
                        "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    // Обновляем ComboBox-столбец и ListView
                    cmbIdPrSale.ItemsSource = db.Product.ToList();
                    RefreshListView();
                    
                    statusText.Text = $"Сохранено {changesCount} изменений в продуктах";
                }
                else
                {
                    MessageBox.Show("Нет изменений для сохранения.", "Информация", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения продуктов: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region 2. Работа с DataGrid и ComboBox-столбцом

        /// <summary>
        /// Сохраняет изменения в таблице деталей продаж
        /// </summary>
        private void SaveDetailSaleChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int changesCount = db.SaveChanges();
                
                if (changesCount > 0)
                {
                    MessageBox.Show($"Сохранено {changesCount} изменений в деталях продаж!", 
                        "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    // Перезагружаем данные с обновленными связями
                    dtDetailSale.ItemsSource = db.DetailSale.Include(d => d.Product).ToList();
                    
                    statusText.Text = $"Сохранено {changesCount} изменений в деталях продаж";
                }
                else
                {
                    MessageBox.Show("Нет изменений для сохранения.", "Информация", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения деталей продаж: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region 3. Работа с ListView - каталог товаров

        /// <summary>
        /// Обновляет ListView с продуктами
        /// </summary>
        private void RefreshListView_Click(object sender, RoutedEventArgs e)
        {
            RefreshListView();
        }

        private void RefreshListView()
        {
            try
            {
                lvProducts.ItemsSource = db.Product.ToList();
                statusText.Text = $"ListView обновлен: {db.Product.Count()} продуктов";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления ListView: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Фильтрует продукты по категории в ListView
        /// </summary>
        private void CategoryFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbCategoryFilter.SelectedItem == null) return;

                string selectedCategory = cmbCategoryFilter.SelectedItem.ToString();
                
                if (selectedCategory == "Все категории")
                {
                    lvProducts.ItemsSource = db.Product.ToList();
                }
                else
                {
                    var filteredProducts = db.Product.Where(p => p.Category == selectedCategory).ToList();
                    lvProducts.ItemsSource = filteredProducts;
                    statusText.Text = $"Показано {filteredProducts.Count} продуктов категории '{selectedCategory}'";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Показывает все продукты в ListView
        /// </summary>
        private void ShowAllProducts_Click(object sender, RoutedEventArgs e)
        {
            cmbCategoryFilter.SelectedIndex = 0;
            RefreshListView();
        }

        #endregion

        #region Дополнительные функции

        /// <summary>
        /// Добавляет тестовый продукт для демонстрации
        /// </summary>
        private void AddTestProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newProduct = new Product
                {
                    nameProduct = $"Тестовый продукт {DateTime.Now:HH:mm:ss}",
                    priceProduct = 99.99m,
                    Category = "Тестовая",
                    ExpiryDays = 7,
                    Description = "Продукт добавлен для демонстрации ЛР-12"
                };

                db.Product.Add(newProduct);
                db.SaveChanges();

                // Обновляем все элементы управления
                LoadData();

                MessageBox.Show($"Добавлен тестовый продукт: {newProduct.nameProduct}", 
                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления тестового продукта: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Закрывает окно
        /// </summary>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        protected override void OnClosed(EventArgs e)
        {
            db?.Dispose();
            base.OnClosed(e);
        }
    }
}