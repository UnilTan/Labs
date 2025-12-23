# Лабораторная работа 11 - Привязки данных Binding

## Тема: Привязки данных Binding. Работа с БД.

### Цель: изучить привязки данных Binding с базой данных молочных продуктов

## Структура проекта

### Модели данных:
- **Product** - главная таблица продуктов
- **Sale** - таблица продаж
- **SaleDetails** - подчиненная таблица деталей продаж

### Реализованные привязки:

## 1. Простая привязка данных к TextBox
```xaml
<StackPanel x:Name="stProduct">
    <TextBox Text="{Binding idProduct}"/>
    <TextBox Text="{Binding nameProduct, Mode=TwoWay}"/>
    <TextBox Text="{Binding priceProduct, Mode=TwoWay}"/>
</StackPanel>
```

```csharp
// Установка контекста данных
var recProduct = db.Product.First();
stProduct.DataContext = recProduct;
```

## 2. Сложная привязка данных к TextBox
```xaml
<StackPanel x:Name="stDetail">
    <TextBox Text="{Binding IdProductDetailSale}"/>
    <TextBox Text="{Binding Product.nameProduct}"/>
    <TextBox Text="{Binding Product.priceProduct}"/>
    <TextBox Text="{Binding QuantityProduct, Mode=TwoWay}"/>
</StackPanel>
```

```csharp
// Привязка с включением связанных данных
var recDetailProduct = db.DetailSale.Include(d => d.Product).First();
stDetail.DataContext = recDetailProduct;
```

## 3. Простая привязка ComboBox
```xaml
<ComboBox x:Name="cmbProduct" 
          DisplayMemberPath="nameProduct" 
          SelectedValuePath="idProduct"/>
```

```csharp
// Установка источника данных
cmbProduct.ItemsSource = db.Product.ToList();
// Получение выбранного значения
int articul = Convert.ToInt32(cmbProduct.SelectedValue);
```

## 4. Сложная привязка ComboBox
```xaml
<ComboBox x:Name="cmbProductDetail" 
          DisplayMemberPath="nameProduct" 
          SelectedValuePath="idProduct"
          SelectedValue="{Binding IdProductDetailSale, Mode=TwoWay}"/>
```

```csharp
// ComboBox автоматически синхронизируется с контекстом данных
cmbProductDetail.ItemsSource = db.Product.ToList();
```

## 5. Доступ к связанным полям
```csharp
var recProductPriceFirst = db.DetailSale.Include(d => d.Product).First();
string nameFirst = recProductPriceFirst.Product.nameProduct;
decimal priceFirst = recProductPriceFirst.Product.priceProduct;
```

## 6. Изменение записи с Mode=TwoWay
```csharp
// Изменения автоматически применяются к объекту
db.SaveChanges(); // Сохранение в базу данных
```

## 7. Добавление новых записей

### Добавление Product:
```csharp
Product productNew = new Product();
stProduct.DataContext = productNew;
// После ввода данных:
db.Product.Add(productNew);
db.SaveChanges();
```

### Добавление SaleDetails:
```csharp
SaleDetails detailSaleNew = new SaleDetails();
stDetail.DataContext = detailSaleNew;
// После ввода данных:
db.DetailSale.Add(detailSaleNew);
db.SaveChanges();
```

## Функциональность приложения

### Кнопки управления:
1. **Загрузить первый Product** - демонстрация простой привязки
2. **Загрузить первый SaleDetail** - демонстрация сложной привязки
3. **Сохранить изменения** - сохранение в БД с Mode=TwoWay
4. **Показать связанные поля** - доступ к связанным данным
5. **Новый Product** - создание нового объекта для добавления
6. **Новый SaleDetail** - создание нового объекта детали продажи
7. **Добавить в БД** - сохранение новых записей

### Особенности:
- Все привязки работают в реальном времени
- Поддержка двусторонней привязки (TwoWay)
- Автоматическая синхронизация ComboBox с данными
- Информативные сообщения о выполненных операциях
- Обработка ошибок подключения к БД

## Запуск проекта

1. Убедитесь, что SQL Server запущен
2. Выполните скрипт `Database/CreateTestData.sql`
3. Запустите приложение
4. Используйте кнопки для демонстрации различных типов привязок

## Технологии
- WPF (.NET 6.0)
- Entity Framework Core 6.0.25
- SQL Server Express
- Data Binding (простые и сложные привязки)
- Mode=TwoWay для двусторонней привязки