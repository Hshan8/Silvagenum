using SilvagenumLogic;

namespace SilvagenumData;
public class DummyRepo : IRepo
{
    private List<Person>? content;

    public DummyRepo()
    {
        content = new List<Person>()
        {
            new Person("Dumbo", Gender.male),
            new Person("Stefan", Gender.male),
            new Person("Genowefa", Gender.female),
            new Person("Kunegunda", Gender.female),
            new Person("Zygryd", Gender.male)
        };
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

    public void Add(Person newPerson)
    {
        content!.Add(newPerson);
    }
}