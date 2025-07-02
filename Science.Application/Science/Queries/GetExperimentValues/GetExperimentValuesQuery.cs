using MediatR;

namespace Science.Application.Science.Queries.GetExperimentValues
{
    /// <summary>
    /// Запрос для получения значений экспериментов
    /// </summary>
    public class GetExperimentValuesQuery : IRequest<ExperimentValuesVm>
    {
        /// <summary>
        /// Имя файла
        /// </summary>
        public string FileName { get; set; }
    }
}
