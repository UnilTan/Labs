using Microsoft.Data.SqlClient;
using System.Text;

namespace MilkProductsCatalog
{
    public static class DatabaseCreator
    {
        public static (bool success, string message) CreateDatabaseAndTables()
        {
            var connectionStrings = new string[]
            {
                "Server=.\\SQLEXPRESS;Integrated Security=true;TrustServerCertificate=true;Encrypt=false;",
                "Server=(localdb)\\mssqllocaldb;Integrated Security=true;TrustServerCertificate=true;",
                "Server=.;Integrated Security=true;TrustServerCertificate=true;Encrypt=false;",
                "Server=localhost;Integrated Security=true;TrustServerCertificate=true;Encrypt=false;"
            };

            foreach (var masterConnectionString in connectionStrings)
            {
                try
                {
                    using var connection = new SqlConnection(masterConnectionString);
                    connection.Open();

                    // Создаем базу данных если она не существует
                    var createDbCommand = new SqlCommand(@"
                        IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Familia22i1L9')
                        BEGIN
                            CREATE DATABASE Familia22i1L9;
                        END", connection);
                    
                    createDbCommand.ExecuteNonQuery();

                    // Переключаемся на созданную базу данных
                    connection.ChangeDatabase("Familia22i1L9");

                    // Создаем таблицы
                    var createTablesScript = GetCreateTablesScript();
                    var createTablesCommand = new SqlCommand(createTablesScript, connection);
                    createTablesCommand.ExecuteNonQuery();

                    // Вставляем тестовые данные
                    var insertDataScript = GetInsertDataScript();
                    var insertDataCommand = new SqlCommand(insertDataScript, connection);
                    insertDataCommand.ExecuteNonQuery();

                    return (true, $"База данных успешно создана с подключением: {masterConnectionString}");
                }
                catch (Exception ex)
                {
                    // Продолжаем со следующей строкой подключения
                    continue;
                }
            }

            return (false, "Не удалось подключиться ни к одному экземпляру SQL Server");
        }

        private static string GetCreateTablesScript()
        {
            return @"
                -- Удаление таблиц если они существуют (в правильном порядке из-за внешних ключей)
                IF OBJECT_ID('SaleDetails', 'U') IS NOT NULL
                    DROP TABLE SaleDetails;
                IF OBJECT_ID('Sale', 'U') IS NOT NULL
                    DROP TABLE Sale;
                IF OBJECT_ID('Product', 'U') IS NOT NULL
                    DROP TABLE Product;

                -- Создание таблицы Product (Товар)
                CREATE TABLE Product (
                    ProductId INT IDENTITY(1,1) PRIMARY KEY,
                    ProductName NVARCHAR(100) NOT NULL,
                    Price DECIMAL(10,2) NOT NULL,
                    Category NVARCHAR(50) NOT NULL,
                    ExpiryDays INT NOT NULL,
                    Description NVARCHAR(255)
                );

                -- Создание таблицы Sale (Продажа)
                CREATE TABLE Sale (
                    SaleId INT IDENTITY(1,1) PRIMARY KEY,
                    SaleDate DATETIME NOT NULL,
                    CustomerName NVARCHAR(100) NOT NULL,
                    TotalAmount DECIMAL(10,2) NOT NULL
                );

                -- Создание таблицы SaleDetails (ДеталиПродаж)
                CREATE TABLE SaleDetails (
                    SaleDetailId INT IDENTITY(1,1) PRIMARY KEY,
                    SaleId INT NOT NULL,
                    ProductId INT NOT NULL,
                    Quantity INT NOT NULL,
                    UnitPrice DECIMAL(10,2) NOT NULL,
                    FOREIGN KEY (SaleId) REFERENCES Sale(SaleId),
                    FOREIGN KEY (ProductId) REFERENCES Product(ProductId)
                );";
        }

        private static string GetInsertDataScript()
        {
            return @"
                -- Заполнение таблицы Product (5 записей)
                INSERT INTO Product (ProductName, Price, Category, ExpiryDays, Description) VALUES
                ('Молоко 3.2%', 65.50, 'Молоко', 7, 'Пастеризованное молоко жирностью 3.2%'),
                ('Творог 9%', 120.00, 'Творог', 5, 'Творог жирностью 9%'),
                ('Сметана 20%', 85.30, 'Сметана', 10, 'Сметана жирностью 20%'),
                ('Кефир 2.5%', 55.80, 'Кефир', 5, 'Кефир жирностью 2.5%'),
                ('Йогурт натуральный', 95.00, 'Йогурт', 14, 'Натуральный йогурт без добавок');

                -- Заполнение таблицы Sale (5 записей)
                INSERT INTO Sale (SaleDate, CustomerName, TotalAmount) VALUES
                ('2024-01-15 10:30:00', 'Иванов И.И.', 346.60),
                ('2024-01-16 14:20:00', 'Петрова А.С.', 275.80),
                ('2024-01-17 09:15:00', 'Сидоров П.П.', 190.00),
                ('2024-01-18 16:45:00', 'Козлова М.В.', 425.30),
                ('2024-01-19 11:30:00', 'Морозов Д.А.', 310.50);

                -- Заполнение таблицы SaleDetails (15 записей)
                INSERT INTO SaleDetails (SaleId, ProductId, Quantity, UnitPrice) VALUES
                (1, 1, 2, 65.50),
                (1, 2, 1, 120.00),
                (1, 3, 1, 85.30),
                (2, 1, 1, 65.50),
                (2, 4, 2, 55.80),
                (2, 5, 1, 95.00),
                (3, 2, 1, 120.00),
                (3, 5, 1, 95.00),
                (4, 1, 3, 65.50),
                (4, 2, 1, 120.00),
                (4, 3, 2, 85.30),
                (4, 4, 1, 55.80),
                (5, 1, 1, 65.50),
                (5, 3, 1, 85.30),
                (5, 5, 2, 95.00);";
        }
    }
}