IF NOT EXISTS (SELECT * FROM master.sys.databases WHERE name='SpendWiseDB')
BEGIN
    CREATE DATABASE [SpendWiseDB]
END
