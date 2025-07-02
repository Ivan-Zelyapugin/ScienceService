using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Science.Domain.Entity;
using Science.Persistence;

namespace Science.Tests.Common
{
    /// <summary>
    /// Создаёт и инициализирует in-memory SQLite контекст для юнит-тестов.
    /// </summary>
    public class ExperimentsContextFactory : IDisposable
    {
        private readonly SqliteConnection _connection;

        /// <summary>
        /// Контекст EF Core с тестовыми данными.
        /// </summary>
        public ExperimentsDbContext Context { get; }

        /// <summary>
        /// Конструктор инициализирует in-memory базу и добавляет тестовые данные.
        /// </summary>
        public ExperimentsContextFactory()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<ExperimentsDbContext>()
                .UseSqlite(_connection)
                .EnableSensitiveDataLogging()
                .Options;

            Context = new ExperimentsDbContext(options);
            Context.Database.EnsureCreated();

            SeedData();
        }

        /// <summary>
        /// Добавляет тестовые сущности в контекст: файлы, значения и результаты экспериментов.
        /// </summary>
        private void SeedData()
        {
            Guid fileId_1 = Guid.NewGuid();
            Guid fileId_2 = Guid.NewGuid();

            FileMetadata testData_1 = new FileMetadata
            {
                Id = fileId_1,
                AuthorName = "Тестовый Автор 1",
                FileName = "test_1.csv",
                FileSize = 31827,
                FileType = "csv",
                CreatedDate = DateTime.Parse("2025-06-29 13:13:14")
            };

            FileMetadata testData_2 = new FileMetadata
            {
                Id = fileId_2,
                AuthorName = "Тестовый Автор 2",
                FileName = "test_2.csv",
                FileSize = 31827,
                FileType = "csv",
                CreatedDate = DateTime.Parse("2025-06-29 13:13:14")
            };

            // Файлы
            Context.Files.AddRange(
                testData_1,
                testData_2
            );

            Context.SaveChanges();

            // Значения
            Context.Values.AddRange(
                new ExperimentValue
                {
                    Id = Guid.NewGuid(),
                    ExperimentDateTime = DateTime.Parse("2025-06-28 00:00:30"),
                    DurationSeconds = 1170,
                    Indicator = 74.14,
                    FileId = fileId_1,
                    FileMetadata = testData_1
                },
                new ExperimentValue
                {
                    Id = Guid.NewGuid(),
                    ExperimentDateTime = DateTime.Parse("2025-06-28 00:00:40"),
                    DurationSeconds = 1339,
                    Indicator = 70.7,
                    FileId = fileId_1,
                    FileMetadata = testData_1
                },
                new ExperimentValue
                {
                    Id = Guid.NewGuid(),
                    ExperimentDateTime = DateTime.Parse("2025-06-28 00:00:50"),
                    DurationSeconds = 862,
                    Indicator = 88.64,
                    FileId = fileId_2,
                    FileMetadata = testData_2
                },
                new ExperimentValue
                {
                    Id = Guid.NewGuid(),
                    ExperimentDateTime = DateTime.Parse("2025-06-28 00:00:00"),
                    DurationSeconds = 1437,
                    Indicator = 18.58,
                    FileId = fileId_2,
                    FileMetadata = testData_2
                }
            );

            // Результат
            Context.Results.AddRange(
                new ExperimentResult
                {
                    Id = Guid.NewGuid(),
                    FirstExperimentStart = DateTime.Parse("2025-06-28 00:00:30"),
                    LastExperimentStart = DateTime.Parse("2025-06-29 03:46:30"),
                    MaxExperimentTime = 3600,
                    MinExperimentTime = 60,
                    AvgExperimentTime = 1827.21239669421,
                    AvgIndicator = 57.431106257379,
                    MedianIndicator = 57.745,
                    MaxIndicator = 100,
                    MinIndicator = 0,
                    ExperimentCount = 2,
                    FileId = fileId_1
                },
                new ExperimentResult
                {
                    Id = Guid.NewGuid(),
                    FirstExperimentStart = DateTime.Parse("2025-06-28 00:00:00"),
                    LastExperimentStart = DateTime.Parse("2025-06-29 03:46:20"),
                    MaxExperimentTime = 3500,
                    MinExperimentTime = 61,
                    AvgExperimentTime = 1808.21239669421,
                    AvgIndicator = 56.431106257379,
                    MedianIndicator = 55.745,
                    MaxIndicator = 80,
                    MinIndicator = 2,
                    ExperimentCount = 2,
                    FileId = fileId_2
                }
            );

            Context.SaveChanges();
        }

        /// <summary>
        /// Освобождает ресурсы, удаляет базу данных и закрывает подключение.
        /// </summary>
        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
            _connection.Close();
            _connection.Dispose();
        }
    }
}
