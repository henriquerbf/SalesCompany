using SalesCompany.Data;
using SalesCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesCompany.Services.Exceptions;

namespace SalesCompany.Services
{
    public class DepartmentService
    {
        private readonly SalesCompanyContext _context;

        public DepartmentService(SalesCompanyContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> FindAllAsync()
        {
            return await _context.Department.OrderBy(d => d.Name).ToListAsync();
        }
        public async Task InsertAsync(Department department)
        {
            _context.Add(department);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var v = await _context.Department.FindAsync(id);
                _context.Department.Remove(v);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        public async Task UpdateAsync(Department department)
        {
            if (!(await _context.Department.AnyAsync(d => d.Id == department.Id)))
            {
                throw new NotFoundException("Id não encontrado.");
            }

            try
            {
                _context.Department.Update(department);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }

        public async Task<Department> FindByIdAsync(int id)
        {
            return await _context.Department.FirstOrDefaultAsync(d => d.Id == id);
        }
    }
}
