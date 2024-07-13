USE SpendWiseDB;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Subscriptions' AND schema_id = SCHEMA_ID('SpendWise'))
BEGIN
   INSERT INTO SpendWise.Subscriptions (email)
VALUES 
    ('sabina-nadejda.barila@student.tuiasi.ro'),
    ('ioan.cirja@student.tuiasi.ro');
END
