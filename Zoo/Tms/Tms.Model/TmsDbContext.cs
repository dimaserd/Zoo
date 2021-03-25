using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Tms.Model.Entities;

namespace Tms.Model
{
    public class TmsDbContext : DbContext
    {
        public TmsDbContext([NotNull] DbContextOptions<TmsDbContext> options) : base(options)
        {
        }

        public DbSet<DayTask> DayTasks { get; set; }

        public DbSet<DayTaskComment> DayTaskComments { get; set; }
    }
}