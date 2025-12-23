using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MilkProductsBinding.Models;
using Microsoft.EntityFrameworkCore;

namespace MilkProductsBinding
{
    public partial class MainWindow : Window
    {
        private SalesContext db = new SalesContext();
        
        // Переменные для новых объектов (видны во всех методах)
        private Product productNew = new Product();
        private SaleDetails detailSaleNew = new SaleDetails();

        public MainWindow()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            try
            {
                // Создаем базу данных если она не существует
                db.Database.EnsureCreated();
                
                // Проверяем подключение к базе данных
                if (!db.Database.CanConnect())
                {
                    MessageBox.Show("Не удается подключиться к базе данных. Проверьте строку подключения.", 
                        "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Заполняем базу данных тестовыми данными если она пустая
                SeedDatabase();

                // 3. Простая привязка ComboBox - загружаем список продуктов
                cmbProduct.ItemsSource = db.Product.ToList();
                
                // 4. Сложная привязка ComboBox - загружаем список продуктов
                cmbProductDetail.ItemsSource = db.Product.ToList();

                statusText.Text = "База данных подключена успешно";
                txtInfo.Text = $"Приложение готово к работе. В базе данных {db.Product.Count()} продуктов. Используйте кнопки для демонстрации привязок данных.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                statusText.Text = "Ошибка подключения к БД";
            }
        }

        /// <summary>
        /// Пересоздает базу данных с новыми данными (для демонстрации)
        /// </summary>
        private void RecreateDatabase()
        {
            try
            {
                // Удаляем базу данных
                db.Database.EnsureDeleted();
                
                // Создаем заново
                db.Database.EnsureCreated();
                
                // Заполняем данными
                SeedDatabase();
                
                // Обновляем ComboBox
                cmbProduct.ItemsSource = db.Product.ToList();
                cmbProductDetail.ItemsSource = db.Product.ToList();
                
                txtInfo.Text = $"База данных пересоздана! Теперь в ней {db.Product.Count()} продуктов.";
                statusText.Text = "База данных обновлена";
                
                MessageBox.Show($"База данных успешно пересоздана с {db.Product.Count()} продуктами!", 
                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка пересоздания БД: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SeedDatabase()
        {
            // Проверяем, есть ли уже данные
            if (db.Product.Any())
                return;

            // Добавляем 10 тестовых продуктов (молочные продукты)
            var products = new[]
            {
                new Product { nameProduct = "Молоко 3.2%", priceProduct = 65.50m, Category = "Молоко", ExpiryDays = 7, Description = "Пастеризованное молоко жирностью 3.2%" },
                new Product { nameProduct = "Творог 9%", priceProduct = 120.00m, Category = "Творог", ExpiryDays = 5, Description = "Творог жирностью 9%" },
                new Product { nameProduct = "Сметана 20%", priceProduct = 85.30m, Category = "Сметана", ExpiryDays = 10, Description = "Сметана жирностью 20%" },
                new Product { nameProduct = "Кефир 2.5%", priceProduct = 55.80m, Category = "Кефир", ExpiryDays = 5, Description = "Кефир жирностью 2.5%" },
                new Product { nameProduct = "Йогурт натуральный", priceProduct = 95.00m, Category = "Йогурт", ExpiryDays = 14, Description = "Натуральный йогурт без добавок" },
                new Product { nameProduct = "Масло сливочное 82.5%", priceProduct = 180.00m, Category = "Масло", ExpiryDays = 30, Description = "Сливочное масло высшего сорта" },
                new Product { nameProduct = "Сыр российский", priceProduct = 450.00m, Category = "Сыр", ExpiryDays = 60, Description = "Твердый сыр российский 45%" },
                new Product { nameProduct = "Ряженка 4%", priceProduct = 68.90m, Category = "Кисломолочные", ExpiryDays = 7, Description = "Ряженка жирностью 4%" },
                new Product { nameProduct = "Творожная масса с изюмом", priceProduct = 135.50m, Category = "Творог", ExpiryDays = 5, Description = "Творожная масса 16% с изюмом" },
                new Product { nameProduct = "Молоко топленое 6%", priceProduct = 78.20m, Category = "Молоко", ExpiryDays = 10, Description = "Топленое молоко жирностью 6%" }
            };

            db.Product.AddRange(products);
            db.SaveChanges();

            // Добавляем 7 тестовых продаж
            var sales = new[]
            {
                new Sale { SaleDate = DateTime.Parse("2024-01-15 10:30:00"), CustomerName = "Иванов И.И.", TotalAmount = 346.60m },
                new Sale { SaleDate = DateTime.Parse("2024-01-16 14:20:00"), CustomerName = "Петрова А.С.", TotalAmount = 275.80m },
                new Sale { SaleDate = DateTime.Parse("2024-01-17 09:15:00"), CustomerName = "Сидоров П.П.", TotalAmount = 190.00m },
                new Sale { SaleDate = DateTime.Parse("2024-01-18 16:45:00"), CustomerName = "Козлова М.В.", TotalAmount = 625.30m },
                new Sale { SaleDate = DateTime.Parse("2024-01-19 11:30:00"), CustomerName = "Морозов Д.А.", TotalAmount = 310.50m },
                new Sale { SaleDate = DateTime.Parse("2024-01-20 15:45:00"), CustomerName = "Васильева О.П.", TotalAmount = 520.70m },
                new Sale { SaleDate = DateTime.Parse("2024-01-21 09:20:00"), CustomerName = "Николаев С.В.", TotalAmount = 415.40m }
            };

            db.Sale.AddRange(sales);
            db.SaveChanges();

            // Добавляем 20+ деталей продаж (используем все 10 продуктов)
            var saleDetails = new[]
            {
                // Продажа 1 - Иванов И.И.
                new SaleDetails { IdSale = 1, IdProductDetailSale = 1, QuantityProduct = 2, UnitPrice = 65.50m },
                new SaleDetails { IdSale = 1, IdProductDetailSale = 2, QuantityProduct = 1, UnitPrice = 120.00m },
                new SaleDetails { IdSale = 1, IdProductDetailSale = 3, QuantityProduct = 1, UnitPrice = 85.30m },
                
                // Продажа 2 - Петрова А.С.
                new SaleDetails { IdSale = 2, IdProductDetailSale = 4, QuantityProduct = 2, UnitPrice = 55.80m },
                new SaleDetails { IdSale = 2, IdProductDetailSale = 5, QuantityProduct = 1, UnitPrice = 95.00m },
                new SaleDetails { IdSale = 2, IdProductDetailSale = 8, QuantityProduct = 1, UnitPrice = 68.90m },
                
                // Продажа 3 - Сидоров П.П.
                new SaleDetails { IdSale = 3, IdProductDetailSale = 2, QuantityProduct = 1, UnitPrice = 120.00m },
                new SaleDetails { IdSale = 3, IdProductDetailSale = 10, QuantityProduct = 1, UnitPrice = 78.20m },
                
                // Продажа 4 - Козлова М.В. (большая покупка)
                new SaleDetails { IdSale = 4, IdProductDetailSale = 6, QuantityProduct = 1, UnitPrice = 180.00m },
                new SaleDetails { IdSale = 4, IdProductDetailSale = 7, QuantityProduct = 1, UnitPrice = 450.00m },
                new SaleDetails { IdSale = 4, IdProductDetailSale = 1, QuantityProduct = 1, UnitPrice = 65.50m },
                
                // Продажа 5 - Морозов Д.А.
                new SaleDetails { IdSale = 5, IdProductDetailSale = 9, QuantityProduct = 1, UnitPrice = 135.50m },
                new SaleDetails { IdSale = 5, IdProductDetailSale = 3, QuantityProduct = 2, UnitPrice = 85.30m },
                
                // Продажа 6 - Васильева О.П.
                new SaleDetails { IdSale = 6, IdProductDetailSale = 7, QuantityProduct = 1, UnitPrice = 450.00m },
                new SaleDetails { IdSale = 6, IdProductDetailSale = 5, QuantityProduct = 1, UnitPrice = 95.00m },
                new SaleDetails { IdSale = 6, IdProductDetailSale = 4, QuantityProduct = 1, UnitPrice = 55.80m },
                
                // Продажа 7 - Николаев С.В.
                new SaleDetails { IdSale = 7, IdProductDetailSale = 6, QuantityProduct = 2, UnitPrice = 180.00m },
                new SaleDetails { IdSale = 7, IdProductDetailSale = 8, QuantityProduct = 1, UnitPrice = 68.90m },
                new SaleDetails { IdSale = 7, IdProductDetailSale = 10, QuantityProduct = 1, UnitPrice = 78.20m }
            };

            db.DetailSale.AddRange(saleDetails);
            db.SaveChanges();
        }

        #region 1. Простая привязка данных к TextBox

        /// <summary>
        /// Загружает первую запись из таблицы Product и привязывает к StackPanel
        /// </summary>
        private void LoadFirstProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Возьмем любую запись из главной таблицы, например первую
                var recProduct = db.Product.First();
                stProduct.DataContext = recProduct;

                txtInfo.Text = $"Загружен продукт: {recProduct.nameProduct}, ID: {recProduct.idProduct}, Цена: {recProduct.priceProduct:C}";
                statusText.Text = "Простая привязка выполнена";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки продукта: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region 2. Сложная привязка данных к TextBox

        /// <summary>
        /// Загружает первую запись из таблицы SaleDetails с связанными данными Product
        /// </summary>
        private void LoadFirstSaleDetail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Возьмем любую запись из таблицы подчиненной, например первую
                var recDetailProduct = db.DetailSale.Include(d => d.Product).First();
                stDetail.DataContext = recDetailProduct;

                txtInfo.Text = $"Загружена деталь продажи: ID {recDetailProduct.IdDetailSale}, " +
                              $"Продукт: {recDetailProduct.Product.nameProduct}, " +
                              $"Количество: {recDetailProduct.QuantityProduct}, " +
                              $"Цена: {recDetailProduct.Product.priceProduct:C}";
                statusText.Text = "Сложная привязка выполнена";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки деталей продажи: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region 3. Простая привязка ComboBox

        /// <summary>
        /// Обработчик выбора элемента в простом ComboBox
        /// </summary>
        private void CmbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbProduct.SelectedValue != null)
            {
                // Получаем выбранное значение
                int articul = Convert.ToInt32(cmbProduct.SelectedValue);
                txtSelectedId.Text = articul.ToString();

                txtInfo.Text = $"В ComboBox выбран продукт с ID: {articul}";
                statusText.Text = "Выбор в ComboBox изменен";
            }
        }

        #endregion

        #region 4. Сложная привязка ComboBox

        // Сложная привязка ComboBox реализована в XAML через SelectedValue="{Binding IdProductDetailSale, Mode=TwoWay}"
        // ComboBox автоматически синхронизируется с полем IdProductDetailSale из контекста данных

        #endregion

        #region Показать данные в формате (новая кнопка)

        /// <summary>
        /// Показывает данные текущей детали продажи в отдельном окне в формате как на рисунке
        /// </summary>
        private void ShowFormattedData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем текущий контекст данных из stDetail
                var currentDetail = stDetail.DataContext as SaleDetails;
                
                if (currentDetail == null)
                {
                    MessageBox.Show("Сначала загрузите данные детали продажи!", "Предупреждение", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Создаем новое окно для отображения
                DetailDisplayWindow displayWindow = new DetailDisplayWindow();
                
                // Если у нас есть загруженный продукт, используем его
                if (currentDetail.Product != null)
                {
                    displayWindow.SetData(currentDetail);
                }
                else
                {
                    // Если продукт не загружен, получаем данные из TextBox
                    string articul = txtArticul.Text;
                    string name = txtNameProduct.Text;
                    string price = txtPriceProduct.Text;
                    string quantity = txtQuantityProduct.Text;
                    
                    displayWindow.SetData(articul, name, price, quantity);
                }

                // Показываем окно
                displayWindow.ShowDialog();

                txtInfo.Text = "Данные отображены в отдельном окне в формате рисунка 1";
                statusText.Text = "Показано окно с форматированными данными";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка отображения данных: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region 5. Доступ к связанным полям

        /// <summary>
        /// Демонстрирует доступ к связанным полям
        /// </summary>
        private void ShowRelatedFields_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var recProductPriceFirst = db.DetailSale.Include(d => d.Product).First();
                
                // Выбираем наименование по связи
                string nameFirst = recProductPriceFirst.Product.nameProduct;
                
                // Выбираем цену по связи
                decimal priceFirst = recProductPriceFirst.Product.priceProduct;

                txtInfo.Text = $"Связанные поля:\n" +
                              $"ID детали: {recProductPriceFirst.IdDetailSale}\n" +
                              $"Наименование продукта: {nameFirst}\n" +
                              $"Цена продукта: {priceFirst:C}\n" +
                              $"Количество: {recProductPriceFirst.QuantityProduct}";

                statusText.Text = "Показаны связанные поля";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения связанных полей: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region 6. Изменение записи с Mode=TwoWay

        /// <summary>
        /// Сохраняет изменения в базу данных
        /// </summary>
        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Сохраняем изменения в базу данных
                int changesCount = db.SaveChanges();
                
                txtInfo.Text = $"Сохранено изменений: {changesCount}.\n" +
                              "Проверьте изменения в СУБД.";
                statusText.Text = $"Сохранено {changesCount} изменений";

                if (changesCount > 0)
                {
                    MessageBox.Show($"Успешно сохранено {changesCount} изменений в базе данных!", 
                        "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region 7. Добавление новых записей

        /// <summary>
        /// Создает новый объект Product для добавления
        /// </summary>
        private void NewProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создаем новый объект Product
                productNew = new Product
                {
                    nameProduct = "",
                    priceProduct = 0,
                    Category = "",
                    ExpiryDays = 0,
                    Description = ""
                };

                // Устанавливаем источник – новую запись
                stProduct.DataContext = productNew;

                txtInfo.Text = "Создан новый объект Product. Заполните поля и нажмите 'Добавить Product в БД'.";
                statusText.Text = "Готов к вводу нового продукта";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания нового продукта: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Добавляет новый Product в базу данных
        /// </summary>
        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(productNew.nameProduct))
                {
                    MessageBox.Show("Заполните название продукта!", "Предупреждение", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Добавляем новый продукт в контекст
                db.Product.Add(productNew);
                
                // Сохраняем в базу данных
                db.SaveChanges();

                txtInfo.Text = $"Новый продукт добавлен в БД:\n" +
                              $"ID: {productNew.idProduct}\n" +
                              $"Название: {productNew.nameProduct}\n" +
                              $"Цена: {productNew.priceProduct:C}";

                statusText.Text = "Новый продукт добавлен в БД";

                // Обновляем ComboBox
                cmbProduct.ItemsSource = db.Product.ToList();
                cmbProductDetail.ItemsSource = db.Product.ToList();

                MessageBox.Show("Новый продукт успешно добавлен в базу данных!", "Успех", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления продукта: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Создает новый объект SaleDetails для добавления
        /// </summary>
        private void NewSaleDetail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создаем новый объект SaleDetails
                detailSaleNew = new SaleDetails
                {
                    IdSale = 1, // Устанавливаем существующий ID продажи
                    IdProductDetailSale = 0,
                    QuantityProduct = 0,
                    UnitPrice = 0
                };

                // Устанавливаем источник – новую запись
                stDetailCombo.DataContext = detailSaleNew;

                txtInfo.Text = "Создан новый объект SaleDetails. Заполните поля и нажмите 'Добавить SaleDetail в БД'.";
                statusText.Text = "Готов к вводу новой детали продажи";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания новой детали продажи: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Добавляет новый SaleDetails в базу данных
        /// </summary>
        private void AddSaleDetail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (detailSaleNew.IdProductDetailSale == 0)
                {
                    MessageBox.Show("Выберите продукт из списка!", "Предупреждение", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (detailSaleNew.QuantityProduct <= 0)
                {
                    MessageBox.Show("Введите количество больше 0!", "Предупреждение", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Добавляем новую деталь продажи в контекст
                db.DetailSale.Add(detailSaleNew);
                
                // Сохраняем в базу данных
                db.SaveChanges();

                txtInfo.Text = $"Новая деталь продажи добавлена в БД:\n" +
                              $"ID: {detailSaleNew.IdDetailSale}\n" +
                              $"ID Продажи: {detailSaleNew.IdSale}\n" +
                              $"ID Продукта: {detailSaleNew.IdProductDetailSale}\n" +
                              $"Количество: {detailSaleNew.QuantityProduct}";

                statusText.Text = "Новая деталь продажи добавлена в БД";

                MessageBox.Show("Новая деталь продажи успешно добавлена в базу данных!", "Успех", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления детали продажи: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Пересоздание базы данных

        /// <summary>
        /// Пересоздает базу данных с 10 продуктами
        /// </summary>
        private void RecreateDatabase_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите пересоздать базу данных? Все текущие данные будут удалены!", 
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                RecreateDatabase();
            }
        }

        #endregion

        protected override void OnClosed(EventArgs e)
        {
            db?.Dispose();
            base.OnClosed(e);
        }
    }
}