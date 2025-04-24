using EmployeeManagement.Core.Models;
using EmployeeManagement.DAL.Context;
using EmployeeManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.DAL.Repositories.Implementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Employee employee)
        {
             await _context.Employees.AddAsync(employee);
        }

        public void Delete(Employee employee)
        {
            _context.Employees.Remove(employee);
        }

        public IQueryable<Employee> GetAll()
        {
            return  _context.Employees
                                 .Include(x => x.User)
                                 .Include(x => x.Attendances)
                                 .AsQueryable();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees
                                 .Include(x => x.User)
                                 .Include(x=>x.Attendances)
                                 .FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<Employee?> GetEmployeeOfUserIdAsync(int id)
        {
            return await _context.Employees
                             .Include(x => x.User)
                             .Include(x => x.Attendances)
                             .FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync()>0;
        }

        public void Update(Employee employee)
        {
            _context.Employees.Update(employee);
        }
    }
}
