using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Science.Application.Common.Exceptions;
using Science.Application.Interfaces;
using Science.Domain.Entity;

namespace Science.Application.Science.Queries.GetExperimentValues
{
    /// <summary>
    /// Обработчик запроса для получения значений экспериментов.
    /// </summary>
    public class GetExperimentValuesQueryHandler : IRequestHandler<GetExperimentValuesQuery, ExperimentValuesVm>
    {
        private readonly IExperimentsDbContext _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Инициализирует новый экземпляр обработчика запроса с указанным контекстом базы данных и маппером.
        /// </summary>
        /// <param name="dbContext">Контекст базы данных для работы с экспериментами.</param>
        /// <param name="mapper">Маппер для преобразования сущностей в DTO.</param>
        public GetExperimentValuesQueryHandler(IExperimentsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Обрабатывает запрос, возвращая список значений экспериментов.
        /// </summary>
        /// <param name="request">Запрос с параметрами.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Модель представления с значениями экспериментов.</returns>
        public async Task<ExperimentValuesVm> Handle(GetExperimentValuesQuery request, CancellationToken cancellationToken)
        {
            var file = await _dbContext.Files
                .Where(f => f.FileName == request.FileName)
                .Select(f => new { f.Id })
                .FirstOrDefaultAsync(cancellationToken);

            if (file == null)
                throw new NotFoundException(nameof(ExperimentValue));


            var valuesQuery = await _dbContext.Values
                .Where(v => v.FileId == file.Id)
                .ProjectTo<ExperimentValuesDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new ExperimentValuesVm { Values = valuesQuery };
        }
    }
}
