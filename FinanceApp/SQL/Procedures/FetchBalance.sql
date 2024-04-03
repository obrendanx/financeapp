CREATE PROCEDURE FetchBalance
    @Email NVARCHAR(256)
AS
BEGIN
    SELECT AccountBalance FROM UserFinance WHERE Email = @Email;
END