CREATE DATABASE SISDB
USE SISDB

CREATE TABLE Students (
    student_id INT NOT NULL,
	CONSTRAINT PK_Students PRIMARY KEY (student_id),
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    date_of_birth DATE,
    email VARCHAR(100) UNIQUE,
    phone_number VARCHAR(15)
    
);

CREATE TABLE Teacher (
    teacher_id INT NOT NULL,
	CONSTRAINT PK_Teacher PRIMARY KEY (teacher_id),
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email VARCHAR(100) UNIQUE
   
);

CREATE TABLE Courses (
    course_id INT NOT NULL,
	CONSTRAINT PK_Courses PRIMARY KEY (course_id),
    course_name VARCHAR(100) NOT NULL,
    credits INT CHECK (credits > 0),
    teacher_id INT NOT NULL,
    CONSTRAINT FK_Courses_Teacher FOREIGN KEY (teacher_id) 
    REFERENCES Teacher(teacher_id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Enrollments (
    enrollment_id INT NOT NULL,
	CONSTRAINT PK_Enrollments PRIMARY KEY (enrollment_id),
    student_id INT NOT NULL,
	CONSTRAINT FK_Enrollments_Student FOREIGN KEY (student_id) REFERENCES Students(student_id) ON DELETE CASCADE ON UPDATE CASCADE, 
    course_id INT NOT NULL,
	CONSTRAINT FK_Enrollments_Course FOREIGN KEY (course_id) REFERENCES Courses(course_id) ON DELETE CASCADE ON UPDATE CASCADE,
    enrollment_date DATE DEFAULT GETDATE(),
   );

CREATE TABLE Payments (
    payment_id INT NOT NULL,
	CONSTRAINT PK_Payments PRIMARY KEY (payment_id),
    student_id INT NOT NULL,
	CONSTRAINT FK_Payments_Student FOREIGN KEY (student_id) REFERENCES Students(student_id) ON DELETE CASCADE ON UPDATE CASCADE,
    amount DECIMAL(10, 2) CHECK (amount > 0),
    payment_date DATE DEFAULT GETDATE()   
);

INSERT INTO Students (student_id, first_name, last_name, date_of_birth, email, phone_number)  
VALUES 
(1, 'Dan', 'Brown', '2002-05-14', 'dan.brown@example.com', '1234567890'),
(2, 'Sophia', 'Thomas', '2003-10-25', 'sophia.t@example.com', '1234567899'),
(3, 'Jane', 'Smith', '2003-03-22', 'jane.smith@example.com', '1234567891'),
(4, 'Alex', 'Brown', '2001-11-10', 'alex.brown@example.com', '1234567892'),
(5, 'Emma', 'Martinez', '2000-12-11', 'emma.m@example.com', '1234567897'),
(6, 'Emily', 'Davis', '2000-07-30', 'emily.davis@example.com', '1234567893'),
(7, 'David', 'Taylor', '2003-06-05', 'david.t@example.com', '1234567896'),
(8, 'Michael', 'Johnson', '2002-09-15', 'michael.j@example.com', '1234567894'),
(9, 'Sarah', 'Wilson', '2001-04-20', 'sarah.w@example.com', '1234567895'),
(10, 'Daniel', 'Anderson', '2002-08-19', 'daniel.a@example.com', '1234567898');


INSERT INTO Teacher (teacher_id, first_name, last_name, email)  
VALUES 

(1, 'Charles', 'Harris', 'charles.harris@example.com'),
(2, 'Elizabeth', 'Clark', 'elizabeth.clark@example.com'),
(3, 'William', 'Lewis', 'william.lewis@example.com'),
(4, 'Barbara', 'Walker', 'barbara.walker@example.com'),
(5, 'Joseph', 'Hall', 'joseph.hall@example.com'),
(6, 'Susan', 'Allen', 'susan.allen@example.com'),
(7, 'Robert', 'King', 'robert.king@example.com'),
(8, 'Linda', 'Moore', 'linda.moore@example.com'),
(9, 'James', 'White', 'james.white@example.com'),
(10, 'Patricia', 'Lee', 'patricia.lee@example.com');

INSERT INTO Courses (course_id, course_name, credits, teacher_id)  
VALUES 
(1, 'Chemistry', 4, 3),
(2, 'Biology', 3, 4),
(3, 'Computer Science', 5, 5),
(4, 'History', 3, 6),
(5, 'Geography', 2, 7),
(6, 'Mathematics', 4, 1),
(7, 'Physics', 3, 2),
(8, 'Economics', 4, 8),
(9, 'Psychology', 3, 9),
(10, 'Philosophy', 2, 10);

INSERT INTO Enrollments (enrollment_id, student_id, course_id, enrollment_date)  
VALUES 
(1, 1, 1, '2024-01-15'),
(2, 2, 2, '2024-01-16'),
(3, 3, 3, '2024-01-17'),
(4, 4, 4, '2024-01-18'),
(5, 5, 5, '2024-01-19'),
(6, 6, 6, '2024-01-20'),
(7, 7, 7, '2024-01-21'),
(8, 8, 8, '2024-01-22'),
(9, 9, 9, '2024-01-23'),
(10, 10, 10, '2024-01-24');

INSERT INTO Payments (payment_id, student_id, amount, payment_date)  
VALUES 
(1, 1, 500.00, '2024-02-01'),
(2, 2, 450.00, '2024-02-02'),
(3, 3, 600.00, '2024-02-03'),
(4, 4, 550.00, '2024-02-04'),
(5, 5, 700.00, '2024-02-05'),
(6, 6, 480.00, '2024-02-06'),
(7, 7, 520.00, '2024-02-07'),
(8, 8, 620.00, '2024-02-08'),
(9, 9, 530.00, '2024-02-09'),
(10, 10, 610.00, '2024-02-10');

SELECT * FROM Students

SELECT * FROM Teacher

SELECT * FROM Courses

SELECT * FROM Enrollments

SELECT * FROM Payments

---task_2

--query_1

INSERT INTO Students (student_id, first_name, last_name, date_of_birth, email, phone_number)
VALUES (11, 'thrisha', 'siva', '1995-08-15', 'thrisha@example.com', '1234567890');

SELECT * FROM Students

--query_2

INSERT INTO Enrollments (enrollment_id, student_id, course_id, enrollment_date)  
VALUES (11, 1, 5, '2024-03-19');

SELECT * FROM Enrollments

--query_3

UPDATE Teacher SET email = 'hiupdated@example.com' where teacher_id = 9 and email = 'james.white@example.com'
SELECT * FROM Teacher

--query_4

DELETE FROM Enrollments WHERE student_id = 1 and course_id = 5

SELECT * FROM Enrollments

--query_5

UPDATE Courses SET teacher_id = 7 WHERE course_id = 3

SELECT * FROM Courses

--query_6

DELETE FROM Enrollments WHERE student_id = 7
DELETE FROM Students WHERE student_id = 7

SELECT * FROM Enrollments
SELECT * FROM Students

--query_7


UPDATE Payments SET amount = 550 WHERE student_id = 1

SELECT * FROM Payments

--task_3

--query_1

SELECT s.first_name, s.last_name, 
SUM(p.amount) AS total_payments  
FROM Students s  
JOIN Payments p ON s.student_id = p.student_id  
WHERE s.student_id = 4  
GROUP BY s.first_name, s.last_name;

--query_2

SELECT c.course_id, c.course_name,  
COUNT(e.student_id) AS student_count  
FROM Courses c  
LEFT JOIN Enrollments e ON c.course_id = e.course_id  
GROUP BY c.course_id, c.course_name  
ORDER BY student_count DESC;

--query_3

SELECT s.first_name, s.last_name as student_name,e.course_id
FROM Students s
LEFT JOIN Enrollments e ON s.student_id = e.student_id
WHERE e.course_id IS NULL

--query_4

SELECT s.first_name,s.last_name,c.course_name
FROM Students s  
JOIN Enrollments e ON s.student_id = e.student_id  
JOIN Courses c ON e.course_id = c.course_id  
ORDER BY s.last_name, s.first_name;

--query_5

SELECT t.first_name,t.last_name,c.course_name
FROM Teacher t  
JOIN Courses c ON t.teacher_id = c.teacher_id  
ORDER BY t.last_name, t.first_name;

--query_6

SELECT s.first_name,s.last_name,e.enrollment_date,c.course_name
FROM Students s  
JOIN Enrollments e ON s.student_id = e.student_id  
JOIN Courses c ON e.course_id = c.course_id  
WHERE c.course_name = 'History'  
ORDER BY e.enrollment_date;

--query_7

SELECT s.first_name,s.last_name
FROM Students s  
LEFT JOIN Payments p ON s.student_id = p.student_id  
WHERE p.payment_id IS NULL  
ORDER BY s.last_name, s.first_name;

--query_8

SELECT c.course_id,c.course_name
FROM Courses c
LEFT JOIN Enrollments e ON c.course_id = e.Course_id
WHERE e.enrollment_id IS NULL
ORDER BY c.course_name;

--query_9

SELECT * FROM Enrollments

SELECT e.student_id , e1.course_id from Enrollments e 
JOIN Enrollments e1 ON e.enrollment_id=e1.enrollment_id

--query_10

SELECT t.first_name,t.last_name,c.course_name
FROM Teacher t
LEFT JOIN Courses c ON t.teacher_id = c.teacher_id
WHERE c.course_id is NULL

--task_4

--query_1

SELECT AVG(student_count) AS average_students_per_course
FROM (
    SELECT course_id,
    COUNT(student_id) AS student_count
    FROM Enrollments
    GROUP BY course_id
) AS course_enrollment_counts;

--query_2


SELECT s.first_name,s.last_name,p.amount AS highest_payment
FROM Students s
JOIN Payments p ON s.student_id = p.student_id
WHERE p.amount = (SELECT MAX(amount) FROM Payments);

--query_3

SELECT c.course_name,COUNT(e.student_id) AS enrollment_count
FROM Courses c
JOIN Enrollments e ON c.course_id = e.course_id
GROUP BY c.course_name
HAVING COUNT(e.student_id) = (
    SELECT MAX(course_enrollment_count) 
    FROM (
        SELECT COUNT(student_id) AS course_enrollment_count 
        FROM Enrollments 
        GROUP BY course_id
    ) AS enrollment_counts
);

--query_4

SELECT t.first_name,t.last_name,SUM(p.amount) AS total_payments
FROM Teacher t
JOIN Courses c ON t.teacher_id = c.teacher_id
LEFT JOIN Enrollments e ON c.course_id = e.course_id
LEFT JOIN Payments p ON e.student_id = p.student_id
GROUP BY t.first_name, t.last_name;

--query_5

SELECT s.first_name,s.last_name
FROM Students s
WHERE (
    SELECT COUNT(DISTINCT e.course_id)
    FROM Enrollments e
    WHERE e.student_id = s.student_id
) = (
    SELECT COUNT(*) 
    FROM Courses
);


--query_6

SELECT first_name,last_name
FROM Teacher t
WHERE t.teacher_id NOT IN (
    SELECT DISTINCT teacher_id 
    FROM Courses
    WHERE teacher_id IS NOT NULL
);

--query_7

SELECT AVG((SELECT DATEDIFF(YEAR, s.date_of_birth, GETDATE()))) AS avg_age
FROM Students s;

--query_8

SELECT course_id, course_name 
FROM Courses c
WHERE course_id NOT IN (
    SELECT DISTINCT course_id 
    FROM Enrollments
);

--query_9

SELECT s.student_id,s.first_name,s.last_name,c.course_name,
    (SELECT SUM(p.amount)
     FROM Payments p
     WHERE p.student_id = s.student_id
    ) AS total_payment
FROM Students s
JOIN Enrollments e ON s.student_id = e.student_id
JOIN Courses c ON e.course_id = c.course_id
ORDER BY s.student_id, c.course_name;

--query_10

SELECT student_id, first_name, last_name
FROM Students s
WHERE student_id IN (
    SELECT p.student_id
    FROM Payments p
    GROUP BY p.student_id
    HAVING COUNT(p.payment_id) > 1
);

--query_11

SELECT s.student_id,s.first_name,s.last_name,
SUM(p.amount) AS total_payments
FROM Students s
LEFT JOIN Payments p ON s.student_id = p.student_id
GROUP BY s.student_id, s.first_name, s.last_name;

--query_12

SELECT c.course_name,
COUNT(e.student_id) AS student_count
FROM Courses c
JOIN Enrollments e ON c.course_id = e.course_id
GROUP BY c.course_name;

--query_13

SELECT s.student_id,s.first_name,s.last_name,
AVG(p.amount) AS average_payment
FROM Students s
JOIN Payments p ON s.student_id = p.student_id
GROUP BY s.student_id, s.first_name, s.last_name;