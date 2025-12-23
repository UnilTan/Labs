-- Создание таблицы ProductWithImage для ЛР-14 Часть 2
-- Работа с изображениями в БД (varbinary)

USE Familia22i1L9;
GO

-- Создание таблицы ProductWithImage
IF OBJECT_ID('ProductWithImage', 'U') IS NOT NULL
    DROP TABLE ProductWithImage;
GO

CREATE TABLE ProductWithImage (
    ProductId INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(100) NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    ExpiryDays INT NOT NULL,
    Description NVARCHAR(255),
    
    -- ПОЛЯ ДЛЯ ИЗОБРАЖЕНИЙ В БД
    ProductImage VARBINARY(MAX) NULL,           -- Само изображение в виде байтов
    ImageFileName NVARCHAR(100) NULL,           -- Имя файла изображения
    ImageContentType NVARCHAR(50) NULL,         -- Тип контента (image/jpeg, image/png)
    ImageFileSize BIGINT NULL                   -- Размер файла в байтах
);

-- Заполнение тестовыми данными (пока без изображений)
INSERT INTO ProductWithImage (ProductName, Price, Category, ExpiryDays, Description) VALUES
('Молоко 3.2%', 65.50, 'Молоко', 7, 'Пастеризованное молоко жирностью 3.2%'),
('Творог 9%', 120.00, 'Творог', 5, 'Творог жирностью 9%'),
('Сметана 20%', 85.30, 'Сметана', 10, 'Сметана жирностью 20%'),
('Кефир 2.5%', 55.80, 'Кефир', 5, 'Кефир жирностью 2.5%'),
('Йогурт натуральный', 95.00, 'Йогурт', 14, 'Натуральный йогурт без добавок'),
('Масло сливочное 82.5%', 180.00, 'Масло', 30, 'Сливочное масло высшего сорта'),
('Сыр российский', 450.00, 'Сыр', 60, 'Твердый сыр российский 45%'),
('Ряженка 4%', 68.90, 'Кисломолочные', 7, 'Ряженка жирностью 4%');

-- Проверяем созданную таблицу
SELECT 
    ProductId,
    ProductName,
    Price,
    Category,
    CASE 
        WHEN ProductImage IS NULL THEN 'Нет изображения'
        ELSE CONCAT('Есть изображение (', DATALENGTH(ProductImage), ' байт)')
    END as ImageStatus,
    ImageFileName,
    ImageContentType
FROM ProductWithImage
ORDER BY ProductId;

-- Информация о структуре таблицы
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'ProductWithImage'
ORDER BY ORDINAL_POSITION;