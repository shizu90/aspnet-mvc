using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;
using System.Threading.Tasks;
using System;

namespace SalesWebMvc.Controllers {
    public class SalesRecordsController : Controller {

        private readonly SalesRecordsService _salesRecordsService;

        public SalesRecordsController(SalesRecordsService salesRecordsService) {
            this._salesRecordsService = salesRecordsService;
        }

        public IActionResult Index() {
            return View();
        }

        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate) {
            if (!minDate.HasValue) minDate = new DateTime(DateTime.Now.Year, 1, 1);
            if (!maxDate.HasValue) maxDate = DateTime.Now;
            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = minDate.Value.ToString("yyyy-MM-dd");
            var result = await this._salesRecordsService.FindByDateAsync(minDate, maxDate);
            return View(result);
        }

        public IActionResult GroupingSearch() {
            return View();
        }
    }
}
