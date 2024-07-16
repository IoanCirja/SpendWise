USE SpendWiseDB;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Contact' AND schema_id = SCHEMA_ID('SpendWise'))
BEGIN
INSERT INTO SpendWise.Contact (firstName, lastName, email, message, status)
VALUES ('Ioan', 'Cîrjă', 'ioan.cirja@student.tuiasi.ro', 'I would like more information.', 'RECEIVED'),
('Sabina', 'Barila', 'sabina-nadejda.barila@student.tuiasi.ro', 'I would like more information.', 'RECEIVED'),
 ('Cristian', 'Ghiuta', 'cristian-daniel.ghiuta@student.tuiasi.ro', 'I would like more information.', 'RECEIVED'),
 ('Simona', 'Huminiuc', 'simona.huminiuc@student.tuiasi.ro', 'I would like more information.', 'RECEIVED'),
 ('Andrei', 'Cazamir', 'andrei.cazamir@student.tuiasi.ro', 'I would like more information.', 'RECEIVED');

END
