using Library_DAL;
using Library_DTO;
using System;
using System.Collections.Generic;

namespace Library_BUS
{
    public class ReaderManager
    {
        private readonly ReaderRepository _readerRepository;
        private readonly ReaderTypeRepository _readerTypeRepository;
        private readonly UserRepository _userRepository;

        public ReaderManager(ReaderRepository readerRepository,
                             ReaderTypeRepository readerTypeRepository,
                             UserRepository userRepository)
        {
            _readerRepository = readerRepository;
            _readerTypeRepository = readerTypeRepository;
            _userRepository = userRepository;
        }

        // Method to add a new reader
        public void AddReader(string username, string firstName, string lastName, int readerTypeId, DateTime startDate)
        {
            // Check if the user already exists in the user table
            var user = _userRepository.GetByUsername(username);
            if (user == null)
                throw new InvalidOperationException("The user does not exist in the system.");

            // Validate reader type
            var readerType = _readerTypeRepository.GetById(readerTypeId);
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

            _readerRepository.Add(reader);
        }

        // Method to update an existing reader's information
        public void UpdateReader(Reader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader), "Reader cannot be null");

            _readerRepository.Update(reader);
        }

        // Method to delete a reader
        public void DeleteReader(string username)
        {
            var reader = _readerRepository.GetByUsername(username);
            if (reader == null)
                throw new InvalidOperationException("Reader not found.");

            _readerRepository.Remove(reader);
        }

        // Method to get all readers
        public List<Reader> GetAllReaders()
        {
            return _readerRepository.GetAll();
        }

        // Method to get a reader by username
        public Reader GetReaderByUsername(string username)
        {
            var reader = _readerRepository.GetByUsername(username);
            if (reader == null)
                throw new InvalidOperationException("Reader not found.");

            return reader;
        }

        // Method to check if a reader is eligible for borrowing
        public bool CanBorrow(Reader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader), "Reader cannot be null");

            // Check if the reader has outstanding debt or has reached the borrowing limit
            if (reader.TotalDebt > 0)
                return false;

            var readerType = _readerTypeRepository.GetById(reader.ReaderTypeId);
            if (readerType == null)
                throw new InvalidOperationException("Invalid reader type.");

            // Assuming each reader type has a different borrowing limit
            return reader.CurrentBorrows < 1000/*readerType.MaxBorrows*/;
        }

        // Method to update debt for a reader
        public void UpdateDebt(string username, decimal additionalDebt)
        {
            var reader = _readerRepository.GetByUsername(username);
            if (reader == null)
                throw new InvalidOperationException("Reader not found.");

            reader.TotalDebt += additionalDebt;
            _readerRepository.Update(reader);
        }

        public List<Reader> GetAllReader()
        {
            return _readerRepository.GetAll();
        }
    }
}
