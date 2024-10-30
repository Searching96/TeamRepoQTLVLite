using Library_DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_DAL
{
    public class BorrowDetailRepository
    {
        private readonly LibraryContext _context;

        public BorrowDetailRepository()
        {
            _context = new LibraryContext();
        }

        public void Add(BorrowDetail _borrow)
        {
            _context.BorrowDetails.Add(_borrow);
            _context.SaveChanges();
        }

        public void Update(BorrowDetail _borrow)
        {
            _context.Entry(_borrow).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Remove(string username)
        {
            var _borrow = _context.BorrowDetails.Find(username);
            if (_borrow != null)
            {
                _context.BorrowDetails.Remove(_borrow);
                _context.SaveChanges();
            }
        }

        public BorrowDetail GetByUsername(string Username)
        {
            return _context.BorrowDetails.Find(Username);
        }

        public BorrowDetail GetByBookId(int BookId)
        {
            return _context.BorrowDetails.Find(BookId);
        }

        //public bool CheckExist(BorrowDetail _borrow)
        //{
        //    return _context.BorrowDetails.Any(u => u.Username == _borrow.Username);
        //}

        public int Count()
        {
            return _context.BorrowDetails.Count();
        }

        public List<BorrowDetail> GetAll()
        {
            return _context.BorrowDetails.ToList();
        }
    }
}
