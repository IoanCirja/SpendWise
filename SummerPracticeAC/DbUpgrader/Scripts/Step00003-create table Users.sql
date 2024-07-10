USE SpendWiseDB;
GO
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users' AND schema_id = SCHEMA_ID('SpendWise'))
BEGIN
    CREATE TABLE SpendWise.Users(
        user_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        name NVARCHAR(40) NOT NULL,
        email NVARCHAR(40) NOT NULL UNIQUE,
        password NVARCHAR(40) NOT NULL,
        phone NVARCHAR(12) NOT NULL UNIQUE,
        role NVARCHAR(30) NOT NULL DEFAULT 'user'
    );
END
