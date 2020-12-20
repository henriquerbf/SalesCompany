using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesCompany.Services;
using Microsoft.AspNetCore.Mvc;

namespace SalesCompany.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly SalesRecordService _salesRecordService;

        public SalesRecordsController(SalesRecordService salesRecordService)
        {
            _salesRecordService = salesRecordService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> SimpleSearch(DateTime? initialDate, DateTime? finalDate)
        {
            if (!initialDate.HasValue)
            {
                initialDate = new DateTime(DateTime.Now.Year, 1, 1);
            }

            if (!finalDate.HasValue)
            {
                finalDate = DateTime.Now;
            }

            ViewData["initialDate"] = initialDate.Value.ToString("yyyy-MM-dd");

            ViewData["finalDate"] = initialDate.Value.ToString("yyyy-MM-dd");

            var result = await _salesRecordService.FindByDateAsync(initialDate, finalDate);
            return View(result);
        }
        public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }

            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }

            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");

            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            var result = await _salesRecordService.FindByDateGroupingAsync(minDate, maxDate);
            return View(result);
        }
    }
}