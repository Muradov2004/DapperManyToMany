CREATE DATABASE ManyToManyDB

USE ManyToManyDB

CREATE TABLE Authors(
[Id] int PRIMARY KEY IDENTITY(1, 1),
[Name] nvarchar(max) NOT NULL
)

CREATE TABLE Books(
[Id] int PRIMARY KEY IDENTITY(1, 1),
[Name] nvarchar(max) NOT NULL,
[Price] money NOT NULL
)

CREATE TABLE AuthorBook(
[AuthorId] int REFERENCES Authors(Id),
[BookId] int REFERENCES Books(Id)
)

INSERT INTO Authors
VALUES
('Author1'),
('Author2'),
('Author3')

INSERT INTO Books
VALUES
('Book1',10),
('Book2',12),
('Book3',15),
('Book4',27),
('Book5',22)


INSERT INTO AuthorBook
VALUES
(1,1),
(1,2),
(2,2),
(2,3),
(3,4),
(3,5)