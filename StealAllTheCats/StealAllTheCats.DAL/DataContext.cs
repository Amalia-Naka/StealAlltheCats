using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StealAllTheCats.DAL.Models;

namespace StealAllTheCats.DAL.Data
{
    public class DataContext : DbContext
    {
        private string _connectionString;

        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {

        }

      /*  public DataContext(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }*/

        public DbSet<Cat> Cats { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<CatTag> catTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CatTag>().HasKey(ct => new { ct.CatId, ct.TagId });

            modelBuilder.Entity<CatTag>().HasOne(ct => ct.Cat).WithMany(c => c.CatTags).HasForeignKey(ct => ct.CatId);

            modelBuilder.Entity<CatTag>().HasOne(ct => ct.Tag).WithMany(t => t.CatTags).HasForeignKey(ct =>ct.TagId);


        }
    }
}
