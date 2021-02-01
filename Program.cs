/****************************
* Author: Alexander Overley *
* 1/15/2021                 *
* This Program Demonstrates *
* My Ability to Take User   *
* Input to Send and Get that*
* Info From Text, Excel, and* 
* SQL Database              *
* ***************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
using System.Configuration;
using System.Data.SqlClient;

namespace Naba_Read_Write_Prj
{
    class Program
    {

        static void Main(string[] args)
        {
            PersonInfo person = new PersonInfo("John", "Doe"); //creation of PersonInfo and SendIt objects
            SendIt send = new SendIt();

            while (true) //creates menu and calls corresponding functions in SnedIt and PersonInfo
            {
                WriteLine("Please Select an Action" + "\n 1: Add Information" + "\n 2: Send Information to Text File"
                        + "\n 3: Retreive Information from Text File" + "\n 4: Send Information to Excel File"
                        + "\n 5: Retreive Information from Excel File" + "\n 6: Send Information to SQL DataBase"
                        + "\n 7: Retreive Information from SQL DataBase" + "\n 8: Exit");
                int action = Convert.ToInt32(ReadLine());
                if (action == 1)
                    person.GetInfo();
                else if (action == 2)
                    send.ToTextFile(person.ToString());
                else if (action == 3)
                    send.ReadTextFile();
                else if (action == 4)
                    send.SetUpExcelFile(person.First, person.Last);
                else if (action == 5)
                    send.ReadExcel();
                else if (action == 6)
                    send.InsertSQL(person.First, person.Last);
                else if (action == 7)
                    send.DisplaySQL();
                else if (action == 8)
                    break;
                else
                    WriteLine("Invalid Input, please enter a number on menu.");

            }
        }
            
    }
}
