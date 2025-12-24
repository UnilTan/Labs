using Microsoft.Data.SqlClient;
using System.Text;
using MilkProducts.Shared.Configuration;

namespace MilkProductsCatalog
{
    public static class DatabaseCreator
    {
        private const string DefaultMasterConnection = "Server=.\\SQLEXPRESS;Integrated Security=true;TrustServerCertificate=true;Encrypt=false;";

        public static (bool success, string message) CreateDatabaseAndTables()
        {
            var candidateConnections = new[]
            {
                ConnectionStringProvider.GetConnectionString(DefaultMasterConnection),
                "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;TrustServerCertificate=true;Encrypt=false;",
                "Server=.;Integrated Security=true;TrustServerCertificate=true;Encrypt=false;",
                "Server=localhost;Integrated Security=true;TrustServerCertificate=true;Encrypt=false;"
            };

            foreach (var masterConnectionString in candidateConnections.Distinct())
            {
                try
                {
                    var builder = new SqlConnectionStringBuilder(masterConnectionString)
                    {
                        InitialCatalog = "master"
                    };

                    using var connection = new SqlConnection(builder.ToString());
                    connection.Open();

                    var createDbCommand = new SqlCommand(@"
                        IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Familia22i1L9')
                        BEGIN
                            CREATE DATABASE Familia22i1L9;
                        END", connection);
                    createDbCommand.ExecuteNonQuery();

                    connection.ChangeDatabase("Familia22i1L9");

                    var createTablesCommand = new SqlCommand(GetCreateTablesScript(), connection);
                    createTablesCommand.ExecuteNonQuery();

                    var insertDataCommand = new SqlCommand(GetInsertDataScript(), connection);
                    insertDataCommand.ExecuteNonQuery();

                    return (true, $"База данных готова по строке подключения: {masterConnectionString}");
                }
                catch
                {
                    continue;
                }
            }

            return (false, "Не удалось создать или проверить БД ни по одной строке подключения. Проверьте SQL Server и доступность ConnectionStrings:SqlServer.");
        }

        private static string GetCreateTablesScript()
        {
            return @"
                IF OBJECT_ID('Product', 'U') IS NULL
                BEGIN
                    CREATE TABLE Product (
                        ProductId INT IDENTITY(1,1) PRIMARY KEY,
                        ProductName NVARCHAR(100) NOT NULL,
                        Price DECIMAL(10,2) NOT NULL,
                        Category NVARCHAR(50) NOT NULL,
                        ExpiryDays INT NOT NULL,
                        Description NVARCHAR(255)
                    );
                END

                IF OBJECT_ID('Sale', 'U') IS NULL
                BEGIN
                    CREATE TABLE Sale (
                        SaleId INT IDENTITY(1,1) PRIMARY KEY,
                        SaleDate DATETIME NOT NULL,
                        CustomerName NVARCHAR(100) NOT NULL,
                        TotalAmount DECIMAL(10,2) NOT NULL
                    );
                END

                IF OBJECT_ID('SaleDetails', 'U') IS NULL
                BEGIN
                    CREATE TABLE SaleDetails (
                        SaleDetailId INT IDENTITY(1,1) PRIMARY KEY,
                        SaleId INT NOT NULL,
                        ProductId INT NOT NULL,
                        Quantity INT NOT NULL,
                        UnitPrice DECIMAL(10,2) NOT NULL,
                        FOREIGN KEY (SaleId) REFERENCES Sale(SaleId),
                        FOREIGN KEY (ProductId) REFERENCES Product(ProductId)
                    );
                END";
        }

        private static string GetInsertDataScript()
        {
            return @"
                IF NOT EXISTS (SELECT 1 FROM Product)
                BEGIN
                    INSERT INTO Product (ProductName, Price, Category, ExpiryDays, Description) VALUES
                    ('Молоко 3.2%', 65.50, 'Молочная продукция', 7, 'Пастеризованное молоко 3.2%'),
                    ('Творог 9%', 120.00, 'Творог', 5, 'Творог жирный 9%'),
                    ('Сметана 20%', 85.30, 'Сметана', 10, 'Сметана 20%'),
                    ('Кефир 2.5%', 55.80, 'Кисломолочные', 5, 'Кефир 2.5%'),
                    ('Йогурт натуральный', 95.00, 'Йогурты', 14, 'Натуральный йогурт без добавок');
                END

                IF NOT EXISTS (SELECT 1 FROM Sale)
                BEGIN
                    INSERT INTO Sale (SaleDate, CustomerName, TotalAmount) VALUES
                    ('2024-01-15 10:30:00', 'Иванов И.И.', 346.60),
                    ('2024-01-16 14:20:00', 'Петров П.П.', 275.80),
                    ('2024-01-17 09:15:00', 'Сидоров С.С.', 190.00),
                    ('2024-01-18 16:45:00', 'Васильев В.В.', 425.30),
                    ('2024-01-19 11:30:00', 'Кузнецов К.К.', 310.50);
                END

                IF NOT EXISTS (SELECT 1 FROM SaleDetails)
                BEGIN
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
                    (5, 5, 2, 95.00);
                END";
        }
    }
}
