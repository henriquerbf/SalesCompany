using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SalesCompany.Models;
using SalesCompany.Models.ViewModels;
using SalesCompany.Services;
using SalesCompany.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace SalesCompany.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService vendedorService, DepartmentService departmentService)
        {
            _sellerService = vendedorService;
            _departmentService = departmentService;
        }
        public async Task<IActionResult> Index()
        {
            var list = await _sellerService.FindAllAsync();
            return View(list);
        }
        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }

            await _sellerService.InsertAsync(seller);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não informado" });
            }

            var seller = await _sellerService.FindByIdAsync(id.Value);

            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não encontrado" });
            }

            return View(seller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sellerService.RemoveAsync(id);

                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não informado" });
            }

            var seller = await _sellerService.FindByIdAsync(id.Value);

            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não correspondente" });
            }

            return View(seller);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não informado" });
            };

            var seller = await _sellerService.FindByIdAsync(id.Value);

            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não encontrado" });
            };

            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            };

            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não correspondente" });
            };

            try
            {
                await _sellerService.UpdateAsync(seller);

                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            };
        }
        public IActionResult Error(string message) //Não precisa ser assincrona por nao ter acesso a dados, aqui só retornamos a view
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                //?? Operador de coalescência nula, o Current? significa que é opcional
                //caso o valor do current? seja nula irá pegar o valor do TraceIdentifier
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(viewModel);

        }
    }
}