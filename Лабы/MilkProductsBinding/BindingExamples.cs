using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MilkProductsBinding.Models;
using Microsoft.EntityFrameworkCore;

namespace MilkProductsBinding
{
    /// <summary>
    /// Класс с примерами различных типов привязок данных для лабораторной работы 11
    /// Демонстрирует все основные сценарии Data Binding в WPF
    /// </summary>
    public class BindingExamples
    {
        private SalesContext db;
        
        // UI элементы (должны быть переданы из MainWindow)
        private StackPanel stProduct;
        private StackPanel stDetail;
        private StackPanel stDetailCombo;
        private ComboBox cmbProduct;
        private ComboBox cmbProductDetail;
        private TextBox txtSelectedId;
        private TextBlock txtInfo;
        private TextBlock statusText;

        public BindingExamples(SalesContext database)
        {
            db = database;
        }

        #region 1. ПРОСТАЯ ПРИВЯЗКА ДАННЫХ К TEXTBOX

        /// <summary>
        /// Демонстрация простой привязки данных к TextBox
        /// Привязываем поля объекта Product напрямую к TextBox через DataContext
        /// </summary>
        /// <param name="productPanel">StackPanel с TextBox для Product</param>
        public void DemoSimpleBinding(StackPanel productPanel)
        {
            try
            {
                // Получаем первую запись из таблицы Product
                var product = db.Product.First();
                
                // КЛЮЧЕВОЙ МОМЕНТ: Устанавливаем DataContext
                // Все дочерние элементы StackPanel теперь могут привязываться к свойствам Product
                productPanel.DataContext = product;
                
                /* 
                 * В XAML это выглядит так:
                 * <TextBox Text="{Binding idProduct}"/>
                 * <TextBox Text="{Binding nameProduct, Mode=TwoWay}"/>
                 * <TextBox Text="{Binding priceProduct, Mode=TwoWay}"/>
                 * 
                 * Binding автоматически ищет свойства в DataContext
                 * Mode=TwoWay означает двустороннюю привязку (изменения в UI → объект)
                 */
                
                Console.WriteLine($"Простая привязка: загружен продукт {product.nameProduct}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка простой привязки: {ex.Message}");
            }
        }

        #endregion

        #region 2. СЛОЖНАЯ ПРИВЯЗКА ДАННЫХ К TEXTBOX

        /// <summary>
        /// Демонстрация сложной привязки данных с навигационными свойствами
        /// Привязываем поля связанных объектов SaleDetails → Product
        /// </summary>
        /// <param name="detailPanel">StackPanel с TextBox для SaleDetails</param>
        public void DemoComplexBinding(StackPanel detailPanel)
        {
            try
            {
                // Загружаем SaleDetails вместе со связанным Product (Include)
                var saleDetail = db.DetailSale.Include(d => d.Product).First();
                
                // Устанавливаем DataContext на SaleDetails
                detailPanel.DataContext = saleDetail;
                
                /* 
                 * В XAML сложная привязка выглядит так:
                 * <TextBox Text="{Binding IdDetailSale}"/>              // Прямое свойство
                 * <TextBox Text="{Binding IdProductDetailSale}"/>       // Прямое свойство (FK)
                 * <TextBox Text="{Binding Product.nameProduct}"/>       // Навигационное свойство!
                 * <TextBox Text="{Binding Product.priceProduct}"/>      // Навигационное свойство!
                 * <TextBox Text="{Binding QuantityProduct, Mode=TwoWay}"/> // Двусторонняя привязка
                 * 
                 * Product.nameProduct означает: 
                 * 1. Взять объект из DataContext (SaleDetails)
                 * 2. Получить его свойство Product (навигационное свойство)
                 * 3. Получить свойство nameProduct у Product
                 */
                
                Console.WriteLine($"Сложная привязка: загружена деталь для продукта {saleDetail.Product.nameProduct}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сложной привязки: {ex.Message}");
            }
        }

        #endregion

        #region 3. ПРОСТАЯ ПРИВЯЗКА COMBOBOX

