CREATE PROCEDURE UpdateUserAccountIsSetup
    @Email NVARCHAR(256)
AS
BEGIN
    UPDATE UserAccounts
    SET IsSetup = 1
    WHERE Email = @Email;
END;