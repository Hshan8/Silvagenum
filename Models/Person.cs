using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilvagenumWebApp.Models;

public enum Gender
{
    male,
    female
}

public class Person
{
    
    public int Id { get; set; }

    [Required(ErrorMessage = "Please select a gender")]
    [Display(Name = "Gender")]
    public Gender Gender { get; set; }

    [Required(ErrorMessage = "Please enter the first name")]
    [Display(Name = "First name")]
    [StringLength(50)]
    public string? Name { get; set; }

    [Display(Name = "Last name")]
    [StringLength (50)]
    public string? Surname { get; set; }

    [BindNever]
    public string FullName => $"{Name} {Surname}".Trim();

    [StringLength(50)]
    [DataType(DataType.Date)]
    public DateOnly? BirthDate { get; set; }

    [StringLength(50)]
    [DataType(DataType.Date)]
    public DateOnly? DeathDate { get; set; }
    public List<Person> Children { get; private set; } = new List<Person>();


    [ForeignKey(nameof(Father))]
    public int? FatherId { get; set; }

    private Person? _father;
    public Person? Father
    {
        get => _father;
        set => _father = this.TryChangingParent(value, _father, Gender.male);
    }

    [ForeignKey(nameof(Mother))]
    public int? MotherId { get; set; }

    private Person? _mother;
    public Person? Mother
    {
        get => _mother;
        set => _mother = this.TryChangingParent(value, _mother, Gender.female);
    }

    public Person(string name, Gender gender)
    {
        Name = name;
        Gender = gender;
    }

    public Person()
    {
        
    }

    public override string ToString() => $"{FullName} (#{Id})";
}