//Insert + Modify + Remove
using Library_DAL;
using Library_DTO;
using System;
using System.Collections.Generic;

namespace Library_BUS
{
    public class BookManager
    {
        private readonly BookRepository _bookRepository;

        public BookManager()
        {
            _bookRepository = new BookRepository();
        }

        public void AddBook(string title, string author, string iSBN)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author) || string.IsNullOrWhiteSpace(iSBN))
            {
                throw new ArgumentException("Title, author and ISBN are required.");
            }

            var book = new Book { Title = title, Author = author, ISBN = iSBN };
            _bookRepository.Add(book);
        }

        public void UpdateBook(int id, string title, string author, string iSBN)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author) || string.IsNullOrWhiteSpace(iSBN))
            {
                throw new ArgumentException("Title, author and ISBN are required.");
            }

            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                throw new ArgumentException("Book not found.");
            }

            book.Title = title;
            book.Author = author;
            book.ISBN = iSBN;
            _bookRepository.Update(book);
        }

        public void RemoveBook(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                throw new ArgumentException("Book not found.");
            }

            _bookRepository.Remove(id);
        }

        public List<Book> GetAllBooks()
        {
            return _bookRepository.GetAll();
        }
    }
}
