using Microsoft.EntityFrameworkCore;
using MilkProductsBinding.Models;
using System;
using System.Linq;
using System.Windows;
using System.Data.Common;

namespace MilkProductsBinding
{
    public partial class Lab13Window : Window
    {
        private SalesContext db;

        public Lab13Window()
        {
            InitializeComponent();
            
            // Инициализация контекста БД
            try
            {
                db = new SalesContext();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к БД: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Загрузка всех данных при открытии окна
        /// </summary>
        private void LoadData()
        {
            // Сначала обновляем структуру БД
            UpdateDatabaseSchema();
            
            LoadSales();
            LoadSaleDetails();
        }

        /// <summary>
        /// Обновление структуры БД для ЛР-13
        /// </summary>
        private void UpdateDatabaseSchema()
        {
            try
            {
                // Проверяем и добавляем недостающие столбцы
                var connection = db.Database.GetDbConnection();
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    // Добавляем столбцы в таблицу Sale, если их нет
                    command.CommandText = @"
                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Sale' AND COLUMN_NAME = 'SummaSale')
                        BEGIN
                            ALTER TABLE Sale ADD SummaSale DECIMAL(10,2) NULL
                        END

                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Sale' AND COLUMN_NAME = 'DiscountSale')
                        BEGIN
                            ALTER TABLE Sale ADD DiscountSale DECIMAL(5,2) NULL
                        END

                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Sale' AND COLUMN_NAME = 'Resultat')
                        BEGIN
                            ALTER TABLE Sale ADD Resultat DECIMAL(10,2) NULL
                        END

                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SaleDetails' AND COLUMN_NAME = 'SummaProduct')
                        BEGIN
                            ALTER TABLE SaleDetails ADD SummaProduct DECIMAL(10,2) NULL
                        END";
                    
                    command.ExecuteNonQuery();
                }

                // Заполняем тестовые данные для скидок, если поля пустые
                var salesWithoutDiscount = db.Sale.Where(s => s.DiscountSale == null).ToList();
                if (salesWithoutDiscount.Any())
                {
                    var random = new Random();
                    foreach (var sale in salesWithoutDiscount)
                    {
                        sale.DiscountSale = (decimal)(random.NextDouble() * 15); // Скидка от 0 до 15%
                    }
                    db.SaveChanges();
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                // Если не удалось обновить БД, продолжаем работу
                System.Diagnostics.Debug.WriteLine($"Ошибка обновления БД: {ex.Message}");
            }
        }

        /// <summary>
        /// Загрузка продаж с включением связанных SaleDetails для вычисляемых свойств
        /// </summary>
        private void btnLoadSales_Click(object sender, RoutedEventArgs e)
        {
            LoadSales();
        }

        private void LoadSales()
        {
            try
            {
                // ВАЖНО: Include для загрузки связанных SaleDetails
                // Без этого вычисляемые свойства не будут работать!
                var sales = db.Sale
                    .Include(s => s.SaleDetails)
                    .ThenInclude(sd => sd.Product)
                    .ToList();

                dgSale.ItemsSource = sales;
                tbSaleInfo.Text = $"Загружено продаж: {sales.Count}";

                // Демонстрация вычисляемых свойств в коде
                if (sales.Any())
                {
                    var firstSale = sales.First();
                    string info = $"Первая продажа: Сумма={firstSale.SummaSaleCalculated:F2}, " +
                                 $"Скидка={firstSale.DiscountAmount:F2}, " +
                                 $"Итого={firstSale.ResultatCalculated:F2}";
                    tbSaleInfo.Text += $" | {info}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки продаж: {ex.Message}", "Ошибка");
                tbSaleInfo.Text = "Ошибка загрузки";
            }
        }

        /// <summary>
        /// Загрузка деталей продаж с включением связанных Product для вычисляемых свойств
        /// </summary>
        private void btnLoadDetails_Click(object sender, RoutedEventArgs e)
        {
            LoadSaleDetails();
        }

        private void LoadSaleDetails()
        {
            try
            {
                // ВАЖНО: Include для загрузки связанных Product
                // Без этого свойства типа Product.priceProduct не будут доступны!
                var details = db.DetailSale
                    .Include(sd => sd.Product)
                    .Include(sd => sd.Sale)
                    .ToList();

                dgSaleDetails.ItemsSource = details;
                tbDetailInfo.Text = $"Загружено позиций: {details.Count}";

                // Демонстрация вычисляемых свойств в коде
                if (details.Any())
                {
                    var firstDetail = details.First();
                    string info = $"Первая позиция: {firstDetail.Product?.nameProduct}, " +
                                 $"Стоимость={firstDetail.SummaProductCalculated:F2}, " +
                                 $"Разница цен={firstDetail.PriceDifference:F2}";
                    tbDetailInfo.Text += $" | {info}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки деталей: {ex.Message}", "Ошибка");
                tbDetailInfo.Text = "Ошибка загрузки";
            }
        }

        /// <summary>
        /// КЛЮЧЕВАЯ ФУНКЦИЯ ЛР-13: Пересчет и сохранение вычисляемых значений в БД
        /// Демонстрирует, как вычисляемые свойства могут обновлять поля в БД
        /// </summary>
        private void btnRecalculateSales_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var sales = db.Sale
                    .Include(s => s.SaleDetails)
                    .ThenInclude(sd => sd.Product)
                    .ToList();

                int updatedCount = 0;

                foreach (var sale in sales)
                {
                    // Используем вычисляемые свойства для обновления полей БД
                    decimal oldSumma = sale.SummaSale ?? 0;
                    decimal oldResultat = sale.Resultat ?? 0;

                    // ДЕМОНСТРАЦИЯ: Вычисляемые свойства → Поля БД
                    sale.SummaSale = sale.SummaSaleCalculated;
                    sale.Resultat = sale.ResultatCalculated;

                    // Проверяем, изменились ли значения
                    if (Math.Abs(oldSumma - sale.SummaSale.Value) > 0.01m || 
                        Math.Abs(oldResultat - sale.Resultat.Value) > 0.01m)
                    {
                        updatedCount++;
                    }
                }

                db.SaveChanges();
                LoadSales(); // Перезагружаем для отображения обновленных данных

                MessageBox.Show($"Пересчитано и сохранено продаж: {updatedCount}", 
                    "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка пересчета продаж: {ex.Message}", "Ошибка");
            }
        }

        /// <summary>
        /// Пересчет и сохранение стоимости позиций в SaleDetails
        /// </summary>
        private void btnRecalculateDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var details = db.DetailSale
                    .Include(sd => sd.Product)
                    .ToList();

                int updatedCount = 0;

                foreach (var detail in details)
                {
                    decimal oldSumma = detail.SummaProduct ?? 0;
                    
                    // ДЕМОНСТРАЦИЯ: Вычисляемое свойство → Поле БД
                    detail.SummaProduct = detail.SummaProductCalculated;

                    if (Math.Abs(oldSumma - detail.SummaProduct.Value) > 0.01m)
                    {
                        updatedCount++;
                    }
                }

                db.SaveChanges();
                LoadSaleDetails(); // Перезагружаем данные

                MessageBox.Show($"Пересчитано и сохранено позиций: {updatedCount}", 
                    "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                // После обновления SaleDetails нужно пересчитать Sale
                btnRecalculateSales_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка пересчета позиций: {ex.Message}", "Ошибка");
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