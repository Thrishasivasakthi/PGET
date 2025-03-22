CREATE DATABASE CourierManagementSystem;
GO

USE CourierManagementSystem;

CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(255) NOT NULL,
    Email VARCHAR(255) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    ContactNumber VARCHAR(20),
    Address TEXT
);

CREATE TABLE Couriers (
    CourierID INT PRIMARY KEY IDENTITY(1,1),
    SenderName VARCHAR(255) NOT NULL,
    SenderAddress TEXT NOT NULL,
    ReceiverName VARCHAR(255) NOT NULL,
    ReceiverAddress TEXT NOT NULL,
    Weight DECIMAL(5, 2) NOT NULL,
    Status VARCHAR(50) NOT NULL,
    TrackingNumber VARCHAR(20) UNIQUE NOT NULL,
    DeliveryDate DATE,
    UserID INT,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

CREATE TABLE CourierServices (
    ServiceID INT PRIMARY KEY IDENTITY(1,1),
    ServiceName VARCHAR(100) NOT NULL,
    Cost DECIMAL(8, 2) NOT NULL
);

CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(255) NOT NULL,
    Email VARCHAR(255) UNIQUE NOT NULL,
    ContactNumber VARCHAR(20),
    Role VARCHAR(50) NOT NULL,
    Salary DECIMAL(10, 2) NOT NULL
);

CREATE TABLE Locations (
    LocationID INT PRIMARY KEY IDENTITY(1,1),
    LocationName VARCHAR(100) NOT NULL,
    Address TEXT NOT NULL
);

CREATE TABLE Payments (
    PaymentID INT PRIMARY KEY IDENTITY(1,1),
    CourierID INT,
    LocationID INT,
    Amount DECIMAL(10, 2) NOT NULL,
    PaymentDate DATE NOT NULL,
    FOREIGN KEY (CourierID) REFERENCES Couriers(CourierID),
    FOREIGN KEY (LocationID) REFERENCES Locations(LocationID)
);

INSERT INTO Users (Name, Email, Password, ContactNumber, Address)
VALUES
('Rahul Sharma', 'rahul.sharma@example.com', 'password123', '9876543210', '123 MG Road, Mumbai'),
('Priya Patel', 'priya.patel@example.com', 'password456', '8765432109', '456 Gandhi Nagar, Delhi'),
('Amit Kumar', 'amit.kumar@example.com', 'password789', '7654321098', '789 Nehru Street, Bangalore'),
('Anjali Singh', 'anjali.singh@example.com', 'password101', '6543210987', '321 Rajpath, Kolkata'),
('Vikram Gupta', 'vikram.gupta@example.com', 'password112', '5432109876', '654 Marine Drive, Chennai'),
('Sneha Joshi', 'sneha.joshi@example.com', 'password131', '4321098765', '987 Church Street, Hyderabad'),
('Rajesh Yadav', 'rajesh.yadav@example.com', 'password415', '3210987654', '159 Park Avenue, Pune'),
('Pooja Reddy', 'pooja.reddy@example.com', 'password161', '2109876543', '753 Jubilee Hills, Ahmedabad'),
('Arun Verma', 'arun.verma@example.com', 'password718', '1098765432', '852 Banjara Hills, Jaipur'),
('Neha Malhotra', 'neha.malhotra@example.com', 'password192', '9876543211', '963 Connaught Place, Lucknow');


