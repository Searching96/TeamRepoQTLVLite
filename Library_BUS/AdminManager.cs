using Library_DAL;
using Library_DTO;
using System;
using System.Collections.Generic;

namespace Library_BUS
{
    public class AdminManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddAdmin(string username, string firstname, string lastname)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(firstname) || string.IsNullOrWhiteSpace(lastname))
                throw new ArgumentException("Username and password are required.");

            var admin = new Admin { Username = username, FirstName = firstname, LastName = lastname };
            _unitOfWork.Admins.Add(admin);
            _unitOfWork.SaveChanges();
        }

        public Admin GetByUsername(string username)
        {
            return _unitOfWork.Admins.GetByUsername(username);
        }

        public List<Admin> GetAllAdmins()
        {
            return _unitOfWork.Admins.GetAll();
        }

        public int Count()
        {
            return _unitOfWork.Admins.GetAll().Count();
        }

    }
}