CREATE DATABASE CarRentalSys

USE CarRentalSys

CREATE TABLE Vehicle
(
vehicleID INT,
make varchar(25),
model varchar(30),
[year] INT,
dailyRate DECIMAL(10,2),
available INT,
passengerCapacity INT,
engineCapacity INT
CONSTRAINT pk_vehicle PRIMARY KEY (vehicleID)
)


CREATE TABLE Customer
(
customerID INT,
firstName varchar(30),
lastName varchar(30),
email varchar(255),
phoneNumber varchar(20),
CONSTRAINT pk_customer PRIMARY KEY (customerID)
)

CREATE TABLE Lease
(
leaseID INT,
vehicleID INT,
customerID INT,
startDate DATE,
endDate DATE,
type varchar(40),
CONSTRAINT pk_lease PRIMARY KEY (leaseID),
CONSTRAINT fk_vehicle FOREIGN KEY (vehicleID) REFERENCES Vehicle(vehicleID) ON DELETE CASCADE,
CONSTRAINT fk_customer FOREIGN KEY (customerID) REFERENCES Customer(customerID) ON DELETE CASCADE
)

CREATE TABLE Payment
(
paymentID INT,
leaseID INT,
paymentDate DATE,
amount money,
CONSTRAINT pk_payment PRIMARY KEY (paymentID)
)
GO

INSERT INTO Vehicle VALUES
(1, 'Toyota', 'Camry', 2022, 50.00, 1, 4, 1450),
(2, 'Honda', 'Civic', 2023, 45.00, 1, 7, 1500),
(3, 'Ford', 'Focus', 2022, 48.00, 0, 4, 1400),
(4, 'Nissan', 'Altima', 2023, 52.00, 1, 7, 1200),
(5, 'Chevrolet', 'Malibu', 2022, 47.00, 1, 4, 1800),
(6, 'Hyundai', 'Sonata', 2023, 49.00,0 , 7, 1400),
(7, 'BMW', '3 Series', 2023, 60.00,1 , 7, 2499),
(8, 'Mercedes', 'C-Class', 2022, 58.00,1, 8, 2599),
(9, 'Audi', 'A4', 2022, 55.00, 0, 4, 2500),
(10, 'Lexus', 'ES', 2023, 54.00, 1, 4, 2500)


INSERT INTO Customer VALUES
(1, 'John', 'Doe', 'johndoe@example.com', '555-555-5555'),
(2, 'Jane', 'Smith', 'janesmith@example.com', '555-123-4567'),
(3, 'Robert', 'Johnson', 'robert@example.com', '555-789-1234'),
(4, 'Sarah', 'Brown', 'sarah@example.com', '555-456-7890'),
(5, 'David', 'Lee', 'david@example.com', '555-987-6543'),
(6, 'Laura', 'Hall', 'laura@example.com', '555-234-5678'),
(7, 'Michael', 'Davis', 'michael@example.com', '555-876-5432'),
(8, 'Emma', 'Wilson', 'emma@example.com', '555-432-1098'),
(9, 'William', 'Taylor', 'william@example.com', '555-321-6547'),
(10, 'Olivia', 'Adams', 'olivia@example.com', '555-765-4321')


INSERT INTO Lease VALUES
(1, 1, 1, '2023-01-01', '2023-01-05', 'Daily'),
(2, 2, 2, '2023-02-15', '2023-02-28', 'Monthly'),
(3, 3, 3, '2023-03-10', '2023-03-15', 'Daily'),
(4, 4, 4, '2023-04-20', '2023-04-30', 'Monthly'),
(5, 5, 5, '2023-05-05', '2023-05-10', 'Daily'),
(6, 4, 3, '2023-06-15', '2023-06-30', 'Monthly'),
(7, 7, 7, '2023-07-01', '2023-07-10', 'Daily'),
(8, 8, 8, '2023-08-12', '2023-08-15', 'Monthly'),
(9, 3, 3, '2023-09-07', '2023-09-10', 'Daily'),
(10, 10, 10, '2023-10-10', '2023-10-31', 'Monthly')


INSERT INTO Payment VALUES
(1, 1, '2023-01-03', 200.00),
(2, 2, '2023-02-20', 1000.00),
(3, 3, '2023-03-12', 75.00),
(4, 4, '2023-04-25', 900.00),
(5, 5, '2023-05-07', 60.00),
(6, 6, '2023-06-18', 1200.00),
(7, 7, '2023-07-03', 40.00),
(8, 8, '2023-08-14', 1100.00),
(9, 9, '2023-09-09', 80.00),
(10, 10, '2023-10-25', 1500.00)

SELECT * FROM Customer

