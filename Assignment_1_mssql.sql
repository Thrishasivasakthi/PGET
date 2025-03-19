create table Customers(
CustomerID int PRIMARY KEY IDENTITY(0,1),
FirstName varchar(25) NOT NULL,
LastName varchar(25) NOT NULL,
Email varchar(255) NOT NULL,
Phone bigint,
Address varchar(255))
GO

create table Products(
ProductID int PRIMARY KEY,
ProductName varchar(255),
Description varchar(255),
Price money
)
GO

create table Orders(
OrderID int PRIMARY KEY,
CustomerID int FOREIGN KEY REFERENCES Customers(CustomerID),
OrderDate date,
TotalAmount money
)
GO

create table OrderDetails(
OrderDetailID int PRIMARY KEY,
OrderID int FOREIGN KEY REFERENCES Orders(OrderID),
ProductID int FOREIGN KEY REFERENCES Products(ProductID),
Quantity int
)
GO

create table Inventory(
InventoryID int PRIMARY KEY,
ProductID int FOREIGN KEY REFERENCES Products(ProductID),
QuantityInStock int,
LaststockUpdate datetime
)
GO

INSERT INTO Customers (FirstName, LastName, Email, Phone, Address)
VALUES 
('John', 'Doe', 'john.doe@example.com', 1234567899, '123 Main St'),
('Jane', 'Smith', 'jane.smith@example.com', 2345678901, '456 Elm St'),
('Alice', 'Johnson', 'alice.johnson@example.com', 3456789012, '789 Oak St'),
('Bob', 'Brown', 'bob.brown@example.com', 4567890123, '101 Pine St'),
('Charlie', 'Davis', 'charlie.davis@example.com', 5678901234, '202 Maple St'),
('Eva', 'Evans', 'eva.evans@example.com', 6789012345, '303 Birch St'),
('Frank', 'Garcia', 'frank.garcia@example.com', 7890123456, '404 Cedar St'),
('Grace', 'Harris', 'grace.harris@example.com', 8901234567, '505 Spruce St'),
('Henry', 'Martinez', 'henry.martinez@example.com', 9012345678, '606 Fir St'),
('Ivy', 'Lopez', 'ivy.lopez@example.com', 0123456789, '707 Redwood St');


INSERT INTO Products (ProductID,ProductName, Description, Price)
VALUES 
(101,'Laptop', '15-inch, 8GB RAM, 256GB SSD', 999.99),
(102,'Smartphone', '6.5-inch, 128GB, 5G', 699.99),
(103,'Tablet', '10-inch, 64GB, Wi-Fi', 399.99),
(104,'Smartwatch', 'Heart rate monitor, GPS', 199.99),
(105,'Headphones', 'Noise-cancelling, wireless', 149.99),
(106,'Speaker', 'Bluetooth, waterproof', 99.99),
(107,'Camera', '20MP, 4K video', 499.99),
(108,'Printer', 'Wireless, color', 199.99),
(109,'Monitor', '27-inch, 4K UHD', 299.99),
(110,'Keyboard', 'Mechanical, RGB', 79.99)

INSERT INTO Orders (OrderID,CustomerID, OrderDate, TotalAmount)
VALUES 
(1,2, '2023-10-01', 999.99),
(2,3, '2023-10-02', 699.99),
(3,4, '2023-10-03', 399.99),
(4,5, '2023-10-04', 199.99),
(5,6, '2023-10-05', 149.99),
(6,7, '2023-10-06', 99.99),
(7,8, '2023-10-07', 499.99),
(8,9, '2023-10-08', 199.99),
(9,10, '2023-10-09', 299.99),
(10,11, '2023-10-10', 79.99);
GO

INSERT INTO OrderDetails (OrderDetailID, OrderID, ProductID, Quantity)
VALUES 
(1, 2, 101, 1),  
(2, 3, 102, 2),  
(3, 4, 103, 7),  
(4, 5, 104, 6),  
(5, 6, 105, 9), 
(6, 7, 106, 5),  
(7, 8, 107, 4),  
(8, 9, 108, 3),  
(9, 10, 109, 6),  
(10, 11, 110, 3); 
GO

INSERT INTO Inventory (InventoryID, ProductID, QuantityInStock, LastStockUpdate)
VALUES 
(1, 101, 10, '2023-10-01'),  
(2, 102, 15, '2023-10-02'),  
(3, 103, 20, '2023-10-03'),  
(4, 104, 25, '2023-10-04'),  
(5, 105, 30, '2023-10-05'),  
(6, 106, 35, '2023-10-06'),  
(7, 107, 40, '2023-10-07'),  
(8, 108, 45, '2023-10-08'),  
(9, 109, 50, '2023-10-09'),  
(10, 110, 55, '2023-10-10'); 
GO

--TASK_2  1

SELECT FirstName,LastName,Email from Customers;

--query_2

select o.orderID,o.OrderDate,c.FirstName from Orders o INNER JOIN Customers c ON o.CustomerID = c.CustomerID

--query_3

INSERT INTO Customers VALUES('thrisha','siva','thrisha@gmail.com',8934567891,'234 edf st');

--query_4

update Products
set Price = Price*1.10;

--query_5

delete from OrderDetails where OrderID=1


delete from Orders where OrderID =1



--query_6

INSERT INTO Orders VALUES(11,12,'2023-10-11',199.99)

--query_7

update Customers 
set Email='sara@gmail.com' , Address='267 john st'
where CustomerID=1

--query_8

select * from Orders
select * from OrderDetails

