using Chat.WebAPI.DataAccess;
using Chat.WebAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.WebAPI.Features.ChatFeatures.Commands
{
    public class CreateChatCommand : IRequest<Guid>
    {
        public Guid SenderId { get; set; }
        public Guid CompanionId { get; set; }
        public class CreateChatCommandHandler : IRequestHandler<CreateChatCommand, Guid>
        {
            private readonly IChatDbContext _context;
            public CreateChatCommandHandler(IChatDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(CreateChatCommand command, CancellationToken cancellationToken)
            {
                //Сначала проверим создан ли чат, чтобы не дублировать его
                var checkChatSender = _context.ChatUsers
                    .Where(x => x.UserId == command.SenderId)
                    .Select(x => x.ChatId)
                    .Distinct();
                var checkChatCompanion = _context.ChatUsers
                    .Where(x => x.UserId == command.CompanionId)
                    .Select(x => x.ChatId)
                    .Distinct();
                var intersectChats = await checkChatCompanion
                    .Intersect(checkChatSender)
                    .FirstOrDefaultAsync();
                //Если есть пересечение по чату у отправителя и получателя, значит чат был ранее создан и вернем его гуид
                if (intersectChats != Guid.Empty)
                {
                    return intersectChats;
                }
                //Иначе создадим чат
                var chat = new Models.Chat();
                chat.ChatId = Guid.NewGuid();
                chat.DateCreate = DateTime.Now;
                //Создадим вспомогательную таблицу по данным собеседников чата
                var chatUsers_1 = new ChatUser();
                chatUsers_1.ChatUserId = Guid.NewGuid();
                chatUsers_1.ChatId = chat.ChatId;
                chatUsers_1.UserId = command.SenderId;
                chatUsers_1.LastTimeView = DateTime.Now;
                var chatUsers_2 = new ChatUser();
                chatUsers_2.ChatUserId = Guid.NewGuid();
                chatUsers_2.ChatId = chat.ChatId;
                chatUsers_2.UserId = command.CompanionId;
                chatUsers_2.LastTimeView = DateTime.Now;

                _context.Chats.Add(chat);
                await _context.SaveChangesAsync();
                _context.ChatUsers.Add(chatUsers_1);
                _context.ChatUsers.Add(chatUsers_2);
                await _context.SaveChangesAsync();
                return chat.ChatId;
            }
        }
    }
}
