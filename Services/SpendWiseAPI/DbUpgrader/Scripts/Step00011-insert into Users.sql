USE SpendWiseDB;
GO

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'PlanDetails' AND schema_id = SCHEMA_ID('SpendWise'))
BEGIN
   INSERT INTO SpendWise.Users (name, email, password, phone, role)
VALUES 
    ('Sabina Barila', 'sabina-nadejda.barila@student.tuiasi.ro', 'Password123', '1234567890', 'user'),
    ('Cirja Ioan', 'ioan.cirja@student.tuiasi.ro', 'Password123', '0987654321', 'user'),
    ('Cazamir Andrei', 'andrei.cazamir@student.tuiasi.ro', 'Password123', '1122334455', 'admin');
END