using SalesWebMvc.Data;
using SalesWebMvc.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SalesWebMvc.Services.Exceptions;

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

        public async Task<List<IGrouping<Department,SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate) {
            var result = from obj in this._context.SalesRecord select obj;
            if (minDate.HasValue) result.Where(x => x.Date >= minDate.Value);
            if (maxDate.HasValue) result.Where(x => x.Date <= maxDate.Value);
            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .GroupBy(x => x.Seller.Department)
                .ToListAsync();
        }

        public async Task<SalesRecord> FindByIdAsync(int id) {
            return await this._context.SalesRecord.Include(obj => obj.Seller).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task InsertAsync(SalesRecord obj) {
            this._context.Add(obj);
            await this._context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id) {
            try {
                var obj = await this._context.SalesRecord.FindAsync(id);
                this._context.SalesRecord.Remove(obj);
                await this._context.SaveChangesAsync();
            } catch (DbUpdateException e) {
                throw new IntegrityException(e.Message);
            }
        }

        public async Task UpdateAsync(SalesRecord obj) {
            bool hasAny = await this._context.SalesRecord.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny) throw new NotFoundException("Sale id not found");
            try {
                this._context.Update(obj);
                await this._context.SaveChangesAsync();
            } catch(DbUpdateException e) {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
