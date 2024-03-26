using Microsoft.AspNetCore.Mvc;
using FinanceApp;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using YourNamespace.Filters;
using FinanceApp.Models;
using System.Security.Claims;

namespace FinanceApp.Controllers
{
    [TypeFilter(typeof(RequireSigninFilter))]
    public class SetupController : Controller
    {
        private readonly UserDbContext _dbContext;

        public SetupController(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> AccountSetup()
        {
            var userEmail = User.Identity.Name;
            var isSetup = await GetSetupStatus(userEmail);

            if (isSetup)
            {
                return RedirectToAction("Welcome", "Setup");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AccountSetup(AccountSetup model)
        {
            if (ModelState.IsValid)
            {
                // Get the user's email
                var userEmail = User.FindFirstValue(ClaimTypes.Email);

                // Update the IsSetup column in the UserAccounts table
                await UpdateUserAccountIsSetup(userEmail);

                // Insert data into the UserFinance table
                var connectionString = _dbContext.Database.GetConnectionString();
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("InsertUserFinance", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Email", model.Email);
                        command.Parameters.AddWithValue("@AccountName", model.AccountName);
                        command.Parameters.AddWithValue("@AccountBalance", model.AccountBalance);

                        await command.ExecuteNonQueryAsync();
                    }
                }

                return RedirectToAction("Welcome", "Setup");
            }

            return View(model);
        }

        private async Task UpdateUserAccountIsSetup(string email)
        {
            var connectionString = _dbContext.Database.GetConnectionString();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("UpdateUserAccountIsSetup", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", email);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public IActionResult Welcome()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var connectionString = _dbContext.Database.GetConnectionString();

            var sqlQuery = "EXECUTE FetchBalance @Email";

            decimal balance = 0;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Email", userEmail);
                    var result = command.ExecuteScalar();
                    if (result != null && decimal.TryParse(result.ToString(), out balance))
                    {
                        ViewBag.Balance = balance;
                    }
                }
            }

            return View();
        }

        private async Task<bool> GetSetupStatus(string email)
        {
            var connectionString = _dbContext.Database.GetConnectionString();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("GetUserSetupStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", email);

                    var result = await command.ExecuteScalarAsync();

                    if (result != null && result != DBNull.Value)
                    {
                        return (bool)result;
                    }

                    return false;
                }
            }
        }
    }
}