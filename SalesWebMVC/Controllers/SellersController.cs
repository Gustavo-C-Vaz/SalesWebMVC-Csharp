using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Services;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Models;

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
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int? id)   // o ? indica que é opcional.
        {
            if (id == null) return NotFound();
            var obj = _sellerService.FindById(id.Value); // id aqui é Nullable, portanto requer .Value
            if (obj == null) return NotFound();
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
            if (id == null) return NotFound();
            var obj = _sellerService.FindById(id.Value); // id aqui é Nullable, portanto requer .Value
            if (obj == null) return NotFound();
            return View(obj);
        }
    }
}