        /// <summary>
        /// Демонстрация простой привязки ComboBox
        /// Показывает как заполнить ComboBox данными и получить выбранное значение
        /// </summary>
        /// <param name="combo">ComboBox для привязки</param>
        /// <param name="selectedTextBox">TextBox для отображения выбранного ID</param>
        public void DemoSimpleComboBoxBinding(ComboBox combo, TextBox selectedTextBox)
        {
            try
            {
                // Заполняем ComboBox списком продуктов
                var products = db.Product.ToList();
                combo.ItemsSource = products;
                
                /* 
                 * В XAML это настраивается так:
                 * <ComboBox x:Name="cmbProduct" 
                 *           DisplayMemberPath="nameProduct"     // Что показывать пользователю
                 *           SelectedValuePath="idProduct"       // Какое значение возвращать
                 *           ItemsSource="{Binding Products}"/>  // Источник данных
                 */
                
                // Обработчик выбора элемента
                combo.SelectionChanged += (sender, e) =>
                {
                    if (combo.SelectedValue != null)
                    {
                        // Получаем ID выбранного продукта
                        int selectedId = Convert.ToInt32(combo.SelectedValue);
                        selectedTextBox.Text = selectedId.ToString();
                        
                        Console.WriteLine($"Выбран продукт с ID: {selectedId}");
                    }
                };
                
                Console.WriteLine($"Простой ComboBox: загружено {products.Count} продуктов");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка привязки ComboBox: {ex.Message}");
            }
        }

        #endregion

        #region 4. СЛОЖНАЯ ПРИВЯЗКА COMBOBOX С TWOWAY

        /// <summary>
        /// Демонстрация сложной привязки ComboBox с двусторонней привязкой
        /// ComboBox автоматически синхронизируется с полем FK в объекте
        /// </summary>
        /// <param name="combo">ComboBox для привязки</param>
        /// <param name="detailPanel">StackPanel с контекстом SaleDetails</param>
        public void DemoComplexComboBoxBinding(ComboBox combo, StackPanel detailPanel)
        {
            try
            {
                // Заполняем ComboBox продуктами
                combo.ItemsSource = db.Product.ToList();
                
                // Загружаем SaleDetails для привязки
                var saleDetail = db.DetailSale.First();
                detailPanel.DataContext = saleDetail;
                
                /* 
                 * В XAML сложная привязка ComboBox выглядит так:
                 * <ComboBox x:Name="cmbProductDetail" 
                 *           DisplayMemberPath="nameProduct" 
                 *           SelectedValuePath="idProduct"
                 *           SelectedValue="{Binding IdProductDetailSale, Mode=TwoWay}"/>
                 * 
                 * SelectedValue="{Binding IdProductDetailSale, Mode=TwoWay}" означает:
                 * 1. При загрузке: ComboBox автоматически выберет элемент с idProduct = IdProductDetailSale
                 * 2. При изменении выбора: IdProductDetailSale автоматически обновится новым значением
                 * 
                 * Это ДВУСТОРОННЯЯ привязка FK!
                 */
                
                Console.WriteLine($"Сложный ComboBox: настроена двусторонняя привязка FK");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сложной привязки ComboBox: {ex.Message}");
            }
        }

        #endregion

        #region 5. ДОСТУП К СВЯЗАННЫМ ПОЛЯМ

        /// <summary>
        /// Демонстрация программного доступа к связанным полям
        /// Показывает как получить данные из навигационных свойств в коде
        /// </summary>
        public void DemoRelatedFieldsAccess()
        {
            try
            {
                // Загружаем SaleDetails со связанным Product
                var saleDetailWithProduct = db.DetailSale.Include(d => d.Product).First();
                
                // Доступ к связанным полям через навигационные свойства
                string productName = saleDetailWithProduct.Product.nameProduct;
                decimal productPrice = saleDetailWithProduct.Product.priceProduct;
                int quantity = saleDetailWithProduct.QuantityProduct;
                
                // Вычисляем общую стоимость
                decimal totalCost = productPrice * quantity;
                
                Console.WriteLine($"Связанные поля:");
                Console.WriteLine($"  Продукт: {productName}");
                Console.WriteLine($"  Цена за единицу: {productPrice:C}");
                Console.WriteLine($"  Количество: {quantity}");
                Console.WriteLine($"  Общая стоимость: {totalCost:C}");
                
                /* 
                 * Важно: Include(d => d.Product) загружает связанные данные
                 * Без Include получим null в saleDetailWithProduct.Product
                 * 
                 * Альтернативы:
                 * 1. Lazy Loading (автоматическая загрузка при обращении)
                 * 2. Explicit Loading (явная загрузка через Load())
                 * 3. Eager Loading (загрузка через Include - используем здесь)
                 */
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка доступа к связанным полям: {ex.Message}");
            }
        }

        #endregion

