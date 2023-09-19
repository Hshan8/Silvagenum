using Microsoft.EntityFrameworkCore;

namespace SilvagenumWebApp.Models;

public class SQLRepo : IRepo
{
    private readonly SQLContext _context;

    public SQLRepo(SQLContext context)
    {
        _context = context;
    }

    public List<Person> GetAll() => _context.People.ToList();

    public List<Person> GetAll(int? pageNumber, int pageSize)
    {
        pageNumber ??= 1;
        IQueryable<Person> people = _context.People;
        people = people.Skip((pageNumber.Value - 1) * pageSize).Take(pageSize);
        return people.AsNoTracking().ToList();
    }

    public int GetCount() => _context.People.Count();

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

    public void Add(Person person)
    {
        _context.People.Add(person);
        Save();
    }

    public void Update(Person toBeUpdated)
    {
        Person? updatedPerson = _context.People.FirstOrDefault(p => p.Id == toBeUpdated.Id) ?? throw new ArgumentException("The person to update could not be found.");

        updatedPerson.Name = toBeUpdated.Name;
        updatedPerson.Surname = toBeUpdated.Surname;
        updatedPerson.Gender = toBeUpdated.Gender;
        updatedPerson.BirthDate = toBeUpdated.BirthDate;
        updatedPerson.DeathDate = toBeUpdated.DeathDate;

        _context.People.Update(updatedPerson);
        Save();
    }

    public void SetRelation(Person child, Person? parent, Gender gender)
    {
        if (gender == Gender.male)
        {
            child.Father = parent;
        }
        else
        {
            child.Mother = parent;
        }
        _context.People.Update(child);
        Save();
    }

    public void Delete(Person toBeDeleted)
    {
        if (toBeDeleted == null)
            throw new ArgumentException("The person to delete could not be found.");

        PopulateChildrenOf(toBeDeleted);
        toBeDeleted.DeleteAllRelations();
        _context.People.Remove(toBeDeleted);
        Save();
    }

    public void Save() => _context.SaveChangesAsync();

    private void PopulateChildrenOf(Person? person)
    {
        person?.Children.AddRange(_context.People.Where(x => x.FatherId == person.Id || x.MotherId == person.Id).ToList());
    }
}