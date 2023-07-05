using SilvagenumLogic;

namespace SilvagenumData;

public class SQLRepo : IRepo
{
    private readonly SQLContext _context;
    public SQLRepo()
    {
        _context = new SQLContext();
        _context.Database.EnsureCreated();          //for testing and demos? replace with some real-life solution?
    }
    public Person Add(string firstName, Gender gender)
    {
        Person person = new(firstName, gender);
        _context.People.Add(person);
        _context.SaveChanges();
        return person;
    }

    public void Delete(Person toBeDeleted)
    {
        _context.People.Remove(toBeDeleted);
    }

    public Person? Get(int id)
    {
        Person? result = _context.People.Find(id);
        PopulateChildrenOf(result);
        return result;
    }

    public List<Person>? Get(string name)
    {
        name = name.ToLowerInvariant();
        return GetAll().FindAll(x => x.FullName.ToLowerInvariant().Contains(name));         //not the most efficient solution, but the easiest that retains the case insensitivity and searching both in the name and surname
    }

    public List<Person> GetAll()
    {
        return _context.People.ToList();
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public void PopulateChildrenOf(Person? person)
    {
        person?.Children.AddRange(_context.People.Where(x => x.FatherId == person.Id || x.MotherId == person.Id).ToList());
    }
}