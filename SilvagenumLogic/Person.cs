namespace SilvagenumLogic;

public enum Gender
{
    male,
    female,
    UNASSIGNED
}

public class Person
{
    public int Id { get; private set; }
    public Gender Gender { get; set; }
    public string Name { get; set; }
    public string? Surname { get; set; }
    public string FullName => $"{Name} {Surname}".Trim();

    public DateOnly? BirthDate { get; set; }
    public DateOnly? DeathDate { get; set; }
    public List<Person> Children { get; } = new List<Person>();

    private Person? _father;
    private Person? _mother;
    private static int personCount = 0;     //instance counter serving as a reference for the Id

    public Person(string firstName, Gender gender)
    {
        Name = firstName;
        personCount++;
        Id = personCount;
        Gender = gender;
    }

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

    public override string ToString() => $"{FullName} (#{Id})";
}