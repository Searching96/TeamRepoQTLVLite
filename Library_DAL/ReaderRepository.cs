using Microsoft.EntityFrameworkCore;
using Library_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library_BUS;

namespace Library_DAL
{
    public class ReaderRepository : IReaderRepository
    {
        private readonly LibraryContext _context;

        public ReaderRepository(LibraryContext context)
        {
            _context = context;
        }

        public void Add(Reader reader)
        {
            _context.Readers.Add(reader);
            _context.SaveChanges();
        }

        public void Update(Reader reader)
        {
            _context.Entry(reader).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Remove(string username)
        {
            var reader = _context.Readers.Find(username);
            if (reader != null)
            {
                _context.Readers.Remove(reader);
                _context.Users.Remove(reader.UsernameNavigation);
                _context.SaveChanges();
            }
        }

        public Reader GetByUsername(string Username)
        {
            return _context.Readers.FirstOrDefault( x => x.Username == Username);
        }

        public bool CheckExist(Reader reader)
        {
            return _context.Readers.Any(u => u.Username == reader.Username);
        }

        public int Count()
        {
            return _context.Readers.Count();
        }

        public List<Reader> GetAll()
        {
            return _context.Readers.ToList();
        }

        public List<Reader> GetByReaderType(string readerType)
        {
            throw new NotImplementedException();
        }
    }
}
