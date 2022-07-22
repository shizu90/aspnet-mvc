using SalesWebMvc.Models;
using SalesWebMvc.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesWebMvc.Services {
    public class SellerService {
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context) {
            this._context = context;
        }

        public List<Seller> FindAll() {
            return this._context.Seller.ToList();
        }

        public void Insert(Seller obj) {
            this._context.Add(obj);
            this._context.SaveChanges();
        }
    }
}
