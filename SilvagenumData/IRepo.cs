using SilvagenumLogic;

namespace SilvagenumData;

public interface IRepo
{
    public List<Person> GetAll();
    public Person? Get(int id);
    public List<Person>? Get(string name);
    public void Delete(Person toBeDeleted);
    public void Add(Person newPerson);
    public void Save();
    public void PopulateChildrenOf(Person? person) { }
}