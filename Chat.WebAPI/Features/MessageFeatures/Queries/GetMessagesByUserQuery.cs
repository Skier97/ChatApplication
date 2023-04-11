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
    public class GetMessagesByUserQuery : IRequest<IEnumerable<Message>>
    {
        public Guid CurrentUserId { get; set; }
        public Guid CompanionId { get; set; }
        public class GetMessagesByUserQueryHandler : IRequestHandler<GetMessagesByUserQuery, IEnumerable<Message>>
        {
            private readonly IChatDbContext _context;
            public GetMessagesByUserQueryHandler(IChatDbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<Message>> Handle(GetMessagesByUserQuery query, CancellationToken cancellationToken)
            {
                var chatCurrentUserId = _context.ChatUsers
                    .Where(x => x.UserId == query.CurrentUserId)
                    .Select(x => x.ChatId);//чаты текущего пользователя
                var chatCompanionId = _context.ChatUsers
                    .Where(x => x.UserId == query.CompanionId)
                    .Select(x => x.ChatId);//чаты собеседника
                var shareChatId = await chatCurrentUserId
                    .Intersect(chatCompanionId)
                    .FirstOrDefaultAsync();//поиск общего чата
                if (shareChatId != Guid.Empty)
                {
                    return await _context.Messages
                        .Where(x => x.ChatId == shareChatId)
                        .OrderBy(x => x.TimeSend)
                        .ToListAsync() ?? new List<Message>();//если такой чат есть, вернуть по нему сообщения
                }
                return new List<Message>();
            }
        }
    }
}
