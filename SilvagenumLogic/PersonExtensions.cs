﻿namespace SilvagenumLogic;

public static class PersonExtensions
{
    /// <summary>
    /// Test summary
    /// </summary>
    public static void ShowDetails(this Person person)
    {
        Console.WriteLine(person);
        Console.WriteLine(person.Gender);

        string? livingUntil = person.DeathDate?.ToString();
        Console.WriteLine($"Living: {person.BirthDate} - {livingUntil ?? "today"}");

        string? fatherName = person.Father?.ToString();
        Console.WriteLine($"Father: {fatherName ?? "undefined"}");

        string? motherName = person.Mother?.ToString();
        Console.WriteLine($"Mother: {motherName ?? "undefined"}\n");
    }

    private static string? IsValidAsParentOf(Person parent, Person child, Gender gender)
    {
        string parentName = gender == Gender.male ? "father" : "mother";

        if (parent.Gender != gender)
        {
            return $"Gender mismatched, {parentName} expected to be {gender}.\n";
        }

        if (child.BirthDate != null && parent.BirthDate != null)
        {
            int parentOlderBy = child.BirthDate.Value.Year - parent.BirthDate.Value.Year;
            if (parentOlderBy < Config.MinParentAge)
            {
                return $"Too young for a {parentName}, would be {parentOlderBy} at childbirth, while the minimum is set to {Config.MinParentAge}.\n";
            }
        }

        return null;
    }

    public static string? IsValidAsMotherOf(this Person mother, Person child)
    {
        if (child.BirthDate != null && mother.BirthDate != null)
        {
            int motherOlderBy = child.BirthDate.Value.Year - mother.BirthDate.Value.Year;
            if (motherOlderBy > Config.MaxMotherAge)
            {
                return $"Too old for a mother, would be {motherOlderBy} at childbirth, while the maximum is set to {Config.MaxMotherAge}.\n";
            }
        }
        if (child.BirthDate != null && mother.DeathDate != null)
        {
            if (mother.DeathDate.Value < child.BirthDate.Value)
            {
                return "Person has been dead before the child was born.\n";
            }
        }
        return IsValidAsParentOf(mother, child, Gender.female);
    }

    public static string? IsValidAsFatherOf(this Person father, Person child)
    {
        if (child.BirthDate != null && father.DeathDate != null)
        {
            if (father.DeathDate.Value.AddMonths(9) < child.BirthDate.Value)
            {
                return "Person has been dead too long before the child was born.\n";
            }
        }
        return IsValidAsParentOf(father, child, Gender.male);
    }

    public static void SetAsChildOf(this Person child, Person parent)
    {
        if (parent.Gender == Gender.male)
        {
            child.Father = parent;
            if (child.Father == parent)
            {
                parent.Children.Add(child);
            }
        }
        else
        {
            child.Mother = parent;
            if (child.Mother == parent)
            {
                parent.Children.Add(child);
            }
        }
    }

    public static void DeleteAllRelations(this Person person)
    {
        person.Father = null;
        person.Mother = null;
        if (person.Gender == Gender.male)
        {
            foreach (Person child in person.Children)
            {
                child.Father = null;
            }
        }
        else
        {
            foreach (Person child in person.Children)
            {
                child.Mother = null;
            }
        }
    }
}