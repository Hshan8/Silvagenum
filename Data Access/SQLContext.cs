using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SilvagenumWebApp.Models;

public class SQLContext : DbContext
{
    public DbSet<Person> People { get; set; }

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
                .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Person>()
            .HasOne(x => x.Mother)
            .WithOne()
                .HasForeignKey<Person>(x => x.MotherId)
                .OnDelete(DeleteBehavior.NoAction);

        base.OnModelCreating(modelBuilder);
    }

    public SQLContext(DbContextOptions<SQLContext> options) : base(options)
    {
    }
}

public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    public DateOnlyConverter() : base(
            d => d.ToDateTime(TimeOnly.MinValue),
            d => DateOnly.FromDateTime(d))
    { }
}