using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MilkProductsWPF.Models;
using Microsoft.EntityFrameworkCore;

namespace MilkProductsWPF
{
    /// <summary>
    /// Отдельный файл с кодом обработчиков кнопок для ProductWindow
    /// Содержит методы для кнопок: "По категориям", "Ins", "Save", "Del"
    /// </summary>
    public partial class ProductWindowHandlers
    {
        // Предполагаемые поля класса (должны быть в основном классе)
        private SalesContext db = new SalesContext();
        private DataGrid dataGrid;
        private TextBox txtId, txtName, txtPrice, txtCategory, txtExpiryDays;
        private ComboBox cmbCity;

        #region Кнопка "По категориям" (button8)
        
        /// <summary>
        /// Обработчик кнопки "По категориям" - сортирует продукты по категориям
        /// </summary>
        private void Button8_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Загружаем продукты с городами, сортируем по категориям, затем по названию
                var sortedProducts = db.Products
                    .Include(p => p.City)
                    .OrderBy(p => p.Category)
                    .ThenBy(p => p.ProductName)
                    .ToList();

                dataGrid.ItemsSource = sortedProducts;

                // Показываем информационное сообщение
                MessageBox.Show($"Продукты отсортированы по категориям. Найдено: {sortedProducts.Count} записей", 
                    "Сортировка", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сортировки по категориям: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Кнопка "Ins" - Добавление записи

        /// <summary>
        /// Обработчик кнопки "Ins" - добавляет новый продукт в базу данных
        /// </summary>
        private void BtnIns_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверяем обязательные поля
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Введите название продукта", "Предупреждение", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPrice.Text))
                {
                    MessageBox.Show("Введите цену продукта", "Предупреждение", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtPrice.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtCategory.Text))
                {
                    MessageBox.Show("Введите категорию продукта", "Предупреждение", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtCategory.Focus();
                    return;
                }

                // Проверяем корректность цены
                if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
                {
                    MessageBox.Show("Введите корректную цену (положительное число)", "Предупреждение", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtPrice.Focus();
                    return;
                }

                // Проверяем корректность срока годности
                int expiryDays = 0;
                if (!string.IsNullOrWhiteSpace(txtExpiryDays.Text))
                {
                    if (!int.TryParse(txtExpiryDays.Text, out expiryDays) || expiryDays < 0)
                    {
                        MessageBox.Show("Введите корректный срок годности (целое неотрицательное число)", 
                            "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        txtExpiryDays.Focus();
                        return;
                    }
                }

                // Создаем новый продукт
                Product product = new Product
                {
                    ProductName = txtName.Text.Trim(),
                    Price = price,
                    Category = txtCategory.Text.Trim(),
                    ExpiryDays = expiryDays,
                    Description = "", // Пустое описание по умолчанию
                    CityId = cmbCity.SelectedValue as int? // Может быть null
                };

                // Добавляем в базу данных
                db.Products.Add(product);
                db.SaveChanges();

                // Обновляем отображение
                LoadData();
                ClearFields();

                MessageBox.Show($"Продукт '{product.ProductName}' успешно добавлен!", "Успех", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления продукта: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Кнопка "Save" - Сохранение изменений

        /// <summary>
        /// Обработчик кнопки "Save" - сохраняет все изменения в базе данных
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверяем, есть ли несохраненные изменения
                var changedEntries = db.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Modified || 
                               e.State == EntityState.Added || 
                               e.State == EntityState.Deleted)
                    .ToList();

                if (!changedEntries.Any())
                {
                    MessageBox.Show("Нет изменений для сохранения", "Информация", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Подтверждение сохранения
                var result = MessageBox.Show($"Сохранить {changedEntries.Count} изменений в базе данных?", 
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Сохраняем изменения
                    int savedChanges = db.SaveChanges();
                    
                    // Обновляем отображение
                    LoadData();

                    MessageBox.Show($"Успешно сохранено {savedChanges} изменений", "Успех", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Кнопка "Del" - Удаление записи

        /// <summary>
        /// Обработчик кнопки "Del" - удаляет продукт по ID
        /// </summary>
        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверяем, введен ли ID
                if (string.IsNullOrWhiteSpace(txtId.Text))
                {
                    MessageBox.Show("Введите ID продукта для удаления", "Предупреждение", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtId.Focus();
                    return;
                }

                // Проверяем корректность ID
                if (!int.TryParse(txtId.Text, out int productId) || productId <= 0)
                {
                    MessageBox.Show("Введите корректный ID продукта (положительное целое число)", 
                        "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtId.Focus();
                    return;
                }

                // Ищем продукт в базе данных
                var productToDelete = db.Products
                    .Include(p => p.City)
                    .FirstOrDefault(p => p.ProductId == productId);

                if (productToDelete == null)
                {
                    MessageBox.Show($"Продукт с ID {productId} не найден", "Предупреждение", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Проверяем, есть ли связанные записи в SaleDetails
                var relatedSaleDetails = db.SaleDetails
                    .Where(sd => sd.ProductId == productId)
                    .ToList();

                string confirmMessage;
                if (relatedSaleDetails.Any())
                {
                    confirmMessage = $"Внимание! Продукт '{productToDelete.ProductName}' используется в {relatedSaleDetails.Count} записях продаж.\n\n" +
                                   "При удалении продукта связанные записи продаж могут стать некорректными.\n\n" +
                                   "Вы уверены, что хотите удалить этот продукт?";
                }
                else
                {
                    confirmMessage = $"Удалить продукт '{productToDelete.ProductName}'?\n\n" +
                                   $"Категория: {productToDelete.Category}\n" +
                                   $"Цена: {productToDelete.Price:C}\n" +
                                   $"Город: {productToDelete.City?.CityName ?? "Не указан"}";
                }

                // Подтверждение удаления
                var result = MessageBox.Show(confirmMessage, "Подтверждение удаления", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Удаляем продукт
                    db.Products.Remove(productToDelete);
                    db.SaveChanges();

                    // Обновляем отображение
                    LoadData();
                    ClearFields();

                    MessageBox.Show($"Продукт '{productToDelete.ProductName}' успешно удален", "Успех", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления продукта: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Вспомогательные методы

        /// <summary>
        /// Загружает данные в DataGrid
        /// </summary>
        private void LoadData()
        {
            try
            {
                var products = db.Products.Include(p => p.City).ToList();
                dataGrid.ItemsSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Очищает все поля ввода
        /// </summary>
        private void ClearFields()
        {
            txtId.Clear();
            txtName.Clear();
            txtPrice.Clear();
            txtCategory.Clear();
            txtExpiryDays.Clear();
            cmbCity.SelectedIndex = -1;
        }

        #endregion
    }
}

