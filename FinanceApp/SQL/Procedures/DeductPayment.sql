CREATE PROCEDURE DeductPaymentAmount
    @Email NVARCHAR(256),
    @PaymentAmount DECIMAL(18, 2)
AS
BEGIN
    UPDATE UserFinance
    SET AccountBalance = AccountBalance - @PaymentAmount
    WHERE Email = @Email;
END