INSERT INTO Couriers (SenderName, SenderAddress, ReceiverName, ReceiverAddress, Weight, Status, TrackingNumber, DeliveryDate, UserID)
VALUES
('Rahul Sharma', '123 MG Road, Mumbai', 'Priya Patel', '456 Gandhi Nagar, Delhi', 2.5, 'In Transit', 'TRK123456', '2023-11-15', 1),
('Amit Kumar', '789 Nehru Street, Bangalore', 'Anjali Singh', '321 Rajpath, Kolkata', 1.8, 'Delivered', 'TRK123457', '2023-11-10', 3),
('Vikram Gupta', '654 Marine Drive, Chennai', 'Sneha Joshi', '987 Church Street, Hyderabad', 3.2, 'Processing', 'TRK123458', '2023-11-20', 5),
('Rajesh Yadav', '159 Park Avenue, Pune', 'Pooja Reddy', '753 Jubilee Hills, Ahmedabad', 4.0, 'In Transit', 'TRK123459', '2023-11-18', 7),
('Arun Verma', '852 Banjara Hills, Jaipur', 'Neha Malhotra', '963 Connaught Place, Lucknow', 2.0, 'Delivered', 'TRK123460', '2023-11-12', 9),
('Priya Patel', '456 Gandhi Nagar, Delhi', 'Rahul Sharma', '123 MG Road, Mumbai', 1.5, 'Processing', 'TRK123461', '2023-11-25', 2),
('Anjali Singh', '321 Rajpath, Kolkata', 'Amit Kumar', '789 Nehru Street, Bangalore', 2.8, 'In Transit', 'TRK123462', '2023-11-22', 4),
('Sneha Joshi', '987 Church Street, Hyderabad', 'Vikram Gupta', '654 Marine Drive, Chennai', 3.5, 'Delivered', 'TRK123463', '2023-11-14', 6),
('Pooja Reddy', '753 Jubilee Hills, Ahmedabad', 'Rajesh Yadav', '159 Park Avenue, Pune', 2.2, 'Processing', 'TRK123464', '2023-11-30', 8),
('Neha Malhotra', '963 Connaught Place, Lucknow', 'Arun Verma', '852 Banjara Hills, Jaipur', 1.0, 'In Transit', 'TRK123465', '2023-11-28', 10);


INSERT INTO CourierServices (ServiceName, Cost)
VALUES
('Standard Delivery', 100.00),
('Express Delivery', 250.00),
('Overnight Delivery', 500.00),
('International Delivery', 1000.00);

INSERT INTO Employees (Name, Email, ContactNumber, Role, Salary)
VALUES
('Ramesh Kumar', 'ramesh.kumar@example.com', '9876543210', 'Manager', 50000.00),
('Suresh Patel', 'suresh.patel@example.com', '8765432109', 'Delivery Executive', 30000.00),
('Anita Sharma', 'anita.sharma@example.com', '7654321098', 'Customer Support', 25000.00),
('Rajesh Singh', 'rajesh.singh@example.com', '6543210987', 'Delivery Executive', 30000.00),
('Priya Gupta', 'priya.gupta@example.com', '5432109876', 'Manager', 50000.00);


INSERT INTO Locations (LocationName, Address)
VALUES
('Mumbai Hub', '123 MG Road, Mumbai'),
('Delhi Hub', '456 Gandhi Nagar, Delhi'),
('Bangalore Hub', '789 Nehru Street, Bangalore'),
('Kolkata Hub', '321 Rajpath, Kolkata'),
('Chennai Hub', '654 Marine Drive, Chennai');

INSERT INTO Payments (CourierID, LocationID, Amount, PaymentDate)
VALUES
(1, 1, 100.00, '2023-11-01'),
(2, 2, 250.00, '2023-11-05'),
(3, 3, 500.00, '2023-11-10'),
(4, 4, 1000.00, '2023-11-15'),
(5, 5, 100.00, '2023-11-20');


--TASK_2

--query_1

SELECT * FROM Users;

--query_2

SELECT * FROM Couriers WHERE UserID = 1

--query_3

SELECT * FROM Couriers;

--query_4

SELECT * FROM Couriers WHERE CourierID = 1

--query_5

SELECT * FROM Couriers WHERE Status = 'Delivered'

--query_6

SELECT * FROM Couriers WHERE Status != 'Delivered'

--query_7

SELECT * FROM Couriers WHERE DeliveryDate = CAST(GETDATE() AS DATE)

--query_8

SELECT * FROM Couriers WHERE Status = 'In Transit'

--query_9

SELECT COUNT(*) AS TotalPackages FROM Couriers GROUP BY UserID

--query_10

SELECT AVG(DATEDIFF(DAY, GETDATE(), DeliveryDate)) AS AvgDeliveryTime FROM Couriers GROUP BY UserID

--query_11

SELECT * FROM Couriers WHERE Weight BETWEEN 2.0 AND 3.0

--query_12

SELECT * FROM Employees WHERE Name LIKE '%John%'

--query_13

SELECT c.* FROM Couriers c
JOIN Payments p ON c.CourierID = p.CourierID
WHERE p.Amount > 50

--TASK_3

--query_1

SELECT e.Name, COUNT(c.CourierID) AS TotalCouriers
FROM Employees e
JOIN Couriers c ON e.EmployeeID = c.CourierID
GROUP BY e.Name

