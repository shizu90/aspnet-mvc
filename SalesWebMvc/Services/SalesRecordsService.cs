using SalesWebMvc.Data;
using SalesWebMvc.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SalesWebMvc.Services {
    public class SalesRecordsService {
        private readonly SalesWebMvcContext _context;

        public SalesRecordsService(SalesWebMvcContext context) {
            this._context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate) {
            var result = from obj in this._context.SalesRecord select obj;
            if (minDate.HasValue) result.Where(x => x.Date >= minDate.Value);
            if (maxDate.HasValue) result.Where(x => x.Date <= maxDate.Value);
            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }
    }
}
