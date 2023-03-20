using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.ViewModel;
using WebApplication1.Models;
using WebApplication1.repo;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    public class CustomerController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        Ireposatory<Customer> repo;
        Ireposatory<Product> productRepo;
        Ireposatory<Supplier_Product> supProductRepo;
        Context context;
        //Ireposatory<ApplicationUser> Apprepo;
        public CustomerController(Context _context,Ireposatory<Supplier_Product> _supProductRepo,Ireposatory<Product> _productRepo,Ireposatory<Customer> _repo,UserManager<ApplicationUser> _userManager) {
            repo = _repo;
            userManager = _userManager;
            productRepo = _productRepo;
            supProductRepo = _supProductRepo;
            context = _context;
        }

        public async Task<IActionResult> index(int id)
        {
            Customer customer = repo.getbyid(id);
            ApplicationUser user = await userManager.FindByIdAsync(customer.ApplicationUserId);
            CustomerProfileVM c = new CustomerProfileVM();
            c.CustomId = customer.ID;
            c.userName = user.UserName;
            ViewData["id"] = c.CustomId;
            ViewData["Username"] = c.userName;

            //ApplicationUser user = await userManager.FindByIdAsync(customer.ApplicationUserId);
            //ViewData["CustomID"] = customer.ID;
            //return View(user);
            List<Product> products = productRepo.getall();
           
            return View(products);
        }
        public async Task< IActionResult> CustomerProfile(int id)
        {
            //Customer c=repo.getbyidCustom(c=>c.ID==id && c.ApplicationUserId==c.ApplicationUser.Id);
            Customer customer=repo.getbyid(id);
            ApplicationUser user = await userManager.FindByIdAsync(customer.ApplicationUserId);
            CustomerProfileVM c=new CustomerProfileVM();
            c.CustomId = customer.ID;
            //ViewData["id"] = c.CustomId;
            c.userName = user.UserName;
            c.Address=user.Address;
            c.Phone = user.PhoneNumber;
            c.Email = user.Email;
            c.Image = user.ImageName;

            return View(c);
        }
        [HttpPost]
        public async Task< IActionResult> CustomerProfile(CustomerProfileVM customer)
        {
           Customer customer1 = repo.getbyid(customer.CustomId);
            ApplicationUser user = await userManager.FindByIdAsync(customer1.ApplicationUserId);
            if (customer != null)
            {
               //Customer oldEmp = new Customer();
                customer1.ApplicationUser.Address=customer.Address;
                customer1.ApplicationUser.PhoneNumber = customer.Phone;
                customer1.ApplicationUser.Email = customer.Email;
                customer1.ApplicationUser.UserName = customer.userName;
                repo.update(customer1);
                return RedirectToAction("CustomerProfile", new {id=customer.CustomId});
            }
            return View(customer);
            
        }
        
        public async Task <IActionResult> AddToCart(int Productid)
        {
            Context cont = new Context();
            //var claims = User.Claims;//keys cookie (NameIdentifier,Name
            Claim idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            string userid = idClaim.Value;
            Customer customer=cont.Customers.FirstOrDefault(c=>c.ApplicationUserId==userid);

            Product product = productRepo.getbyid(Productid);
             //Customer customer = repo.getbyid(CustomID);
            List<Supplier_Product> sup=context.Supplier_Products.Where(a=>a.ProductID==product.ID).ToList();
            //foreach (Supplier_Product supp in sup)
            //{
            //    product.Supplier_Products.Add(supp);
            //}
            ViewData["supplierProducts"] = sup;
            //foreach(Supplier_Product s in ViewBag.SupplierProducts)
            //{
            //    if (product.ID == s.ProductID)
            //    {
                    
            //    }
            //}
            // List<CustomerSelected_SupplierProduct> selected = context.SelectedItems.Where(s => s.CustomerID == customer.ID).ToList();
            // foreach (CustomerSelected_SupplierProduct supp in selected)
            // {
            //     customer.SelectedItems.Add(supp);
            // }
            SelectedItemsInCartVM selectedItemsInCartVM = new SelectedItemsInCartVM();
            selectedItemsInCartVM.Image = product.ImageName;
            selectedItemsInCartVM.productName = product.Name;
             selectedItemsInCartVM.Selected_items = product.Supplier_Products;
            selectedItemsInCartVM.Description = product.Description;
            selectedItemsInCartVM.CustomId = customer.ID;
            //selectedItemsInCartVM.Selected_items = product.Supplier_Products;

            foreach (Supplier_Product a in ViewBag.supplierProducts)
            {
                selectedItemsInCartVM.Quantity = a.Quantity;
                selectedItemsInCartVM.Price = a.Price;
            }
             return View(selectedItemsInCartVM);
        }
    }
}
