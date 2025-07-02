using MediatR;
using Microsoft.EntityFrameworkCore;
using Science.Application.Interfaces;
using Science.Domain.Entity;
using EFCore.BulkExtensions;

namespace Science.Application.Science.Commands.UploadExperiment
{
    /// <summary>
    /// Обработчик команды загрузки файла эксперимента.
    /// </summary>
    public class UploadExperimentFileCommandHandler : IRequestHandler<UploadExperimentFileCommand, (bool Success, string Message, int ValidRecords)>
    {
        private readonly IExperimentsDbContext _dbContext;

        /// <summary>
        /// Инициализирует новый экземпляр обработчика команды с указанным контекстом базы данных.
        /// </summary>
        /// <param name="dbContext">Контекст базы данных для работы с экспериментами.</param>
        public UploadExperimentFileCommandHandler(IExperimentsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Обрабатывает команду загрузки файла эксперимента, парсит данные, сохраняет метаданные и результаты.
        /// </summary>
        /// <param name="request">Команда с данными файла эксперимента.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Кортеж, содержащий результат выполнения (успех/неуспех), сообщение и количество валидных записей.</returns>
        public async Task<(bool Success, string Message, int ValidRecords)> Handle(UploadExperimentFileCommand request, CancellationToken cancellationToken)
        {
            if (request.FileContent == null || request.FileSize == 0)
                return (false, "Файл не загружен", 0);

            var fileName = Path.GetFileName(request.FileName);
            if (!fileName.EndsWith(".csv"))
                return (false, "Недопустимый формат файла. Принимаются только CSV-файлы", 0);

            var validRecords = await ParseCsvFileAsync(request.FileContent);
            if (validRecords.Count == 0)
                return (false, "В файле не найдено валидных записей", 0);

            try
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

                var fileMetadata = await _dbContext.Files
                    .FirstOrDefaultAsync(f => f.FileName == fileName, cancellationToken);

                if (fileMetadata != null)
                {
                    _dbContext.Files.Remove(fileMetadata);
                    await _dbContext.SaveChangesAsync(cancellationToken);        
                }

                fileMetadata = new FileMetadata
                {
                    Id = Guid.NewGuid(),
                    FileName = fileName,
                    AuthorName = request.AuthorName,
                    FileSize = request.FileSize,
                    FileType = "csv",
                    CreatedDate = request.CreatedDate
                };

                _dbContext.Files.Add(fileMetadata);
                await _dbContext.SaveChangesAsync(cancellationToken);

                fileMetadata.AuthorName = request.AuthorName;
                fileMetadata.FileSize = request.FileSize;
                fileMetadata.FileType = "csv";
                fileMetadata.CreatedDate = request.CreatedDate;

                const int deleteChunkSize = 1000;

                var valuesToDelete = await _dbContext.Values
                    .Where(v => v.FileId == fileMetadata.Id)
                    .ToListAsync(cancellationToken);

                for (int i = 0; i < valuesToDelete.Count; i += deleteChunkSize)
                {
                    var chunk = valuesToDelete.Skip(i).Take(deleteChunkSize).ToList();
                    _dbContext.Values.RemoveRange(chunk);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                }

                var resultsToDelete = await _dbContext.Results
                    .Where(r => r.FileId == fileMetadata.Id)
                    .ToListAsync(cancellationToken);

                for (int i = 0; i < resultsToDelete.Count; i += deleteChunkSize)
                {
                    var chunk = resultsToDelete.Skip(i).Take(deleteChunkSize).ToList();
                    _dbContext.Results.RemoveRange(chunk);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                }

                _dbContext.Files.Update(fileMetadata);
                await _dbContext.SaveChangesAsync(cancellationToken);

                var values = validRecords.Select(r => new ExperimentValue
                {
                    Id = Guid.NewGuid(),
                    FileId = fileMetadata.Id,
                    ExperimentDateTime = r.DateTime,
                    DurationSeconds = r.Duration,
                    Indicator = r.Indicator
                }).ToList();

                await (_dbContext as DbContext)!.BulkInsertAsync(values, new BulkConfig
                {
                    BatchSize = 1000,
                    PreserveInsertOrder = false,
                    SetOutputIdentity = false
                }, cancellationToken: cancellationToken);

                var result = new ExperimentResult
                {
                    Id = Guid.NewGuid(),
                    FileId = fileMetadata.Id,
                    FirstExperimentStart = validRecords.Min(r => r.DateTime),
                    LastExperimentStart = validRecords.Max(r => r.DateTime),
                    MaxExperimentTime = validRecords.Max(r => r.Duration),
                    MinExperimentTime = validRecords.Min(r => r.Duration),
                    AvgExperimentTime = validRecords.Average(r => r.Duration),
                    AvgIndicator = validRecords.Average(r => r.Indicator),
                    MedianIndicator = CalculateMedian(validRecords.Select(r => r.Indicator).ToList()),
                    MaxIndicator = (float)validRecords.Max(r => r.Indicator),
                    MinIndicator = (float)validRecords.Min(r => r.Indicator),
                    ExperimentCount = validRecords.Count
                };

                _dbContext.Results.Add(result);
                await _dbContext.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return (true, "Файл успешно обработан", validRecords.Count);
            }
            catch (Exception ex)
            {
                return (false, $"Ошибка при обработке файла: {ex.Message}", 0);
            }
        }

        /// <summary>
        /// Парсит CSV-файл и извлекает валидные записи экспериментов.
        /// </summary>
        /// <param name="fileContent">Поток данных CSV-файла.</param>
        /// <returns>Список кортежей с данными эксперимента (дата, длительность, показатель).</returns>
        private async Task<List<(DateTime DateTime, int Duration, double Indicator)>> ParseCsvFileAsync(Stream fileContent)
        {
            var validRecords = new List<(DateTime DateTime, int Duration, double Indicator)>();
            var uniqueRecords = new HashSet<(DateTime, int, double)>();

            using var reader = new StreamReader(fileContent);
            int lineCount = 0;

            while (!reader.EndOfStream && lineCount < 10000)
            {
                var line = await reader.ReadLineAsync();
                lineCount++;
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(';');
                if (parts.Length != 3) continue;

                if (!DateTime.TryParseExact(parts[0], "yyyy-MM-dd_HH-mm-ss", null, System.Globalization.DateTimeStyles.None, out var dateTime) ||
                    !int.TryParse(parts[1], out var duration) ||
                    !double.TryParse(parts[2].Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var indicator))
                {
                    continue;
                }

                if (dateTime < new DateTime(2000, 1, 1) || dateTime > DateTime.Now ||
                    duration < 0 || indicator < 0)
                {
                    continue;
                }

                var record = (dateTime, duration, indicator);
                if (uniqueRecords.Add(record))
                {
                    validRecords.Add(record);
                }
            }

            return validRecords;
        }

        /// <summary>
        /// Вычисляет медиану для списка значений показателей.
        /// </summary>
        /// <param name="values">Список значений показателей.</param>
        /// <returns>Медианное значение или 0, если список пуст.</returns>
        private static double CalculateMedian(List<double> values)
        {
            if (!values.Any()) return 0;
            var sorted = values.OrderBy(x => x).ToList();
            int count = sorted.Count;
            return count % 2 == 0
                ? (sorted[count / 2 - 1] + sorted[count / 2]) / 2
                : sorted[count / 2];
        }
    }
}
