CREATE PROCEDURE RemovePayment
    @PaymentId INT
AS
BEGIN
    DELETE FROM Payments
    WHERE PaymentId = @PaymentId;
END