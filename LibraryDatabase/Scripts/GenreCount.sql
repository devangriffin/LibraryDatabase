SELECT G.GenreName, COUNT(BT.BookTitleID) AS BookCount
FROM LibraryDB.Genre G
	LEFT JOIN LibraryDB.BookTitle BT ON BT.GenreID = G.GenreID
GROUP BY G.GenreName
ORDER BY G.GenreName;