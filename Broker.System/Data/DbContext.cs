using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Broker.System.Data
{
    public class DbContext : IdentityDbContext
    {
        public DbContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}