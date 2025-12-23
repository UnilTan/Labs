-- Проверка подключения и создание таблиц
USE Familia22i1L9;
GO

-- Проверяем, существует ли таблица City
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'City')
BEGIN
    PRINT 'Создаем таблицу City...';
    
    CREATE TABLE City (
        CityId INT IDENTITY(1,1) PRIMARY KEY,
        CityName NVARCHAR(100) NOT NULL,
        Region NVARCHAR(100) NULL,
        Country NVARCHAR(100) NULL,
        Population INT NOT NULL DEFAULT 0
    );
    
    -- Вставляем города
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
    
    PRINT 'Таблица City создана и заполнена!';
END
ELSE
BEGIN
    PRINT 'Таблица City уже существует.';
END

-- Проверяем, есть ли поле CityId в таблице Product
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Product' AND COLUMN_NAME = 'CityId')
BEGIN
    PRINT 'Добавляем поле CityId в таблицу Product...';
    
    ALTER TABLE Product ADD CityId INT NULL;
    ALTER TABLE Product ADD CONSTRAINT FK_Product_City 
        FOREIGN KEY (CityId) REFERENCES City(CityId) ON DELETE SET NULL;
    
    -- Обновляем существующие продукты, назначая им города
    UPDATE Product SET CityId = 1 WHERE ProductId = 1; -- Москва
    UPDATE Product SET CityId = 2 WHERE ProductId = 2; -- Санкт-Петербург
    UPDATE Product SET CityId = 3 WHERE ProductId = 3; -- Казань
    UPDATE Product SET CityId = 4 WHERE ProductId = 4; -- Новосибирск
    UPDATE Product SET CityId = 5 WHERE ProductId = 5; -- Екатеринбург
    
    PRINT 'Поле CityId добавлено и заполнено!';
END
ELSE
BEGIN
    PRINT 'Поле CityId уже существует в таблице Product.';
END

-- Показываем результат
SELECT 'Города:' as Info;
SELECT CityId, CityName, Region, Country, Population FROM City;

SELECT 'Продукты с городами:' as Info;
SELECT p.ProductId, p.ProductName, p.Price, p.Category, c.CityName 
FROM Product p 
LEFT JOIN City c ON p.CityId = c.CityId;