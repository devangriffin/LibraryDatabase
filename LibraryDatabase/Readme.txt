This is a WPF application that displays Book Data and uses a SQL Database to store information.

The Objects folder contains the C# objects that correlate to the columns in SQL
	The Genre and Audience Enumerations are used to easily hook up the Genre and Audience IDs with the WPF application

The App Class (App.xaml.cs) connects the application to the SQL library and inserts a majority of the information into the database.

The Main Window (MainWindow.xaml) is the main window which displays the different books. It can also display the different patrons and a books per genre query.
	The MainWindow.xaml.cs deals with functionality for the WPF Elements and uses SQL commands to get information from the database.

The Add Window (AddWindow.xaml) is opened when the Add Book Button is pressed