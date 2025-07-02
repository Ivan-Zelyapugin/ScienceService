using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Science.Application.Common.Exceptions;
using Science.Application.Interfaces;
using Science.Domain.Entity;

namespace Science.Application.Science.Queries.GetExperimentResults
{
    /// <summary>
    /// Обработчик запроса для получения результатов экспериментов с применением фильтров.
    /// </summary>
    public class GetExperimentResultsQueryHandler : IRequestHandler<GetExperimentResultsQuery, ExperimentResultsVm>
    {
        private readonly IExperimentsDbContext _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Инициализирует новый экземпляр обработчика запроса с указанным контекстом базы данных и маппером.
        /// </summary>
        /// <param name="dbContext">Контекст базы данных для работы с экспериментами.</param>
        /// <param name="mapper">Маппер для преобразования сущностей в DTO.</param>
        public GetExperimentResultsQueryHandler(IExperimentsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Обрабатывает запрос, применяя фильтры и возвращая список результатов экспериментов.
        /// </summary>
        /// <param name="request">Запрос с параметрами фильтрации.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Модель представления с результатами экспериментов.</returns>
        public async Task<ExperimentResultsVm> Handle(GetExperimentResultsQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.FileName))
            {
                var fileExists = await _dbContext.Files
                    .AnyAsync(f => f.FileName == request.FileName, cancellationToken);

                if (!fileExists)
                    throw new NotFoundException(nameof(ExperimentResult));
            }

            var query = _dbContext.Results.AsQueryable();

            if (!string.IsNullOrEmpty(request.FileName))
                query = query.Where(r => r.FileMetadata.FileName == request.FileName);

            if (request.MinAvgIndicator.HasValue)
                query = query.Where(r => r.AvgIndicator >= request.MinAvgIndicator.Value);

            if (request.MaxAvgIndicator.HasValue)
                query = query.Where(r => r.AvgIndicator <= request.MaxAvgIndicator.Value);

            if (request.MinAvgDuration.HasValue)
                query = query.Where(r => r.AvgExperimentTime >= request.MinAvgDuration.Value);

            if (request.MaxAvgDuration.HasValue)
                query = query.Where(r => r.AvgExperimentTime <= request.MaxAvgDuration.Value);

            var results = await query
                .ProjectTo<ExperimentResultsDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new ExperimentResultsVm
            {
                Results = results
            };
        }
    }
}
