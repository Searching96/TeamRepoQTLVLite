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
            _context.Borrows.Add(borrow);
            _context.SaveChanges();
        }

        public void Update(Borrow borrow)
        {
            _context.Entry(borrow).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Remove(Borrow borrow)
        {
            var item = _context.Borrows.Find(borrow.BorrowId);
            if (item != null)
            {
                _context.Borrows.Remove(item);
                _context.SaveChanges();
            }
        }

        public Borrow GetById(int BorrowId)
        {
            return _context.Borrows.LastOrDefault(r => r.BorrowId == BorrowId);
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
