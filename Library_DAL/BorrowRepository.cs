using Library_DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_DAL
{
    public class BorrowRepository
    {
        private readonly LibraryContext _context;

        public BorrowRepository()
        {
            _context = new LibraryContext();
        }

        public void Add(Borrow borrow)
        {
            if (_context.Borrows.FirstOrDefault() == null)
                borrow.BorrowId = 1;
            else borrow.BorrowId = _context.Borrows.First().BorrowId + 1;
            _context.Borrows.Add(borrow);
            _context.Users.First(x => x.Username == borrow.Username).BookCount++;
            _context.Books.First(x => x.BookId == borrow.BorrowBookId).isBorrowed = true;
            _context.SaveChanges();
        }

        public void Update(Borrow borrow)
        {
            _context.Entry(borrow).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Remove(int BorrowId)
        {
            var borrow = _context.Borrows.Find(BorrowId);
            if (borrow != null)
            {
                _context.Borrows.Remove(borrow);
                _context.Users.First(x => x.Username == borrow.Username).BookCount--;
                _context.Books.First(x => x.BookId == borrow.BorrowBookId).isBorrowed = false;
                _context.SaveChanges();
            }
        }

        public Borrow GetById(int BorrowId)
        {
            return _context.Borrows.Find(BorrowId);
        }

        public bool CheckExists(int BorrowBookId)
        {
            if (BorrowBookId == 0) return false;
            return _context.Borrows.Any(x => x.BorrowBookId == BorrowBookId);
        }

        public int Count()
        {
            return _context.Borrows.Count();
        }

        public List<Borrow> GetAll()
        {
            return _context.Borrows.ToList();
        }
    }
}
