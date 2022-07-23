using SalesWebMvc.Models;
using SalesWebMvc.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;

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

        public Seller FindById(int id) {
            return this._context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id);
        }

        public void Remove(int id) {
            var obj = this._context.Seller.Find(id);
            this._context.Seller.Remove(obj);
            this._context.SaveChanges();
        }

        public void Update(Seller obj) {
            if (!this._context.Seller.Any(x => x.Id == obj.Id)) throw new NotFoundException("Id not found");
            try {
                this._context.Update(obj);
                this._context.SaveChanges();
            } catch(DbConcurrencyException e) {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
