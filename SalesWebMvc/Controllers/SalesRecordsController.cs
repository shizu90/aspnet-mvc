using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Models;
using SalesWebMvc.Services.Exceptions;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Collections.Generic;


namespace SalesWebMvc.Controllers {
    public class SalesRecordsController : Controller {

        private readonly SalesRecordsService _salesRecordsService;
        private readonly SellerService _sellerService;

        public SalesRecordsController(SalesRecordsService salesRecordsService, SellerService sellerService) {
            this._salesRecordsService = salesRecordsService;
            this._sellerService = sellerService;
        }

        public IActionResult Index() {
            return View();
        }

        public async Task<IActionResult> Create() {

            var sellers = await this._sellerService.FindAllAsync();
            var viewModel = new SalesRecordViewModel { Sellers = sellers };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalesRecord salesRecord) {
            if(!ModelState.IsValid) {
                var sellers = await this._sellerService.FindAllAsync();
                var viewModel = new SalesRecordViewModel { SalesRecord = salesRecord, Sellers = sellers };
                return View(viewModel);
            }
            await this._salesRecordsService.InsertAsync(salesRecord);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id) {
            if (id == null) return RedirectToAction(nameof(Error), new { message = "Id not provided" });

            var obj = await this._salesRecordsService.FindByIdAsync(id.Value);
            if (obj == null) return RedirectToAction(nameof(Error), new { message = "Id not found" });

            List<Seller> sellers = await this._sellerService.FindAllAsync();
            SalesRecordViewModel viewModel = new SalesRecordViewModel { SalesRecord = obj, Sellers = sellers };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SalesRecord salesRecord) {
            if (!ModelState.IsValid) {

                var sellers = await this._sellerService.FindAllAsync();
                var viewModel = new SalesRecordViewModel { SalesRecord = salesRecord, Sellers = sellers };
                return View(viewModel);
            }
            if (id != salesRecord.Id) return RedirectToAction(nameof(Error), new { message = "Id mismatch" });

            try {
                await this._salesRecordsService.UpdateAsync(salesRecord);
                return RedirectToAction(nameof(Index));
            } catch (ApplicationException e) { return RedirectToAction(nameof(Error), new { message = e.Message }); }
        }

        public async Task<IActionResult> Delete(int? id) {
            if (id == null) return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            var obj = await this._salesRecordsService.FindByIdAsync(id.Value);
            if (obj == null) return RedirectToAction(nameof(Error), new { message = "Id not found" });

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id) {
            try {
                await this._salesRecordsService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            } catch (IntegrityException e) {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate) {
            if (!minDate.HasValue) minDate = new DateTime(DateTime.Now.Year, 1, 1);
            if (!maxDate.HasValue) maxDate = DateTime.Now;
            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");
            var result = await this._salesRecordsService.FindByDateAsync(minDate, maxDate);
            return View(result);
        }

        public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate) {
            if (!minDate.HasValue) minDate = new DateTime(DateTime.Now.Year, 1, 1);
            if (!maxDate.HasValue) maxDate = DateTime.Now;
            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");
            var result = await this._salesRecordsService.FindByDateGroupingAsync(minDate, maxDate);
            return View(result);
        }

        public IActionResult Error(string message) {
            var viewModel = new ErrorViewModel { Message = message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View(viewModel);
        }
    }
}
