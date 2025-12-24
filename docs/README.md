# Руководство по лабораторным (MilkProducts)

## Описание проектов
- **MilkProductsCatalog** (WinForms, .NET 8, EF Core 8) — CRUD по товарам/продажам + диагностика подключения.
- **MilkProductsBinding** (WPF, .NET 6, EF Core 6) — лабораторные по Binding (Lab12–Lab15), работа с контекстом данных и CRUD.
- **MilkProductsWPF** (WPF, .NET 6, EF Core 6) — отдельное CRUD-приложение с дополнительным справочником City.
- **MilkProductsImages** (WPF, .NET 9, EF Core 8) — хранение изображений (таблица ProductWithImage).
- **Shared** — общее: загрузка конфигурации (`ConnectionStringProvider`), фабрика DbContext опций.

## Требования
- .NET SDK: 8.0 (и 6.0 для WPF проектов), MSBuild 17+.
- SQL Server: LocalDB/Express/полная версия **или** Docker Desktop.
- Docker: для `docker-compose` с SQL Server 2022.
- Инструменты: PowerShell 7+, `dotnet-ef` (ставится автоматически из скрипта).

## Конфигурация
- Главный ключ: `ConnectionStrings:SqlServer` (или env `ConnectionStrings__SqlServer`).
- Файлы конфигурации в каждом проекте: `appsettings.json` (docker/remote), `appsettings.Development.json` (LocalDB). Они копируются в bin.
- Переменные окружения имеют приоритет. Пример для Docker:
  ```
  ConnectionStrings__SqlServer=Server=localhost,14333;Database=Familia22i1L9;User Id=sa;Password=<SA_PASSWORD>;TrustServerCertificate=true;Encrypt=false;
  ```
- `.env` / `.env.example` — пароль SA и строка подключения для docker-compose.

## Запуск
### Локально (LocalDB/Express)
1. Убедитесь, что SQL Server доступен (`(localdb)\MSSQLLocalDB` или `.\\SQLEXPRESS`).
2. Проверьте/обновите `appsettings.Development.json` или переменную `ConnectionStrings__SqlServer`.
3. Запуск:
   - `dotnet run --project src/MilkProductsCatalog/MilkProductsCatalog.csproj`
   - `dotnet run --project src/MilkProductsBinding/MilkProductsBinding.csproj`
   - `dotnet run --project src/MilkProductsWPF/MilkProductsWPF.csproj`
   - `dotnet run --project src/MilkProductsImages/MilkProductsImages.csproj`

### Через Docker (SQL Server в контейнере)
1. Заполните `.env` (пароль SA + строка подключения на порт 14333).
2. `./scripts/start-docker-db.ps1 -Detach`
3. Проверьте здоровье контейнера: `docker ps` (статус `healthy`).
4. Убедитесь, что `ConnectionStrings__SqlServer` указывает на контейнер (пример выше).
5. Запускайте приложения локально (см. шаги для локального запуска).

## Работа с БД и схемой
- Стратегия по умолчанию: `EnsureCreated` + безопасное сидирование (без удаления таблиц). Класс `DatabaseCreator` создаёт БД и заполняет демо-данными при отсутствии данных.
- Для эволюции схемы используйте миграции EF Core:
  - Создать миграцию: `dotnet ef migrations add Init --project src/MilkProductsCatalog/MilkProductsCatalog.csproj`
  - Применить (если миграции есть): `./scripts/apply-migrations.ps1 -Project src/MilkProductsCatalog/MilkProductsCatalog.csproj`
  - Скрипт пропускает выполнение, если папки `Migrations` нет.

## Тесты и качество
- Юнит-тесты: `dotnet test Labs.sln` (покрытие конфигурации, можно расширять).
- Форматирование: `dotnet format Labs.sln` (настраивается через `.editorconfig`).
- Анализаторы .NET включаются автоматически с SDK (warnings доступны в build output).

## Структура репозитория
```
src/
  MilkProductsCatalog/      // WinForms + EF Core 8
  MilkProductsBinding/      // WPF Binding демо + EF Core 6
  MilkProductsWPF/          // WPF CRUD + EF Core 6
  MilkProductsImages/       // WPF Images + EF Core 8
  Shared/MilkProducts.Shared// Общие утилиты конфигурации/DbContext
docker/
  docker-compose.yml        // SQL Server 2022, порт 14333
  Dockerfile                // Утилитарный образ с dotnet-ef
scripts/
  start-docker-db.ps1       // Поднять SQL Server в docker
  apply-migrations.ps1      // Применить EF миграции, если есть
tests/
  MilkProducts.Shared.Tests // xUnit, проверка конфигурации
docs/
  README.md                 // Этот файл
  archive-samples/          // Архив старых образцов UI/SQL инструкций
```

## Troubleshooting
- **Не удаётся подключиться к SQL Server**: проверьте `ConnectionStrings__SqlServer`, запущен ли SQL/контейнер, совпадает ли порт (14333 в Docker), разрешён ли TrustServerCertificate.
- **Падение при CRUD**: проверьте обязательные поля (ProductName/Category/Price/Quantity), убедитесь, что БД создана (см. `DatabaseCreator` или `EnsureCreated`).
- **Binding не обновляет UI**: после `SaveChanges` обновите источники данных (перечитайте из БД) — в WinForms `LoadProducts`, в WPF обновите `ItemsSource`.
- **Нет миграций**: скрипт `apply-migrations` пропустит запуск; для демо используется `EnsureCreated`. Добавьте миграции перед продовым развёртыванием.

## Подсказки для студентов
- Привязки WPF: смотрите `MainWindow.xaml`/`.cs` в `MilkProductsBinding` и окна Lab12–Lab15. Экспериментируйте с DataContext и TwoWay Binding.
- CRUD + диагностика: `MilkProductsCatalog` формы `Form1`, `ConnectionTestForm`, `DatabaseCreator`.
- Расширения: добавляйте сервисный слой и валидацию перед `SaveChanges`, используйте `ConnectionStringProvider` (не хардкодьте строки).
- Подключение к Docker-БД: достаточно обновить `ConnectionStrings__SqlServer`, пересобирать проекты не нужно.
