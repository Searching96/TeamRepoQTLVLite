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
    public class BorrowDetailRepository : IBorrowDetailRepository
    {
        private readonly LibraryContext _context;

        public BorrowDetailRepository(LibraryContext context)
        {
            _context = context;
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

        public List<BorrowDetail> GetByBorrowId(int BorrowId)
        {
            var query = _context.BorrowDetails.AsQueryable();
            query = query.Where(r => r.BorrowId == BorrowId);
            return query.ToList();
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
