using FluentValidation;

namespace Science.Application.Science.Queries.GetExperimentValues
{
    /// <summary>
    /// Валидатор для команды получения значений эксперимента.
    /// </summary>
    public class GetExperimentValuesQueryValidator : AbstractValidator<GetExperimentValuesQuery>
    {
        public GetExperimentValuesQueryValidator() 
        {
            /// <summary>
            /// Проверяет, что имя файла не пустое и не превышает 250 символов.
            /// </summary>
            RuleFor(getExperimentValuesQuery =>
                getExperimentValuesQuery.FileName).NotEmpty().MaximumLength(250);
        }
    }
}
