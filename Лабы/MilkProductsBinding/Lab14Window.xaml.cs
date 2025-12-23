using MilkProductsBinding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MilkProductsBinding
{
    public partial class Lab14Window : Window
    {
        private SalesContext db;
        private List<Product> allProducts;

        public Lab14Window()
        {
            InitializeComponent();
            
            // Инициализация контекста БД
            try
            {
                db = new SalesContext();
                LoadData();
                tbStatus.Text = "ЛР-14 успешно загружена! Нажмите кнопки для загрузки данных.";
            }
            catch (Exception ex)
            {
                tbStatus.Text = $"Ошибка подключения к БД: {ex.Message}";
                MessageBox.Show($"Ошибка подключения к БД: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Загрузка всех данных при открытии окна
        /// </summary>
        private void LoadData()
        {
            try
            {
                // Проверяем подключение к БД
                if (db.Database.CanConnect())
                {
                    tbStatus.Text = "Подключение к БД успешно. Готов к работе с изображениями.";
                }
                else
                {
                    tbStatus.Text = "Проблема с подключением к БД.";
                }
            }
            catch (Exception ex)
            {
                tbStatus.Text = $"Ошибка инициализации: {ex.Message}";
            }
        }

        /// <summary>
        /// Загрузка продуктов в DataGrid
        /// </summary>
        private void btnLoadProducts_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Загружаем все продукты
                allProducts = db.Product.ToList();
                dgProducts.ItemsSource = allProducts;
                
                // Статистика по изображениям
                int withImages = allProducts.Count(p => p.HasImage);
                int withoutImages = allProducts.Count - withImages;
                
                tbProductInfo.Text = $"Загружено продуктов: {allProducts.Count} | С фото: {withImages}, Без фото: {withoutImages}";
                tbStatus.Text = "Продукты загружены в DataGrid. Проверьте отображение изображений.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки продуктов: {ex.Message}", "Ошибка");
                tbProductInfo.Text = "Ошибка загрузки";
                tbStatus.Text = $"Ошибка: {ex.Message}";
            }
        }

        /// <summary>
        /// Загрузка каталога в ListView
        /// </summary>
        private void btnLoadCatalog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (allProducts == null)
                {
                    allProducts = db.Product.ToList();
                }
                
                lvProducts.ItemsSource = allProducts;
                tbCatalogInfo.Text = $"Каталог: {allProducts.Count} товаров";
                tbStatus.Text = "Каталог товаров загружен в ListView. Проверьте отображение плиток с изображениями.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки каталога: {ex.Message}", "Ошибка");
                tbCatalogInfo.Text = "Ошибка загрузки каталога";
                tbStatus.Text = $"Ошибка каталога: {ex.Message}";
            }
        }

        /// <summary>
        /// Обработчик выбора продукта в DataGrid
        /// </summary>
        private void dgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedProduct = dgProducts.SelectedItem as Product;
            if (selectedProduct != null)
            {
                tbStatus.Text = $"Выбран продукт: {selectedProduct.nameProduct} (ID: {selectedProduct.idProduct}) - {selectedProduct.ImageStatus}";
            }
        }

        /// <summary>
        /// Освобождение ресурсов при закрытии окна
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            db?.Dispose();
            base.OnClosed(e);
        }
    }
}