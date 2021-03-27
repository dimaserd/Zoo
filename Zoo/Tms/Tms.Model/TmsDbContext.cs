using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Tms.Model.Entities;

namespace Tms.Model
{
    /// <summary>
    /// DbContext для системы управления заданиями
    /// </summary>
    public class TmsDbContext : DbContext
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="options"></param>
        public TmsDbContext([NotNull] DbContextOptions<TmsDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Задания на день
        /// </summary>
        public DbSet<DayTask> DayTasks { get; set; }

        /// <summary>
        /// Комментарии к заданиям
        /// </summary>
        public DbSet<DayTaskComment> DayTaskComments { get; set; }
    }
}