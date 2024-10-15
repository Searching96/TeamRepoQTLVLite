using Library_DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_DAL
{
    public class BookRepository
    {
        private readonly LibraryContext _context;

        public BookRepository()
        {
            _context = new LibraryContext();
        }

        public void Add(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void Update(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Remove(int BookId)
        {
            var book = _context.Books.Find(BookId);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }

        public Book GetById(int BookId)
        {
            return _context.Books.Find(BookId);
        }

        public int Count()
        {
            return _context.Books.Count();
        }

        public List<Book> GetAll()
        {
            return _context.Books.ToList();
        }
    }
}
