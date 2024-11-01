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
    public class BorrowRepository : IBorrowRepository
    {
        private readonly LibraryContext _context;

        public BorrowRepository(LibraryContext context)
        {
            _context = context;
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

        public List<Borrow> GetByUsername(string Username)
        {
            var query = _context.Borrows.AsQueryable();
            query = query.Where(x => x.Username == Username);
            return query.ToList();
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
