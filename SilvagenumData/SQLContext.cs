using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SilvagenumLogic;

namespace SilvagenumData;

public class SQLContext : DbContext
{
    public DbSet<Person> People { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source= (localdb)\\MSSQLLocalDB; Initial Catalog=SilvagenumData");        //for showcase purposes only, actual app should have a variable string
    }
    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder.Properties<DateOnly>()
               .HaveConversion<DateOnlyConverter>()
               .HaveColumnType("date");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasOne(x => x.Father)
            .WithOne()
                .HasForeignKey<Person>(x => x.FatherId)
                .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Person>()
            .HasOne(x => x.Mother)
            .WithOne()
                .HasForeignKey<Person>(x => x.MotherId)
                .OnDelete(DeleteBehavior.SetNull);

        base.OnModelCreating(modelBuilder);
    }
}

public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    public DateOnlyConverter() : base(
            d => d.ToDateTime(TimeOnly.MinValue),
            d => DateOnly.FromDateTime(d))
    { }
}