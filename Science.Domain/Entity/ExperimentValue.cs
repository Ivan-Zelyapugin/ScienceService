namespace Science.Domain.Entity
{
    /// <summary>
    /// Модель значения эксперимента.
    /// </summary>
    public class ExperimentValue
    {
        /// <summary>
        /// Уникальный идентификатор значения эксперимента.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Дата и время проведения эксперимента.
        /// </summary>
        public DateTime ExperimentDateTime { get; set; }

        /// <summary>
        /// Длительность эксперимента в секундах.
        /// </summary>
        public int DurationSeconds { get; set; }

        /// <summary>
        /// Показатель эксперимента.
        /// </summary>
        public double Indicator { get; set; }

        /// <summary>
        /// Идентификатор связанного файла
        /// </summary>
        public Guid FileId { get; set; }

        /// <summary>
        /// Навигационное свойство.
        /// </summary>
        public FileMetadata FileMetadata { get; set; }
    }
}
