using Chat.WebAPI.DataAccess;
using Chat.WebAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.WebAPI.Features.MessageFeatures.Commands
{
    public class CreateMessageCommand : IRequest<Guid>
    {
        public Guid ChatId { get; set; }
        public Guid SenderId { get; set; }
        public string TextMessage { get; set; }
        public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, Guid>
        {
            private readonly IChatDbContext _context;
            public CreateMessageCommandHandler(IChatDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(CreateMessageCommand command, CancellationToken cancellationToken)
            {
                //Создание сообщения
                var mess = new Message();
                mess.MessageId = Guid.NewGuid();
                mess.TimeSend = DateTime.Now;
                mess.ChatId = command.ChatId;
                mess.TextMessage = command.TextMessage;
                mess.SenderId = command.SenderId;
                _context.Messages.Add(mess);
                await _context.SaveChangesAsync();
                
                return mess.MessageId;
            }
        }
    }
}
