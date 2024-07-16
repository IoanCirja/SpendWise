USE SpendWiseDB;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Contact' AND schema_id = SCHEMA_ID('SpendWise'))
BEGIN
    CREATE TABLE SpendWise.Contact(
        contact_id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        firstName NVARCHAR(40) NOT NULL,
		lastName NVARCHAR(40) NOT NULL,
        email NVARCHAR(40) NOT NULL,
        message NVARCHAR(300) NOT NULL,
		status NVARCHAR(30) NOT NULL DEFAULT 'RECEIVED'
    );
END
GO