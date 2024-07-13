USE SpendWiseDB;
GO
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Transactions' AND schema_id = SCHEMA_ID('SpendWise'))
BEGIN
  INSERT INTO SpendWise.Transactions (name, monthlyPlan_id, date, category, amount)
VALUES 
    ('Food', 'C56A4180-65AA-42EC-A945-5FD21DEC0538', '2024-07-01', 'Food', 150),
    ('Rent', 'C56A4180-65AA-42EC-A945-5FD21DEC0538', '2024-07-02', 'Utilities', 75),
    ('Utilities', 'A56A4180-65AA-42EC-A945-5FD21DEC0538', '2024-07-03', 'Health', 50),
    ('Entertainment', 'A56A4180-65AA-42EC-A945-5FD21DEC0538', '2024-07-04', 'Utilities', 60),
    ('Clothes', 'B56A4180-65AA-42EC-A945-5FD21DEC0538', '2024-07-05', 'Entertainment', 80),
    ('Transportation', 'B56A4180-65AA-42EC-A945-5FD21DEC0538', '2024-07-06', 'Transport', 200),
    ('Miscellaneous', 'B56C4180-65AA-42EC-A945-5FD21DEC0538', '2024-07-06', 'Transport', 200);
END
