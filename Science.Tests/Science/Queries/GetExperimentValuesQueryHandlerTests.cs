using AutoMapper;
using Science.Application.Common.Exceptions;
using Science.Application.Science.Queries.GetExperimentResults;
using Science.Application.Science.Queries.GetExperimentValues;
using Science.Persistence;
using Science.Tests.Common;
using Shouldly;

namespace Science.Tests.Science.Queries
{
    /// <summary>
    /// Тесты для обработчика команды GetExperimentValuesQueryHandler.
    /// Проверяется корректность получния значений эксперимента.
    /// </summary>
    [Collection("QueryCollection")]
    public class GetExperimentValuesQueryHandlerTests
    {
        private readonly ExperimentsDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Конструктор тестового класса, инициализирует контекст базы данных и маппер из фикстуры.
        /// </summary>
        /// <param name="fixture">Фикстура с тестовыми данными и настройками.</param>
        public GetExperimentValuesQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        /// <summary>
        /// Проверяет успешное получение значений экспериментов по существующему имени файла.
        /// </summary>
        [Fact]
        public async Task GetExperimentValuesQueryHandler_Success()
        {
            // Arrange
            var fileName = _context.Files.First().FileName;
            var handler = new GetExperimentValuesQueryHandler(_context, _mapper);
            var query = new GetExperimentValuesQuery { FileName = fileName };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<ExperimentValuesVm>();
            result.Values.Count.ShouldBe(2);
        }

        /// <summary>
        /// Проверяет, что при запросе с несуществующим именем файла выбрасывается исключение NotFoundException.
        /// </summary>
        [Fact]
        public async Task GetExperimentValuesQueryHandler_Fail()
        {
            // Arrange
            var handler = new GetExperimentValuesQueryHandler(_context, _mapper);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new GetExperimentValuesQuery
                    {
                        FileName = "fileName.csv"
                    },
                    CancellationToken.None));
        }
    }
}
