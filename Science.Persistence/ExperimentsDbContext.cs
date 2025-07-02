using Microsoft.EntityFrameworkCore;
using Science.Application.Interfaces;
using Science.Domain.Entity;
using Science.Persistence.EntityTypeConfigurations;

namespace Science.Persistence
{
    /// <summary>
    /// Контекст базы данных для работы с экспериментами, реализация EF Core и интерфейса IExperimentsDbContext.
    /// </summary>
    public class ExperimentsDbContext : DbContext, IExperimentsDbContext
    {
        /// <summary>
        /// Таблица метаданных файлов.
        /// </summary>
        public DbSet<FileMetadata> Files { get; set; }

        /// <summary>
        /// Таблица значений экспериментов.
        /// </summary>
        public DbSet<ExperimentValue> Values { get; set; }

        /// <summary>
        /// Таблица результатов экспериментов.
        /// </summary>
        public DbSet<ExperimentResult> Results { get; set; }

        /// <summary>
        /// Конструктор контекста базы данных с передачей опций.
        /// </summary>
        /// <param name="options">Опции конфигурации DbContext.</param>
        public ExperimentsDbContext(DbContextOptions<ExperimentsDbContext> options)
            : base(options) { }

        /// <summary>
        /// Настройка модели базы данных с применением конфигураций сущностей.
        /// </summary>
        /// <param name="builder">Построитель модели.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new FileMetadataConfiguration());
            builder.ApplyConfiguration(new ExperimentValueConfiguration());
            builder.ApplyConfiguration(new ExperimentResultConfiguration());
            base.OnModelCreating(builder);
        }
    }
}
