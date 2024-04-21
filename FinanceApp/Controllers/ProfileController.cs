using FinanceApp.Models;
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

        public ProfileController(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult ShowPayments()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var connectionString = _dbContext.Database.GetConnectionString();

            var sqlQuery = "EXECUTE GetAllPayments @Email";

            List<Payments> payments = new List<Payments>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Email", userEmail);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Payments payment = new Payments
                            {
                                PaymentId = Convert.ToInt32(reader["PaymentId"]),
                                Email = Convert.ToString(reader["Email"]),
                                PaymentName = Convert.ToString(reader["PaymentName"]),
                                PaymentTotal = Convert.ToDecimal(reader["PaymentTotal"]),
                                PaymentDate = Convert.ToString(reader["PaymentDate"]),
                                PaymentFreq = Convert.ToString(reader["PaymentFreq"]),
                            };
                            payments.Add(payment);
                        }
                    }
                }

                ViewBag.Payments = payments;
            }

            return View();
        }

        [HttpPost]
        public ActionResult RemovePayment(int paymentId)
        {
            var connectionString = _dbContext.Database.GetConnectionString();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("RemovePayment", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PaymentId", paymentId);
                    command.ExecuteNonQuery();
                }
            }

            // Redirect back to the Index action after removing the payment
            return RedirectToAction("ShowPayments");
        }
        
        [HttpPost]
        public ActionResult EditPayment(Payments model)
        {
            if (ModelState.IsValid)
            {
                // Insert data into the UserFinance table
                var connectionString = _dbContext.Database.GetConnectionString();
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand("UpdatePayment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Email", model.Email);
                        command.Parameters.AddWithValue("@PaymentName", model.PaymentName);
                        command.Parameters.AddWithValue("@PaymentTotal", model.PaymentTotal);
                        command.Parameters.AddWithValue("@PaymentDate", model.PaymentDate);
                        command.Parameters.AddWithValue("@PaymentFreq", model.PaymentFreq);
                        command.Parameters.AddWithValue("@PaymentID", model.PaymentId);

                        command.ExecuteNonQueryAsync();
                    }
                }

                return RedirectToAction("ShowPayments", "Profile");
            }

            return RedirectToAction("ShowPayments", "Profile");
        }
    }
}
