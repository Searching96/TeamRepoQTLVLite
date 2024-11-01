using Library_DAL;
using Library_DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_BUS
{
    public class UserManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Create
        public void AddUser(string username, string password, string typeOfUser, string email)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty", nameof(password));
            if (string.IsNullOrWhiteSpace(typeOfUser))
                throw new ArgumentException("User type cannot be empty", nameof(typeOfUser));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));

            var user = new User 
            { 
                Username = username, 
                Password = password, 
                TypeOfUser = typeOfUser, 
                Email = email 
            };
            
            _unitOfWork.Users.Add(user);
            _unitOfWork.SaveChanges();
        }

        // Read
        public User GetByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));
            
            return _unitOfWork.Users.GetByUsername(username);
        }

        public List<User> GetAllUsers()
        {
            return _unitOfWork.Users.GetAll();
        }

        // Update
        public void UpdateUser(string username, string password, string typeOfUser, string email)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));

            var user = _unitOfWork.Users.GetByUsername(username);
            if (user == null)
                throw new InvalidOperationException($"User with username {username} not found");

            if (!string.IsNullOrWhiteSpace(password))
                user.Password = password;
            if (!string.IsNullOrWhiteSpace(typeOfUser))
                user.TypeOfUser = typeOfUser;
            if (!string.IsNullOrWhiteSpace(email))
                user.Email = email;

            _unitOfWork.Users.Update(user);
            _unitOfWork.SaveChanges();
        }

        // Delete
        public void RemoveUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));

            _unitOfWork.Users.Remove(username);
            _unitOfWork.SaveChanges();
        }

        public bool ValidateUser(string username, string password)
        {
            var user = _unitOfWork.Users.GetByUsername(username);
            return user != null && user.Password == password;
        }

        public string GetUserType(string username)
        {
            var user = _unitOfWork.Users.GetByUsername(username);
            return user?.GetType().Name;
        }

        public int Count()
        {
            return _unitOfWork.Users.GetAll().Count();
        }



    }
}
