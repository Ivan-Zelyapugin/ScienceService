namespace Science.Application.Science.Queries.GetExperimentValues
{
    /// <summary>
    /// Модель представления для списка значений экспериментов.
    /// </summary>
    public class ExperimentValuesVm
    {
        /// <summary>
        /// Список значений экспериментов.
        /// </summary>
        public IList<ExperimentValuesDto> Values { get; set; }
    }
}
