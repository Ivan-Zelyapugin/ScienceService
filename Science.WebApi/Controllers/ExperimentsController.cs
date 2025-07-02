using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Science.Application.Science.Commands.UploadExperiment;
using Science.Application.Science.Queries.GetExperimentResults;
using Science.Application.Science.Queries.GetExperimentValues;

namespace Science.WebApi.Controllers
{
    /// <summary>
    /// Контроллер для загрузки файлов экспериментов и получения их результатов и значений.
    /// </summary>
    [ApiController]
    [Route("science/")]
    public class ExperimentsController : BaseController
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="ExperimentsController"/>.
        /// </summary>
        /// <param name="mediator">Интерфейс MediatR для отправки запросов и команд.</param>
        public ExperimentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Загружает файл эксперимента и инициирует команду на его обработку.
        /// </summary>
        /// <param name="file">Файл, содержащий данные эксперимента.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Результат обработки файла: сообщение об успехе или ошибке.</returns>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadExperimentFile(IFormFile file, CancellationToken cancellationToken)
        {
            var tempPath = Path.GetTempFileName();
            await using (var fs = System.IO.File.Create(tempPath))
                await file.CopyToAsync(fs, cancellationToken);

            var fileInfo = new FileInfo(tempPath);

            var command = new UploadExperimentFileCommand
            {
                FileContent = file.OpenReadStream(),
                FileName = file.FileName,
                FileSize = file.Length,
                AuthorName = fileInfo.GetAccessControl().GetOwner(typeof(System.Security.Principal.NTAccount)).ToString(),
                CreatedDate = fileInfo.CreationTimeUtc
            };

            var result = await _mediator.Send(command, cancellationToken);

            System.IO.File.Delete(tempPath);

            return result.Success
                ? Ok(new { result.Message, result.ValidRecords })
                : BadRequest(result.Message);
        }

        /// <summary>
        /// Получает результаты экспериментов с возможностью фильтрации по различным параметрам.
        /// </summary>
        /// <param name="fileName">Имя файла эксперимента.</param>
        /// <param name="minAvgIndicator">Минимальное значение среднего индикатора.</param>
        /// <param name="maxAvgIndicator">Максимальное значение среднего индикатора.</param>
        /// <param name="minAvgDuration">Минимальное значение средней продолжительности.</param>
        /// <param name="maxAvgDuration">Максимальное значение средней продолжительности.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Список отфильтрованных результатов экспериментов.</returns>
        [HttpGet("results")]
        public async Task<ActionResult<ExperimentResultsVm>> GetExperimentResults(
            [FromQuery] string? fileName,
            [FromQuery] double? minAvgIndicator,
            [FromQuery] double? maxAvgIndicator,
            [FromQuery] double? minAvgDuration,
            [FromQuery] double? maxAvgDuration,
            CancellationToken cancellationToken)
        {
            var query = new GetExperimentResultsQuery
            {
                FileName = fileName,
                MinAvgIndicator = minAvgIndicator,
                MaxAvgIndicator = maxAvgIndicator,
                MinAvgDuration = minAvgDuration,
                MaxAvgDuration = maxAvgDuration
            };

            var results = await _mediator.Send(query, cancellationToken);
            return Ok(results);
        }

        /// <summary>
        /// Получает значения эксперимента по имени файла.
        /// </summary>
        /// <param name="fileName">Имя файла, для которого необходимо получить значения.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Список значений эксперимента.</returns>
        [HttpGet("values")]
        public async Task<ActionResult<ExperimentValuesVm>> GetExperimentValues(
            [FromQuery] string fileName,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return BadRequest("Не указано имя файла");
            }

            var query = new GetExperimentValuesQuery
            {
                FileName = fileName
            };

            var values = await _mediator.Send(query, cancellationToken);
            return Ok(values);
        }
    }
}
