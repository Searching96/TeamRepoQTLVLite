using Library_DAL;
using Library_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_BUS
{
    class BorrowManager
    {
        private readonly BorrowRepository _borrowRepository;

        public BorrowManager()
        {
            _borrowRepository = new BorrowRepository();
        }

        public void AddBorrow(string username, int borrowBookId)
        {
            var borrow = new Borrow { Username = username, BorrowBookId = borrowBookId, StartDate = DateTime.Now };
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username are required.");
            }
            if (_borrowRepository.CheckExists(borrowBookId))
            {
                throw new ArgumentException("Book is already borrowed.");
            }
            _borrowRepository.Add(borrow);
        }

        public void RemoveBorrow(int id)
        {
            var borrow = _borrowRepository.GetById(id);
            if (borrow == null)
            {
                throw new ArgumentException("Borrow not found.");
            }
            _borrowRepository.Remove(id);
        }

        public List<Borrow> GetAllBorrows()
        {
            return _borrowRepository.GetAll();
        }
    }
}
