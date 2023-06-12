using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilvagenumLogic;

public enum Gender
{
    male,
    female
}

public enum LevelOfDetail
{
    simple,
    detailed
}

public class Person
{
    public int Id { get; set; }
    public Gender Gender { get; set; }
    public string Name { get; set; }
    public string? Surname { get; set; }
    public string FullName => $"{Name} {Surname}".Trim();

    public DateOnly? BirthDate { get; set; }
    public DateOnly? DeathDate { get; set; }
    public List<Person> Children { get; private set; } = new List<Person>();

    private Person? _father;
    private Person? _mother;
    [NotMapped]
    private static int personCount = 0;     //instance counter serving as a reference for the Id

    public Person(string name, Gender gender)
    {
        Name = name;
        Gender = gender;
        if (Config.GenerateIdsInternally)
        {
            personCount++;
            Id = personCount;
        }
    }

    [ForeignKey(nameof(Father))]
    public int? FatherId { get; set; }

    public Person? Father
    {
        get { return _father; }
        set
        {
            string? invalidReason = value?.IsValidAsFatherOf(this);
            if (invalidReason is null)
            {
                _father?.Children.Remove(this);
                _father = value;
                _father?.Children.Add(this);
            }
            else
            {
                Console.WriteLine($"Incorrect assignment! {invalidReason}. Father left unchanged.\n");
            }
        }
    }

    [ForeignKey(nameof(Mother))]
    public int? MotherId { get; set; }

    public Person? Mother
    {
        get { return _mother; }
        set
        {
            string? invalidReason = value?.IsValidAsMotherOf(this);
            if (invalidReason is null)
            {
                _mother?.Children.Remove(this);
                _mother = value;
                _mother?.Children.Add(this);
            }
            else
            {
                Console.WriteLine($"Incorrect assignment! {invalidReason}. Mother left unchanged.\n");
            }
        }
    }

    public override string ToString() => ToString(LevelOfDetail.simple);
    public string ToString(LevelOfDetail levelOfDetail)
    {
        return levelOfDetail switch
        {
            LevelOfDetail.simple => $"{FullName} (#{Id})",
            LevelOfDetail.detailed => $"{this}\n"
                            + $"{Gender}\n"
                            + $"Living: {BirthDate} - {(DeathDate?.ToString()) ?? "today"}\n"
                            + $"Father: {(Father?.ToString()) ?? "undefined"}\n"
                            + $"Mother: {(Mother?.ToString()) ?? "undefined"}\n",
            _ => $"{FullName} (#{Id})",
        };
    }
}