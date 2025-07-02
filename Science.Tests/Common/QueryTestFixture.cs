using AutoMapper;
using Science.Application.Common.Mappings;
using Science.Application.Interfaces;
using Science.Persistence;

namespace Science.Tests.Common
{
    /// <summary>
    /// Общий фикстурный класс для тестов запросов (queries), предоставляет контекст и маппер.
    /// </summary>
    public class QueryTestFixture : IDisposable
    {
        public ExperimentsContextFactory Factory { get; }

        /// <summary>
        /// EF Core контекст, инициализированный тестовыми данными.
        /// </summary>
        public ExperimentsDbContext Context => Factory.Context;

        /// <summary>
        /// AutoMapper для преобразования DTO/VM.
        /// </summary>
        public IMapper Mapper { get; }

        /// <summary>
        /// Конструктор создаёт контекст и настраивает AutoMapper.
        /// </summary>
        public QueryTestFixture()
        {
            Factory = new ExperimentsContextFactory();

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AssemblyMappingProfile(typeof(IExperimentsDbContext).Assembly));
            });

            Mapper = configurationProvider.CreateMapper();
        }

        public void Dispose()
        {
            Factory.Dispose();
        }
    }

    /// <summary>
    /// Коллекция для xUnit, позволяет использовать QueryTestFixture в нескольких тестах.
    /// </summary>
    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
}
