using AutoMapper;
using Science.Application.Common.Exceptions;
using Science.Application.Science.Queries.GetExperimentResults;
using Science.Persistence;
using Science.Tests.Common;
using Shouldly;

namespace Science.Tests.Science.Queries
{
    /// <summary>
    /// Тесты для обработчика команды GetExperimentResultsQueryHandler.
    /// Проверяется корректность получния результатов эксперимента.
    /// </summary>
    [Collection("QueryCollection")]
    public class GetExperimentResultsQueryHandlerTests
    {
        private readonly ExperimentsDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Конструктор тестового класса, инициализирует контекст базы данных и маппер из фикстуры.
        /// </summary>
        /// <param name="fixture">Фикстура с тестовыми данными и настройками.</param>
        public GetExperimentResultsQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        /// <summary>
        /// Проверяет, что при отсутствии фильтров возвращаются все результаты.
        /// </summary>
        [Fact]
        public async Task GetExperimentResultsQueryHandler_Success_ReturnAll()
        {
            // Arrange
            var handler = new GetExperimentResultsQueryHandler(_context, _mapper);
            var query = new GetExperimentResultsQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<ExperimentResultsVm>();
            result.Results.Count.ShouldBe(2);
        }

        /// <summary>
        /// Проверяет фильтрацию результатов по имени файла.
        /// </summary>
        [Fact]
        public async Task GetExperimentResultsQueryHandler_Success_ReturnByFileName()
        {
            // Arrange
            var fileName = _context.Files.First().FileName;
            var handler = new GetExperimentResultsQueryHandler(_context, _mapper);
            var query = new GetExperimentResultsQuery { FileName = fileName };

            // Act            
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<ExperimentResultsVm>();
            result.Results.Count.ShouldBe(1);
        }

        /// <summary>
        /// Проверяет фильтрацию результатов по диапазону среднего показателя (AvgIndicator).
        /// </summary>
        [Fact]
        public async Task GetExperimentResultsQueryHandler_Success_ReturnByAvgIndicator()
        {
            // Arrange
            var handler = new GetExperimentResultsQueryHandler(_context, _mapper);
            var query = new GetExperimentResultsQuery 
            { 
                MinAvgIndicator = 57.0,
                MaxAvgIndicator = 100.0
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<ExperimentResultsVm>();
            result.Results.Count.ShouldBe(1);
        }

        /// <summary>
        /// Проверяет фильтрацию результатов по диапазону среднего времени эксперимента (AvgExperimentTime).
        /// </summary>
        [Fact]
        public async Task GetExperimentResultsQueryHandler_Success_ReturnByAvgDuration()
        {
            // Arrange
            var handler = new GetExperimentResultsQueryHandler(_context, _mapper);
            var query = new GetExperimentResultsQuery
            {
                MinAvgDuration = 1800,
                MaxAvgDuration = 1820
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<ExperimentResultsVm>();
            result.Results.Count.ShouldBe(1);
        }

        /// <summary>
        /// Проверяет фильтрацию результатов с применением нескольких условий одновременно.
        /// </summary>
        [Fact]
        public async Task GetExperimentResultsQueryHandler_Success_ReturnByCombinedConditions()
        {
            // Arrange
            var fileName = _context.Files.First().FileName;
            var handler = new GetExperimentResultsQueryHandler(_context, _mapper);
            var query = new GetExperimentResultsQuery
            {
                FileName = fileName,
                MinAvgIndicator = 0,
                MaxAvgIndicator = 100,
                MinAvgDuration = 1000,
                MaxAvgDuration = 2000
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<ExperimentResultsVm>();
            result.Results.Count.ShouldBe(1);
            var res = result.Results.First();
            res.AvgIndicator.ShouldBeInRange(0, 100);
            res.AvgExperimentTime.ShouldBeInRange(1000, 2000);
        }

        /// <summary>
        /// Проверяет, что при передаче имени несуществующего файла выбрасывается исключение NotFoundException.
        /// </summary>
        [Fact]
        public async Task GetExperimentResultsQueryHandler_Fail_ReturnByFileName()
        {
            // Arrange
            var handler = new GetExperimentResultsQueryHandler(_context, _mapper);
            

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new GetExperimentResultsQuery 
                    { 
                        FileName = "fileName.csv" 
                    },
                    CancellationToken.None));
        }
    }
}
