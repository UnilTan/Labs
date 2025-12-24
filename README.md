# MilkProducts Labs (.NET WinForms/WPF, EF Core, SQL Server)

Учебный набор лабораторных работ (№9–15) по работе с базой данных Familia22i1L9:
- `MilkProductsCatalog` — WinForms CRUD по каталогу молочной продукции (EF Core 8).
- `MilkProductsBinding` — WPF демо привязок и CRUD (EF Core 6).
- `MilkProductsWPF` — WPF CRUD + справочники (EF Core 6).
- `MilkProductsImages` — WPF работа с изображениями (EF Core 8, доп. таблица).

## Требования
- .NET SDK 8.0 (и 6.0 для WPF-проектов).
- SQL Server (LocalDB/Express/полная версия) **или** Docker Desktop.
- PowerShell 7+ для скриптов.

## Быстрый старт
### Вариант 1: локальный SQL Server
1) Проверьте, что SQL Server запущен и доступен.  
2) Убедитесь, что строка подключения задана в `appsettings.Development.json` или переменной `ConnectionStrings__SqlServer`. Пример для LocalDB:  
   `Server=(localdb)\MSSQLLocalDB;Database=Familia22i1L9;Trusted_Connection=True;TrustServerCertificate=true;`
3) Запустите приложения:
   - WinForms: `dotnet run --project src/MilkProductsCatalog/MilkProductsCatalog.csproj`
   - WPF Binding: `dotnet run --project src/MilkProductsBinding/MilkProductsBinding.csproj`
   - WPF CRUD: `dotnet run --project src/MilkProductsWPF/MilkProductsWPF.csproj`
   - WPF Images: `dotnet run --project src/MilkProductsImages/MilkProductsImages.csproj`

### Вариант 2: Docker Compose (SQL Server в контейнере)
1) Создайте `.env` (или обновите уже лежащий) с `SA_PASSWORD` и `SQLSERVER_CONNECTIONSTRING` (пример в `.env.example`).  
2) Поднимите БД: `./scripts/start-docker-db.ps1 -Detach`  
3) Убедитесь, что строка подключения указывает на контейнер (порт 14333 по умолчанию):  
   `Server=localhost,14333;Database=Familia22i1L9;User Id=sa;Password=<ваш пароль>;TrustServerCertificate=true;Encrypt=false;`
4) Запустите приложения локально (см. выше) — они подключатся к контейнерной БД.

## Настройка БД
- Имя базы: `Familia22i1L9`.  
- Конфигурация: `appsettings.json` / `appsettings.Development.json` в каждом проекте (копируются в bin) + переменная окружения `ConnectionStrings__SqlServer`.  
- Инициализация/диагностика: 
  - WinForms `ConnectionTestForm` использует конфигурацию и набор типовых строк, ищет рабочее подключение. 
  - `DatabaseCreator` создаёт БД и таблицы без удаления существующих данных (создаёт, если нет таблиц, и заполняет демо-данными, если пусто).
- Стратегия схемы: для учебных целей используется `EnsureCreated` + безопасный сидер. Для продовой эволюции схемы используйте миграции (`scripts/apply-migrations.ps1` — пропускает, если миграций нет).

## Команды
- Сборка решения: `dotnet build Labs.sln`
- Тесты: `dotnet test Labs.sln`
- Форматирование (при желании): `dotnet format Labs.sln`
- Docker БД: `./scripts/start-docker-db.ps1 -Detach`
- (Опционально) миграции: `./scripts/apply-migrations.ps1 -Project src/MilkProductsCatalog/MilkProductsCatalog.csproj`

## Структура репозитория
- `src/` — проекты (WinForms/WPF + Shared).
- `src/Shared/MilkProducts.Shared` — конфигурация/утилиты (загрузка connection string, DbContext options).
- `docker/` — `docker-compose.yml` с SQL Server и Dockerfile для EF-инструментов.
- `scripts/` — запуск docker-compose, вспомогательный запуск миграций.
- `tests/` — xUnit тесты (пока покрывают конфигурацию).
- `docs/` — развёрнутые инструкции и troubleshooting.

## Troubleshooting
- **Нет подключения к БД**: проверьте `ConnectionStrings__SqlServer`, запущен ли SQL Server/контейнер, доступен ли порт 14333 (для Docker), включён ли TrustServerCertificate.
- **Тайм-аут/зависание UI**: операции БД выполняются синхронно, дождитесь ответа либо убедитесь, что SQL доступен. Для долгих операций — перезапуск с корректной строкой подключения.
- **EF Core ошибки миграций**: миграций нет — сидирование/EnsureCreated. Добавляйте миграции в проект и запускайте `scripts/apply-migrations.ps1`.
- **Binding не обновляет UI**: после `SaveChanges` перезагрузите источники данных (комбо/гриды). Пример — метод перезагрузки в `MilkProductsBinding` и обновление DataGrid в WinForms после операций.

## Для учебы и расширений
- Логика лабораторных по привязкам/CRUD находится в `MilkProductsBinding` (окно `MainWindow` и окна Lab12–Lab15).
- Базовый CRUD + диагностика подключения — `MilkProductsCatalog`.
- Расширенный WPF CRUD + справочники — `MilkProductsWPF` (добавляет таблицу City).
- Работа с изображениями — `MilkProductsImages` (таблица `ProductWithImage`, хранение blob).
- Новые фичи добавляйте через сервисы/контексты, не дублируя строку подключения: используйте `MilkProducts.Shared.Configuration.ConnectionStringProvider`.
