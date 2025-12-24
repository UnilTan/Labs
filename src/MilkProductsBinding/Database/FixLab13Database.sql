-- Исправление БД для ЛР-13: Добавление недостающих столбцов
USE Familia22i1L9;
GO

-- Добавляем столбцы в таблицу Sale, если их нет
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Sale' AND COLUMN_NAME = 'SummaSale')
BEGIN
    ALTER TABLE Sale ADD SummaSale DECIMAL(10,2) NULL
    PRINT 'Добавлен столбец SummaSale в таблицу Sale'
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Sale' AND COLUMN_NAME = 'DiscountSale')
BEGIN
    ALTER TABLE Sale ADD DiscountSale DECIMAL(5,2) NULL
    PRINT 'Добавлен столбец DiscountSale в таблицу Sale'
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Sale' AND COLUMN_NAME = 'Resultat')
BEGIN
    ALTER TABLE Sale ADD Resultat DECIMAL(10,2) NULL
    PRINT 'Добавлен столбец Resultat в таблицу Sale'
END

-- Добавляем столбец в таблицу SaleDetails, если его нет
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SaleDetails' AND COLUMN_NAME = 'SummaProduct')
BEGIN
    ALTER TABLE SaleDetails ADD SummaProduct DECIMAL(10,2) NULL
    PRINT 'Добавлен столбец SummaProduct в таблицу SaleDetails'
END

-- Заполняем тестовые данные для скидок
UPDATE Sale 
SET DiscountSale = 
    CASE SaleId % 5
        WHEN 0 THEN 0.0
        WHEN 1 THEN 5.0
        WHEN 2 THEN 10.0
        WHEN 3 THEN 7.5
        WHEN 4 THEN 12.0
    END
WHERE DiscountSale IS NULL;

PRINT 'Заполнены тестовые скидки'

-- Проверяем результат
SELECT 'Структура таблицы Sale:' as Info
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'Sale'
ORDER BY ORDINAL_POSITION

SELECT 'Структура таблицы SaleDetails:' as Info
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'SaleDetails'
ORDER BY ORDINAL_POSITION

PRINT 'Обновление БД для ЛР-13 завершено успешно!'