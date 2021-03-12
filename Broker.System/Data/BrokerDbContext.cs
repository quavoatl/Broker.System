using Broker.System.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Broker.System.Data
{
    public class BrokerDbContext : IdentityDbContext
    {
        public BrokerDbContext(DbContextOptions options) : base(options)
        {
     
        }

       

        public DbSet<Limit> Limits { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}