SELECT * FROM Vehicle

SELECT * FROM Lease

SELECT * FROM Payment


--query_1
--Update the daily rate for a Mercedes car to 68.

UPDATE vehicle
SET dailyRate=68.00
WHERE make='Mercedes'

SELECT * FROM Vehicle WHERE make='Mercedes'


--query_2
-- Delete a specific customer and all associated leases and payments.

DELETE FROM Customer
WHERE customerID = 1

--query_3
--Rename the "paymentDate" column in the Payment table to "transactionDate".

Exec sp_rename 'Payment.paymentDate', 'transactionDate','COLUMN'

SELECT transactionDate FROM Payment

--query_4
--Find a specific customer by email. 

SELECT CONCAT(firstName,SPACE(1),lastName) AS CUSTOMERNAME
FROM Customer
WHERE email='robert@example.com'

--query_5
--Get active leases for a specific customer.

SELECT * FROM Lease
WHERE customerID = 4 AND GETDATE() BETWEEN startDate AND endDate

--query_6
--Find all payments made by a customer with a specific phone number.

SELECT P.* FROM Payment p JOIN Lease l 
ON p.leaseID=l.leaseID JOIN
Customer c ON l.customerID=c.customerID
WHERE phoneNumber='555-123-4567'

--query_7
--Calculate the average daily rate of all available cars. 

SELECT AVG(dailyRate) AS AverageRate FROM vehicle
WHERE available = 1

--query_8
--Find the car with the highest daily rate. 


SELECT TOP 1 dailyRate,vehicleID
FROM Vehicle
ORDER BY  dailyRate DESC


--query_9
--Retrieve all cars leased by a specific customer.

SELECT v.* FROM Vehicle v
JOIN Lease l ON v.vehicleID = l.vehicleID
WHERE l.customerID = 3

--query_10
-- Find the details of the most recent lease.

SELECT TOP 1 *
FROM Lease
ORDER BY endDate DESC

--query_11
--List all payments made in the year 2023. 

SELECT * FROM Payment
WHERE YEAR(transactionDate) = 2023

--query_12
--  Retrieve customers who have not made any payments.

SELECT C.* FROM Customer c 
WHERE NOT EXISTS( 
    SELECT customerID
    FROM Lease l
    JOIN Payment p ON l.leaseID = p.leaseID
    WHERE l.customerID = c.customerID
	)

--query_13
-- Retrieve Car Details and Their Total Payments.
SELECT v.make,v.model,v.year,v.dailyRate,v.passengerCapacity,v.engineCapacity,SUM(p.amount) AS TOTALPAYMENT FROM Vehicle v 
JOIN Lease l ON v.vehicleID=l.vehicleID
JOIN Payment p ON l.leaseID=p.leaseID
GROUP BY v.make,v.model,v.year,v.dailyRate,v.passengerCapacity,v.engineCapacity


--query_14
-- Calculate Total Payments for Each Customer. 

SELECT CONCAT(c.firstName,SPACE(1),c.lastName) AS CUSTOMER_NAME, SUM(p.amount) AS TotalPaid
FROM Customer c
JOIN Lease l ON c.customerID = l.customerID
JOIN Payment p ON l.leaseID = p.leaseID
GROUP BY c.firstName, c.lastName

--query_15
--List Car Details for Each Lease. 

SELECT l.leaseID, v.make, v.model, v.year, l.startDate, l.endDate
FROM Lease l
JOIN Vehicle v ON l.vehicleID = v.vehicleID


--query_16
--Retrieve Details of Active Leases with Customer and Car Information.

SELECT l.leaseID, c.firstName, c.lastName, v.make, v.model, l.startDate, l.endDate
FROM Lease l
JOIN Customer c ON l.customerID = c.customerID
JOIN Vehicle v ON l.vehicleID = v.vehicleID
WHERE GETDATE() BETWEEN l.startDate AND l.endDate


--query_17
-- Find the Customer Who Has Spent the Most on Leases.

SELECT TOP 1 c.firstName, c.lastName, SUM(p.amount) AS TotalSpent
FROM Customer c
JOIN Lease l ON c.customerID = l.customerID
JOIN Payment p ON l.leaseID = p.leaseID
GROUP BY c.firstName, c.lastName
ORDER BY TotalSpent DESC


--query_18
-- List All Cars with Their Current Lease Information.

SELECT v.make, v.model, l.startDate, l.endDate, c.firstName, c.lastName
FROM Vehicle v
LEFT JOIN Lease l ON v.vehicleID = l.vehicleID AND GETDATE() BETWEEN l.startDate AND l.endDate
LEFT JOIN Customer c ON l.customerID = c.customerID
