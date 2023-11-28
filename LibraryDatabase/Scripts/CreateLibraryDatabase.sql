IF SCHEMA_ID(N'LibraryDB') IS NULL
	EXEC(N'CREATE SCHEMA LibraryDB;');
GO

-- DROP TABLE IF EXISTS --
DROP TABLE IF EXISTS LibraryDB.Author;
DROP TABLE IF EXISTS LibraryDB.Audience;
DROP TABLE IF EXISTS LibraryDB.Genre;
DROP TABLE IF EXISTS LibraryDB.BookTitle;
DROP TABLE IF EXISTS LibraryDB.Shelf;
DROP TABLE IF EXISTS LibraryDB.BookCopy;
DROP TABLE IF EXISTS LibraryDB.History;
DROP TABLE IF EXISTS LibraryDB.Patron;

CREATE TABLE LibraryDB.Author
(
	AuthorID INT IDENTITY NOT NULL PRIMARY KEY,
	FullName NVARCHAR(64) NOT NULL,
	College NVARCHAR(64),
	HomeCountry NVARCHAR(64),
	BirthDate DATETIME,
	LifeStatus NVARCHAR(32)
);

CREATE TABLE LibraryDB.Audience
(
	AudienceID INT IDENTITY NOT NULL PRIMARY KEY,
	AudienceName NVARCHAR(32) NOT NULL,
	KidsRead BIT NOT NULL
);

CREATE TABLE LibraryDB.Genre
(
	GenreID INT IDENTITY NOT NULL PRIMARY KEY,
	GenreName NVARCHAR(64) NOT NULL
)

CREATE TABLE LibraryDB.BookTitle
(
	BookTitleID INT IDENTITY NOT NULL PRIMARY KEY,
	AuthorID INT UNIQUE NOT NULL FOREIGN KEY
		REFERENCES LibraryDB.Author(AuthorID), 
	AudienceID INT UNIQUE NOT NULL FOREIGN KEY
		REFERENCES LibraryDB.Audience(AudienceID),
	GenreID INT UNIQUE NOT NULL FOREIGN KEY
		REFERENCES LibraryDB.Genre(GenreID),
	ISBN INT UNIQUE NOT NULL,
	Title NVARCHAR(128) NOT NULL,
	PublishDate DATE,
	Publisher NVARCHAR(64)
);

CREATE TABLE LibraryDB.Shelf
(
	ShelfID INT IDENTITY NOT NULL PRIMARY KEY,
	ShelfNumber INT,
	Section INT
);

CREATE TABLE LibraryDB.BookCopy
(
	BookCopyID INT IDENTITY NOT NULL PRIMARY KEY,
	BookTitleID INT NOT NULL FOREIGN KEY
		REFERENCES LibraryDB.BookTitle(BookTitleID),
	ShelfID INT NOT NULL FOREIGN KEY
		REFERENCES LibraryDB.Shelf(ShelfID),
	[PageCount] INT NOT NULL,
	DamageLevel INT NOT NULL
);

CREATE TABLE LibraryDB.History
(
	HistoryID INT IDENTITY NOT NULL PRIMARY KEY,
	BookCopyID INT NOT NULL FOREIGN KEY
		REFERENCES LibraryDB.BookCopy(BookCopyID),
	CheckedOutDate DATETIME NOT NULL,
	CheckedInDate DATETIME,
);

CREATE TABLE LibraryDB.Patron
(
	PatronID INT IDENTITY NOT NULL PRIMARY KEY,
	HistoryID INT NOT NULL FOREIGN KEY
		REFERENCES LibraryDB.History(HistoryID),
	CardNumber INT NOT NULL UNIQUE,
	[FullName] NVARCHAR(64) NOT NULL,
	PhoneNumber NVARCHAR(16),
	[Address] NVARCHAR(128) NOT NULL,
	BirthDate DATE NOT NULL,
	KidReader BIT NOT NULL
);

SELECT *
FROM LibraryDB.Author