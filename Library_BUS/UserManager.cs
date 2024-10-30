using Library_DAL;
using Library_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_BUS
{
    public class UserManager
    {
        private readonly UserRepository _userRepository;

        public UserManager()
        {
            _userRepository = new UserRepository();
        }

        public void AddUser(string Username, string Password, string TypeOfUser, string Email)
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(TypeOfUser) || string.IsNullOrWhiteSpace(Email))
            {
                throw new ArgumentException("All fields are required.");
            }

            var user = new User { Username = Username, Password = Password, TypeOfUser = TypeOfUser, Email = Email};
            _userRepository.Add(user);
        }

        public void UpdateUser(string Username, string Password, string TypeOfUser, string Email)
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(TypeOfUser) || string.IsNullOrWhiteSpace(Email))
            {
                throw new ArgumentException("All fields are required.");
            }

            var user = _userRepository.GetByUsername(Username);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }
            user.Password = Password;
            user.Email = Email;
            user.TypeOfUser = TypeOfUser;
            _userRepository.Update(user);
        }

        public void RemoveUser(string Username)
        {
            var user = _userRepository.GetByUsername(Username);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            _userRepository.Remove(Username);
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }
    }
}
