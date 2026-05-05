using Chat.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Infrastructure.Auth
{
    public sealed class TestCurrentUserService : ICurrentUserService
    {
        public Guid UserId => Guid.Parse("11111111-1111-1111-1111-111111111111");

        public string AuthUserId => "fake-auth-user-1";

        public bool IsAuthenticated => true;
    }
}
