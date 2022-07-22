using System.Linq;
using SalesWebMvc.Models;

namespace SalesWebMvc.Data {
    public class SeedingService {
        private SalesWebMvcContext _context;

        public SeedingService(SalesWebMvcContext context) {
            this._context = context;
        }

        public void Seed() {
            if (this._context.Department.Any() ||
               this._context.Seller.Any() ||
               this._context.SalesRecord.Any()) return;
            _context.SaveChanges();
            
        }
    }
}
