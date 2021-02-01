/************************************
* Author: Alexander Overley         *
* 1/15/2021                         *
* This Class Has Funstions That     *
* Send and Get Info from a Text File*
* an Excel File, and SQL Database   *         
* ***********************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Configuration;
using System.Data.SqlClient;

namespace Naba_Read_Write_Prj
{

    class SendIt
    {
        Excel.Application App; //creates object that represent parts of the Excel File, application being the application itself, the sheet being the active sheet worked on, and the workbook being the active workbook being used
        Excel.Worksheet Sheet;
        Excel.Workbook Book;

        SqlConnection connection; //establish connection object for SQL database

        string connectionString = @"Data Source=LAPTOP-VEP3VEGL;Initial Catalog=NABA;Integrated Security=True"; //the string that represents the connection to the SQL database

        public void ToTextFile(string info) //Sends Info to Text File, info is comprised of a first and last name information obtained in PersonInfo class
        {
            System.IO.File.WriteAllText(@"C:\Users\overx\OneDrive - University of Louisville\Excess Computer Programs\NABA\Info.txt", info);
        }

        public void ReadTextFile() //reads contents of text file
        {
            Console.WriteLine(System.IO.File.ReadAllText(@"C:\Users\overx\OneDrive - University of Louisville\Excess Computer Programs\NABA\Info.txt"));
        }

        public void SetUpExcelFile(string first, string last) //adds first and last name to excel file
        {
            
            App = new Excel.Application(); //creates a new application, workbook, and sheet
            Book = App.Workbooks.Add();
            Sheet = Book.Worksheets[1];

            Sheet.Cells[1,1] = first; //stores data in 1st row 1st and 2nd columns 
            Sheet.Cells[1,2] = last;

            Book.SaveAs(@"C:\Users\overx\OneDrive - University of Louisville\Excess Computer Programs\NABA\ExhibitFile.xlsx"); //saves file to this location
            Book.Close(); //closes active workbook
            App.Quit();   //shuts down active application(Excel)
        }

        public void ReadExcel() //retreives information from excel file
        {
            App = new Excel.Application(); 

            string file = @"C:\Users\overx\OneDrive - University of Louisville\Excess Computer Programs\NABA\ExhibitFile.xlsx";
            
            Book = App.Workbooks.Open(file); //opens the file in the freshly launched application and activated workbook, then activates the worksheet
            Sheet = Book.Worksheets[1];

            //Retreives the Data as Stored in SetupExcel by using the Range Object and .Value returns them as a Recordset and specified as string
            var range = (string)(Sheet.Cells[1, 1] as Excel.Range).Value; 
            var range2 = (string)(Sheet.Cells[1, 2] as Excel.Range).Value;

            System.Console.WriteLine(range + " " + range2); //outputs values retreived from Excel File

            Book.Close();
            App.Quit();
        }

        public void CloseExcel() //emergency use function to close the excel file if it was not closed 
        {
            Book.Close();
            App.Quit();
        }

        public void InsertSQL(string First, string Last) //inserts first and last name into SQL database
        {

            string query = "INSERT INTO CharInfo Values(@CharInfoFirst, @CharInfoLast)"; //insert query , @CharInfoFirst and Last are Parameters

            using(connection = new SqlConnection(connectionString)) //establishes new SQL connection : as these are IDisposable Objects, using command auto closes them
            using(SqlCommand command = new SqlCommand(query, connection)) //creates SQL command to run query and connection
            {
                connection.Open(); //opens connection

                System.Console.WriteLine("INSERTING INTO DATABASE...");

                command.Parameters.AddWithValue("@CharInfoFirst", First); //adds parameter with given First and Last information
                command.Parameters.AddWithValue("@CharInfoLast", Last);

                command.ExecuteNonQuery(); //executes query
            }
        }

        public void DisplaySQL() // displays what is stored in SQL database
        {
            string query2 = "SELECT * FROM CharInfo";

            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query2, connection))
                using (SqlDataReader reader = command.ExecuteReader()) //used to read database
                {
                    while (reader.Read()) //goes through all records/rows 
                    {
                        for (int i = 0; i < reader.FieldCount; i++) //inside a record, goes through number of columns and gets and prints value
                            System.Console.Write(" " + reader.GetValue(i));
                        System.Console.Write("\n");
                    }
                    reader.Close(); //closes reader
                }
            }


        }
    }
}
