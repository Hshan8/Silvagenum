namespace SilvagenumWebApp.Models;

public static class Config          //simplified solution until a rules engine is implemented
{
    private static readonly int maxLifespan = 120;
    private static readonly int minParentAge = 15;
    private static readonly int maxMotherAge = 60;

    public static int MaxLifespan => maxLifespan;
    public static int MinParentAge => minParentAge;
    public static int MaxMotherAge => maxMotherAge;
}