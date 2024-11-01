using Library_BUS;
using Library_DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_DAL
{
    public class ReturnRepository : IReturnRepository
    {
        private readonly LibraryContext _context;

        public ReturnRepository(LibraryContext context)
        {
            _context = context;
        }

        public void Add(Return _return)
        {
            _context.Returns.Add(_return);
            _context.SaveChanges();
        }

        public void Update(Return _return)
        {
            _context.Entry(_return).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Remove(int ReturnId)
        {
            var _return = _context.Returns.Find(ReturnId);
            if (_return != null)
            {
                _context.Returns.Remove(_return);
                //_context.Readers.First(x => x.Username == _return.Username).CurrentBorrows--;
                //_context.Books.First(x => x.BookId == return.ReturnBookId).isReturned = false;
                _context.SaveChanges();
            }
        }

        public Return GetById(int ReturnId)
        {
            return _context.Returns.Find(ReturnId);
        }

        public bool CheckExists(int ReturnBookId)
        {
            if (ReturnBookId == 0) return false;
            return _context.Returns.Any(x => x.ReturnId == ReturnBookId);
        }

        public int Count()
        {
            return _context.Returns.Count();
        }

        public List<Return> GetAll()
        {
            return _context.Returns.ToList();
        }
    }
}
