//Insert + Modify + Remove
using Library_DAL;
using Library_DTO;
using System;
using System.Collections.Generic;

namespace Library_BUS
{
    public class BookManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddBook(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title is required.");
            }

            var book = new Book { Title = title };
            _unitOfWork.Books.Add(book);
            _unitOfWork.SaveChanges();
        }

        public void UpdateBook(int id, string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title is required.");
            }

            var book = _unitOfWork.Books.GetById(id);
            if (book == null)
            {
                throw new ArgumentException("Book not found.");
            }

            book.Title = title;
            _unitOfWork.Books.Update(book);
            _unitOfWork.SaveChanges();
        }

        public void RemoveBook(int id)
        {
            var book = _unitOfWork.Books.GetById(id);
            if (book == null)
            {
                throw new ArgumentException("Book not found.");
            }
            if (book.BorrowId != null)
            {
                throw new ArgumentException("Book currently borrowed.");
            }
            _unitOfWork.Books.Remove(id);
            _unitOfWork.SaveChanges();
        }

        public int Count()
        {
            return _unitOfWork.Books.GetAll().Count();
        }

        public List<Book> GetAllBooks()
        {
            return _unitOfWork.Books.GetAll();
        }
    }
}
