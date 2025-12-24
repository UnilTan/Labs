-- Скрипт для проверки подключения и данных в базе Familia22i1L9

USE Familia22i1L9;
GO

-- Проверка существования таблиц
SELECT 
    TABLE_NAME as 'Таблица',
    TABLE_TYPE as 'Тип'
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_CATALOG = 'Familia22i1L9'
ORDER BY TABLE_NAME;

-- Проверка данных в таблице Product
SELECT 'Товары:' as 'Проверка';
SELECT COUNT(*) as 'Количество товаров' FROM Product;
SELECT TOP 3 ProductName, Price, Category FROM Product;

-- Проверка данных в таблице Sale
SELECT 'Продажи:' as 'Проверка';
SELECT COUNT(*) as 'Количество продаж' FROM Sale;
SELECT TOP 3 CustomerName, SaleDate, TotalAmount FROM Sale;

-- Проверка данных в таблице SaleDetails
SELECT 'Детали продаж:' as 'Проверка';
SELECT COUNT(*) as 'Количество записей' FROM SaleDetails;
SELECT TOP 3 
    sd.Quantity,
    sd.UnitPrice,
    p.ProductName,
    s.CustomerName
FROM SaleDetails sd
JOIN Product p ON sd.ProductId = p.ProductId
JOIN Sale s ON sd.SaleId = s.SaleId;

-- Проверка целостности данных
SELECT 'Проверка связей:' as 'Проверка';
SELECT 
    (SELECT COUNT(*) FROM SaleDetails) as 'Детали продаж',
    (SELECT COUNT(DISTINCT SaleId) FROM SaleDetails) as 'Уникальные продажи в деталях',
    (SELECT COUNT(*) FROM Sale) as 'Всего продаж',
    (SELECT COUNT(DISTINCT ProductId) FROM SaleDetails) as 'Уникальные товары в деталях',
    (SELECT COUNT(*) FROM Product) as 'Всего товаров';