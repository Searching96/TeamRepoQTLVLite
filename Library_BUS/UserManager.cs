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

        public void AddUser(string Username, string Password)
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                throw new ArgumentException("Username, Password are required.");
            }

            var user = new User { Username = Username, Password = Password};
            _userRepository.Add(user);
        }

        public void UpdateUser(int id, string Username, string Password)
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                throw new ArgumentException("Username, Password and Display name are required.");
            }

            var user = _userRepository.GetByUsername(Username);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            user.Username = Username;
            user.Password = Password;
            //user.DisplayName = DisplayName;
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
