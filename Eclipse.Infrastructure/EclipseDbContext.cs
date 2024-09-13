using Eclipse.Core.Models;
using Eclipse.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace Eclipse.Infrastructure
{
    public class EclipseDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public EclipseDbContext(DbContextOptions<EclipseDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
