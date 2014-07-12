using System;
using Android.Database.Sqlite;
using System.IO;

namespace ListViewSample
{
	public class EmployeeDatabase
	{
		//SQLiteDatabase object for database handling
		private SQLiteDatabase sqldb;
		//String for Query handling
		private string sqldb_query;
		//String for Message handling
		private string sqldb_message;
		//Bool to check for database availability
		private bool sqldb_available;
		//Zero argument constructor, initializes a new instance of Database class
		public EmployeeDatabase()
		{
			sqldb_message = "";
			sqldb_available = false;
		}
		//One argument constructor, initializes a new instance of Database class with database name parameter
		public EmployeeDatabase(string sqldb_name)
		{
			try
			{
				sqldb_message = "";
				sqldb_available = false;
				CreateDatabase(sqldb_name);
			}
			catch (SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
		}
		//Gets or sets value depending on database availability
		public bool DatabaseAvailable
		{
			get{ return sqldb_available; }
			set{ sqldb_available = value; }
		}
		//Gets or sets the value for message handling
		public string Message
		{
			get{ return sqldb_message; }
			set{ sqldb_message = value; }
		}
		//Creates a new database which name is given by the parameter
		public void CreateDatabase(string sqldb_name)
		{
			try
			{
				sqldb_message = "";
				string sqldb_location = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
				string sqldb_path = Path.Combine(sqldb_location, sqldb_name);
				bool sqldb_exists = File.Exists(sqldb_path);
				if(!sqldb_exists)
				{
					sqldb = SQLiteDatabase.OpenOrCreateDatabase(sqldb_path,null);
					sqldb_query = "CREATE TABLE IF NOT EXISTS Employee (_id INTEGER PRIMARY KEY AUTOINCREMENT, Id VARCHAR, Title VARCHAR, Email VARCHAR, City VARCHAR, Picture VARCHAR, ImageUrl VARCHAR, ReportCount VARCHAR, FName VARCHAR, LName VARCHAR, ManagerId VARCHAR, Department VARCHAR, OfficePhone VARCHAR, CellPhone VARCHAR);";
					sqldb.ExecSQL(sqldb_query);
					sqldb_message = "Database: " + sqldb_name + " created";
				}
				else
				{
					sqldb = SQLiteDatabase.OpenDatabase(sqldb_path, null, DatabaseOpenFlags.OpenReadwrite);
					sqldb_message = "Database: " + sqldb_name + " opened";
				}
				sqldb_available=true;
			}
			catch(SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
		}
		//Adds a new record with the given parameters
		public void AddRecord(Employee emp)
		{
			try
			{
				//sqldb_query = "INSERT INTO Employee (Name, LastName, Age) VALUES ('" + sName + "','" + sLastName + "'," + iAge + ");";
				sqldb_query = "INSERT INTO Employee (Id, Title, Email, City, Picture, ImageUrl, ReportCount, FName, LName, ManagerId, Department, OfficePhone, CellPhone) VALUES ('" + emp.Id + "','" + emp.Title + "','" + emp.Email + "','" + emp.City + "','" + emp.Picture + "','" + emp.ImageDownloadPath + "','" + emp.ReportCount + "','" + emp.FName + "','" + emp.LName + "','" + emp.ManagerId + "','" + emp.Department + "','" +  emp.OfficePhone + "','" + emp.CellPhone + "');";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "Record saved";
			}
			catch(SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
		}
		//Updates an existing record with the given parameters depending on id parameter
		public void UpdateRecord(int iId, string sName, string sLastName, int iAge)
		{
			try
			{
				sqldb_query="UPDATE MyTable SET Name ='" + sName + "', LastName ='" + sLastName + "', Age ='" + iAge + "' WHERE _id ='" + iId + "';";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "Record " + iId + " updated";
			}
			catch(SQLiteException ex)
			{
				sqldb_message = ex.Message;
			}
		}
		//Deletes the record associated to id parameter
		public void DeleteRecord(int iId)
		{
			try
			{
				sqldb_query = "DELETE FROM MyTable WHERE _id ='" + iId + "';";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "Record " + iId + " deleted";
			}
			catch(SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
		}
		//Searches a record and returns an Android.Database.ICursor cursor
		//Shows all the records from the table
		public Android.Database.ICursor GetRecordCursor()
		{
			Android.Database.ICursor sqldb_cursor = null;
			try
			{
				sqldb_query = "SELECT FName,LName,ImageUrl  FROM Employee;";
				sqldb_cursor = sqldb.RawQuery(sqldb_query, null);
				if(!(sqldb_cursor != null))
				{
					sqldb_message = "Record not found";
				}
			}
			catch(SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
			return sqldb_cursor;
		}



		//Searches a record and returns an Android.Database.ICursor cursor
		//Shows records according to search criteria
		public Android.Database.ICursor GetRecordCursor(string sColumn, string sValue)
		{
			Android.Database.ICursor sqldb_cursor = null;
			try
			{
				sqldb_query = "SELECT*FROM MyTable WHERE " + sColumn + " LIKE '" + sValue + "%';";
				sqldb_cursor = sqldb.RawQuery(sqldb_query, null);
				if(!(sqldb_cursor != null))
				{
					sqldb_message = "Record not found";
				}
			}
			catch(SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
			return sqldb_cursor;
		}}
}

