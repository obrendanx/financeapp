using System.Security.Claims;
using FinanceApp.DataAccessLayer;
using FinanceApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Payments = FinanceApp.Models.Payments;

namespace FinanceApp.Controllers;

public class CalenderController : Controller
{
    private readonly DatabaseMethods _dbMethods;

    public CalenderController(UserDbContext dbContext, DatabaseMethods dbMethod)
    {
        _dbMethods = dbMethod;
    }

    // GET
    public IActionResult Index()
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        CalenderPayment model = new CalenderPayment();
        
        model.Balance = _dbMethods.FetchBalance(userEmail);
        model.Payments = _dbMethods.GetAllPayments(userEmail);
            
        return View(model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddNewPayment(CalenderPayment model)
    {
        if (ModelState.IsValid)
        {
            Payments payment = new Payments()
            {
                PaymentDate = model.PaymentDate,
                PaymentName = model.PaymentName,
                PaymentFreq = model.PaymentFreq,
                PaymentTotal = model.PaymentTotal,
                Email = User.FindFirstValue(ClaimTypes.Email)
            };
            
            _dbMethods.InsertPayment(payment);

            return RedirectToAction("Index", "Calender");
        }
        
        return RedirectToAction("Index", "Calender");
    }
    
    [HttpPost]
    public IActionResult EditPayment(CalenderPayment model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                Models.Payments payments = new Models.Payments()
                {
                    PaymentDate = model.PaymentDate,
                    PaymentName = model.PaymentName,
                    PaymentTotal = model.PaymentTotal,
                    PaymentFreq = model.PaymentFreq,
                    PaymentId = model.PaymentId,
                    Email = User.FindFirstValue(ClaimTypes.Email)
                };
                
                _dbMethods.EditPayment(payments);
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine("An error occurred while executing the stored procedure: " + ex.Message);
        }

        return RedirectToAction("Index", "Calender");
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult IncreaseBalance(CalenderPayment model)
    {
        if (ModelState.IsValid)
        {
            // Insert data into the UserFinance table
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            _dbMethods.IncreaseBalance(model.PaymentIncrease, userEmail);

            return RedirectToAction("Index", "Calender");
        }
        
        return RedirectToAction("Index", "Calender");
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DecreaseBalance(CalenderPayment model)
    {
        if (ModelState.IsValid)
        {
            // Insert data into the UserFinance table
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            _dbMethods.DecreaseBalance(model.PaymentDecrease, userEmail);

            return RedirectToAction("Index", "Calender");
        }

        return RedirectToAction("Index", "Calender");
    }
}