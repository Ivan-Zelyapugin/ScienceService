using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Science.Application.Common.Behaviors;
using System.Reflection;
using FluentValidation;

namespace Science.Application
{
    /// <summary>
    /// Статический класс для настройки внедрения зависимостей приложения.
    /// </summary>
    public static class DependencyIjection
    {
        /// <summary>
        /// Добавляет сервисы приложения в коллекцию зависимостей.
        /// </summary>
        /// <param name="services">Коллекция сервисов для внедрения зависимостей.</param>
        /// <returns>Обновлённая коллекция сервисов с зарегистрированными зависимостями.</returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            /// <summary>
            /// Регистрирует сервисы MediatR из текущей сборки.
            /// </summary>
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            /// <summary>
            /// Регистрирует валидаторы FluentValidation из текущей сборки.
            /// </summary>
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            /// <summary>
            /// Регистрирует поведение валидации для конвейера MediatR.
            /// </summary>
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
