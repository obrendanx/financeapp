using System.Data;
using FinanceApp.Helpers;
using FinanceApp.Models;
using FinanceApp.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Payments = FinanceApp.Models.Payments;

namespace FinanceApp.DataAccessLayer;

//TODO: Model state coming back as null for increase, decresee, get payments etc

public class DatabaseMethods
{
    private readonly UserDbContext _dbContext;
    
    public DatabaseMethods(UserDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool RegisterUser(UserRegister model)
    {
        try
        {
            string hashedPassword = PasswordHasher.HashPassword(model.Password);
            var connectionString = _dbContext.Database.GetConnectionString();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("RegisterUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Username", model.Username);
                    command.Parameters.AddWithValue("@Email", model.Email);
                    command.Parameters.AddWithValue("@Password", hashedPassword);
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (SqlException ex)
        {
            if (ex.Message.Contains("UQ_Username") || ex.Message.Contains("UQ_Email")) // This is based on the constraint name.
            {
                // Return false or set a specific flag to indicate a duplicate username
                return false;
            }
            else
            {
                // Re-throw the exception or handle it accordingly
                throw;
            }
        }

        return true;
    }
    
    public UserAccount GetUser(UserLogin model)
    {
        var connectionString = _dbContext.Database.GetConnectionString();
        UserAccount user = new UserAccount();

        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("GetUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Username", model.Username);
                    command.ExecuteNonQuery();
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.Id = Convert.ToInt32(reader["UserID"]);
                            user.Email = Convert.ToString(reader["Email"]);
                            user.Username = Convert.ToString(reader["Username"]);
                            user.Password = Convert.ToString(reader["Password"]);
                            user.IsSetup = Convert.ToBoolean(reader["IsSetup"]);
                            return user;
                        }
                    }
                }

                return user = null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
    
    public List<ViewModels.Payments> GetAllPayments(string Email)
    {
        var connectionString = _dbContext.Database.GetConnectionString();
        
        var sqlQuery = "EXECUTE GetAllPayments @Email";
        
        List<ViewModels.Payments> payments = new List<ViewModels.Payments>();

        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Email", Email);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ViewModels.Payments payment = new ViewModels.Payments
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

                return payments;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public bool RemovePayment(int paymentId)
    {
        try
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
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
            return false;
        }

        return true;
    }

    public bool EditPayment(Payments model)
    {
        try
        {
            var connectionString = _dbContext.Database.GetConnectionString();
            
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("UpdatePayment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", model.Email);
                    command.Parameters.AddWithValue("@PaymentName", model.PaymentName);
                    command.Parameters.AddWithValue("@PaymentTotal", SqlDbType.Decimal).Value = model.PaymentTotal;
                    command.Parameters.AddWithValue("@PaymentDate", model.PaymentDate);
                    command.Parameters.AddWithValue("@PaymentFreq", model.PaymentFreq);
                    command.Parameters.AddWithValue("@PaymentID", model.PaymentId);

                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
            return false;
        }

        return true;
    }

    public bool InsertUserFinance(AccountSetup model)
    {
        try
        {
            // Insert data into the UserFinance table
            var connectionString = _dbContext.Database.GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("InsertUserFinance", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", model.Email);
                    command.Parameters.AddWithValue("@AccountName", model.AccountName);
                    command.Parameters.AddWithValue("@AccountBalance", model.AccountBalance);

                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
            return false;
        }

        return true;
    }

    public bool UpdateUserAccountIsSetup(string email)
    {
        try
        {
            var connectionString = _dbContext.Database.GetConnectionString();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("UpdateUserAccountIsSetup", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", email);

                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
            return false;
        }

        return true;
    }

    public decimal FetchBalance(string email)
    {
        decimal balance = 0;
        
        try
        {
            var connectionString = _dbContext.Database.GetConnectionString();

            var sqlQuery = "EXECUTE FetchBalance @Email";
            

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    var result = command.ExecuteScalar();
                    if (result != null && decimal.TryParse(result.ToString(), out balance))
                    {
                        return balance;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
            return 0;
        }

        return balance;
    }

    public bool InsertPayment(Payments model)
    {
        try
        {
            // Insert data into the UserFinance table
            var connectionString = _dbContext.Database.GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("InsertPayment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", model.Email);
                    command.Parameters.AddWithValue("@PaymentName", model.PaymentName);
                    command.Parameters.AddWithValue("@PaymentTotal", model.PaymentTotal);
                    command.Parameters.AddWithValue("@PaymentDate", model.PaymentDate);
                    command.Parameters.AddWithValue("@PaymentFreq", model.PaymentFreq);

                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
            return false;
        }

        return true;
    }

    public bool DecreaseBalance(decimal? paymentAmount, string Email)
    {
        try
        {
            var connectionString = _dbContext.Database.GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("DeductPaymentAmount", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@PaymentAmount", paymentAmount);

                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
            return false;
        }

        return true;
    }

    public bool IncreaseBalance(decimal? paymentAmount, string Email)
    {
        try
        {
            var connectionString = _dbContext.Database.GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("IncreaseBalance", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@PaymentAmount", paymentAmount);

                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
            return false;
        }

        return true;
    }

    public bool GetUserSetupStatus(string email)
    {
        try
        {
            var connectionString = _dbContext.Database.GetConnectionString();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("GetUserSetupStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", email);

                    var res =  command.ExecuteScalar();

                    if ((bool)res)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                    return false;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
            return false;
        }
    }
}