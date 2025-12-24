-- Обновление базы данных для ЛР-14: Добавление поля photo для изображений
USE Familia22i1L9;
GO

-- ЧАСТЬ 1: Добавляем поле photo в таблицу Product для хранения имени файла изображения
ALTER TABLE Product ADD 
    photo NVARCHAR(255) NULL;  -- Имя файла изображения

-- Заполняем поле photo для существующих продуктов СЛУЧАЙНЫМ ОБРАЗОМ
-- Используем реальные имена файлов изображений из папки Images
DECLARE @ImageFiles TABLE (ImageName NVARCHAR(50));
INSERT INTO @ImageFiles VALUES 
    ('moloko.png'), ('kefir.png'), ('tvorog.png'), 
    ('smetana.png'), ('ryajenka.png');

-- Обновляем продукты случайными изображениями
-- Примерно 70% продуктов получат изображения, 30% останутся с заглушкой
UPDATE Product SET photo = 
    CASE 
        -- Используем остаток от деления ID на число для псевдослучайности
        WHEN (ProductId % 10) IN (1,2,3,4,5,6,7) THEN 
            (SELECT TOP 1 ImageName FROM @ImageFiles 
             ORDER BY NEWID())  -- NEWID() дает случайный порядок
        ELSE NULL  -- Эти продукты будут использовать заглушку picture.jpg
    END;

-- Дополнительно: убеждаемся что у нас есть разнообразие
-- Принудительно назначаем конкретные изображения первым продуктам для демонстрации
UPDATE Product SET photo = 'moloko.png' WHERE ProductId = 1;
UPDATE Product SET photo = 'kefir.png' WHERE ProductId = 2;
UPDATE Product SET photo = 'tvorog.png' WHERE ProductId = 3;
UPDATE Product SET photo = NULL WHERE ProductId = 4;  -- Заглушка для демонстрации
UPDATE Product SET photo = 'smetana.png' WHERE ProductId = 5;
UPDATE Product SET photo = 'ryajenka.png' WHERE ProductId = 6;

-- ЧАСТЬ 2: Создаем отдельную таблицу для изображений в БД (varbinary)
-- Эта таблица будет использоваться во втором проекте
CREATE TABLE ProductWithImage (
    ProductId INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(100) NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    ExpiryDays INT NOT NULL DEFAULT 30,
    Description NVARCHAR(255) NULL,
    
    -- Поля для изображений в БД
    ProductImage VARBINARY(MAX) NULL,      -- Само изображение в виде байтов
    ImageFileName NVARCHAR(100) NULL,      -- Имя файла изображения
    ImageContentType NVARCHAR(50) NULL,    -- Тип контента (image/jpeg, image/png и т.д.)
    ImageFileSize BIGINT NULL              -- Размер файла в байтах
);

-- Копируем данные из основной таблицы Product в новую таблицу
INSERT INTO ProductWithImage (ProductName, Price, Category, ExpiryDays, Description)
SELECT ProductName, Price, Category, ExpiryDays, Description
FROM Product;

-- Проверяем результат
SELECT 
    ProductId,
    ProductName,
    Price,
    Category,
    photo,
    CASE 
        WHEN photo IS NULL THEN 'Будет использована заглушка picture.jpg'
        ELSE 'Будет использовано изображение: ' + photo
    END as ImageStatus
FROM Product
ORDER BY ProductId;

SELECT 
    ProductId,
    ProductName,
    Price,
    Category,
    CASE 
        WHEN ProductImage IS NULL THEN 'Нет изображения в БД'
        ELSE 'Есть изображение в БД (' + CAST(DATALENGTH(ProductImage) AS NVARCHAR(20)) + ' байт)'
    END as ImageInDatabaseStatus
FROM ProductWithImage
ORDER BY ProductId;

PRINT 'База данных успешно обновлена для ЛР-14!';
PRINT 'Добавлено поле photo в таблицу Product';
PRINT 'Создана таблица ProductWithImage для изображений в БД';