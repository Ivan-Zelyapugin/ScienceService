using FluentValidation;
using MediatR;

namespace Science.Application.Common.Behaviors
{
    /// <summary>
    /// Конвейер для валидации запросов MediatR с использованием FluentValidation.
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса, реализующий <see cref="IRequest{TResponse}"/>.</typeparam>
    /// <typeparam name="TResponse">Тип ответа на запрос.</typeparam>
    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        /// <summary>
        /// Инициализирует новый экземпляр поведения валидации с коллекцией валидаторов.
        /// </summary>
        /// <param name="validators">Коллекция валидаторов для запроса типа <typeparamref name="TRequest"/>.</param>
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) =>
            _validators = validators;

        /// <summary>
        /// Обрабатывает запрос, выполняя валидацию перед вызовом следующего обработчика в конвейере.
        /// </summary>
        /// <param name="request">Запрос, подлежащий валидации.</param>
        /// <param name="next">Делегат следующего обработчика в конвейере.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Результат выполнения запроса типа <typeparamref name="TResponse"/>.</returns>
        /// <exception cref="ValidationException">Выбрасывается, если валидация запроса не прошла.</exception>
        public async Task<TResponse> Handle(TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(failure => failure != null)
                .ToList();

            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }

            return await next();
        }
    }
}