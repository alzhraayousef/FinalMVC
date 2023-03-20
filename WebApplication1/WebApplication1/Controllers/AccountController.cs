using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Models;
using WebApplication1.repo;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public Ireposatory<Customer> reposatory;

        public AccountController(Ireposatory<Customer> _reposatory,UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            reposatory= _reposatory;
        }


        public IActionResult Registration()
        {
            RegistrationVM temp = new RegistrationVM();
            temp.IdentityRoleS = roleManager.Roles.Where(R => R.Name != "Admin").ToList();
            return View(temp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegistrationVM newUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = newUser.UserName;
                user.PasswordHash = newUser.Password;
                user.Address = newUser.Address;
                user.ImageName = newUser.ImageName;


                IdentityResult result = await userManager.CreateAsync(user, newUser.Password);//user.PasswordHash
                if (result.Succeeded)
                {
                    //await userManager.AddToRoleAsync(user, "Admin");
                    await userManager.AddToRoleAsync(user, newUser.RoleName);
                    await signInManager.SignInAsync(user, false);

                    if (newUser.RoleName == "Admin")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (newUser.RoleName == "Customer")
                    {
                        Customer customer = new Customer();
                        /// ApplicationUser applicationUser= await userManager.FindByNameAsync(newUser.UserName);
                        customer.ApplicationUserId = user.Id;//applicationUser.Id;
                        reposatory.create(customer);
                        return RedirectToAction("index", "Customer", new { id = customer.ID });
                    }
                    else if (newUser.RoleName == "Supplier")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else // Delivery
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            newUser.IdentityRoleS = roleManager.Roles.Where(R => R.Name != "Admin").ToList();
            return View(newUser);
        }

        //public IActionResult TestCookie()
        //{
        //    Claim idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        //    string userid = idClaim.Value;
        //    return Content(userid);

        //}

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM userVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByNameAsync(userVM.UserName);
                if (user != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, userVM.Password, userVM.RememberMe, false);
                    if (result.Succeeded)
                    {
                        //List<string> role =  userManager.GetRolesAsync(user);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Wrong Password");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Wrong Data (UserNAme Does not Exist)");
                }
            }
            return View(userVM);
        }


        public async Task<IActionResult> signOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

    }

}
