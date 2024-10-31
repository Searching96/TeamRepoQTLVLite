using Library_DAL;
using Library_DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Library_BUS
{
    public class BorrowManager
    {
        private readonly BorrowRepository _borrowRepository;
        private readonly BorrowDetailRepository _borrowDetailRepository;
        private readonly ReaderRepository _readerRepository;
        private readonly BookRepository _bookRepository;

        public BorrowManager(BorrowRepository borrowRepository, BorrowDetailRepository borrowDetailRepository,
                             ReaderRepository readerRepository, BookRepository bookRepository)
        {
            _borrowRepository = borrowRepository;
            _borrowDetailRepository = borrowDetailRepository;
            _readerRepository = readerRepository;
            _bookRepository = bookRepository;
        }

        public void ProcessBorrow(Reader reader, List<Tuple<Book, DateTime>> booklist, DateTime? time)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader), "Reader cannot be null");
            // Check for max limit
            //if (reader.CurrentBorrows >= reader.MaxBorrows)
            //    throw new InvalidOperationException("Reader has reached their borrowing limit.");

            var borrow = new Borrow { Username = reader.Username, Date = time ?? DateTime.Now };
            _borrowRepository.Add(borrow);

            foreach (var item in booklist)
            {
                var book = item.Item1;
                if (book == null) throw new ArgumentNullException(nameof(book), "Book cannot be null");

                // Ensure book is available
                if (book.BorrowId != 0)
                    throw new InvalidOperationException($"Book {book.Title} is already borrowed.");

                // Add borrow details
                var borrowDetails = new BorrowDetail
                {
                    BookId = book.BookId,
                    BorrowId = borrow.BorrowId,
                    EndDate = item.Item2
                };
                _borrowDetailRepository.Add(borrowDetails);

                // Update book status
                book.BorrowId = borrow.BorrowId;
                _bookRepository.Update(book);

                // Increment reader's current borrows count
                reader.CurrentBorrows += 1;
            }

            // Update reader after processing all books
            _readerRepository.Update(reader);
        }

        public List<Borrow> GetAllBorrows()
        {
            return _borrowRepository.GetAll();
        }

        public Borrow GetById(int BorrowId)
        {
            return _borrowRepository.GetById(BorrowId);
        }
    }
}
