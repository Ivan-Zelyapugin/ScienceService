using Microsoft.EntityFrameworkCore;
using Science.Application.Science.Commands.UploadExperiment;
using Science.Tests.Common;
using System.Globalization;
using System.Text;

namespace Science.Tests.Science.Commands
{
    /// <summary>
    /// Тесты для обработчика команды UploadExperimentFileCommandHandler.
    /// Проверяется корректность загрузки файлов с экспериментами.
    /// </summary>
    public class UploadExperimentFileCommandHandlerTests : TestCommandBase
    {
        /// <summary>
        /// Проверяет успешную загрузку корректного CSV-файла.
        /// </summary>
        [Fact]
        public async Task UploadExperimentFileCommandHandler_Success()
        {
            // Arrange
            var handler = new UploadExperimentFileCommandHandler(Context);
            var fileName = "test_success.csv";
            var csvData = GenerateCsvContent("valid");
            var fileContent = new MemoryStream(Encoding.UTF8.GetBytes(csvData));
            var fileSize = 84763;
            var authorName = "Тестовый Автор";
            var createdDate = DateTime.Parse("2025-06-29 13:13:14");

            // Act
            var uploadInfo = await handler.Handle(new UploadExperimentFileCommand
            {
                FileName = fileName,
                FileContent = fileContent,
                FileSize = fileSize,
                AuthorName = authorName,
                CreatedDate = createdDate

            }, CancellationToken.None);

            // Assert
            var file = await Context.Files.SingleOrDefaultAsync(f => f.FileName == fileName);

            Assert.NotNull(file);
            Assert.Equal(authorName, file.AuthorName);
            Assert.True(uploadInfo.Success);
            Assert.True(uploadInfo.ValidRecords > 0);
        }

        /// <summary>
        /// Проверяет, что невалидные строки в CSV-файле пропускаются.
        /// </summary>
        [Fact]
        public async Task UploadExperimentFileCommandHandler_SkipsInvalidRecords()
        {
            // Arrange
            var handler = new UploadExperimentFileCommandHandler(Context);
            var fileName = "test_mixed.csv";
            var csvData = GenerateCsvContent("mixed");
            var fileContent = new MemoryStream(Encoding.UTF8.GetBytes(csvData));
            var fileSize = 84763;
            var authorName = "Тестовый Автор";
            var createdDate = DateTime.Parse("2025-06-29 13:13:14");

            // Act
            var uploadInfo = await handler.Handle(new UploadExperimentFileCommand
            {
                FileName = fileName,
                FileContent = fileContent,
                FileSize = fileSize,
                AuthorName = authorName,
                CreatedDate = createdDate

            }, CancellationToken.None);

            // Assert
            var file = await Context.Files.SingleOrDefaultAsync(f => f.FileName == fileName);

            Assert.NotNull(file);
            Assert.Equal(authorName, file.AuthorName);
            Assert.True(uploadInfo.ValidRecords > 0);
        }

        /// <summary>
        /// Проверяет, что при отсутствии валидных записей возвращается ошибка.
        /// </summary>
        [Fact]
        public async Task UploadExperimentFileCommandHandler_FailsWithOnlyInvalidRecords()
        {
            // Arrange
            var handler = new UploadExperimentFileCommandHandler(Context);
            var fileName = "test_invalid.csv";
            var csvData = GenerateCsvContent("invalid");
            var fileContent = new MemoryStream(Encoding.UTF8.GetBytes(csvData));
            var fileSize = 84763;
            var authorName = "Тестовый Автор";
            var createdDate = DateTime.Parse("2025-06-29 13:13:14");

            // Act
            var uploadInfo = await handler.Handle(new UploadExperimentFileCommand
            {
                FileName = fileName,
                FileContent = fileContent,
                FileSize = fileSize,
                AuthorName = authorName,
                CreatedDate = createdDate

            }, CancellationToken.None);

            // Assert
            Assert.False(uploadInfo.Success);
            Assert.Equal("В файле не найдено валидных записей", uploadInfo.Message);
            Assert.Equal(0, uploadInfo.ValidRecords);
        }

