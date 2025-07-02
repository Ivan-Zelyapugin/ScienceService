using FluentValidation;
using Science.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace Science.WebApi.Middleware
{
    /// <summary>
    /// Пользовательский middleware для глобальной обработки исключений.
    /// Перехватывает необработанные исключения и возвращает структурированный JSON-ответ с соответствующим HTTP-статусом.
    /// </summary>
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Инициализирует middleware с делегатом следующего компонента в конвейере.
        /// </summary>
        /// <param name="next">Следующий делегат обработки HTTP-запроса.</param>
        public CustomExceptionHandlerMiddleware(RequestDelegate next) =>
            _next = next;

        /// <summary>
        /// Вызывает следующий middleware и обрабатывает исключения, если они возникают.
        /// </summary>
        /// <param name="context">Контекст текущего HTTP-запроса.</param>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        /// <summary>
        /// Формирует и отправляет HTTP-ответ в случае исключения.
        /// </summary>
        /// <param name="context">Контекст HTTP-запроса.</param>
        /// <param name="exception">Возникшее исключение.</param>
        /// <returns>Задача, представляющая асинхронную операцию отправки ответа.</returns>
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;
            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(validationException.Errors);
                    break;
                case NotFoundException:
                    code = HttpStatusCode.NotFound;
                    break;
            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if (result == string.Empty)
            {
                result = JsonSerializer.Serialize(new { error = exception.Message });
            }

            return context.Response.WriteAsync(result);
        }
    }
}
