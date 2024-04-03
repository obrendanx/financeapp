CREATE PROCEDURE InsertPayment
    @PaymentName NVARCHAR(100),
    @PaymentTotal DECIMAL(18, 2),
    @PaymentDate NVARCHAR(50),
    @PaymentFreq NVARCHAR(20),
    @Email NVARCHAR(256)
AS
BEGIN
    INSERT INTO Payments (PaymentName, PaymentTotal, PaymentDate, PaymentFreq, Email)
    VALUES (@PaymentName, @PaymentTotal, @PaymentDate, @PaymentFreq, @Email);
END