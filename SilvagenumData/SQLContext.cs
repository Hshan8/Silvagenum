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
    }
}
