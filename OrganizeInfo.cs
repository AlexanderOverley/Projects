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
using System.Configuration;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;

namespace WPF_NABA
{
    public class OrganizeInfo
    {
        Excel.Application App; //Establishes objects for Excel
        Excel.Worksheet Sheet;
        Excel.Workbook Book;

        SqlConnection connection; //Establishes SQL connection object

        string connectionString = @"Data Source=LAPTOP-VEP3VEGL;Initial Catalog=NABA;Integrated Security=True"; //connection string for SQL

        public void ToTextFile(string first, string last) //Writes parameters first and last to Text File 
        {
            System.IO.File.WriteAllText(@"C:\Users\overx\OneDrive - University of Louisville\Excess Computer Programs\WPF_NABA\InfoWPF.txt", first + " " + last);
        }

        public string ReadTextFile()//Read What is on Text File
        {
            return System.IO.File.ReadAllText(@"C:\Users\overx\OneDrive - University of Louisville\Excess Computer Programs\WPF_NABA\InfoWPF.txt");
        }

        public void SetUpExcelFile(string first, string last) //Writes parameters first and last to Excel File 
        {

            App = new Excel.Application();
            //App.Visible = true;
            Book = App.Workbooks.Add();
            Sheet = (Excel.Worksheet)Book.Worksheets.get_Item(1);

            //for(int count = 0; count < ArrayLength; count++) //if multiple name entries are used/allowed, can be stored in an array or list and added via loop
            Sheet.Cells[1, 1] = first;
            Sheet.Cells[1, 2] = last;

            Book.SaveAs(@"C:\Users\overx\OneDrive - University of Louisville\Excess Computer Programs\WPF_NABA\ExhibitFileWPF.xlsx");
            Book.Close();
            App.Quit();
        }

        public string ReadExcel() //calls the Exhibit excel file and displays information in cells 1,1 and 1,2
        {
            App = new Excel.Application(); //establishes new Excel application

            string file = @"C:\Users\overx\OneDrive - University of Louisville\Excess Computer Programs\WPF_NABA\ExhibitFileWPF.xlsx"; //gets Excel file

            Book = App.Workbooks.Open(file); //opens workbook using above file
            Sheet = Book.Worksheets[1]; //gets first sheet of book in file

            string range = (Sheet.Cells[1, 1] as Excel.Range).Value + " " + (Sheet.Cells[1, 2] as Excel.Range).Value; //Retreives the Data as Stored in SetupExcel by using the Range Object and .Value returns them as a Recordset and specified as string

            Book.Close(); //closes book and terminates application
            App.Quit();

            return range; //returns string value Range of cells was stored in
        }

        public void InsertSQL(string First, string Last) //inserts First and Last names into CharInfo table
        {

            string query = "INSERT INTO CharInfo Values(@CharInfoFirst, @CharInfoLast)"; //query to insert input into database: //insert query , @CharInfoFirst and Last are Parameters

            using (connection = new SqlConnection(connectionString)) //establishes database connection using connectionString : as these are IDisposable Objects, using command auto closes them
            using (SqlCommand command = new SqlCommand(query, connection)) //creates SQL command to use query
            {
                connection.Open();//opens query

                command.Parameters.AddWithValue("@CharInfoFirst", First); //inserts input parameters to database using variables
                command.Parameters.AddWithValue("@CharInfoLast", Last);

                command.ExecuteNonQuery(); //executes query
            }
        }

        public List<string> DisplaySQL() //Displays Data from Char Info Databse, 
        {
            string query2 = "SELECT * FROM CharInfo";

            List<string> data = new List<string>();

            using (connection = new SqlConnection(connectionString)) //connects to database using connection and opens connection
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query2, connection)) //creates command to execute query
                using (SqlDataReader reader = command.ExecuteReader()) //uses reader function to read database
                {
                    while (reader.Read()) //goes through all records/rows 
                    {
                        for (int i = 0; i < reader.FieldCount; i++) //inside a record, goes through number of columns(using FieldCount) and gets and prints value
                            data.Add(reader.GetValue(i).ToString());
                    }

                    reader.Close(); //closes reader (it does not auto close)

                    return data; //returns list data which contains what is in the database
                }
            }


        }

        public void ClearSQL() //clears all data stored in table CharInfo
        {
            string query = "DELETE FROM CharInfo";
            //string receiveQ;

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.ExecuteNonQuery();
            }
        }
    }
}
