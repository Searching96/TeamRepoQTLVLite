using Library_DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_DAL
{
    public class ReaderTypeRepository
    {
        private readonly LibraryContext _context;

        public ReaderTypeRepository()
        {
            _context = new LibraryContext();
        }

        public void Add(ReaderType type)
        {
            _context.ReaderTypes.Add(type);
            _context.SaveChanges();
        }

        public void Update(ReaderType type)
        {
            _context.Entry(type).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Remove(string username)
        {
            var type = _context.ReaderTypes.Find(username);
            if (type != null)
            {
                _context.ReaderTypes.Remove(type);
                _context.SaveChanges();
            }
        }

        public ReaderType GetByTypename(string Typename)
        {
            return _context.ReaderTypes.FirstOrDefault(x => x.ReaderTypeName == Typename);
        }

        public ReaderType GetById(int id)
        {
            return _context.ReaderTypes.Find(id);
        }

        //public bool CheckExist(ReaderType type)
        //{
        //    return _context.ReaderTypes.Any(u => u.Username == type.Username);
        //}

        public int Count()
        {
            return _context.ReaderTypes.Count();
        }

        public List<ReaderType> GetAll()
        {
            return _context.ReaderTypes.ToList();
        }
    }
}
