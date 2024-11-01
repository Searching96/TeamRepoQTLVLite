using Library_DAL;
using Library_DTO;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_BUS
{
    public interface IUnitOfWork : IDisposable
    {
        IAdminRepository Admins { get; }
        IBookRepository Books { get; }
        IBorrowRepository Borrows { get; }
        IBorrowDetailRepository BorrowDetails { get; }
        IReaderRepository Readers { get; }
        IReaderTypeRepository ReaderTypes { get; }
        IReturnRepository Returns { get; }
        IReturnDetailRepository ReturnDetails { get; }
        IUserRepository Users { get; }

        void SaveChanges();
        IDbContextTransaction BeginTransaction();
    }

    public interface IGenericRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        List<T> GetAll();
    }

    public interface IBookRepository : IGenericRepository<Book>
    {
        void Remove(int bookId);
        Book GetById(int bookId);
        Book GetByTitle(string title);
        List<Book> GetAvailable();
    }

    public interface IAdminRepository : IGenericRepository<Admin>
    {
        Admin GetByUsername(string username);
        void Remove(string username);
    }

    public interface IBorrowRepository : IGenericRepository<Borrow>
    {
        Borrow GetById(int borrowId);
        List<Borrow> GetByUsername(string username);
    }

    public interface IBorrowDetailRepository : IGenericRepository<BorrowDetail>
    {
        List<BorrowDetail> GetByBorrowId(int borrowId);
        BorrowDetail GetByBookId(int bookId);
    }

    public interface IReaderTypeRepository : IGenericRepository<ReaderType>
    {
        ReaderType GetById(int readerTypeId);
        void Remove(string name);
    }

    public interface IReturnRepository : IGenericRepository<Return>
    {
        Return GetById(int returnId);
    }

    public interface IReturnDetailRepository : IGenericRepository<ReturnDetail>
    {
        List<ReturnDetail> GetByReturnId(int returnId);
        ReturnDetail GetByBookId(int bookId);
    }

    public interface IReaderRepository : IGenericRepository<Reader>
    {
        Reader GetByUsername(string username);
        void Remove(string username);
        List<Reader> GetByReaderType(string readerType);
    }

    public interface IUserRepository : IGenericRepository<User>
    {
        void Remove(string username);
        User GetByUsername(string username);
        bool CheckExist(User user);
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryContext _context;
        private bool _disposed = false;

        public IAdminRepository Admins { get; private set; }
        public IBookRepository Books { get; private set; }
        public IBorrowRepository Borrows { get; private set; }
        public IBorrowDetailRepository BorrowDetails { get; private set; }
        public IReaderRepository Readers { get; private set; }
        public IReaderTypeRepository ReaderTypes { get; private set; }
        public IReturnRepository Returns { get; private set; }
        public IReturnDetailRepository ReturnDetails { get; private set; }
        public IUserRepository Users { get; private set; }

        public UnitOfWork(LibraryContext context)
        {
            _context = context;
            InitializeRepositories();
        }

        private void InitializeRepositories()
        {
            Admins = new AdminRepository(_context);
            Books = new BookRepository(_context);
            Borrows = new BorrowRepository(_context);
            BorrowDetails = new BorrowDetailRepository(_context);
            Readers = new ReaderRepository(_context);
            ReaderTypes = new ReaderTypeRepository(_context);
            Returns = new ReturnRepository(_context);
            ReturnDetails = new ReturnDetailRepository(_context);
            Users = new UserRepository(_context);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
