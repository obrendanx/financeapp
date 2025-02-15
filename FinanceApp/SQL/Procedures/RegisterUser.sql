CREATE PROCEDURE RegisterUser
    @Username NVARCHAR(50),
    @Email NVARCHAR(255),
    @Password NVARCHAR(255)
AS
BEGIN
    -- Insert the user values into the Users table
INSERT INTO Users (Username, Email, Password)
VALUES (@Username, @Email, @Password, GETDATE());
END;