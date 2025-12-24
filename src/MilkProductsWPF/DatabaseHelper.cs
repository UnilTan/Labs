using System;
using System.Linq;
using System.Data.SqlClient;
using System.Windows;
using MilkProductsWPF.Models;
using Microsoft.EntityFrameworkCore;

namespace MilkProductsWPF
{
    public static class DatabaseHelper
    {
        public static bool CheckAndCreateTables()
        {
            try
            {
                using (var context = new SalesContext())
                {
                    // Проверяем подключение
                    if (!context.Database.CanConnect())
                    {
                        MessageBox.Show("Не удается подключиться к базе данных. Проверьте SQL Server и строку подключения.", 
                            "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }

                    // Создаем базу данных если не существует
                    context.Database.EnsureCreated();

                    // Проверяем, существует ли таблица City
                    try
                    {
                        var cityCount = context.Cities.Count();
                        if (cityCount == 0)
                        {
                            // Создаем города если таблица пуста
                            CreateCities(context);
                        }
                    }
                    catch (Exception)
                    {
                        // Таблица City не существует, создаем её
                        CreateCityTable(context);
                        CreateCities(context);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке базы данных: {ex.Message}", 
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private static void CreateCityTable(SalesContext context)
        {
            try
            {
                // Выполняем SQL команду для создания таблицы City
                context.Database.ExecuteSqlRaw(@"
                    CREATE TABLE City (
                        CityId INT IDENTITY(1,1) PRIMARY KEY,
                        CityName NVARCHAR(100) NOT NULL,
                        Region NVARCHAR(100) NULL,
                        Country NVARCHAR(100) NULL,
                        Population INT NOT NULL DEFAULT 0
                    )");

                // Добавляем поле CityId в таблицу Product если его нет
                try
                {
                    context.Database.ExecuteSqlRaw(@"
                        ALTER TABLE Product ADD CityId INT NULL;
                        ALTER TABLE Product ADD CONSTRAINT FK_Product_City 
                            FOREIGN KEY (CityId) REFERENCES City(CityId) ON DELETE SET NULL;");
                }
                catch
                {
                    // Поле уже существует
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка создания таблицы City: {ex.Message}");
            }
        }

        private static void CreateCities(SalesContext context)
        {
            var cities = new[]
            {
                new City { CityName = "Москва", Region = "Московская область", Country = "Россия", Population = 12500000 },
                new City { CityName = "Санкт-Петербург", Region = "Ленинградская область", Country = "Россия", Population = 5400000 },
                new City { CityName = "Казань", Region = "Республика Татарстан", Country = "Россия", Population = 1300000 },
                new City { CityName = "Новосибирск", Region = "Новосибирская область", Country = "Россия", Population = 1600000 },
                new City { CityName = "Екатеринбург", Region = "Свердловская область", Country = "Россия", Population = 1500000 },
                new City { CityName = "Нижний Новгород", Region = "Нижегородская область", Country = "Россия", Population = 1250000 },
                new City { CityName = "Челябинск", Region = "Челябинская область", Country = "Россия", Population = 1200000 },
                new City { CityName = "Самара", Region = "Самарская область", Country = "Россия", Population = 1150000 },
                new City { CityName = "Омск", Region = "Омская область", Country = "Россия", Population = 1100000 },
                new City { CityName = "Ростов-на-Дону", Region = "Ростовская область", Country = "Россия", Population = 1140000 }
            };

            context.Cities.AddRange(cities);
            context.SaveChanges();

            // Обновляем существующие продукты, назначая им города
            var products = context.Products.ToList();
            for (int i = 0; i < products.Count && i < cities.Length; i++)
            {
                products[i].CityId = cities[i].CityId;
            }
            context.SaveChanges();
        }
    }
}