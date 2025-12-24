-- Обновление базы данных для ЛР-13: Добавление полей для вычисляемых свойств
USE Familia22i1L9;
GO

-- Добавляем поля в таблицу Sale
ALTER TABLE Sale ADD 
    SummaSale DECIMAL(10,2) NULL,      -- Сумма всех продуктов в продаже
    DiscountSale DECIMAL(5,2) NULL,    -- Скидка в процентах (например, 10.50 для 10.5%)
    Resultat DECIMAL(10,2) NULL;       -- Итоговая сумма с учетом скидки

-- Добавляем поле в таблицу SaleDetails  
ALTER TABLE SaleDetails ADD
    SummaProduct DECIMAL(10,2) NULL;   -- Стоимость позиции (количество * цена)

-- Обновляем существующие данные для демонстрации
-- Заполняем SummaProduct = Quantity * UnitPrice
UPDATE SaleDetails 
SET SummaProduct = Quantity * UnitPrice;

-- Заполняем DiscountSale случайными значениями от 0 до 15%
UPDATE Sale SET DiscountSale = 
    CASE SaleId
        WHEN 1 THEN 5.0
        WHEN 2 THEN 10.0  
        WHEN 3 THEN 0.0
        WHEN 4 THEN 7.5
        WHEN 5 THEN 12.0
    END;

-- Вычисляем SummaSale для каждой продажи
UPDATE Sale 
SET SummaSale = (
    SELECT SUM(SummaProduct) 
    FROM SaleDetails 
    WHERE SaleDetails.SaleId = Sale.SaleId
);

-- Вычисляем Resultat = SummaSale - (SummaSale * DiscountSale / 100)
UPDATE Sale 
SET Resultat = SummaSale - (SummaSale * DiscountSale / 100.0);

-- Проверяем результат
SELECT 
    s.SaleId,
    s.CustomerName,
    s.SummaSale,
    s.DiscountSale,
    s.Resultat,
    (SELECT COUNT(*) FROM SaleDetails sd WHERE sd.SaleId = s.SaleId) as ItemsCount
FROM Sale s
ORDER BY s.SaleId;

SELECT 
    sd.SaleDetailId,
    sd.SaleId,
    p.ProductName,
    sd.Quantity,
    sd.UnitPrice,
    sd.SummaProduct
FROM SaleDetails sd
JOIN Product p ON sd.ProductId = p.ProductId
ORDER BY sd.SaleId, sd.SaleDetailId;