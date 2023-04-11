using Chat.WebAPI.DataAccess;
using Chat.WebAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.WebAPI.Features.UserFeatures.Queries
{
    public class LoginUserQuery : IRequest<User>
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery,User>
        {
            private readonly IChatDbContext _context;
            public LoginUserQueryHandler(IChatDbContext context)
            {
                _context = context;
            }
            public async Task<User> Handle(LoginUserQuery query, CancellationToken cancellationToken)
            {
                var result = await _context.Users
                    .Where(x => x.Login == query.Login && x.Password == query.Password)
                    .FirstOrDefaultAsync();
                return result ?? new User();
            }
        }
    }
}
