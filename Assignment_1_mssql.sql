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
(1, 1, 101, 1),  
(2, 2, 102, 1),  
(3, 3, 103, 1),  
(4, 4, 104, 1),  
(5, 5, 105, 1), 
(6, 6, 106, 1),  
(7, 7, 107, 1),  
(8, 8, 108, 1),  
(9, 9, 109, 1),  
(10, 10, 110, 1); 
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

--query_8

--query_9

--query_10

INSERT INTO Products VALUES(111,'TV','32-inch HD Screen',29999.99)

--query_11

--query_12

ALTER TABLE Customers
ADD OrderCount int

update Customers set OrderCount = (select count(OrderID) from Orders where Orders.CustomerID=Customers.CustomerID)

--task_3

--query_1

select o.orderID,o.CustomerID,o.OrderDate,o.TotalAmount,c.FirstName,c.LastName from Orders o INNER JOIN Customers c ON o.CustomerID=c.CUstomerID

--query_2--error

select p.ProductName,SUM(p.Price*o.Quantity) AS TotalRevenue from Products p INNER JOIN OrderDetails o ON p.ProductID=o.ProductID group by p.ProductID

--query_3

select c.FirstName ,c.LastName ,c.Email,c.Phone from Customers c INNER JOIN Orders o ON c.CustomerID=o.CustomerID where OrderCount>=1;

--query_4

select top 1 p.ProductID,p.ProductName,o.Quantity from Products p INNER JOIN OrderDetails o ON p.ProductID=o.ProductID group by p.ProductID,p.productName,o.Quantity order by o.Quantity desc

--query_5
SELECT ProductID, ProductName, Description 
FROM Products 
GROUP BY ProductID, ProductName, Description;

--query_6
