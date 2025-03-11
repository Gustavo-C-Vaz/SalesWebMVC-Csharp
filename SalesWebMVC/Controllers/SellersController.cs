using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Services;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Models;
using SalesWebMVC.Services.Exceptions;
using System.Diagnostics;

namespace SalesWebMVC.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellersService, DepartmentService departmentService)
        {
            _sellerService = sellersService;
            _departmentService = departmentService;
        }
        public IActionResult Index()
        {
            var list = _sellerService.FindAll();
            return View(list);
        }
        public IActionResult Create()
        {
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost]      // Indica que a função abaixo é de POST
        [ValidateAntiForgeryToken]    // Protege contra ataques CSRF
        public IActionResult Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = _departmentService.FindAll();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel); // Enquanto o vendedor não for válido, retorna para a criação.
            }
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int? id)   // o ? indica que é opcional.
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "id not provided!" });
            }         
            var obj = _sellerService.FindById(id.Value); // id aqui é Nullable, portanto requer .Value
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "id not found!" });
            }
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "id not provided!" });
            }
            var obj = _sellerService.FindById(id.Value); // id aqui é Nullable, portanto requer .Value
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "id not found!" });
            }
            return View(obj);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null) 
            {
                return RedirectToAction(nameof(Error), new { message = "id not provided!" });
            }
            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "id not found!" });
            }

            List<Department> departments = _departmentService.FindAll();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = _departmentService.FindAll();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel); // Enquanto o vendedor não for válido, retorna para a edição.
            }
            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "id mismatch!" });
            }
            try
            {
                _sellerService.Update(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message});
            }
            catch (DbConcurrencyException e)   // ambas podem ser substituídas por uma ApplicationException
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }
        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                // ^ macete do framework para pegar o ID interno da requisição.
            };
            return View(viewModel);

        }
    }
}
