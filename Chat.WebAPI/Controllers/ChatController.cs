using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Chat.WebAPI.Features.ChatFeatures.Commands;
using Chat.WebAPI.Features.ChatFeatures.Queires;

namespace Chat.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private IMediator _mediator;
        private static NLog.Logger _logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        /// <summary>
        /// Создание чата.
        /// </summary>
        /// <param name="command">Команда на создание чата</param>
        /// <returns>Ответ по созданию записи в БД</returns>
        [Produces("application/json")]
        [HttpPost("CreateChat")]
        public async Task<IActionResult> CreateChat(CreateChatCommand command)
        {
            try
            {
                return Ok(await Mediator.Send(command));
            }
            catch(Exception ex)
            {
                _logger.Error($"Error in Chat/CreateChat(): {ex}");
                return BadRequest(ex);
            }            
        }

        /// <summary>
        /// Получение чатов юзера
        /// </summary>
        /// <param name="userId">Гуид юзера</param>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpGet("GetChatsForUserQuery")]
        public async Task<IActionResult> GetChatsForUserQuery(Guid userId)
        {
            try
            {
                return Ok(await Mediator.Send(new GetChatsForUserQuery { UserId = userId }));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in Chat/GetChatsForUserQuery(): {ex}");
                return BadRequest(ex);
            }
        }
    }
}
