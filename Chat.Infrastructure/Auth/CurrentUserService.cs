using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Infrastructure.Auth
{
    public sealed class CurrentUserService
    {
        //    using System.Security.Claims;
        //using Chat.Application.Abstractions;
        //using Microsoft.AspNetCore.Http;

        //namespace Chat.Infrastructure.Auth;

        //    public sealed class CurrentUserService : ICurrentUserService
        //    {
        //        private readonly IHttpContextAccessor _httpContextAccessor;

        //        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        //        {
        //            _httpContextAccessor = httpContextAccessor;
        //        }

        //        public string AuthUserId
        //        {
        //            get
        //            {
        //                var user = _httpContextAccessor.HttpContext?.User;

        //                var authUserId =
        //                    user?.FindFirstValue(ClaimTypes.NameIdentifier)
        //                    ?? user?.FindFirstValue("sub");

        //                if (string.IsNullOrWhiteSpace(authUserId))
        //                    throw new UnauthorizedAccessException("Current user is not authenticated.");

        //                return authUserId;
        //            }
        //        }

        //        public Guid UserId
        //        {
        //            get
        //            {
        //                var userIdValue =
        //                    _httpContextAccessor.HttpContext?.User?.FindFirstValue("chat_user_id");

        //                if (!Guid.TryParse(userIdValue, out var userId))
        //                    throw new UnauthorizedAccessException("Chat user id was not found in the current user claims.");

        //                return userId;
        //            }
        //        }

        //        public bool IsAuthenticated =>
        //            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;
        //    }
    }
}
