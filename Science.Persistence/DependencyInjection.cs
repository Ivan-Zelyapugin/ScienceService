using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Science.Application.Interfaces;

namespace Science.Persistence
{
    /// <summary>
    /// Класс расширения для регистрации слоя Persistence в DI-контейнере.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Регистрирует контекст базы данных и интерфейс доступа к нему в контейнере зависимостей.
        /// </summary>
        /// <param name="services">Коллекция сервисов приложения.</param>
        /// <param name="configuration">Конфигурация приложения с параметрами подключения.</param>
        /// <returns>Модифицированная коллекция сервисов.</returns>
        public static IServiceCollection AddPersistence(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration["Dbconnection"];
            services.AddDbContext<ExperimentsDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });
            services.AddScoped<IExperimentsDbContext>(provider =>
                provider.GetService<ExperimentsDbContext>());
            return services;
        }
    }
}
