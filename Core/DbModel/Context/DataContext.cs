using Core.DbModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Core.DbModel.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {

        }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionType> PermissionTypes { get; set; }
    }
}
