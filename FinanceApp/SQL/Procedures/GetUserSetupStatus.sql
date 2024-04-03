CREATE PROCEDURE GetUserSetupStatus
    @Email NVARCHAR(100)
AS
BEGIN
    SELECT isSetup
    FROM UserAccounts
    WHERE Email = @Email;
END;