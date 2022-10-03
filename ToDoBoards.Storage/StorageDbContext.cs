using Microsoft.EntityFrameworkCore;
using ToDoBoards.Common.Models;

namespace ToDoBoards.Storage
{
    /// <summary>
    /// Database storage context
    /// </summary>
    internal class StorageDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageDbContext"/> class
        /// </summary>
        /// <param name="options">The options</param>
        public StorageDbContext(DbContextOptions<StorageDbContext> options) : base(options)
        {
        }

        internal DbSet<ToDo> ToDos { get; set; }
        internal DbSet<Board> Boards { get; set; }
        internal DbSet<RateLimit> RateLimits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDo>().HasKey(x => x.Id);
            modelBuilder.Entity<Board>().HasKey(x => x.Id);
            modelBuilder.Entity<ToDo>().HasOne(x => x.Board)
                .WithMany(x => x.ToDos);

            modelBuilder.Entity<RateLimit>().HasKey(x => x.Id);
            modelBuilder.Entity<RateLimit>().HasIndex(x => x.ApiPath);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}