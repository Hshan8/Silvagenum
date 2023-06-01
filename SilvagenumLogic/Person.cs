using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilvagenumLogic;

public enum Gender
{
    male,
    female,
    UNASSIGNED
}

public class Person
{
    [Key]
    public int PersonId { get;  set; }
    public Gender Gender { get; set; }
    public string Name { get; set; }
    public string? Surname { get; set; }
    
    [NotMapped]
    public string FullName => $"{Name} {Surname}".Trim();

    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }
    
    
    [NotMapped]
    public List<Person> Children { get; } = new List<Person>();
    private Person? _father;
    private Person? _mother;
    private Person? parent;

    public Person()
    {
        
    }

    public Person(string firstName, Gender gender)
    {
        Name = firstName;
        Gender = gender;
    }
    
    //remove NotMapped when you map relationship in db or by EF corewith person
    [ForeignKey("Person")]
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
    //remove NotMapped when you map relationship in db or by EF corewith person
    [ForeignKey("Person")]
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

    public override string ToString() => $"{FullName} (#{PersonId})";
}