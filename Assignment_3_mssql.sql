CREATE DATABASE HMBank

USE HMBank

CREATE TABLE Customers
(
customer_id INT IDENTITY(1,1),
CONSTRAINT pk_customers PRIMARY KEY (customer_id),
first_name nvarchar(25) NOT NULL,
last_name nvarchar(25) NOT NULL,
DOB DATE NOT NULL,
email nvarchar(25) UNIQUE NOT NULL,
phone_number BIGINT,
[address] nvarchar(255) NOT NULL)
GO

CREATE TABLE Accounts
(
account_id INT IDENTITY(1,1),
CONSTRAINT pk_account PRIMARY KEY (account_id),
customer_id INT,
CONSTRAINT fk_customer FOREIGN KEY (customer_id) REFERENCES Customers (customer_id),
account_type nvarchar(10),
balance money
)
GO

CREATE TABLE Transactions
(
transaction_id INT IDENTITY(1,1),
CONSTRAINT pk_transaction PRIMARY KEY (transaction_id),
account_id INT,
CONSTRAINT fk_accounts FOREIGN KEY (account_id) REFERENCES Accounts(account_id),
transaction_type varchar(30),
amount money,
transaction_date DATE
)
GO
SELECT * FROM Customers
SELECT * FROM Accounts
SELECT * FROM Transactions

--task_2

--query_1

INSERT INTO Customers (first_name, last_name, DOB, email, phone_number, address)
VALUES
('Rahul', 'Sharma', '1990-05-15', 'rahul.sharma@example.com', 9876543210, '123 MG Road, Mumbai'),
('Priya', 'Patel', '1985-08-22', 'priya.patel@example.com', 8765432109, '456 Gandhi Nagar, Delhi'),
('Amit', 'Kumar', '1992-11-30', 'amit.kumar@example.com', 7654321098, '789 Nehru Street, Bangalore'),
('Anjali', 'Singh', '1988-03-10', 'anjali.singh@example.com', 6543210987, '321 Rajpath, Kolkata'),
('Vikram', 'Gupta', '1995-07-25', 'vikram.gupta@example.com', 5432109876, '654 Marine Drive, Chennai'),
('Sneha', 'Joshi', '1991-09-12', 'sneha.joshi@example.com', 4321098765, '987 Church Street, Hyderabad'),
('Rajesh', 'Yadav', '1987-12-05', 'rajesh.yadav@example.com', 3210987654, '159 Park Avenue, Pune'),
('Pooja', 'Reddy', '1993-04-18', 'pooja.reddy@example.com', 2109876543, '753 Jubilee Hills, Ahmedabad'),
('Arun', 'Verma', '1989-06-20', 'arun.verma@example.com', 1098765432, '852 Banjara Hills, Jaipur'),
('Neha', 'Malhotra', '1994-02-28', 'neha.malhotra@example.com', 9876543211, '963 Connaught Place, Lucknow');


ALTER TABLE Accounts
ALTER COLUMN account_type NVARCHAR(20);

INSERT INTO Accounts (customer_id, account_type, balance)
VALUES
(1, 'Savings', 50000.00),
(2, 'Current', 100000.00),
(3, 'Savings', 75000.00),
(4, 'Zero Balance', 0.00),
(5, 'Savings', 120000.00),
(6, 'Current', 80000.00),
(7, 'Savings', 30000.00),
(8, 'Zero Balance', 0.00),
(9, 'Current', 150000.00),
(10, 'Savings', 60000.00);

SELECT * FROM Accounts

INSERT INTO Transactions (account_id, transaction_type, amount, transaction_date)
VALUES
(5, 'Deposit', 20000.00, '2023-10-01'),
(6, 'Withdrawal', 5000.00, '2023-10-02'),
(7, 'Deposit', 10000.00, '2023-10-03'),
(8, 'Deposit', 0.00, '2023-10-04'),
(9, 'Withdrawal', 20000.00, '2023-10-05'),
(10, 'Deposit', 15000.00, '2023-10-06'),
(11, 'Withdrawal', 10000.00, '2023-10-07'),
(12, 'Deposit', 0.00, '2023-10-08'),
(13, 'Withdrawal', 30000.00, '2023-10-09'),
(14, 'Deposit', 25000.00, '2023-10-10');

--query_2.1

SELECT CONCAT(c.first_name,SPACE(1),c.last_name),
a.account_type,c.email
FROM Customers c JOIN Accounts a 
ON c.customer_id=a.customer_id

--query_2.2

SELECT t.transaction_id, t.transaction_type, t.amount, t.transaction_date
FROM Transactions t
JOIN Accounts a ON t.account_id = a.account_id
JOIN Customers c ON a.customer_id = c.customer_id
WHERE c.customer_id = 1

--query_2.3

UPDATE Accounts
SET balance = balance+5000.00
WHERE account_id=5

SELECT * FROM Accounts

--query_2.4

SELECT CONCAT(first_name,SPACE(1),last_name) as CUSTOMER_NAME
from Customers

--query_2.5

DELETE FROM Accounts WHERE balance=0.00 AND account_type='Savings'
SELECT * FROM Accounts

--query_2.6

SELECT first_name, last_name, address
FROM Customers
WHERE address LIKE '%Mumbai%';

--query_2.7

SELECT balance
FROM Accounts
WHERE account_id = 5

--query_2.8

SELECT *
FROM Accounts
WHERE account_type = 'Current' AND balance > 1000.00;

