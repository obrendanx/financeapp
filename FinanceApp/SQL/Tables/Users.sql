CREATE TABLE Users (
                       UserID INT IDENTITY(1,1) PRIMARY KEY,  -- auto-incrementing primary key
                       Email NVARCHAR(255) NOT NULL,           -- email column with nvarchar(255)
                       Username NVARCHAR(50) NOT NULL,        -- username column with nvarchar(50)
                       Password NVARCHAR(255) NOT NULL        -- password column with nvarchar(255)
);