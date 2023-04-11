using Chat.WebAPI.Features.MessageFeatures.Commands;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Chat.WebAPI.Features.UserFeatures.Queries;
using Chat.WebAPI.Features.UserFeatures.Commands;

namespace Chat.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IMediator _mediator;
        private static NLog.Logger _logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        /// <summary>
        /// Получение юзеров
        /// </summary>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpGet("GetUsersQuery")]
        public async Task<IActionResult> GetUsersQuery()
        {
            try
            {
                return Ok(await Mediator.Send(new GetUsersQuery()));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in User/GetUsersQuery(): {ex}");
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Создание пользователя.
        /// </summary>
        /// <param name="command">Команда на создание пользователя</param>
        /// <returns>Ответ по созданию записи в БД</returns>
        [Produces("application/json")]
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(CreateUserCommand command)
        {
            try
            {
                return Ok(await Mediator.Send(command));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in User/CreateUser(): {ex}");
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Проверка логина пользователя
        /// </summary>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpGet("LoginUserQuery")]
        public async Task<IActionResult> LoginUserQuery(string login, string password)
        {
            try
            {
                return Ok(await Mediator.Send(new LoginUserQuery { Login = login, Password = password }));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in User/LoginUserQuery(): {ex}");
                return BadRequest(ex);
            }
        }
    }
}