        /// <summary>
        /// Проверяет, что при загрузке пустого файла возвращается ошибка.
        /// </summary>
        [Fact]
        public async Task UploadExperimentFileCommandHandler_FailsWithEmptyFile()
        {
            // Arrange
            var handler = new UploadExperimentFileCommandHandler(Context);
            var fileName = "test_empty.csv";
            var csvData = "";
            var fileContent = new MemoryStream(Encoding.UTF8.GetBytes(csvData));
            var fileSize = 0;
            var authorName = "Тестовый Автор";
            var createdDate = DateTime.Parse("2025-06-29 13:13:14");

            // Act
            var uploadInfo = await handler.Handle(new UploadExperimentFileCommand
            {
                FileName = fileName,
                FileContent = fileContent,
                FileSize = fileSize,
                AuthorName = authorName,
                CreatedDate = createdDate

            }, CancellationToken.None);

            // Assert
            Assert.False(uploadInfo.Success);
            Assert.Equal("Файл не загружен", uploadInfo.Message);
            Assert.Equal(0, uploadInfo.ValidRecords);
        }

        /// <summary>
        /// Проверяет, что при загрузке файла с неподдерживаемым расширением возвращается ошибка.
        /// </summary>
        [Fact]
        public async Task UploadExperimentFileCommandHandler_FailsWithNonCsvExtension()
        {
            // Arrange
            var handler = new UploadExperimentFileCommandHandler(Context);
            var fileName = "test_invalid_format.pdf";
            var csvData = GenerateCsvContent("mixed");
            var fileContent = new MemoryStream(Encoding.UTF8.GetBytes(csvData));
            var fileSize = 84763;
            var authorName = "Тестовый Автор";
            var createdDate = DateTime.Parse("2025-06-29 13:13:14");

            // Act
            var uploadInfo = await handler.Handle(new UploadExperimentFileCommand
            {
                FileName = fileName,
                FileContent = fileContent,
                FileSize = fileSize,
                AuthorName = authorName,
                CreatedDate = createdDate

            }, CancellationToken.None);

            // Assert
            Assert.False(uploadInfo.Success);
            Assert.Equal("Недопустимый формат файла. Принимаются только CSV-файлы", uploadInfo.Message);
            Assert.Equal(0, uploadInfo.ValidRecords);
        }

        /// <summary>
        /// Генерирует CSV-строки с указанным режимом: valid, invalid, mixed.
        /// </summary>
        private string GenerateCsvContent(string mode, int rowCount = 10)
        {
            var sb = new StringBuilder();
            var random = new Random();

            for (int i = 0; i < rowCount; i++)
            {
                string dateStr;
                int duration = random.Next(60, 3601);
                double indicator = Math.Round(random.NextDouble() * 100, 2);

                switch (mode.ToLower())
                {
                    case "valid":
                        dateStr = DateTime.Now.AddMinutes(-i).ToString("yyyy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture);
                        sb.AppendLine($"{dateStr};{duration};{indicator}");
                        break;
                    case "invalid":
                        sb.AppendLine(i % 2 == 0
                            ? $"INVALID_DATE;{duration};{indicator}"
                            : $"{DateTime.Now.AddMinutes(-i):yyyy/MM/dd HH:mm:ss},{duration},{indicator}");
                        break;
                    case "mixed":
                        sb.AppendLine(i % 2 == 0
                            ? $"{DateTime.Now.AddMinutes(-i):yyyy-MM-dd_HH-mm-ss};{duration};{indicator}"
                            : $"WRONG_DATE;{duration},WRONG");
                        break;
                    default:
                        throw new ArgumentException("Mode must be one of: valid, invalid, mixed");
                }
            }

            return sb.ToString();
        }
    }
}
