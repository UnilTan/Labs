using MilkProductsCatalog.Models;
using Microsoft.EntityFrameworkCore;

namespace MilkProductsCatalog
{
    public partial class ConnectionTestForm : Form
    {
        private TextBox txtResults = null!;
        private Button btnTest = null!;
        private Button btnCreateDB = null!;
        private Button btnAutoSetup = null!;

        public ConnectionTestForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Тест подключения к базе данных";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            // TextBox для результатов
            txtResults = new TextBox
            {
                Location = new Point(10, 10),
                Size = new Size(560, 280),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                Font = new Font("Consolas", 9)
            };

            // Кнопка автоматической настройки
            btnAutoSetup = new Button
            {
                Location = new Point(10, 300),
                Size = new Size(180, 35),
                Text = "Автоматическая настройка",
                BackColor = Color.LightGreen
            };
            btnAutoSetup.Click += BtnAutoSetup_Click;

            // Кнопка тестирования
            btnTest = new Button
            {
                Location = new Point(200, 300),
                Size = new Size(120, 35),
                Text = "Тест подключения"
            };
            btnTest.Click += BtnTest_Click;

            // Кнопка создания БД
            btnCreateDB = new Button
            {
                Location = new Point(330, 300),
                Size = new Size(120, 35),
                Text = "Создать БД"
            };
            btnCreateDB.Click += BtnCreateDB_Click;

            this.Controls.AddRange(new Control[] { txtResults, btnAutoSetup, btnTest, btnCreateDB });
        }

        private void BtnAutoSetup_Click(object? sender, EventArgs e)
        {
            txtResults.Clear();
            txtResults.AppendText("🔧 АВТОМАТИЧЕСКАЯ НАСТРОЙКА БАЗЫ ДАННЫХ\r\n");
            txtResults.AppendText("=" + new string('=', 50) + "\r\n\r\n");

            txtResults.AppendText("Шаг 1: Создание базы данных и таблиц...\r\n");
            
            var (success, message) = DatabaseCreator.CreateDatabaseAndTables();
            
            if (success)
            {
                txtResults.AppendText("✓ " + message + "\r\n\r\n");
                
                txtResults.AppendText("Шаг 2: Проверка подключения...\r\n");
                
                var (testSuccess, workingConnection, error) = DatabaseHelper.TestConnections();
                
                if (testSuccess)
                {
                    txtResults.AppendText("✓ Подключение успешно установлено!\r\n");
                    txtResults.AppendText($"✓ Рабочая строка подключения: {workingConnection}\r\n\r\n");
                    
                    // Устанавливаем рабочую строку подключения для всего приложения
                    SalesContext.SetWorkingConnectionString(workingConnection);
                    
                    txtResults.AppendText("🎉 НАСТРОЙКА ЗАВЕРШЕНА УСПЕШНО!\r\n");
                    txtResults.AppendText("Теперь вы можете закрыть это окно и использовать приложение.\r\n");
                    
                    MessageBox.Show(
                        "База данных успешно создана и настроена!\nТеперь приложение готово к работе.",
                        "Успех",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    txtResults.AppendText("⚠️ База данных создана, но есть проблемы с подключением\r\n");
                    txtResults.AppendText($"Ошибка: {error}\r\n");
                }
            }
            else
            {
                txtResults.AppendText("❌ " + message + "\r\n\r\n");
                txtResults.AppendText("ВОЗМОЖНЫЕ ПРИЧИНЫ:\r\n");
                txtResults.AppendText("• SQL Server не запущен\r\n");
                txtResults.AppendText("• SQL Server Express не установлен\r\n");
                txtResults.AppendText("• Нет прав для создания базы данных\r\n\r\n");
                txtResults.AppendText("РЕШЕНИЯ:\r\n");
                txtResults.AppendText("1. Установите SQL Server Express\r\n");
                txtResults.AppendText("2. Запустите SQL Server через Services\r\n");
                txtResults.AppendText("3. Используйте SQL Server Management Studio для создания БД вручную\r\n");
            }
        }

        private void BtnTest_Click(object? sender, EventArgs e)
        {
            txtResults.Clear();
            txtResults.AppendText("Тестирование подключений к базе данных...\r\n\r\n");

            var connectionStrings = DatabaseHelper.GetConnectionStrings();
            
            for (int i = 0; i < connectionStrings.Length; i++)
            {
                txtResults.AppendText($"Тест {i + 1}: ");
                
                try
                {
                    using var context = new SalesContext();
                    var options = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<SalesContext>()
                        .UseSqlServer(connectionStrings[i])
                        .Options;

                    using var testContext = new SalesContext(options);
                    
                    if (testContext.Database.CanConnect())
                    {
                        txtResults.AppendText("✓ УСПЕШНО\r\n");
                        txtResults.AppendText($"   Строка: {connectionStrings[i]}\r\n");
                        
                        // Устанавливаем рабочую строку подключения
                        SalesContext.SetWorkingConnectionString(connectionStrings[i]);
                        
                        // Проверяем наличие таблиц
                        try
                        {
                            var productCount = testContext.Products.Count();
                            txtResults.AppendText($"   Товаров в базе: {productCount}\r\n\r\n");
                        }
                        catch
                        {
                            txtResults.AppendText("   Таблицы не найдены или пусты\r\n\r\n");
                        }
                    }
                    else
                    {
                        txtResults.AppendText("✗ НЕ УДАЛОСЬ ПОДКЛЮЧИТЬСЯ\r\n");
                    }
                }
                catch (Exception ex)
                {
                    txtResults.AppendText($"✗ ОШИБКА: {ex.Message}\r\n");
                }
                
                txtResults.AppendText("\r\n");
            }

            txtResults.AppendText("Тестирование завершено.");
        }

        private void BtnCreateDB_Click(object? sender, EventArgs e)
        {
            txtResults.Clear();
            txtResults.AppendText("Попытка создания базы данных...\r\n\r\n");

            var (success, workingConnection, error) = DatabaseHelper.TestConnections();
            
            if (success)
            {
                txtResults.AppendText($"Найдено рабочее подключение: {workingConnection}\r\n\r\n");
                
                if (DatabaseHelper.CreateDatabaseIfNotExists(workingConnection))
                {
                    txtResults.AppendText("✓ База данных создана или уже существует\r\n");
                }
                else
                {
                    txtResults.AppendText("✗ Не удалось создать базу данных\r\n");
                }
            }
            else
            {
                txtResults.AppendText($"✗ Не найдено рабочее подключение: {error}\r\n");
                txtResults.AppendText("\r\nПроверьте:\r\n");
                txtResults.AppendText("1. Запущен ли SQL Server\r\n");
                txtResults.AppendText("2. Правильность имени экземпляра\r\n");
                txtResults.AppendText("3. Настройки Windows Authentication\r\n");
            }
        }
    }
}