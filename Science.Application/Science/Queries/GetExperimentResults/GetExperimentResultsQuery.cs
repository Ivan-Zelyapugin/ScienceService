using MediatR;

namespace Science.Application.Science.Queries.GetExperimentResults
{
    /// <summary>
    /// Запрос для получения результатов экспериментов с возможностью фильтрации.
    /// </summary>
    public class GetExperimentResultsQuery : IRequest<ExperimentResultsVm>
    {
        /// <summary>
        /// Имя файла для фильтрации результатов
        /// </summary>
        public string? FileName { get; set; }

        /// <summary>
        /// Минимальное среднее значение показателя для фильтрации
        /// </summary>
        public double? MinAvgIndicator { get; set; }

        /// <summary>
        /// Максимальное среднее значение показателя для фильтрации
        /// </summary>
        public double? MaxAvgIndicator { get; set; }

        /// <summary>
        /// Минимальная средняя длительность эксперимента для фильтрации
        /// </summary>
        public double? MinAvgDuration { get; set; }

        /// <summary>
        /// Максимальная средняя длительность эксперимента для фильтрации
        /// </summary>
        public double? MaxAvgDuration { get; set; }
    }
}
