namespace SilvagenumWebApp.Models;

public enum LevelOfDetail
{
    simple,
    detailed
}

public static class PersonExtensions
{
    /// <summary>
    /// Do not use explicitly. This is parent setting logic and will not do anything useful when called outside of a Person object.
    /// </summary>
    /// <returns>
    /// Person passed as <paramref name="potentialParent"/> if they're examined as a valid parent, <paramref name="oldParent"/> otherwise.
    /// </returns>
    /// <param name="child">The person the parent change is attempted for. Normally <c>this</c> is passed in.</param>
    /// <param name="potentialParent">The person to be set as the new parent. If null is passed in the parent of the given <paramref name="gender"/> is deleted. Normally <c>value</c> is passed in.</param>
    /// <param name="oldParent">The current parent of the given <paramref name="gender"/>. Normally the appropriate parent backing field is passed in.</param>
    /// <param name="gender">The expected gender of the parent.</param>
    public static Person? TryChangingParent(this Person child, Person? potentialParent, Person? oldParent, Gender gender)
    {
        Person? newParent;
        string? invalidReason = potentialParent?.IsValidAsParentOf(child, gender);
        if (invalidReason is null)
        {
            oldParent?.Children.Remove(child);
            newParent = potentialParent;
            newParent?.Children.Add(child);
            return newParent;
        }
        else
        {
            Console.WriteLine($"Incorrect assignment! {invalidReason}. {gender.ParentTerm()} left unchanged.\n");
            return oldParent;
        }
    }

    private static string? IsValidAsParentOf(this Person parent, Person child, Gender gender)
    {
        string parentName = gender.ParentTerm().ToLower();

        if (parent.Gender != gender)
        {
            return $"Gender mismatched, {parentName} expected to be {gender}.\n";
        }

        if (child.BirthDate == null)
        {
            return null;
        }

        if (parent.BirthDate != null)
        {
            int parentOlderBy = child.BirthDate.Value.Year - parent.BirthDate.Value.Year;
            if (parentOlderBy < Config.MinParentAge)
            {
                return $"Too young for a {parentName}, would be {parentOlderBy} at childbirth, while the minimum is set to {Config.MinParentAge}.\n";
            }
            if (gender == Gender.female && parentOlderBy > Config.MaxMotherAge)
            {
                return $"Too old for a mother, would be {parentOlderBy} at childbirth, while the maximum is set to {Config.MaxMotherAge}.\n";
            }
        }

        if (parent.DeathDate != null)
        {
            DateOnly lastTimePossibleAsParent = gender == Gender.female ? parent.DeathDate.Value : parent.DeathDate.Value.AddMonths(9);
            if (lastTimePossibleAsParent < child.BirthDate)
            {
                return "Person has been dead (too long) before the child was born.\n";
            }
        }

        return null;
    }

    public static void SetParent(this Person child, Person parent)
    {
        SetOrDeleteParent(child, parent, parent.Gender);
    }

    public static void DeleteParent(this Person child, Gender gender)
    {
        SetOrDeleteParent(child, null, gender);
    }

    private static void SetOrDeleteParent(Person child, Person? parent, Gender gender)
    {
        if (gender == Gender.male)
        {
            child.Father = parent;
        }
        else
        {
            child.Mother = parent;
        }
    }

    public static void DeleteAllRelations(this Person person)
    {
        person.Father = null;
        person.Mother = null;
        foreach (Person child in person.Children)
        {
            child.DeleteParent(person.Gender);
        }
    }

    public static string DescribeParent(this Person child, Gender gender)
    {
        Person? parent = gender == Gender.male ? child.Father : child.Mother;
        if (parent == null)
        {
            return $"There is no {gender.ParentTerm().ToLower()} currently set for {child}.";
        }
        return $"The current {gender.ParentTerm().ToLower()} of {child} is {parent}.";
    }

    public static string ParentTerm(this Gender gender) => gender == Gender.male ? "Father" : "Mother";

    public static string ToString(this Person person, LevelOfDetail levelOfDetail)
    {
        return levelOfDetail switch
        {
            LevelOfDetail.simple => $"{person}",
            LevelOfDetail.detailed => $"{person}\n"
                            + $"{person.Gender}\n"
                            + $"Living: {person.BirthDate} - {(person.DeathDate?.ToString()) ?? "today"}\n"
                            + $"Father: {(person.Father?.ToString()) ?? "undefined"}\n"
                            + $"Mother: {(person.Mother?.ToString()) ?? "undefined"}\n",
            _ => $"{person}"
        };
    }
}