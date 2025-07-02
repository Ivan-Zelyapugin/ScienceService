using FluentValidation;

namespace Science.Application.Science.Commands.UploadExperiment
{
    public class UploadExperimentFileCommandValidator : AbstractValidator<UploadExperimentFileCommand>
    {
        /// <summary>
        /// Валидатор для команды загрузки файла эксперимента.
        /// </summary>
        public UploadExperimentFileCommandValidator() 
        {
            /// <summary>
            /// Проверяет, что имя файла не пустое и не превышает 250 символов.
            /// </summary>
            RuleFor(uploadExperimentFileCommand =>
                uploadExperimentFileCommand.FileName).NotEmpty().MaximumLength(250);

            /// <summary>
            /// Проверяет, что имя автора не пустое и не превышает 100 символов.
            /// </summary>
            RuleFor(uploadExperimentFileCommand =>
                uploadExperimentFileCommand.AuthorName).NotEmpty().MaximumLength(100);

            /// <summary>
            /// Проверяет, что размер файла не пустой и не равен null.
            /// </summary>
            RuleFor(uploadExperimentFileCommand =>
                uploadExperimentFileCommand.FileSize).NotEmpty().NotNull();

            /// <summary>
            /// Проверяет, что дата создания файла не пуста, не позднее текущего времени и не ранее 1 января 2000 года.
            /// </summary>
            RuleFor(uploadExperimentFileCommand =>
                uploadExperimentFileCommand.CreatedDate)
                .NotEmpty()
                .Must(date => date <= DateTime.Now)
                .Must(date => date >= new DateTime(2000, 1, 1, 0, 0, 0));

            /// <summary>
            /// Проверяет, что имя файла указано.
            /// </summary>
            RuleFor(uploadExperimentFileCommand =>
                uploadExperimentFileCommand.FileName).NotEmpty();

        }
    }
}
