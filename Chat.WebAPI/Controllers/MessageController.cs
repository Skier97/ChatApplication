using Chat.WebAPI.Features.MessageFeatures.Commands;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Chat.WebAPI.Features.MessageFeatures.Queries;

namespace Chat.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private IMediator _mediator;
        private static NLog.Logger _logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
        
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        /// <summary>
        /// Создание сообщения в чате.
        /// </summary>
        /// <param name="command">Команда на создание сообщения</param>
        /// <returns>Ответ по созданию записи в БД</returns>
        [Produces("application/json")]
        [HttpPost("CreateMessage")]
        public async Task<IActionResult> CreateMessage(CreateMessageCommand command)
        {
            try
            {
                return Ok(await Mediator.Send(command));
            }
            catch(Exception ex)
            {
                _logger.Error($"Error in Message/CreateMessage(): {ex}");
                return BadRequest(ex);
            }            
        }

        /// <summary>
        /// Получение сообщений по заданному чату
        /// </summary>
        /// <param name="chatId">Гуид чата</param>
        /// <param name="userId">Гуид текущего юзера</param>
        /// <param name="updateLastView">Параметр, отвечающий за просмотр сообщений в чате</param>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpGet("GetMessagesByChatQuery")]
        public async Task<IActionResult> GetMessagesByChatQuery(Guid chatId, Guid userId, bool updateLastView = false)
        {
            try
            {
                return Ok(await Mediator.Send(new GetMessagesByChatQuery { ChatId = chatId, CurrentUserId = userId, UpdateTimeView = updateLastView }));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in Message/GetMessagesByChatQuery(): {ex}");
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Получение сообщений с данными пользователями
        /// </summary>
        /// <param name="currentUserId">Гуид текущего юзера</param>
        /// <param name="companionId">Гуид собеседника</param>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpGet("GetMessagesByUserQuery")]
        public async Task<IActionResult> GetMessagesByUserQuery(Guid currentUserId, Guid companionId)
        {
            try
            {
                return Ok(await Mediator.Send(new GetMessagesByUserQuery { CurrentUserId = currentUserId, CompanionId = companionId }));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in Message/GetMessagesByUserQuery(): {ex}");
                return BadRequest(ex);
            }
        }
    }
}
