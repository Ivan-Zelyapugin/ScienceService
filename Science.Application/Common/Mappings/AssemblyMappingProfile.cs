using AutoMapper;
using System.Reflection;

namespace Science.Application.Common.Mappings
{
    /// <summary>
    /// Профиль маппинга AutoMapper для автоматической регистрации маппингов из указанной сборки.
    /// </summary>
    public class AssemblyMappingProfile : Profile
    {
        /// <summary>
        /// Инициализирует новый экземпляр профиля маппинга, применяя маппинги из указанной сборки.
        /// </summary>
        /// <param name="assembly">Сборка, содержащая типы с маппингами.</param>
        public AssemblyMappingProfile(Assembly assembly) =>
            ApplyMappingsFromAssembly(assembly);

        /// <summary>
        /// Применяет маппинги из типов сборки, реализующих интерфейс <see cref="IMapWith{T}"/>.
        /// </summary>
        /// <param name="assembly">Сборка, из которой извлекаются типы с маппингами.</param>
        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(type => type.GetInterfaces()
                    .Any(i => i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IMapWith<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}
