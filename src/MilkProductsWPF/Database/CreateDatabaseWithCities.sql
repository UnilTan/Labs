-- Создание базы данных Familia22i1L9 с таблицей городов
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Familia22i1L9')
BEGIN
    CREATE DATABASE Familia22i1L9;
END
GO

USE Familia22i1L9;
GO

-- Удаление таблиц если они существуют (в правильном порядке из-за внешних ключей)
IF OBJECT_ID('SaleDetails', 'U') IS NOT NULL
    DROP TABLE SaleDetails;
IF OBJECT_ID('Sale', 'U') IS NOT NULL
    DROP TABLE Sale;
IF OBJECT_ID('Product', 'U') IS NOT NULL
    DROP TABLE Product;
IF OBJECT_ID('City', 'U') IS NOT NULL
    DROP TABLE City;
GO

-- Создание таблицы City (Город)
CREATE TABLE City (
    CityId INT IDENTITY(1,1) PRIMARY KEY,
    CityName NVARCHAR(100) NOT NULL,
    Region NVARCHAR(100) NULL,
    Country NVARCHAR(100) NULL,
    Population INT NOT NULL DEFAULT 0
);

-- Создание таблицы Product (Товар) с внешним ключом на City
CREATE TABLE Product (
    ProductId INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(100) NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    ExpiryDays INT NOT NULL,
    Description NVARCHAR(255),
    CityId INT NULL,
    FOREIGN KEY (CityId) REFERENCES City(CityId) ON DELETE SET NULL
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
);

-- Заполнение таблицы City (10 городов)
INSERT INTO City (CityName, Region, Country, Population) VALUES
('Москва', 'Московская область', 'Россия', 12500000),
('Санкт-Петербург', 'Ленинградская область', 'Россия', 5400000),
('Казань', 'Республика Татарстан', 'Россия', 1300000),
('Новосибирск', 'Новосибирская область', 'Россия', 1600000),
('Екатеринбург', 'Свердловская область', 'Россия', 1500000),
('Нижний Новгород', 'Нижегородская область', 'Россия', 1250000),
('Челябинск', 'Челябинская область', 'Россия', 1200000),
('Самара', 'Самарская область', 'Россия', 1150000),
('Омск', 'Омская область', 'Россия', 1100000),
('Ростов-на-Дону', 'Ростовская область', 'Россия', 1140000);

-- Заполнение таблицы Product (10 продуктов, каждый из своего города)
INSERT INTO Product (ProductName, Price, Category, ExpiryDays, Description, CityId) VALUES
('Молоко пастеризованное 3.2%', 65.50, 'Молоко', 5, 'Натуральное коровье молоко', 1), -- Москва
('Творог зерненый 9%', 120.00, 'Творог', 7, 'Творог с натуральными сливками', 2), -- Санкт-Петербург
('Йогурт питьевой клубничный', 45.30, 'Йогурт', 14, 'С кусочками клубники', 3), -- Казань
('Сметана домашняя 20%', 85.00, 'Сметана', 10, 'Густая домашняя сметана', 4), -- Новосибирск
('Кефир традиционный 2.5%', 55.20, 'Кефир', 5, 'Кисломолочный напиток', 5), -- Екатеринбург
('Ряженка украинская 4%', 60.00, 'Ряженка', 7, 'Традиционная ряженка', 6), -- Нижний Новгород
('Масло сливочное 82.5%', 180.00, 'Масло', 30, 'Натуральное сливочное масло', 7), -- Челябинск
('Сыр российский твердый', 350.00, 'Сыр', 60, 'Твердый сыр классический', 8), -- Самара
('Простокваша мечниковская', 48.00, 'Простокваша', 4, 'Кисломолочный продукт', 9), -- Омск
('Снежок ванильный', 42.50, 'Снежок', 7, 'Сладкий молочный коктейль', 10); -- Ростов-на-Дону

-- Заполнение таблицы Sale (5 продаж)
INSERT INTO Sale (SaleDate, CustomerName, TotalAmount) VALUES
('2024-01-15 10:30:00', 'Иванов И.И.', 346.60),
('2024-01-16 14:20:00', 'Петрова А.С.', 275.80),
('2024-01-17 09:15:00', 'Сидоров П.П.', 190.00),
('2024-01-18 16:45:00', 'Козлова М.В.', 425.30),
('2024-01-19 11:30:00', 'Морозов Д.А.', 310.50);

-- Заполнение таблицы SaleDetails (15 записей)
INSERT INTO SaleDetails (SaleId, ProductId, Quantity, UnitPrice) VALUES
-- Продажа 1
(1, 1, 2, 65.50),  -- Молоко из Москвы
(1, 2, 1, 120.00), -- Творог из СПб
(1, 3, 2, 45.30),  -- Йогурт из Казани
-- Продажа 2
(2, 4, 1, 85.00),  -- Сметана из Новосибирска
(2, 5, 2, 55.20),  -- Кефир из Екатеринбурга
(2, 6, 1, 60.00),  -- Ряженка из Н.Новгорода
-- Продажа 3
(3, 7, 1, 180.00), -- Масло из Челябинска
(3, 8, 1, 350.00), -- Сыр из Самары
-- Продажа 4
(4, 9, 3, 48.00),  -- Простокваша из Омска
(4, 10, 2, 42.50), -- Снежок из Ростова
(4, 1, 1, 65.50),  -- Молоко из Москвы
-- Продажа 5
(5, 2, 1, 120.00), -- Творог из СПб
(5, 4, 1, 85.00),  -- Сметана из Новосибирска
(5, 6, 2, 60.00),  -- Ряженка из Н.Новгорода
(5, 8, 1, 350.00); -- Сыр из Самары

PRINT 'База данных успешно создана с таблицей городов!';
PRINT 'Создано 10 городов и 10 продуктов, каждый продукт привязан к своему городу.';