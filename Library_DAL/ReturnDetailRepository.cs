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
    public class ReturnDetailRepository : IReturnDetailRepository
    {
        private readonly LibraryContext _context;

        public ReturnDetailRepository(LibraryContext context)
        {
            _context = context;
        }

        public void Add(ReturnDetail _return)
        {
            _context.ReturnDetails.Add(_return);
            _context.SaveChanges();
        }

        public void Update(ReturnDetail _return)
        {
            _context.Entry(_return).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Remove(string username)
        {
            var _return = _context.ReturnDetails.Find(username);
            if (_return != null)
            {
                _context.ReturnDetails.Remove(_return);
                _context.SaveChanges();
            }
        }

        public ReturnDetail GetByUsername(string Username)
        {
            return _context.ReturnDetails.Find(Username);
        }

        public List<ReturnDetail> GetByReturnId(int ReturnId)
        {
            var query = _context.ReturnDetails.AsQueryable();
            query = query.Where(r => r.ReturnId == ReturnId);
            return query.ToList();
        }

        public ReturnDetail GetByBookId(int BookId)
        {
                var query = _context.ReturnDetails.AsQueryable();
                query = query.Where(r => r.BookId == BookId);
                return query.ToList().Last();
        }

        public int Count()
        {
            return _context.ReturnDetails.Count();
        }

        public List<ReturnDetail> GetAll()
        {
            return _context.ReturnDetails.ToList();
        }
    }
}
