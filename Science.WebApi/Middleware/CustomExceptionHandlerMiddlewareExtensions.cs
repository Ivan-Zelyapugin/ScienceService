namespace Science.WebApi.Middleware
{
    /// <summary>
    /// Класс расширения для регистрации пользовательского middleware обработки исключений.
    /// </summary>
    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        /// <summary>
        /// Подключает <see cref="CustomExceptionHandlerMiddleware"/> в конвейер обработки HTTP-запросов.
        /// </summary>
        /// <param name="builder">Построитель приложения.</param>
        /// <returns>Обновлённый построитель приложения.</returns>
        public static IApplicationBuilder UseCustomExceptionHandler(this
            IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}