--query_2

SELECT l.LocationName, SUM(p.Amount) AS TotalRevenue
FROM Locations l
JOIN Payments p ON l.LocationID = p.LocationID
GROUP BY l.LocationName

--query_3

SELECT l.LocationName, COUNT(c.CourierID) AS TotalCouriers
FROM Locations l
JOIN Couriers c ON l.LocationID = c.CourierID
GROUP BY l.LocationName;

--query_4

SELECT TOP 1 c.CourierID, AVG(DATEDIFF(DAY, GETDATE(), c.DeliveryDate)) AS AvgDeliveryTime
FROM Couriers c
GROUP BY c.CourierID
ORDER BY AvgDeliveryTime DESC;

--query_5

SELECT l.LocationName, SUM(p.Amount) AS TotalPayments
FROM Locations l
JOIN Payments p ON l.LocationID = p.LocationID
GROUP BY l.LocationName
HAVING SUM(p.Amount) < 1000;

--query_6

SELECT l.LocationName, SUM(p.Amount) AS TotalPayments
FROM Locations l
JOIN Payments p ON l.LocationID = p.LocationID
GROUP BY l.LocationName;

--query_7

SELECT c.CourierID
FROM Couriers c
JOIN Payments p ON c.CourierID = p.CourierID
WHERE p.LocationID = 1 
GROUP BY c.CourierID
HAVING SUM(p.Amount) > 1000;

--query_8

SELECT c.CourierID, c.SenderName, c.ReceiverName, SUM(p.Amount) AS TotalPayments
FROM Couriers c
JOIN Payments p ON c.CourierID = p.CourierID
WHERE p.LocationID = 1 -- Replace 1 with the desired LocationID
GROUP BY c.CourierID, c.SenderName, c.ReceiverName
HAVING SUM(p.Amount) > 1000;

--query_9

SELECT c.CourierID, c.SenderName, c.ReceiverName, SUM(p.Amount) AS TotalPayments
FROM Couriers c
JOIN Payments p ON c.CourierID = p.CourierID
WHERE p.PaymentDate > '2023-11-01' -- Replace '2023-11-01' with the desired date
GROUP BY c.CourierID, c.SenderName, c.ReceiverName
HAVING SUM(p.Amount) > 1000;


--query_10


SELECT l.LocationID, l.LocationName, SUM(p.Amount) AS TotalAmountReceived
FROM Locations l
JOIN Payments p ON l.LocationID = p.LocationID
WHERE p.PaymentDate < '2023-11-30' 
GROUP BY l.LocationID, l.LocationName
HAVING SUM(p.Amount) > 500;

--task_4

--1.
SELECT p.*, c.SenderName, c.ReceiverName
FROM Payments p
INNER JOIN Couriers c ON p.CourierID = c.CourierID;

--2.
SELECT p.*, l.LocationName
FROM Payments p
INNER JOIN Locations l ON p.LocationID = l.LocationID;

--3.
SELECT p.*, c.SenderName, c.ReceiverName, l.LocationName
FROM Payments p
INNER JOIN Couriers c ON p.CourierID = c.CourierID
INNER JOIN Locations l ON p.LocationID = l.LocationID;

--4
SELECT p.*, c.SenderName, c.ReceiverName
FROM Payments p
LEFT JOIN Couriers c ON p.CourierID = c.CourierID;

--5
SELECT c.CourierID, c.SenderName, c.ReceiverName, SUM(p.Amount) AS TotalPayments
FROM Couriers c
LEFT JOIN Payments p ON c.CourierID = p.CourierID
GROUP BY c.CourierID, c.SenderName, c.ReceiverName;

--6
SELECT *
FROM Payments
WHERE PaymentDate = '2023-11-15'

--7
SELECT p.PaymentID, p.Amount, p.PaymentDate, c.CourierID, c.SenderName, c.ReceiverName
FROM Payments p
INNER JOIN Couriers c ON p.CourierID = c.CourierID;
--8

SELECT p.PaymentID, p.Amount, p.PaymentDate, l.LocationName, l.Address
FROM Payments p
INNER JOIN Locations l ON p.LocationID = l.LocationID;

--9

