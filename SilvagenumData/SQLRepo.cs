using SilvagenumLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilvagenumData
{
    public class SQLRepo : IRepo
    {
        private readonly SQLContext _context;
        public SQLRepo()
        {
            _context = new SQLContext();
            _context.Database.EnsureCreated();          //for testing and demos? replace with some real-life solution?
        }
        public void Add(Person newPerson)
        {
            _context.People.Add(newPerson);
            _context.SaveChanges();
        }

        public void Delete(Person toBeDeleted)
        {
            _context.People.Remove(toBeDeleted);
        }

        public Person? Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<Person>? Get(string name)
        {
            throw new NotImplementedException();
        }

        public List<Person> GetAll()
        {
            List<Person> list = _context.People.ToList();
            return list;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
