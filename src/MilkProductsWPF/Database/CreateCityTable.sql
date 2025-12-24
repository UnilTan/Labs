-- Создание таблицы City
CREATE TABLE City (
    CityId int IDENTITY(1,1) PRIMARY KEY,
    CityName nvarchar(100) NOT NULL,
    Region nvarchar(100) NULL,
    Country nvarchar(100) NULL,
    Population int NOT NULL DEFAULT 0
);

-- Добавление внешнего ключа в таблицу Product
ALTER TABLE Product 
ADD CityId int NULL;

ALTER TABLE Product 
ADD CONSTRAINT FK_Product_City 
FOREIGN KEY (CityId) REFERENCES City(CityId) ON DELETE SET NULL;

-- Вставка тестовых данных в таблицу City
INSERT INTO City (CityName, Region, Country, Population) VALUES
('Москва', 'Московская область', 'Россия', 12500000),
('Санкт-Петербург', 'Ленинградская область', 'Россия', 5400000),
('Новосибирск', 'Новосибирская область', 'Россия', 1600000),
('Екатеринбург', 'Свердловская область', 'Россия', 1500000),
('Казань', 'Республика Татарстан', 'Россия', 1300000),
('Нижний Новгород', 'Нижегородская область', 'Россия', 1250000),
('Челябинск', 'Челябинская область', 'Россия', 1200000),
('Самара', 'Самарская область', 'Россия', 1150000),
('Омск', 'Омская область', 'Россия', 1100000),
('Ростов-на-Дону', 'Ростовская область', 'Россия', 1140000);

-- Обновление существующих продуктов, добавив им города
UPDATE Product SET CityId = 1 WHERE ProductId % 3 = 1;  -- Москва
UPDATE Product SET CityId = 2 WHERE ProductId % 3 = 2;  -- Санкт-Петербург  
UPDATE Product SET CityId = 3 WHERE ProductId % 3 = 0;  -- Новосибирск