SELECT c.CourierID, c.SenderName, c.ReceiverName, SUM(p.Amount) AS TotalPayments
FROM Couriers c
LEFT JOIN Payments p ON c.CourierID = p.CourierID
GROUP BY c.CourierID, c.SenderName, c.ReceiverName;

--10

SELECT *
FROM Payments
WHERE PaymentDate BETWEEN '2023-11-01' AND '2023-11-30'

--11

SELECT u.UserID, u.Name, c.CourierID, c.SenderName, c.ReceiverName
FROM Users u
FULL OUTER JOIN Couriers c ON u.UserID = c.UserID;

--12

SELECT c.CourierID, c.SenderName, cs.ServiceName
FROM Couriers c
FULL OUTER JOIN CourierServices cs ON c.CourierID = cs.ServiceID;

--13

SELECT e.EmployeeID, e.Name, p.PaymentID, p.Amount
FROM Employees e
FULL OUTER JOIN Payments p ON e.EmployeeID = p.CourierID;

--14

SELECT u.UserID, u.Name, cs.ServiceID, cs.ServiceName
FROM Users u
CROSS JOIN CourierServices cs;

--15

SELECT e.EmployeeID, e.Name, l.LocationID, l.LocationName
FROM Employees e
CROSS JOIN Locations l;


--16

SELECT c.CourierID, c.SenderName, c.SenderAddress
FROM Couriers c
LEFT JOIN Users u ON c.SenderName = u.Name;

--17

SELECT c.CourierID, c.ReceiverName, c.ReceiverAddress
FROM Couriers c
LEFT JOIN Users u ON c.ReceiverName = u.Name;

---18

SELECT c.CourierID, c.SenderName, cs.ServiceName, cs.Cost
FROM Couriers c
LEFT JOIN CourierServices cs ON c.CourierID = cs.ServiceID;

--19

SELECT e.EmployeeID, e.Name, COUNT(c.CourierID) AS TotalCouriers
FROM Employees e
LEFT JOIN Couriers c ON e.EmployeeID = c.CourierID
GROUP BY e.EmployeeID, e.Name;

--20

SELECT l.LocationID, l.LocationName, SUM(p.Amount) AS TotalPayments
FROM Locations l
LEFT JOIN Payments p ON l.LocationID = p.LocationID
GROUP BY l.LocationID, l.LocationName;

--21

SELECT SenderName, COUNT(*) AS TotalCouriers
FROM Couriers
GROUP BY SenderName
HAVING COUNT(*) > 1;

--22

SELECT Role, COUNT(*) AS TotalEmployees
FROM Employees
GROUP BY Role
HAVING COUNT(*) > 1;

--23

SELECT l.LocationName, COUNT(p.PaymentID) AS TotalPayments
FROM Locations l
JOIN Payments p ON l.LocationID = p.LocationID
GROUP BY l.LocationName;

--24

SELECT SenderAddress, COUNT(*) AS TotalCouriers
FROM Couriers
GROUP BY SenderAddress
HAVING COUNT(*) > 1;

--25

SELECT e.EmployeeID, e.Name, COUNT(c.CourierID) AS TotalCouriersDelivered
FROM Employees e
LEFT JOIN Couriers c ON e.EmployeeID = c.CourierID
GROUP BY e.EmployeeID, e.Name;

--26

SELECT c.CourierID, c.SenderName, p.Amount, cs.Cost
FROM Couriers c
JOIN Payments p ON c.CourierID = p.CourierID
JOIN CourierServices cs ON c.CourierID = cs.ServiceID
WHERE p.Amount > cs.Cost;

--27

SELECT *
FROM Couriers
WHERE Weight > (SELECT AVG(Weight) FROM Couriers);

--28

SELECT Name
FROM Employees
WHERE Salary > (SELECT AVG(Salary) FROM Employees);


--29
SELECT SUM(Cost) AS TotalCost
FROM CourierServices
WHERE Cost < (SELECT MAX(Cost) FROM CourierServices);

--30

SELECT *
FROM Couriers
WHERE CourierID IN (SELECT CourierID FROM Payments);

--31

SELECT l.LocationName, p.Amount
FROM Locations l
JOIN Payments p ON l.LocationID = p.LocationID
WHERE p.Amount = (SELECT MAX(Amount) FROM Payments);

--32

SELECT *
FROM Couriers
WHERE Weight > ALL (
    SELECT Weight
    FROM Couriers
    WHERE SenderName = 'Rahul Sharma' 
);




