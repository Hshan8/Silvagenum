using Microsoft.EntityFrameworkCore;
using SilvagenumLogic;

namespace SilvagenumData
{
    public class SQLContext : DbContext
    {
        public DbSet<Person> People { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source= (localdb)\\MSSQLLocalDB; Initial Catalog=SilvagenumData");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasOne<Person>(x => x.Father)
                .WithOne()
                .HasForeignKey<Person>(x => x.FatherId);
            
            modelBuilder.Entity<Person>()
                .HasOne<Person>(x => x.Mother)
                .WithOne()
                .HasForeignKey<Person>(x => x.MotherId);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
