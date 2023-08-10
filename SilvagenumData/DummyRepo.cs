using SilvagenumLogic;

namespace SilvagenumData;
public class DummyRepo : IRepo
{
    private readonly List<Person>? content;
    private static int idCounter = 0;

    public DummyRepo()
    {
        content = new List<Person>();
        Add("Dumbo", Gender.male);
        Add("Stefan", Gender.male);
        Add("Genowefa", Gender.female);
        Add("Kunegunda", Gender.female);
        Add("Zygryd", Gender.male);
    }

    public List<Person> GetAll() => content!;

    public Person? Get(int id)
    {
        return content!.Find(x => x.Id == id);
    }

    public List<Person>? Get(string name)
    {
        name = name.ToLowerInvariant();
        return content!.FindAll(x => x.FullName.ToLowerInvariant().Contains(name));
    }

    public void Delete(Person toBeDeleted)
    {
        toBeDeleted.DeleteAllRelations();
        content!.Remove(toBeDeleted);
    }

    public Person Add(string firstName, Gender gender)
    {
        Person person = new(firstName, gender);
        idCounter++;
        person.Id = idCounter;
        content!.Add(person);
        return person;
    }

    public void Save()
    {
        Console.WriteLine("This is a dummy repo, it does not save changes after the app closes!");
    }
}