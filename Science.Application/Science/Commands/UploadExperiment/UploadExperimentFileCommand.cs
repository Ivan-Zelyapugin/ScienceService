using MediatR;

namespace Science.Application.Science.Commands.UploadExperiment
{
    /// <summary>
    /// Команда для загрузки файла эксперимента.
    /// </summary>
    public class UploadExperimentFileCommand : IRequest<(bool Success, string Message, int ValidRecords)>
    {
        /// <summary>
        /// Имя файла эксперимента.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Поток данных содержимого файла.
        /// </summary>
        public Stream FileContent { get; set; }

        /// <summary>
        /// Размер файла в байтах.
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Имя автора файла.
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// Дата создания файла.
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}
