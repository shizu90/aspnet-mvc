using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using System.Collections.Generic;
using SalesWebMvc.Services.Exceptions;
using System.Diagnostics;
using System;
using System.Threading.Tasks;

namespace SalesWebMvc.Controllers {
    public class SellersController : Controller {

        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService) {
            this._sellerService = sellerService;
            this._departmentService = departmentService;
        }

        public async Task<IActionResult> Index() {
            
            var list = await this._sellerService.FindAllAsync();
            return View(list);
        }

        public async Task<IActionResult> Create() {

            var departments = await this._departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller) {
            if (!ModelState.IsValid) {

                var departments = await this._departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(seller); 
            }
            await this._sellerService.InsertAsync(seller);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id) {
            if (id == null) return RedirectToAction(nameof(Error), new { message = "Id not provided"});
            var obj = await this._sellerService.FindByIdAsync(id.Value);
            if (obj == null) return RedirectToAction(nameof(Error), new { message = "Id not found" });

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id) {
            await this._sellerService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id) {
            if(id == null) return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            var obj = await this._sellerService.FindByIdAsync(id.Value);
            if (obj == null) return RedirectToAction(nameof(Error), new { message = "Id not found" });

            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id) {
            if (id == null) return RedirectToAction(nameof(Error), new { message = "Id not provided" });

            var obj = await this._sellerService.FindByIdAsync(id.Value);
            if (obj == null) return RedirectToAction(nameof(Error), new { message = "Id not found" });

            List<Department> departments = await this._departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller) {
            if (!ModelState.IsValid) {

                var departments = await this._departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(seller); 
            }
            if (id != seller.Id) return RedirectToAction(nameof(Error), new { message = "Id mismatch" });

            try {
                await this._sellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            } catch (ApplicationException e) { return RedirectToAction(nameof(Error), new { message = e.Message }); }
        }

        public IActionResult Error(string message) {
            var viewModel = new ErrorViewModel { Message = message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier};
            return View(viewModel);
        }
    }
}
