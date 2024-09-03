using System.Data;
using FinanceApp.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.DataAccessLayer;

//TODO: Model state coming back as null for increase, decresee, get payments etc

public class DatabaseMethods
{
    private readonly UserDbContext _dbContext;
    
    public DatabaseMethods(UserDbContext dbContext)
    {
        _dbContext = dbContext;
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