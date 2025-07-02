namespace Science.Domain.Entity
{
    /// <summary>
    /// Модель метаданных файла.
    /// </summary>
    public class FileMetadata
    {
        /// <summary>
        /// Уникальный идентификатор метаданных файла.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Имя автора файла.
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// Имя файла.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Размер файла в байтах.
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Тип файла.
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// Дата создания файла.
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}