--query_2.9

SELECT *
FROM Transactions
WHERE account_id = 5;

--query_2.10

SELECT account_id, balance * 0.045 AS interest_accrued
FROM Accounts
WHERE account_type = 'Savings';

--query_2.11

SELECT * FROM Accounts
WHERE balance<50000.00

--query_2.12

SELECT * FROM Customers
WHERE  address NOT LIKE '%Delhi%'

--task_3

--query_1

SELECT AVG(balance) AS AVG_BAL,customer_id
FROM Accounts
GROUP BY customer_id

--query_2

SELECT TOP 10 balance
FROM Accounts
ORDER BY balance DESC

--query_3

SELECT * FROM Transactions

SELECT SUM(amount) AS TOTAL_DEPOSIT 
FROM Transactions
WHERE transaction_type = 'Deposit' AND transaction_date = '2023-10-01'

--query_4

SELECT MIN(DOB) AS oldest_customer, MAX(DOB) AS newest_customer
FROM Customers;

--query_5

SELECT t.transaction_id,t.account_id,t.transaction_type,t.amount,t.transaction_date,a.account_type 
FROM Transactions t JOIN Accounts a 
ON t.account_id=a.account_id

--query_6

SELECT c.first_name, c.last_name, a.account_id, a.account_type, a.balance
FROM Customers c
JOIN Accounts a ON c.customer_id = a.customer_id;

--query_7

SELECT t.transaction_id, t.transaction_type, t.amount, t.transaction_date, c.first_name, c.last_name
FROM Transactions t
JOIN Accounts a ON t.account_id = a.account_id
JOIN Customers c ON a.customer_id = c.customer_id
WHERE a.account_id = 6 

--query_8

SELECT customer_id,COUNT(account_id) AS NO_OF_ACC
FROM Accounts
GROUP BY customer_id
HAVING COUNT(account_id)>1

--query_9
SELECT 
    (SELECT SUM(amount) FROM Transactions WHERE transaction_type = 'Deposit') -
    (SELECT SUM(amount) FROM Transactions WHERE transaction_type = 'Withdrawal') AS net_difference

--query_10

SELECT  account_id,AVG(balance) AS AVG_BAL
FROM Accounts
WHERE account_id IN (
        SELECT DISTINCT account_id
		FROM Transactions
		WHERE transaction_date BETWEEN '2023-10-01' AND '2023-10-31'
		)
GROUP BY account_id

--query_11

SELECT account_type,SUM(balance)
FROM Accounts 
GROUP BY account_type

--query_12

SELECT account_id,COUNT(transaction_id) AS TRANSAC_COUNT
FROM Transactions
GROUP BY account_id
ORDER BY COUNT(transaction_id) DESC

--query_13

SELECT c.customer_id,c.first_name,c.last_name,
SUM(a.balance) AS ACC_BAL,a.account_type
FROM Customers c JOIN Accounts a
ON c.customer_id=a.customer_id
GROUP BY c.customer_id,c.first_name,c.last_name,a.account_type
ORDER BY SUM(a.balance) DESC

--query_14

SELECT account_id,amount,transaction_date,
COUNT(*) AS duplicate_count
FROM Transactions
GROUP BY account_id, amount, transaction_date
HAVING COUNT(*) > 1;

--task_4

--query_1

SELECT c.first_name, c.last_name, a.balance
FROM Customers c
JOIN Accounts a ON c.customer_id = a.customer_id
WHERE a.balance = (SELECT MAX(balance) FROM Accounts);

--query_2

SELECT AVG(balance) AS average_balance
FROM Accounts
WHERE customer_id IN (
    SELECT customer_id
    FROM Accounts
    GROUP BY customer_id
    HAVING COUNT(account_id) > 1
);

--query_3

SELECT * FROM Accounts
WHERE account_id IN(
      SELECT account_id 
	  FROM Transactions
	  WHERE amount>(SELECT AVG(amount) FROM Transactions)
	  )

--query_4

SELECT c.customer_id, c.first_name, c.last_name
FROM Customers c
LEFT JOIN Accounts a ON c.customer_id = a.customer_id
LEFT JOIN Transactions t ON a.account_id = t.account_id
WHERE t.transaction_id IS NULL;

--query_5

SELECT SUM(a.balance) AS total_balance
FROM Accounts a
LEFT JOIN Transactions t ON a.account_id = t.account_id
WHERE t.transaction_id IS NULL;

--query_6

SELECT t.*
FROM Transactions t
JOIN Accounts a ON t.account_id = a.account_id
WHERE a.balance = (SELECT MIN(balance) FROM Accounts);

--query_7

SELECT c.customer_id, c.first_name, c.last_name
FROM Customers c
JOIN Accounts a ON c.customer_id = a.customer_id
GROUP BY c.customer_id, c.first_name, c.last_name
HAVING COUNT(DISTINCT a.account_type) > 1;

--query_8

SELECT account_type,
COUNT(*) * 100.0 / (SELECT COUNT(*) FROM Accounts) AS percentage
FROM Accounts
GROUP BY account_type;

--query_9

SELECT t.*
FROM Transactions t
JOIN Accounts a ON t.account_id = a.account_id
WHERE a.customer_id = 1

--query_10

SELECT account_type,
(SELECT SUM(balance) FROM Accounts a2 WHERE a2.account_type = a1.account_type) AS total_balance
FROM Accounts a1
GROUP BY account_type;