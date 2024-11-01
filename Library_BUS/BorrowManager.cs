using Library_DAL;
using Library_DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Library_BUS
{
    public class BorrowManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public BorrowManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void ProcessBorrow(Reader reader, List<Tuple<Book, DateTime>> booklist, DateTime? time)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader), "Reader cannot be null");

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var borrow = new Borrow { Username = reader.Username, Date = time ?? DateTime.Now };
                    _unitOfWork.Borrows.Add(borrow);
                    _unitOfWork.SaveChanges();

                    foreach (var item in booklist)
                    {
                        var book = item.Item1;
                        if (book == null) 
                            throw new ArgumentNullException(nameof(book), "Book cannot be null");

                        if (book.BorrowId != null)
                            throw new InvalidOperationException($"Book {book.Title} is already borrowed.");

                        var borrowDetails = new BorrowDetail
                        {
                            BookId = book.BookId,
                            BorrowId = borrow.BorrowId,
                            EndDate = item.Item2
                        };
                        _unitOfWork.BorrowDetails.Add(borrowDetails);

                        book.BorrowId = borrow.BorrowId;
                        _unitOfWork.Books.Update(book);
                    }

                    reader.CurrentBorrows += booklist.Count;
                    _unitOfWork.Readers.Update(reader);
                    _unitOfWork.SaveChanges();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public List<Borrow> GetAllBorrows()
        {
            return _unitOfWork.Borrows.GetAll();
        }

        public Borrow GetById(int borrowId)
        {
            return _unitOfWork.Borrows.GetById(borrowId);
        }

        public int Count()
        {
            return _unitOfWork.Borrows.GetAll().Count();
        }

    }
}
