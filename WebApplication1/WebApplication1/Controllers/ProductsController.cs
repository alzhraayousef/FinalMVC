using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.repo;

namespace WebApplication1.Controllers
{
    public class ProductsController : Controller
    {
        Ireposatory<Product> productRepo;
        Ireposatory<Category> categoryRepo;
        public ProductsController(Ireposatory<Product> _productRepo, Ireposatory<Category> _categoryRepo) { 
            productRepo = _productRepo;
            categoryRepo = _categoryRepo;
        }
        public IActionResult Index()
        {
            List<Product> products=productRepo.getall();
            //foreach (Product product in products)
            //{
            //    Category category = categoryRepo.getbyid(product.CategoryID);
            //    ViewData["Category"] = category.Name;
            //    //if (product.CategoryID == product.Category.ID)
            //    //{
            //    //    ViewData["Category"]=product.Category.Name;
            //    //}
            //    //if (product.BrandID == product.Brand.ID)
            //    //{
            //    //    ViewData["Brand"] = product.Brand.Name;
            //    //}
            //}
            return View(products);
        }
        //public IActionResult AddToCart(int id) {
        //  Product product=productRepo.getbyid(id);
        //    List<Supplier_Product> supplier_Products=product.Supplier_Products.ToList();

        //}
    }
}
