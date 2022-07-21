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

            Department departmentTest = new Department(1, "Test");
            Seller sellerTest = new Seller(1, "Test", "test@test.test", 0.0, new System.DateTime(2000, 01, 1), departmentTest);
            SalesRecord salesRecordTest = new SalesRecord(1, new System.DateTime(2010, 02, 2), 50.00, Models.Enums.SaleStatus.Billed, sellerTest);

            this._context.Department.Add(departmentTest);
            this._context.Seller.Add(sellerTest);
            this._context.SalesRecord.Add(salesRecordTest);

            _context.SaveChanges();
            
        }
    }
}