        #region 6. СОХРАНЕНИЕ ИЗМЕНЕНИЙ С MODE=TWOWAY

        /// <summary>
        /// Демонстрация сохранения изменений при двусторонней привязке
        /// Показывает как Mode=TwoWay автоматически обновляет объекты
        /// </summary>
        public void DemoSaveChangesWithTwoWay()
        {
            try
            {
                // При Mode=TwoWay изменения в UI автоматически применяются к объектам
                // Нужно только вызвать SaveChanges() для сохранения в БД
                
                int changesCount = db.SaveChanges();
                
                Console.WriteLine($"Сохранено изменений: {changesCount}");
                
                /* 
                 * Как это работает:
                 * 1. Пользователь изменяет текст в TextBox с Mode=TwoWay
                 * 2. WPF автоматически обновляет свойство объекта в DataContext
                 * 3. Entity Framework отслеживает изменения в объектах
                 * 4. SaveChanges() генерирует и выполняет SQL UPDATE
                 * 
                 * Без Mode=TwoWay изменения в UI не попадут в объект!
                 */
                
                if (changesCount > 0)
                {
                    MessageBox.Show($"Успешно сохранено {changesCount} изменений в базе данных!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        #endregion

        #region 7. ДОБАВЛЕНИЕ НОВЫХ ЗАПИСЕЙ ЧЕРЕЗ BINDING

        /// <summary>
        /// Демонстрация добавления новых записей через привязку данных
        /// Показывает как создать новый объект и привязать его для ввода данных
        /// </summary>
        /// <param name="productPanel">StackPanel для ввода данных Product</param>
        public Product DemoAddNewProductWithBinding(StackPanel productPanel)
        {
            try
            {
                // Создаем новый объект Product
                var newProduct = new Product
                {
                    nameProduct = "",
                    priceProduct = 0,
                    Category = "",
                    ExpiryDays = 0,
                    Description = ""
                };
                
                // Привязываем новый объект к UI для ввода данных
                productPanel.DataContext = newProduct;
                
                /* 
                 * Теперь пользователь может заполнить поля в UI
                 * Благодаря Mode=TwoWay данные автоматически попадут в newProduct
                 * 
                 * После заполнения нужно:
                 * 1. db.Product.Add(newProduct);
                 * 2. db.SaveChanges();
                 */
                
                Console.WriteLine("Новый Product готов для ввода данных");
                return newProduct;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания нового продукта: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Сохранение нового продукта в базу данных
        /// </summary>
        /// <param name="newProduct">Продукт для сохранения</param>
        public void SaveNewProduct(Product newProduct)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newProduct.nameProduct))
                {
                    MessageBox.Show("Заполните название продукта!");
                    return;
                }
                
                // Добавляем в контекст Entity Framework
                db.Product.Add(newProduct);
                
                // Сохраняем в базу данных
                db.SaveChanges();
                
                Console.WriteLine($"Новый продукт сохранен: ID={newProduct.idProduct}, Название={newProduct.nameProduct}");
                MessageBox.Show("Новый продукт успешно добавлен в базу данных!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения продукта: {ex.Message}");
            }
        }

        /// <summary>
        /// Демонстрация добавления новой детали продажи
        /// </summary>
        /// <param name="detailPanel">StackPanel для ввода данных SaleDetails</param>
        public SaleDetails DemoAddNewSaleDetailWithBinding(StackPanel detailPanel)
        {
            try
            {
                // Создаем новый объект SaleDetails
                var newSaleDetail = new SaleDetails
                {
                    IdSale = 1, // Устанавливаем существующий ID продажи
                    IdProductDetailSale = 0, // Будет выбран в ComboBox
                    QuantityProduct = 0,
                    UnitPrice = 0
                };
                
                // Привязываем к UI
                detailPanel.DataContext = newSaleDetail;
                
                Console.WriteLine("Новый SaleDetails готов для ввода данных");
                return newSaleDetail;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания новой детали продажи: {ex.Message}");
                return null;
            }
        }

        #endregion

        #region 8. ОБНОВЛЕНИЕ COMBOBOX ПОСЛЕ ИЗМЕНЕНИЙ

        /// <summary>
        /// Обновление источников данных ComboBox после изменений в БД
        /// </summary>
        /// <param name="productCombo">ComboBox с продуктами</param>
        /// <param name="productDetailCombo">ComboBox с продуктами для деталей</param>
        public void RefreshComboBoxes(ComboBox productCombo, ComboBox productDetailCombo)
        {
            try
            {
                // Перезагружаем данные из БД
                var products = db.Product.ToList();
                
                // Обновляем источники данных
                productCombo.ItemsSource = products;
                productDetailCombo.ItemsSource = products;
                
                Console.WriteLine($"ComboBox обновлены: {products.Count} продуктов");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления ComboBox: {ex.Message}");
            }
        }

        #endregion

        #region 9. ТИПОВЫЕ ОШИБКИ И ИХ РЕШЕНИЯ

        /// <summary>
        /// Демонстрация типовых ошибок при работе с привязками данных
        /// </summary>
        public void DemoCommonBindingErrors()
        {
            Console.WriteLine("=== ТИПОВЫЕ ОШИБКИ ПРИВЯЗОК ДАННЫХ ===");
            
            Console.WriteLine("\n1. ОШИБКА: Забыли Include для навигационных свойств");
            Console.WriteLine("   НЕПРАВИЛЬНО: var detail = db.DetailSale.First();");
            Console.WriteLine("   ПРАВИЛЬНО:   var detail = db.DetailSale.Include(d => d.Product).First();");
            
            Console.WriteLine("\n2. ОШИБКА: Забыли Mode=TwoWay для редактируемых полей");
            Console.WriteLine("   НЕПРАВИЛЬНО: <TextBox Text=\"{Binding nameProduct}\"/>");
            Console.WriteLine("   ПРАВИЛЬНО:   <TextBox Text=\"{Binding nameProduct, Mode=TwoWay}\"/>");
            
            Console.WriteLine("\n3. ОШИБКА: Неправильное имя свойства в Binding");
            Console.WriteLine("   НЕПРАВИЛЬНО: <TextBox Text=\"{Binding ProductName}\"/>");
            Console.WriteLine("   ПРАВИЛЬНО:   <TextBox Text=\"{Binding nameProduct}\"/>");
            
            Console.WriteLine("\n4. ОШИБКА: Не установили DataContext");
            Console.WriteLine("   РЕШЕНИЕ: stProduct.DataContext = product;");
            
            Console.WriteLine("\n5. ОШИБКА: Забыли SaveChanges() после изменений");
            Console.WriteLine("   РЕШЕНИЕ: db.SaveChanges();");
            
            Console.WriteLine("\n6. ОШИБКА: Неправильная привязка ComboBox");
            Console.WriteLine("   ПРАВИЛЬНО: DisplayMemberPath=\"nameProduct\" SelectedValuePath=\"idProduct\"");
        }

        #endregion

        #region 10. ПОЛНЫЙ ПРИМЕР ИСПОЛЬЗОВАНИЯ

        /// <summary>
        /// Полный пример использования всех типов привязок
        /// Демонстрирует типичный сценарий работы с данными
        /// </summary>
        public void DemoCompleteBindingScenario()
        {
            try
            {
                Console.WriteLine("=== ПОЛНЫЙ СЦЕНАРИЙ РАБОТЫ С ПРИВЯЗКАМИ ===");
                
                // 1. Загружаем данные для простой привязки
                var product = db.Product.First();
                Console.WriteLine($"1. Загружен продукт: {product.nameProduct}");
                
                // 2. Загружаем данные для сложной привязки
                var saleDetail = db.DetailSale.Include(d => d.Product).First();
                Console.WriteLine($"2. Загружена деталь продажи для: {saleDetail.Product.nameProduct}");
                
                // 3. Демонстрируем доступ к связанным полям
                string relatedProductName = saleDetail.Product.nameProduct;
                decimal relatedProductPrice = saleDetail.Product.priceProduct;
                Console.WriteLine($"3. Связанные данные: {relatedProductName} - {relatedProductPrice:C}");
                
                // 4. Создаем новый объект для добавления
                var newProduct = new Product
                {
                    nameProduct = "Новый продукт",
                    priceProduct = 100.00m,
                    Category = "Тестовая категория"
                };
                Console.WriteLine($"4. Создан новый продукт: {newProduct.nameProduct}");
                
                // 5. Сохраняем изменения
                db.Product.Add(newProduct);
                int changes = db.SaveChanges();
                Console.WriteLine($"5. Сохранено изменений: {changes}");
                
                Console.WriteLine("\n=== СЦЕНАРИЙ ЗАВЕРШЕН УСПЕШНО ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в полном сценарии: {ex.Message}");
            }
        }

        #endregion
    }
}