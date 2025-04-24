using EmployeeManagement.Core.Interfaces.Services;
using EmployeeManagement.Core.Models;
using EmployeeManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.BLL.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<bool> CreateAsync(User user)
        {
            await userRepository.AddAsync(user);
            return await userRepository.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await userRepository.GetByIdAsync(id);
        }

        public async Task<User?> GetByUserNameAsync(string UserName)
        {
            return await userRepository.GetByUsernameAsync(UserName);
        }

        public async Task<bool> UpdateAsync(User user)
        {
            userRepository.Update(user);
            return await userRepository.SaveChangesAsync();
        }
    }
}