update Orders 
set Orders.TotalAmount = (select sum(od.Quantity*p.price) from OrderDetails od INNER JOIN Products p ON od.ProductID=p.ProductID where od.OrderID=Orders.OrderID)


--query_9


delete from OrderDetails where OrderID IN(select OrderID from Orders where CustomerID=2)
delete from Orders where CustomerID=2


--query_10

INSERT INTO Products VALUES(111,'TV','32-inch HD Screen',29999.99)

--query_11

ALTER TABLE Orders
ADD Order_status varchar(25) 

select * from Orders

update Orders
set Order_status = 'pending'
where OrderID=2

update Orders
set Order_status = 'pending'
where OrderID=3

update Orders
set Order_status = 'shipped'
where OrderID=4


update Orders
set Order_status = 'shipped'
where OrderID=5


update Orders
set Order_status = 'shipped'
where OrderID=6


update Orders
set Order_status = 'shipped'
where OrderID=7


update Orders
set Order_status = 'pending'
where OrderID=8


update Orders
set Order_status = 'shipped'
where OrderID=9


update Orders
set Order_status = 'pending'
where OrderID=10


update Orders
set Order_status = 'shipped'
where OrderID=11


update Orders
set Order_status = 'shipped'
where OrderID = 10

--query_12

ALTER TABLE Customers
ADD OrderCount int

update Customers set OrderCount = (select count(OrderID) from Orders where Orders.CustomerID=Customers.CustomerID)

--task_3

--query_1

select o.orderID,o.CustomerID,o.OrderDate,o.TotalAmount,c.FirstName,c.LastName from Orders o INNER JOIN Customers c ON o.CustomerID=c.CUstomerID

--query_2

select p.ProductName,SUM(p.Price*o.Quantity) AS TotalRevenue from Products p INNER JOIN OrderDetails o ON p.ProductID=o.ProductID group by p.ProductName

--query_3

select c.FirstName ,c.LastName ,c.Email,c.Phone from Customers c INNER JOIN Orders o ON c.CustomerID=o.CustomerID where OrderCount>=1;

--query_4

select top 1 p.ProductID,p.ProductName,o.Quantity from Products p INNER JOIN OrderDetails o ON p.ProductID=o.ProductID group by p.ProductID,p.productName,o.Quantity order by o.Quantity desc

--query_5
SELECT ProductID, ProductName, Description 
FROM Products 
GROUP BY ProductID, ProductName, Description;

--query_6

select CONCAT(FirstName,SPACE(1),LastName) AS CustomerName,
avg(OrderCount) AS Average_order_val
from Customers
GROUP BY FirstName,LastName

--query_7

select  top 1 o.OrderID,CONCAT(c.FirstName,SPACE(1),c.LastName) AS CustomerName,o.TotalAmount
from Orders o INNER JOIN Customers c ON o.CustomerID=c.CustomerID
ORDER BY TotalAmount DESC

--query_8

select p.ProductID,p.ProductName,COUNT(o.ProductID)
from OrderDetails o INNER JOIN Products p ON o.ProductID=p.ProductID
GROUP BY p.ProductID,p.ProductName

--query_9

select Customers.FirstName from Customers JOIN Orders ON Customers.CustomerID=Orders.CustomerID
JOIN OrderDetails ON Orders.OrderID=OrderDetails.OrderID
JOIN Products ON OrderDetails.ProductID=Products.ProductID
where Products.ProductName='Laptop'

--query_10

select SUM(TotalAmount) AS TotalRevenue from Orders
where OrderDate BETWEEN '2023-10-1' AND '2023-10-5'

--task_4

--query_1

select * from Orders


select c.CustomerID,c.FirstName 
from Customers c
where CustomerID NOT IN (select o.CustomerID from Orders o)

--query_2--error

SELECT COUNT(*) AS TotalProducts
FROM Products;

--query_3

SELECT SUM(TotalAmount) AS TotalRevenue
FROM Orders;

--query_4


SELECT AVG(OrderDetails.Quantity) AS AverageQuantityOrdered
FROM OrderDetails
JOIN Products ON OrderDetails.ProductID = Products.ProductID
WHERE Products.ProductName='Laptop'

--query_5

select SUM(o.TotalAmount) AS TOTAL_REVENUE,c.FirstName
from Orders o JOIN Customers c ON o.CustomerID=c.CustomerID
where c.CustomerID=4
GROUP BY c.FirstName

--query_6

select  top 1 CONCAT(FirstName,SPACE(1),LastName) AS CustomerName,
CustomerID,OrderCount from Customers
order by OrderCount desc

--query_7

SELECT TOP 1 Products.ProductName, SUM(OrderDetails.Quantity) AS TotalQuantityOrdered
FROM OrderDetails
JOIN Products ON OrderDetails.ProductID = Products.ProductID
GROUP BY Products.ProductName
ORDER BY TotalQuantityOrdered DESC;

--query_8

SELECT TOP 1 Customers.CustomerID, Customers.FirstName, Customers.LastName, SUM(Orders.TotalAmount) AS TotalSpending
FROM Customers
JOIN Orders ON Customers.CustomerID = Orders.CustomerID
GROUP BY Customers.CustomerID, Customers.FirstName, Customers.LastName
ORDER BY TotalSpending DESC;

--query_9

SELECT AVG(TotalAmount) AS AverageOrderValue
FROM Orders

--query_10

SELECT Customers.CustomerID, Customers.FirstName, Customers.LastName, COUNT(Orders.OrderID) AS OrderCount
FROM Customers
LEFT JOIN Orders ON Customers.CustomerID = Orders.CustomerID
GROUP BY Customers.CustomerID, Customers.FirstName, Customers.LastName


SELECT * FROM Customers