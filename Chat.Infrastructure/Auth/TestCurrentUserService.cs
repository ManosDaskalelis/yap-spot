using Chat.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Infrastructure.Auth
{
    public sealed class TestCurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TestCurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                var value = _httpContextAccessor.HttpContext?.Request.Headers["X-Fake-User-Id"].FirstOrDefault();
                if (Guid.TryParse(value, out var userId))
                {
                    return userId;
                }

                var query = _httpContextAccessor.HttpContext?.Request.Query["fakeUserId"].FirstOrDefault();
                if (Guid.TryParse(value, out var queryUserId))
                {
                    return queryUserId;
                }

                return Guid.Parse("11111111-1111-1111-1111-111111111111");
            }
        }

        public string AuthUserId => "fake-auth-user-1";

        public bool IsAuthenticated => true;
    }
}
