-- Добавление дополнительных продуктов для увеличения до 10 позиций
INSERT INTO Product (ProductName, Price, Category, ExpiryDays, Description, CityId) VALUES
('Молоко пастеризованное 3.2%', 65.50, 'Молоко', 5, 'Натуральное коровье молоко', 1),
('Творог зерненый 9%', 120.00, 'Творог', 7, 'Творог с натуральными сливками', 2),
('Йогурт питьевой клубничный', 45.30, 'Йогурт', 14, 'С кусочками клубники', 3),
('Сметана 20%', 85.00, 'Сметана', 10, 'Густая домашняя сметана', 4),
('Кефир 2.5%', 55.20, 'Кефир', 5, 'Кисломолочный напиток', 5),
('Ряженка 4%', 60.00, 'Ряженка', 7, 'Традиционная ряженка', 6),
('Масло сливочное 82.5%', 180.00, 'Масло', 30, 'Натуральное сливочное масло', 7),
('Сыр российский', 350.00, 'Сыр', 60, 'Твердый сыр классический', 8),
('Простокваша', 48.00, 'Простокваша', 4, 'Кисломолочный продукт', 9),
('Снежок ванильный', 42.50, 'Снежок', 7, 'Сладкий молочный коктейль', 10);

-- Добавление продаж для новых продуктов
INSERT INTO Sale (SaleDate, CustomerName, TotalAmount) VALUES
('2024-01-15', 'Петров А.И.', 250.50),
('2024-01-16', 'Сидорова М.П.', 180.30),
('2024-01-17', 'Козлов В.С.', 420.00),
('2024-01-18', 'Морозова Е.А.', 315.20),
('2024-01-19', 'Волков Д.Н.', 195.75);

-- Добавление деталей продаж
INSERT INTO SaleDetails (SaleId, ProductId, Quantity, UnitPrice) VALUES
-- Для продажи 1
((SELECT MAX(SaleId)-4 FROM Sale), (SELECT ProductId FROM Product WHERE ProductName LIKE 'Молоко%' AND ProductId = (SELECT MIN(ProductId) FROM Product WHERE ProductName LIKE 'Молоко%')), 2, 65.50),
((SELECT MAX(SaleId)-4 FROM Sale), (SELECT ProductId FROM Product WHERE ProductName LIKE 'Творог%' AND ProductId = (SELECT MIN(ProductId) FROM Product WHERE ProductName LIKE 'Творог%')), 1, 120.00),

-- Для продажи 2  
((SELECT MAX(SaleId)-3 FROM Sale), (SELECT ProductId FROM Product WHERE ProductName LIKE 'Йогурт%' AND ProductId = (SELECT MIN(ProductId) FROM Product WHERE ProductName LIKE 'Йогурт%')), 3, 45.30),
((SELECT MAX(SaleId)-3 FROM Sale), (SELECT ProductId FROM Product WHERE ProductName LIKE 'Кефир%' AND ProductId = (SELECT MIN(ProductId) FROM Product WHERE ProductName LIKE 'Кефир%')), 1, 55.20),

-- Для продажи 3
((SELECT MAX(SaleId)-2 FROM Sale), (SELECT ProductId FROM Product WHERE ProductName LIKE 'Масло%' AND ProductId = (SELECT MIN(ProductId) FROM Product WHERE ProductName LIKE 'Масло%')), 2, 180.00),
((SELECT MAX(SaleId)-2 FROM Sale), (SELECT ProductId FROM Product WHERE ProductName LIKE 'Кефир%' AND ProductId = (SELECT MIN(ProductId) FROM Product WHERE ProductName LIKE 'Кефир%')), 1, 60.00),

-- Для продажи 4
((SELECT MAX(SaleId)-1 FROM Sale), (SELECT ProductId FROM Product WHERE ProductName LIKE 'Сыр%' AND ProductId = (SELECT MIN(ProductId) FROM Product WHERE ProductName LIKE 'Сыр%')), 1, 350.00),

-- Для продажи 5
((SELECT MAX(SaleId) FROM Sale), (SELECT ProductId FROM Product WHERE ProductName LIKE 'Снежок%' AND ProductId = (SELECT MIN(ProductId) FROM Product WHERE ProductName LIKE 'Снежок%')), 4, 42.50),
((SELECT MAX(SaleId) FROM Sale), (SELECT ProductId FROM Product WHERE ProductName LIKE 'Простокваша%' AND ProductId = (SELECT MIN(ProductId) FROM Product WHERE ProductName LIKE 'Простокваша%')), 1, 48.00);