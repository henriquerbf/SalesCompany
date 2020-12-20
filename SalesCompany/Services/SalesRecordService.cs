using SalesCompany.Data;
using SalesCompany.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesCompany.Services
{
    public class SalesRecordService
    {
        private readonly SalesCompanyContext _context;

        public SalesRecordService(SalesCompanyContext context)
        {
            _context = context;
        }
        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? initialDate, DateTime? finalDate)
        {
            var result = from model in _context.SalesRecord select model;

            if (initialDate.HasValue)
            {
                result = result.Where(r => r.Data >= initialDate.Value);
            }

            if (finalDate.HasValue)
            {
                result = result.Where(r => r.Data <= finalDate.Value);
            }

            return await result
                .Include(r => r.Vendendor)
                .Include(r => r.Vendendor
                .Department)
                .OrderByDescending(r => r.Data)
                .ToListAsync();
        }
        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from model in _context.SalesRecord select model;

            if (minDate.HasValue)
            {
                result = result.Where(r => r.Data >= minDate.Value);
            }

            if (maxDate.HasValue)
            {
                result = result.Where(r => r.Data <= maxDate.Value);
            }

            return await result
                .Include(r => r.Vendendor)
                .Include(r => r.Vendendor.Department)
                .OrderByDescending(r => r.Data)
                .GroupBy(r => r.Vendendor.Department)
                .ToListAsync();
        }
    }
}
