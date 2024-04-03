CREATE PROCEDURE GetAllPayments
    @Email NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT *
    FROM Payments
    WHERE Email = @Email
END