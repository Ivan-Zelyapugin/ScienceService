namespace Science.Domain.Entity
{
    /// <summary>
    /// Модель результата эксперимента.
    /// </summary>
    public class ExperimentResult
    {
        /// <summary>
        /// Уникальный идентификатор результата эксперимента.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Время начала первого эксперимента.
        /// </summary>
        public DateTime FirstExperimentStart { get; set; }

        /// <summary>
        /// Время начала последнего эксперимента.
        /// </summary>
        public DateTime LastExperimentStart { get; set; }

        /// <summary>
        /// Максимальная длительность эксперимента.
        /// </summary>
        public int MaxExperimentTime { get; set; }

        /// <summary>
        /// Минимальная длительность эксперимента
        /// </summary>
        public int MinExperimentTime { get; set; }

        /// <summary>
        /// Средняя длительность экспериментов
        /// </summary>
        public double AvgExperimentTime { get; set; }

        /// <summary>
        /// Среднее значение показателя эксперимента.
        /// </summary>
        public double AvgIndicator { get; set; }

        /// <summary>
        /// Медианное значение показателя эксперимента.
        /// </summary>
        public double MedianIndicator { get; set; }

        /// <summary>
        /// Максимальное значение показателя эксперимента.
        /// </summary>
        public float MaxIndicator { get; set; }

        /// <summary>
        /// Минимальное значение показателя эксперимента.
        /// </summary>
        public float MinIndicator { get; set; }

        /// <summary>
        /// Общее количество проведённых экспериментов.
        /// </summary>
        public int ExperimentCount { get; set; }

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
