using Microsoft.EntityFrameworkCore;
using Library_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_DAL
{
    public class AdminRepository
    {
        private readonly LibraryContext _context;

        public AdminRepository()
        {
            _context = new LibraryContext();
        }

        public void Add(Admin admin)
        {
            _context.Admins.Add(admin);
            _context.SaveChanges();
        }

        public void Update(Admin admin)
        {
            _context.Entry(admin).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Remove(string Adminname)
        {
            var admin = _context.Admins.Find(Adminname);
            if (admin != null)
            {
                _context.Admins.Remove(admin);
                _context.SaveChanges();
            }
        }

        public Admin GetByUsername(string Username)
        {
            return _context.Admins.Find(Username);
        }

        public bool CheckExist(Admin admin)
        {
            return _context.Admins.Any(u => u.Username == admin.Username);
        }

        public int Count()
        {
            return _context.Admins.Count();
        }

        public List<Admin> GetAll()
        {
            return _context.Admins.ToList();
        }
    }
}
