CREATE PROCEDURE UpdatePayment
    @PaymentName NVARCHAR(100),
    @PaymentTotal DECIMAL(18, 2),
    @PaymentDate NVARCHAR(50),
    @PaymentFreq NVARCHAR(20),
    @Email NVARCHAR(256),
    @PaymentID SMALLINT
AS
BEGIN
UPDATE Payments 
SET PaymentName = @PaymentName, PaymentTotal = @PaymentTotal, PaymentDate = @PaymentDate, PaymentFreq = @PaymentFreq, Email = @Email
WHERE PaymentId = @PaymentID;
END