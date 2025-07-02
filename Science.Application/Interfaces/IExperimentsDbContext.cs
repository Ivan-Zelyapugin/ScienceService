using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Science.Domain.Entity;

namespace Science.Application.Interfaces
{
    /// <summary>
    /// Интерфейс контекста базы данных для работы с экспериментами.
    /// </summary>
    public interface IExperimentsDbContext
    {
        /// <summary>
        /// Набор данных для сущностей метаданных файлов.
        /// </summary>
        DbSet<FileMetadata> Files { get; set; }

        /// <summary>
        /// Набор данных для значений экспериментов.
        /// </summary>
        DbSet<ExperimentValue> Values { get; set; }

        /// <summary>
        /// Набор данных для результатов экспериментов.
        /// </summary>
        DbSet<ExperimentResult> Results { get; set; }

        /// <summary>
        /// Асинхронно сохраняет изменения в базе данных.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Количество затронутых записей в базе данных.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Предоставляет доступ к функциональности базы данных.
        /// </summary>
        DatabaseFacade Database { get; }
    }
}
