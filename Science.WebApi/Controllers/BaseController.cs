using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Science.WebApi.Controllers
{
    /// <summary>
    /// Базовый контроллер, предоставляющий доступ к <see cref="IMediator"/> через внедрение зависимостей.
    /// </summary>
    [ApiController]
    [Route("science/[controller]/[action]")]
    public abstract class BaseController : ControllerBase
    {
        private IMediator _mediator;

        /// <summary>
        /// Свойство для получения экземпляра <see cref="IMediator"/> из сервисов запроса.
        /// </summary>
        protected IMediator Mediator =>
            _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
