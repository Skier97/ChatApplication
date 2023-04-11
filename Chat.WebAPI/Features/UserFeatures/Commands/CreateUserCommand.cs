using Chat.WebAPI.DataAccess;
using Chat.WebAPI.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.WebAPI.Features.UserFeatures.Commands
{
    public class CreateUserCommand : IRequest<ErrorViewModel>
    {
        public string Login { get; set; }
        public string NameUser { get; set; }
        public string Password { get; set; }
        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ErrorViewModel>
        {
            private readonly IChatDbContext _context;
            public CreateUserCommandHandler(IChatDbContext context)
            {
                _context = context;
            }
            public async Task<ErrorViewModel> Handle(CreateUserCommand command, CancellationToken cancellationToken)
            {
                var result = new ErrorViewModel();
                //Предварительная проверка на создание пользователя с таким же логином
                var checkLogin = _context.Users.Any(x => x.Login == command.Login);
                if (checkLogin)
                {
                    result.Error = "Пользователь с таким логином уже существует!";
                    result.Entity = new User();
                    return result;
                }
                var user = new User()
                { 
                    UserId = Guid.NewGuid(),
                    Login = command.Login,
                    NameUser = command.NameUser,
                    Password = command.Password
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                result.Error = "";
                result.Entity = user;
                return result;
            }
        }
    }
}
