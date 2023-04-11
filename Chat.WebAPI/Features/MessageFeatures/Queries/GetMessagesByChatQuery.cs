using Chat.WebAPI.DataAccess;
using Chat.WebAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.WebAPI.Features.MessageFeatures.Queries
{
    public class GetMessagesByChatQuery : IRequest<ListMessagesUser>
    {
        public Guid ChatId { get; set; }
        public Guid CurrentUserId { get; set; }
        public bool UpdateTimeView { get; set; }
        public class GetMessagesByChatQueryHandler : IRequestHandler<GetMessagesByChatQuery, ListMessagesUser>
        {
            private readonly IChatDbContext _context;
            public GetMessagesByChatQueryHandler(IChatDbContext context)
            {
                _context = context;
            }
            public async Task<ListMessagesUser> Handle(GetMessagesByChatQuery query, CancellationToken cancellationToken)
            {
                ChatUser chatUser = null;
                if (query.CurrentUserId != Guid.Empty && query.ChatId != Guid.Empty)
                {
                    //Определим чат по входным данным
                    chatUser = await _context.ChatUsers
                        .Where(a => a.ChatId == query.ChatId && a.UserId == query.CurrentUserId)
                        .FirstOrDefaultAsync();
                    if (chatUser != null && query.UpdateTimeView)//Если передан параметр на обновление просмотра сообщения, то обновим время просмотра
                    {
                        chatUser.LastTimeView = DateTime.Now;
                        await _context.SaveChangesAsync();
                    }
                    else if (chatUser == null)
                        throw new Exception("Не найдена запись о данных по чату для запрошенных данных!");
                }
                else
                    throw new Exception("Указан пустой гуид пользователя или чата!");
                
                var result = await _context.Messages
                    .Where(a => a.ChatId == query.ChatId)
                    .OrderBy(x => x.TimeSend)
                    .ToListAsync();
                return new ListMessagesUser() { LastDateView = chatUser.LastTimeView, ListMessages = result };
            }
        }
    }
}
