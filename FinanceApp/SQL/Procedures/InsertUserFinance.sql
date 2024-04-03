CREATE PROCEDURE InsertUserFinance
    @Email NVARCHAR(256),
    @AccountName NVARCHAR(100),
    @AccountBalance DECIMAL(18, 2)
AS
BEGIN
    -- Check if the account name already exists for the user
    IF NOT EXISTS (SELECT 1 FROM UserFinance WHERE Email = @Email AND AccountName = @AccountName)
    BEGIN
        INSERT INTO UserFinance (AccountName, AccountBalance, Email)
        VALUES (@AccountName, @AccountBalance, @Email);
    END
END