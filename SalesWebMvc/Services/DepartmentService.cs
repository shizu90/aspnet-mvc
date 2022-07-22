using SalesWebMvc.Data;
using System.Linq;
using System.Collections.Generic;
using SalesWebMvc.Models;

namespace SalesWebMvc.Services {
    public class DepartmentService {
        private readonly SalesWebMvcContext _context;

        public DepartmentService(SalesWebMvcContext context) {
            this._context = context;
        }

        public List<Department> FindAll() {
            return this._context.Department.OrderBy(x => x.Name).ToList();
        }
    }
}
