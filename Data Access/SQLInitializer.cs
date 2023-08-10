namespace SilvagenumWebApp.Models
{
    public static class SQLInitializer
    {
        public static void Seed(IApplicationBuilder appBuilder)
        {
            SQLContext context = appBuilder.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<SQLContext>();
            if (!context.People.Any())
            {
                context.AddRange
                (
                    new Person("Damon", Gender.male) { Surname = "Lannister" },
                    new Person("Cerissa", Gender.female) { Surname = "Brax" },
                    new Person("Gerold", Gender.male) { Surname = "Lannister" },
                    new Person("Tybolt", Gender.male) { Surname = "Lannister" },
                    new Person("Alysanne", Gender.female) { Surname = "Farman" },
                    new Person("Rohanne", Gender.female) { Surname = "Webber" },
                    new Person("Teora", Gender.female) { Surname = "Kyndall" },
                    new Person("Cerelle", Gender.female) { Surname = "Lannister" },
                    new Person("Tywald", Gender.male) { Surname = "Lannister" },
                    new Person("Tion", Gender.male) { Surname = "Lannister" },
                    new Person("Tytos", Gender.male) { Surname = "Lannister" },
                    new Person("Jason", Gender.male) { Surname = "Lannister" },
                    new Person("Jeyne", Gender.female) { Surname = "Marbrand" },
                    new Person("Alys", Gender.female) { Surname = "Stackspear" },
                    new Person("Marla", Gender.male) { Surname = "Prester" }
                );
            }
            context.SaveChanges();
        }
    }
}
