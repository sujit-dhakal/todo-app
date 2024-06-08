﻿using TodoApp.Model;
using TodoApp.Repositories;

namespace TodoApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) { 
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<User>> GetUsers() => await _userRepository.GetUsers();

        public async Task<User> Login(AddUser adduser) => await _userRepository.Login(adduser);

        public async Task<User> Register(AddUser adduser) => await _userRepository.Register(adduser);
    }
}