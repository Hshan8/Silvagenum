namespace SilvagenumLogic;

public static class Config          //simplified solution until a bootstrapper and a config file are implemented
{
    private static readonly int maxLifespan = 120;
    private static readonly int minParentAge = 15;
    private static readonly int maxMotherAge = 60;
    private static readonly bool generateIdsInternally = false;

    public static int MaxLifespan => maxLifespan;
    public static int MinParentAge => minParentAge;
    public static int MaxMotherAge => maxMotherAge;
    public static bool GenerateIdsInternally => generateIdsInternally;
}