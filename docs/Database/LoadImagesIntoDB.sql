-- Скрипт для массовой загрузки изображений в БД (ЛР-14 Часть 2)
-- ВНИМАНИЕ: Этот скрипт демонстрирует принцип, но реальная загрузка 
-- изображений должна выполняться через приложение!

USE Familia22i1L9;
GO

-- Проверяем существование таблицы ProductWithImage
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ProductWithImage')
BEGIN
    PRINT 'Таблица ProductWithImage не найдена. Сначала выполните UpdateDatabaseForLab14.sql';
    RETURN;
END

-- Обновляем записи в ProductWithImage, добавляя информацию о файлах изображений
-- (сами изображения будут загружены через WPF приложение)

UPDATE ProductWithImage SET 
    ImageFileName = 
        CASE ProductId
            WHEN 1 THEN 'moloko.png'
            WHEN 2 THEN 'kefir.png'
            WHEN 3 THEN 'tvorog.png'
            WHEN 4 THEN NULL  -- Без изображения
            WHEN 5 THEN 'smetana.png'
            WHEN 6 THEN 'ryajenka.png'
            ELSE 
                -- Для остальных продуктов случайно выбираем изображение
                CASE (ProductId % 5)
                    WHEN 0 THEN 'moloko.png'
                    WHEN 1 THEN 'kefir.png'
                    WHEN 2 THEN 'tvorog.png'
                    WHEN 3 THEN 'smetana.png'
                    WHEN 4 THEN 'ryajenka.png'
                END
        END,
    ImageContentType = 
        CASE 
            WHEN ImageFileName LIKE '%.png' THEN 'image/png'
            WHEN ImageFileName LIKE '%.jpg' THEN 'image/jpeg'
            ELSE NULL
        END;

-- Показываем результат
SELECT 
    ProductId,
    ProductName,
    Price,
    ImageFileName,
    ImageContentType,
    CASE 
        WHEN ProductImage IS NULL THEN 'Изображение нужно загрузить через приложение'
        ELSE 'Изображение загружено (' + CAST(DATALENGTH(ProductImage) AS NVARCHAR(20)) + ' байт)'
    END as ImageStatus
FROM ProductWithImage
ORDER BY ProductId;

PRINT '';
PRINT '=== ИНСТРУКЦИЯ ===';
PRINT 'Поля ImageFileName заполнены.';
PRINT 'Теперь запустите проект MilkProductsImages и используйте кнопку';
PRINT '"Загрузить все изображения из папки" для загрузки файлов в БД.';
PRINT '';
PRINT 'Или загружайте изображения по одному через кнопку "Загрузить изображение".';