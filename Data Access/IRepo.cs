namespace SilvagenumWebApp.Models;

public interface IRepo
{
    public List<Person> GetAll();
    public Person? Get(int id);
    public List<Person>? Get(string name);
    public void Add(Person toBeAdded);
    public void Update(Person toBeUpdated);
    public void SetRelation(Person child, Person? parent, Gender gender);
    public void Delete(Person toBeDeleted);
    public void Save();
}