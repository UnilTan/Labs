using Microsoft.Win32;
using MilkProductsImages.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MilkProductsImages;

public partial class MainWindow : Window
{
    private ImageContext db;
    private List<ProductWithImage> allProducts;

    public MainWindow()
    {
        InitializeComponent();
        
        // Инициализация контекста БД
        try
        {
            db = new ImageContext();
            
            // Создаем базу данных если она не существует
            db.Database.EnsureCreated();
            
            LoadProducts();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка подключения к БД: {ex.Message}", "Ошибка", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Загрузка продуктов из БД
    /// </summary>
    private void btnLoadProducts_Click(object sender, RoutedEventArgs e)
    {
        LoadProducts();
    }

    private void LoadProducts()
    {
        try
        {
            allProducts = db.ProductWithImage.ToList();
            dgProducts.ItemsSource = allProducts;
            
            int withImages = allProducts.Count(p => p.HasImageInDatabase);
            int withoutImages = allProducts.Count - withImages;
            
            tbInfo.Text = $"Загружено продуктов: {allProducts.Count} | С изображениями: {withImages} | Без изображений: {withoutImages}";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка загрузки продуктов: {ex.Message}", "Ошибка");
            tbInfo.Text = "Ошибка загрузки";
        }
    }

    /// <summary>
    /// Загрузка изображения для выбранного продукта
    /// </summary>
    private void btnLoadImage_Click(object sender, RoutedEventArgs e)
    {
        var selectedProduct = dgProducts.SelectedItem as ProductWithImage;
        if (selectedProduct == null)
        {
            MessageBox.Show("Выберите продукт для загрузки изображения!", "Предупреждение");
            return;
        }

        try
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Выберите изображение для продукта",
                Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Все файлы|*.*",
                FilterIndex = 1
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Загружаем изображение в объект
                selectedProduct.LoadImageFromFile(openFileDialog.FileName);
                
                // Сохраняем в БД
                db.SaveChanges();
                
                // Обновляем отображение
                dgProducts.Items.Refresh();
                ShowSelectedProductDetails(selectedProduct);
                
                tbInfo.Text = $"Изображение загружено для продукта '{selectedProduct.NameProduct}' ({selectedProduct.ImageSizeFormatted})";
                
                MessageBox.Show($"Изображение успешно загружено в БД!\nРазмер: {selectedProduct.ImageSizeFormatted}", 
                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}", "Ошибка");
        }
    }

    /// <summary>
    /// Сохранение изображения из БД в файл
    /// </summary>
    private void btnSaveImage_Click(object sender, RoutedEventArgs e)
    {
        var selectedProduct = dgProducts.SelectedItem as ProductWithImage;
        if (selectedProduct == null)
        {
            MessageBox.Show("Выберите продукт!", "Предупреждение");
            return;
        }

        if (!selectedProduct.HasImageInDatabase)
        {
            MessageBox.Show("У выбранного продукта нет изображения в БД!", "Предупреждение");
            return;
        }

        try
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Сохранить изображение как",
                FileName = selectedProduct.ImageFileName ?? $"product_{selectedProduct.IdProduct}.jpg",
                Filter = "JPEG|*.jpg|PNG|*.png|BMP|*.bmp|Все файлы|*.*"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                selectedProduct.SaveImageToFile(saveFileDialog.FileName);
                
                tbInfo.Text = $"Изображение сохранено: {saveFileDialog.FileName}";
                
                MessageBox.Show("Изображение успешно сохранено!", "Успех", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка сохранения изображения: {ex.Message}", "Ошибка");
        }
    }

    /// <summary>
    /// Удаление изображения из БД
    /// </summary>
    private void btnDeleteImage_Click(object sender, RoutedEventArgs e)
    {
        var selectedProduct = dgProducts.SelectedItem as ProductWithImage;
        if (selectedProduct == null)
        {
            MessageBox.Show("Выберите продукт!", "Предупреждение");
            return;
        }

        if (!selectedProduct.HasImageInDatabase)
        {
            MessageBox.Show("У выбранного продукта нет изображения в БД!", "Предупреждение");
            return;
        }

        try
        {
            var result = MessageBox.Show($"Удалить изображение для продукта '{selectedProduct.NameProduct}'?", 
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                selectedProduct.ProductImage = null;
                selectedProduct.ImageFileName = null;
                selectedProduct.ImageContentType = null;
                selectedProduct.ImageFileSize = null;
                
                db.SaveChanges();
                
                // Обновляем отображение
                dgProducts.Items.Refresh();
                ShowSelectedProductDetails(selectedProduct);
                
                tbInfo.Text = $"Изображение удалено для продукта '{selectedProduct.NameProduct}'";
                
                MessageBox.Show("Изображение успешно удалено из БД!", "Успех", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка удаления изображения: {ex.Message}", "Ошибка");
        }
    }

    /// <summary>
    /// Массовая загрузка изображений из папки
    /// </summary>
    private void btnLoadAllImages_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Путь к папке с изображениями
            string imagesPath = @"..\..\..\..\MilkProductsBinding\Images";
            
            if (!Directory.Exists(imagesPath))
            {
                MessageBox.Show($"Папка с изображениями не найдена: {imagesPath}", "Ошибка");
                return;
            }

            int loadedCount = 0;
            int errorCount = 0;

            tbLoadProgress.Text = "Загрузка изображений...";

            // Получаем все продукты, у которых указан ImageFileName, но нет изображения в БД
            var productsToLoad = allProducts?.Where(p => 
                !string.IsNullOrEmpty(p.ImageFileName) && 
                !p.HasImageInDatabase).ToList();

            if (productsToLoad == null || !productsToLoad.Any())
            {
                MessageBox.Show("Нет продуктов для загрузки изображений", "Информация");
                return;
            }

            foreach (var product in productsToLoad)
            {
                try
                {
                    string filePath = Path.Combine(imagesPath, product.ImageFileName!);
                    
                    if (File.Exists(filePath))
                    {
                        product.LoadImageFromFile(filePath);
                        loadedCount++;
                        
                        tbLoadProgress.Text = $"Загружено: {loadedCount}/{productsToLoad.Count}";
                        
                        // Обновляем UI
                        Application.Current.Dispatcher.Invoke(() => { });
                    }
                    else
                    {
                        errorCount++;
                        tbLoadProgress.Text = $"Файл не найден: {product.ImageFileName}";
                    }
                }
                catch (Exception ex)
                {
                    errorCount++;
                    tbLoadProgress.Text = $"Ошибка загрузки {product.ImageFileName}: {ex.Message}";
                }
            }

            // Сохраняем изменения в БД
            db.SaveChanges();
            
            // Обновляем отображение
            dgProducts.Items.Refresh();
            
            tbLoadProgress.Text = $"Завершено! Загружено: {loadedCount}, Ошибок: {errorCount}";
            
            MessageBox.Show($"Массовая загрузка завершена!\n" +
                          $"Загружено изображений: {loadedCount}\n" +
                          $"Ошибок: {errorCount}", 
                          "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка массовой загрузки: {ex.Message}", "Ошибка");
            tbLoadProgress.Text = "Ошибка массовой загрузки";
        }
    }

    /// <summary>
    /// Экспорт всех изображений из БД в папку
    /// </summary>
    private void btnExportAllImages_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Создаем папку для экспорта
            string exportPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ExportedImages");
            
            if (!Directory.Exists(exportPath))
            {
                Directory.CreateDirectory(exportPath);
            }

            int exportedCount = 0;
            int errorCount = 0;

            tbExportProgress.Text = "Экспорт изображений...";

            // Получаем все продукты с изображениями в БД
            var productsWithImages = allProducts?.Where(p => p.HasImageInDatabase).ToList();

            if (productsWithImages == null || !productsWithImages.Any())
            {
                MessageBox.Show("Нет изображений для экспорта", "Информация");
                return;
            }

            foreach (var product in productsWithImages)
            {
                try
                {
                    string fileName = product.ImageFileName ?? $"product_{product.IdProduct}.jpg";
                    string filePath = Path.Combine(exportPath, fileName);
                    
                    product.SaveImageToFile(filePath);
                    exportedCount++;
                    
                    tbExportProgress.Text = $"Экспортировано: {exportedCount}/{productsWithImages.Count}";
                }
                catch (Exception ex)
                {
                    errorCount++;
                    tbExportProgress.Text = $"Ошибка экспорта {product.NameProduct}: {ex.Message}";
                }
            }

            tbExportProgress.Text = $"Завершено! Экспортировано: {exportedCount}, Ошибок: {errorCount}";
            
            MessageBox.Show($"Экспорт завершен!\n" +
                          $"Экспортировано изображений: {exportedCount}\n" +
                          $"Ошибок: {errorCount}\n" +
                          $"Папка: {exportPath}", 
                          "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
                          
            // Открываем папку с экспортированными файлами
            System.Diagnostics.Process.Start("explorer.exe", exportPath);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка экспорта: {ex.Message}", "Ошибка");
            tbExportProgress.Text = "Ошибка экспорта";
        }
    }

