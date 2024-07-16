USE SpendWiseDB;
GO

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Users' AND schema_id = SCHEMA_ID('SpendWise'))
BEGIN
   INSERT INTO SpendWise.Users (name, email, password, phone, role)
VALUES 
    ('Sabina Barila', 'sabina-nadejda.barila@student.tuiasi.ro', 'I8c5Yn3zefY=.ON+XQHdP3eJaIl4zr2d0zg==', '1234567890', 'user'),
    ('Cirja Ioan', 'ioan.cirja@student.tuiasi.ro', 'I8c5Yn3zefY=.ON+XQHdP3eJaIl4zr2d0zg==', '0987654321', 'user'),
    ('Cazamir Andrei', 'andrei.cazamir@student.tuiasi.ro', 'I8c5Yn3zefY=.ON+XQHdP3eJaIl4zr2d0zg==', '1122334455', 'admin');
END