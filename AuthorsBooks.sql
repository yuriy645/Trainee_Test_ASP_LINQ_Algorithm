
-- DROP DATABASE Test 
CREATE DATABASE Test 
GO     

ALTER DATABASE Test
COLLATE Cyrillic_General_CI_AS	
GO

USE Test
GO

--DROP TABLE Genres
CREATE TABLE Genres
(
	GenreId int NOT NULL IDENTITY CONSTRAINT PK_Genres_GenreId PRIMARY KEY,
	GenreName nvarchar(50) NOT NULL
); 
GO

INSERT INTO Genres(GenreName)
	VALUES
		('Detective'),     --1
		('Fantasy'),   --2
		('Humor'),         --3
		('Scientific book') --4		
GO		


--DROP TABLE Books
CREATE TABLE Books
(
	BookId  int NOT NULL IDENTITY CONSTRAINT PK_Books_BookId PRIMARY KEY,
	BookName nvarchar(50) NOT NULL, 
	GenreId int NOT NULL CONSTRAINT FK_Books_Genres FOREIGN KEY (GenreId)  REFERENCES Genres (GenreId),
	Pages  int NOT NULL
); 
GO

INSERT INTO Books(BookName, GenreId, Pages)
	VALUES
		('The mystery of the missing cat', 1, 352), --1
		('War of the Worlds', 2, 576),              --2
		('Humorous', 3, 153),                       --3
		('Semiconductor circuitry', 4, 806)         --4
GO		

--DROP TABLE FirstNames
CREATE TABLE FirstNames
(
	FirstNameId int NOT NULL IDENTITY CONSTRAINT PK_FirstNames_FirstNameId PRIMARY KEY,
	FirstName   nvarchar(50) NOT NULL
); 
GO

INSERT INTO FirstNames(FirstName)
	VALUES
		('Edmond'), --1
		('Enid'),   --2
		('Pavlo'),  --3
		('Tietze')  --4		
GO	

--DROP TABLE Patronymics
CREATE TABLE Patronymics
(
	PatronymicId int NOT NULL IDENTITY CONSTRAINT PK_Patronymics_PatronymicId PRIMARY KEY,
	Patronymic nvarchar(50) NOT NULL
); 
GO

INSERT INTO Patronymics(Patronymic)
	VALUES
		('Moore'),       --1
		('Mary'),        --2
		('Prokopovich'), --3
		('-')            --4		
GO	

--DROP TABLE Authors		
CREATE TABLE Authors (
  AuthorId         int NOT NULL IDENTITY CONSTRAINT PK_Authors_AuthorId PRIMARY KEY,
  AuthorSecondName nvarchar(50) NOT NULL,
  FirstNameId      int NOT NULL  CONSTRAINT FK_Authors_FirstNames FOREIGN KEY (FirstNameId)  REFERENCES FirstNames (FirstNameId), 
  PatronymicId     int NOT NULL  CONSTRAINT FK_Authors_Patronymics FOREIGN KEY (PatronymicId)  REFERENCES Patronymics (PatronymicId),
  BirthDay         date NULL 
)
GO

INSERT INTO Authors(AuthorSecondName, FirstNameId, PatronymicId) VALUES
('Hamilton', 1, 1 ), --1
('Blyton', 2, 2 ),   --2
('Glazoviy', 3, 3 ), --3
('Ulrih', 4, 4 )     --4
GO

-- Связь между таблицами - многие ко многим, нужна еще одна таблица:

CREATE TABLE AuthorsBooks (
  AuthorId int NOT NULL CONSTRAINT FK_AuthorsBooks_Authors FOREIGN KEY (AuthorId)  REFERENCES Authors (AuthorId),
  BookId   int NOT NULL CONSTRAINT FK_AuthorsBooks_Books FOREIGN KEY (BookId)  REFERENCES Books (BookId),
  CONSTRAINT PK_AuthorsBooks PRIMARY KEY (AuthorId, BookId)
)
GO

INSERT INTO AuthorsBooks (AuthorId, BookId)
	VALUES
		('1', '2'), 
		('2', '1'), 
		('3', '3'), 
        ('4', '4') 		
GO		


  

