using SalesCompany.Data;
using SalesCompany.Models;
using SalesCompany.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesCompany.Services
{
    public class SellerService 
    {
        private readonly SalesCompanyContext _context;

        public SellerService(SalesCompanyContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.Include(s => s.Department).ToListAsync();
        }

        public async Task InsertAsync(Seller seller)
        {
            _context.Add(seller);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var v = await _context.Seller.FindAsync(id);
                _context.Seller.Remove(v);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        public async Task UpdateAsync(Seller seller)
        {
            if (!(await _context.Seller.AnyAsync(s => s.Id == seller.Id)))
            {
                throw new NotFoundException("Id não encontrado.");
            }

            try
            {
                _context.Seller.Update(seller);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Seller.Include(s => s.Department).FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
