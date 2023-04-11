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
    public class GetUsersQuery : IRequest<IEnumerable<User>>
    {
        public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<User>>
        {
            private readonly IChatDbContext _context;
            public GetUsersQueryHandler(IChatDbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<User>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
            {
                return await _context.Users.ToListAsync() ?? new List<User>();
            }
        }
    }
}
