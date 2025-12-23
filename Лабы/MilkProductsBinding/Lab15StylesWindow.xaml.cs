using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using MilkProductsBinding.Models;

namespace MilkProductsBinding
{
    public partial class Lab15StylesWindow : Window
    {
        private SalesContext db = new SalesContext();
        private bool isDarkTheme = false;

        public Lab15StylesWindow()
        {
            InitializeComponent();
            LoadDemoData();
        }

        /// <summary>
        /// Загружает демонстрационные данные
        /// </summary>
        private void LoadDemoData()
        {
            try
            {
                // Загружаем данные для DataGrid
                var products = db.Product.Take(5).Select(p => new
                {
                    ID = p.idProduct,
                    Название = p.nameProduct,
                    Цена = p.priceProduct,
                    Категория = p.Category
                }).ToList();

                dgDemo.ItemsSource = products;

                // Загружаем данные для ComboBox
                var categories = db.Product.Select(p => p.Category).Distinct().ToList();
                cmbDemo.ItemsSource = categories;
                if (categories.Any())
                    cmbDemo.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Обновляет данные в элементах управления
        /// </summary>
        private void RefreshData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadDemoData();
                MessageBox.Show("Данные обновлены!", "Информация", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления данных: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Демонстрирует смену темы (изменение цветов)
        /// </summary>
        private void ChangeTheme_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isDarkTheme)
                {
                    // Переключаем на темную тему
                    this.Background = new SolidColorBrush(Colors.DarkSlateGray);
                    isDarkTheme = true;
                    MessageBox.Show("Применена темная тема!", "Смена темы", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Возвращаем к стилю по умолчанию
                    this.ClearValue(BackgroundProperty); // Возвращает к стилю WinStyle
                    isDarkTheme = false;
                    MessageBox.Show("Возвращена стандартная тема!", "Смена темы", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка смены темы: {ex.Message}", "Ошибка", 
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

        protected override void OnClosed(EventArgs e)
        {
            db?.Dispose();
            base.OnClosed(e);
        }
    }
}