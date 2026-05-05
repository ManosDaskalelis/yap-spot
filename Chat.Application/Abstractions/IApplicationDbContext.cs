using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Abstractions
{
    public interface IApplicationDbContext
    {
        public DbSet<Message> Messages { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
