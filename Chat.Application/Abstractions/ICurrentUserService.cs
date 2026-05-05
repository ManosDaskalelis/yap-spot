using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Abstractions
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
        string AuthUserId { get; }
        bool IsAuthenticated { get; }
    }
}
