namespace Science.Application.Science.Queries.GetExperimentResults
{
    /// <summary>
    /// Модель представления для списка результатов экспериментов.
    /// </summary>
    public class ExperimentResultsVm
    {
        /// <summary>
        /// Список результатов экспериментов.
        /// </summary>
        public IList<ExperimentResultsDto> Results { get; set; }
    }
}