    /// <summary>
    /// Обработчик выбора продукта в DataGrid
    /// </summary>
    private void dgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ShowSelectedProductDetails(dgProducts.SelectedItem as ProductWithImage);
    }

    /// <summary>
    /// Отображение детальной информации о выбранном продукте
    /// </summary>
    private void ShowSelectedProductDetails(ProductWithImage product)
    {
        if (product == null)
        {
            // Очищаем информацию
            imgSelectedProduct.Source = null;
            tbNoImage.Visibility = Visibility.Visible;
            tbSelectedName.Text = "Продукт не выбран";
            tbSelectedPrice.Text = "";
            tbSelectedCategory.Text = "";
            tbSelectedDescription.Text = "";
            tbSelectedImageStatus.Text = "";
            tbSelectedImageFileName.Text = "";
            tbSelectedImageSize.Text = "";
            tbSelectedImageType.Text = "";
            return;
        }

        try
        {
            // Заполняем информацию о продукте
            tbSelectedName.Text = product.NameProduct;
            tbSelectedPrice.Text = $"Цена: {product.PriceProduct:F2} ₽";
            tbSelectedCategory.Text = $"Категория: {product.Category}";
            tbSelectedDescription.Text = $"Описание: {product.Description ?? "Не указано"}";
            
            // Информация об изображении
            tbSelectedImageStatus.Text = product.ImageStatus;
            tbSelectedImageFileName.Text = $"Файл: {product.ImageFileName ?? "Не указан"}";
            tbSelectedImageSize.Text = $"Размер: {product.ImageSizeFormatted}";
            tbSelectedImageType.Text = $"Тип: {product.ImageContentType ?? "Не указан"}";
            
            // ДЕМОНСТРАЦИЯ: Отображение изображения из БД
            if (product.HasImageInDatabase)
            {
                imgSelectedProduct.Source = product.ImageSource;
                tbNoImage.Visibility = Visibility.Collapsed;
            }
            else
            {
                imgSelectedProduct.Source = null;
                tbNoImage.Visibility = Visibility.Visible;
            }
        }
        catch (Exception ex)
        {
            tbSelectedImageStatus.Text = $"Ошибка отображения: {ex.Message}";
            imgSelectedProduct.Source = null;
            tbNoImage.Visibility = Visibility.Visible;
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