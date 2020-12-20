using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesCompany.Data;
using SalesCompany.Models;
using SalesCompany.Models.ViewModels;
using SalesCompany.Services;
using SalesCompany.Services.Exceptions;

namespace SalesCompany.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly DepartmentService _departmentService;

        public DepartmentsController(DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            return View(await _departmentService.FindAllAsync());
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não informado" });
            }

            var department = await _departmentService.FindByIdAsync(id.Value);

            if (department == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não correspondente" });
            }

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não informado" });
            }

            await _departmentService.InsertAsync(department);
            return RedirectToAction(nameof(Index));
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não informado" });
            }

            var department = await _departmentService.FindByIdAsync(id.Value);

            if (department == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não encontrado" });
            }

            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            if (!ModelState.IsValid)
            {
                return View(department);
            }

            if (id != department.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não correspondente" });
            };

            try
            {
                await _departmentService.UpdateAsync(department);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não informado" });
            }

            var department = await _departmentService.FindByIdAsync(id.Value);

            if (department == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não encontrado" });
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _departmentService.RemoveAsync(id);

                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
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
