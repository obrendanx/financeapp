using FinanceApp.Models;
using FinanceApp.DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Security.Claims;
using YourNamespace.Filters;

namespace FinanceApp.Controllers
{
    [TypeFilter(typeof(RequireSigninFilter))]
    public class ProfileController : Controller
    {
        private readonly UserDbContext _dbContext;
        private readonly DatabaseMethods _dbMethods;

        public ProfileController(UserDbContext dbContext, DatabaseMethods dbMethod)
        {
            _dbContext = dbContext;
            _dbMethods = dbMethod;
        }

        public IActionResult ShowPayments()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            //List<Payments> payments = _dbMethods.GetAllPayments(userEmail);

            //ViewBag.Payments = payments;

            return View();
        }

        [HttpPost]
        public IActionResult RemovePayment(int paymentId)
        {
            if (_dbMethods.RemovePayment(paymentId))
            {
                // Redirect back to the Index action after removing the payment
                return RedirectToAction("ShowPayments");   
            }
            
            return RedirectToAction("ShowPayments");  
        }
        
        [HttpPost]
        public IActionResult EditPayment(Payments model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                     _dbMethods.EditPayment(model);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("An error occurred while executing the stored procedure: " + ex.Message);
            }

            return RedirectToAction("ShowPayments", "Profile");
        }
    }
}
