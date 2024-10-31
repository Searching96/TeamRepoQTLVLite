using Library_DAL;
using Library_DTO;
using System;
using System.Collections.Generic;

namespace Library_BUS
{
    public class ReturnManager
    {
        private readonly ReturnRepository _returnRepository;
        private readonly BorrowRepository _borrowRepository;
        private readonly BorrowDetailRepository _borrowDetailRepository;
        private readonly ReturnDetailRepository _returnDetailRepository;
        private readonly ReaderRepository _readerRepository;
        private readonly BookRepository _bookRepository;

        public ReturnManager(ReturnRepository returnRepository, BorrowRepository borrowRepository,
                             BorrowDetailRepository borrowDetailRepository, ReturnDetailRepository returnDetailRepository,
                             ReaderRepository readerRepository, BookRepository bookRepository)
        {
            _returnRepository = returnRepository;
            _returnDetailRepository = returnDetailRepository;
            _borrowRepository = borrowRepository;
            _borrowDetailRepository = borrowDetailRepository;
            _readerRepository = readerRepository;
            _bookRepository = bookRepository;
        }

        public void ProcessReturn(Reader reader, List<Book> bookList, DateTime? returnDate = null)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader), "Reader cannot be null");

            DateTime actualReturnDate = returnDate ?? DateTime.Now;
            decimal totalFine = 0;

            foreach (var book in bookList)
            {
                if (book == null)
                    throw new ArgumentNullException(nameof(book), "Book cannot be null");

                // Get borrow details to ensure the book is borrowed by the reader
                var borrowDetail = _borrowDetailRepository.GetByBookId(book.BookId);

                if (borrowDetail == null || borrowDetail.BorrowId == 0)
                    throw new InvalidOperationException($"Book {book.Title} is not currently borrowed.");

                if (_borrowRepository.GetById(borrowDetail.BorrowId).Username != reader.Username)
                    throw new InvalidOperationException($"Book {book.Title} was not borrowed by this user.");

                // Calculate fine if the book is overdue
                if (actualReturnDate > borrowDetail.EndDate)
                {
                    TimeSpan overdueDuration = actualReturnDate - borrowDetail.EndDate;
                    decimal fine = CalculateFine(overdueDuration.Days);
                    totalFine += fine;
                }

                // Update book status to make it available
                book.BorrowId = 0;
                _bookRepository.Update(book);

                // Decrement the reader's current borrow count
                reader.CurrentBorrows -= 1;

                // Record the return in the Returns and ReturnDetails tables
                var returnRecord = new Return
                {
                    Username = reader.Username,
                    Date = actualReturnDate
                };
                _returnRepository.Add(returnRecord);

                var returnDetail = new ReturnDetail
                {
                    BookId = book.BookId,
                    ReturnId = returnRecord.ReturnId,
                    ReturnDate = actualReturnDate,
                    Note = totalFine > 0 ? $"Fine: {totalFine:C}" : "No Fine"
                };
                _returnDetailRepository.Add(returnDetail);
            }

            // Update reader's fine and borrow count in the database
            if (totalFine > 0)
                reader.TotalDebt = reader.TotalDebt + totalFine;

            _readerRepository.Update(reader);
        }

        private decimal CalculateFine(int overdueDays)
        {
            const decimal dailyFine = 1.00m; // Example daily fine rate
            return overdueDays * dailyFine;
        }

        public List<Return> GetAllReturns()
        {
            return _returnRepository.GetAll();
        }
    }
}
