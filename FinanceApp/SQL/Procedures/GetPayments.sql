CREATE PROCEDURE GetPayments
    @Email NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT *
    FROM Payments
    WHERE Email = @Email
    AND DAY(PaymentDate) = DAY(GETDATE()); -- Filter by current day
END