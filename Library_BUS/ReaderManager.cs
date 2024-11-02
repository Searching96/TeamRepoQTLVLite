using Library_DAL;
using Library_DTO;
using System;
using System.Collections.Generic;

namespace Library_BUS
{
    public class ReaderManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReaderManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddReader(string username, string firstName, string lastName, int readerTypeId, DateTime startDate)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be empty", nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be empty", nameof(lastName));
            if (readerTypeId <= 0)
                throw new ArgumentException("Invalid reader type ID", nameof(readerTypeId));
            if (startDate == default)
                throw new ArgumentException("Start date must be set", nameof(startDate));

            // Check if the user already exists in the user table
            var user = _unitOfWork.Users.GetByUsername(username);
            if (user == null)
                throw new InvalidOperationException("The user does not exist in the system.");

            // Validate reader type
            var readerType = _unitOfWork.ReaderTypes.GetById(readerTypeId);
            if (readerType == null)
                throw new InvalidOperationException("Invalid reader type.");

            // Create new reader
            var reader = new Reader
            {
                Username = username,
                FirstName = firstName,
                LastName = lastName,
                ReaderTypeId = readerTypeId,
                StartDate = startDate,
                EndDate = startDate.AddYears(1), // Example duration for membership, adjust as needed
                CurrentBorrows = 0,
                TotalDebt = 0,
                ReaderType = readerType,
                UsernameNavigation = user
            };
            
            _unitOfWork.Readers.Add(reader);
            _unitOfWork.SaveChanges();
        }

        public Reader GetByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));
            
            return _unitOfWork.Readers.GetByUsername(username);
        }

        public List<Reader> GetAllReaders()
        {
            return _unitOfWork.Readers.GetAll();
        }

        public void UpdateReader(string username, string firstName, string lastName, int readerTypeId)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));

            var reader = _unitOfWork.Readers.GetByUsername(username);
            if (reader == null)
                throw new InvalidOperationException($"Reader with username {username} not found");

            reader.FirstName = firstName;
            reader.LastName = lastName;
            reader.ReaderTypeId = readerTypeId;

            _unitOfWork.Readers.Update(reader);
            _unitOfWork.SaveChanges();
        }

        public bool CanBorrow(Reader reader)
        {
            return true;
        }

        public void RemoveReader(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));

            _unitOfWork.Readers.Remove(username);
            _unitOfWork.SaveChanges();
        }

        public int Count()
        {
            return _unitOfWork.Readers.GetAll().Count();
        }

    }
}
