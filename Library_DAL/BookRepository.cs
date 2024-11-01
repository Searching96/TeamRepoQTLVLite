using Library_BUS;
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
    public class BookRepository : IBookRepository
    {
        private readonly LibraryContext _context;

        public BookRepository(LibraryContext context)
        {
            _context = context;
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

        public void Remove(int bookId)
        {
            var book = _context.Books.Find(bookId);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }

        public Book GetById(int bookId)
        {
            return _context.Books.Find(bookId);
        }

        public Book GetByTitle(string title)
        {
            return _context.Books.FirstOrDefault(x => x.Title == title);
        }

        public List<Book> GetAvailable()
        {
            return _context.Books.Where(r => r.BorrowId == null).ToList();
        }

        public List<Book> GetAll()
        {
            return _context.Books.ToList();
        }
    }
}
