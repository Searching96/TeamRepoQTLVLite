using Library_DAL;
using Library_DTO;
using System;
using System.Collections.Generic;

namespace Library_BUS
{
    public class ReturnManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReturnManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void ProcessReturn(Borrow borrow, List<Book> books)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var returnRecord = new Return 
                    { 
                        Username = borrow.Username,
                        Date = DateTime.Now 
                    };
                    _unitOfWork.Returns.Add(returnRecord);
                    _unitOfWork.SaveChanges();

                    foreach (var book in books)
                    {
                        var returnDetail = new ReturnDetail
                        {
                            ReturnId = returnRecord.ReturnId,
                            BookId = book.BookId
                        };
                        _unitOfWork.ReturnDetails.Add(returnDetail);

                        book.BorrowId = null;
                        _unitOfWork.Books.Update(book);
                    }

                    var reader = _unitOfWork.Readers.GetByUsername(borrow.Username);
                    reader.CurrentBorrows -= books.Count;
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

        public List<Return> GetAllReturns()
        {
            return _unitOfWork.Returns.GetAll();
        }

        public int Count()
        {
            return _unitOfWork.Returns.GetAll().Count();
        }

    }
}
