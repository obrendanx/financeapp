using Microsoft.AspNetCore.Mvc;
using FinanceApp;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using YourNamespace.Filters;
using FinanceApp.Models;
using System.Security.Claims;
using FinanceApp.DataAccessLayer;

namespace FinanceApp.Controllers
{
    [TypeFilter(typeof(RequireSigninFilter))]
    public class SetupController : Controller
    {
        private readonly UserDbContext _dbContext;
        private readonly DatabaseMethods _dbMethods;

        public SetupController(UserDbContext dbContext, DatabaseMethods dbMethod)
        {
            _dbContext = dbContext;
            _dbMethods = dbMethod;
        }

        public async Task<IActionResult> AccountSetup()
        {
            var userEmail = User.Identity.Name;
            var isSetup = GetSetupStatus(userEmail);

            if (isSetup)
            {
                return RedirectToAction("Index", "Calender");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AccountSetup(AccountSetup model)
        {
            if (ModelState.IsValid)
            {
                // Get the user's email
                var userEmail = User.FindFirstValue(ClaimTypes.Email);

                // Update the IsSetup column in the UserAccounts table
                UpdateUserAccountIsSetup(userEmail);

                _dbMethods.InsertUserFinance(model);

                return RedirectToAction("Index", "Calender");
            }

            return View(model);
        }

        private async Task UpdateUserAccountIsSetup(string email)
        {
            _dbMethods.UpdateUserAccountIsSetup(email);
        }
        
        /*public IActionResult Welcome()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            decimal balance = _dbMethods.FetchBalance(userEmail);
            ViewBag.Balance = balance;
            
            List<Payments> payments = _dbMethods.GetAllPayments(userEmail);
            ViewBag.Payments = payments;
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Welcome(Payments model)
        {
            if (ModelState.IsValid)
            {
                _dbMethods.InsertPayment(model);

                return RedirectToAction("Welcome", "Setup");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPayment(decimal paymentAmount, string Email)
        {
            if (ModelState.IsValid)
            {
                // Insert data into the UserFinance table
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                _dbMethods.DeductPayment(paymentAmount, Email);

                return RedirectToAction("Welcome", "Setup");
            }

            return RedirectToAction("Welcome", "Setup");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFunds(decimal paymentAmount, string Email)
        {
            if (ModelState.IsValid)
            {
                // Insert data into the UserFinance table
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                _dbMethods.IncreaseBalance(paymentAmount, Email);

                return RedirectToAction("Welcome", "Setup");
            }

            return RedirectToAction("Welcome", "Setup");
        }*/

        public bool GetSetupStatus(string email)
        {
            return _dbMethods.GetUserSetupStatus(email);
        }
    }
}