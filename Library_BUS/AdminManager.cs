using Library_DAL;
using Library_DTO;
using System;
using System.Collections.Generic;

namespace Library_BUS
{
    public class AdminManager
    {
        private readonly AdminRepository _adminRepository;
        private readonly UserRepository _userRepository;

        public AdminManager(AdminRepository adminRepository, UserRepository userRepository)
        {
            _adminRepository = adminRepository;
            _userRepository = userRepository;
        }

        public void AddAdmin(string username, string firstName, string lastName)
        {
            // Check if user exists first
            var user = _userRepository.GetByUsername(username);
            if (user == null)
                throw new InvalidOperationException("The user does not exist in the system.");

            // Create new admin
            var admin = new Admin
            {
                Username = username,
                FirstName = firstName,
                LastName = lastName,
                UsernameNavigation = user
            };

            _adminRepository.Add(admin);
        }

        public void UpdateAdmin(string username, string firstName, string lastName)
        {
            var admin = _adminRepository.GetByUsername(username);
            if (admin == null)
                throw new InvalidOperationException("Admin not found.");

            admin.FirstName = firstName;
            admin.LastName = lastName;

            _adminRepository.Update(admin);
        }

        public void RemoveAdmin(string username)
        {
            var admin = _adminRepository.GetByUsername(username);
            if (admin == null)
                throw new InvalidOperationException("Admin not found.");

            _adminRepository.Remove(username);
        }

        public Admin GetAdminByUsername(string username)
        {
            return _adminRepository.GetByUsername(username);
        }

        public List<Admin> GetAllAdmins()
        {
            return _adminRepository.GetAll();
        }
    }
}