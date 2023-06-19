CREATE DATABASE ManyToManyDB

USE ManyToManyDB

CREATE TABLE Author(
[Id] int IDENTITY(1, 1),
[Name] nvarchar(max) NOT NULL
)

CREATE TABLE Book(
[Id] int IDENTITY(1, 1),
[Name] nvarchar(max) NOT NULL,
[Price] money NOT NULL
)
