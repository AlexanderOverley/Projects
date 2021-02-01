/**********************************************************
* Author: Alexander Overley                               *
* 1/3/2021                                                *
* This Program Creates a Class That Has a Constructor to  *  
* Assign Default Values to 2 Properties, First and Last   *
* And a GetInfo Function to Collect User Input into       *
* Mentioned Properties. It Also Has a ToString to Return  *
* First and Last as a single String                       *
***********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Naba_Read_Write_Prj
{
    public class PersonInfo
    {
        public PersonInfo(string first, string last) //constructor to assign default values 
        {
            First = first;
            Last = last;
        }

        public string First { get; set; } //Auto Implemented Properties that sets and gets First and Last name
        public string Last { get; set; }

        public void GetInfo() //asks user for input and stores in respective property
        {
            WriteLine("What is the First Name: ");
            First = ReadLine();
            WriteLine("What is the Last Name: ");
            Last = ReadLine();
        }
        override public string ToString() //a custom ToString for this class that returns First and Last names as a single combined string
        {
            return First + " " + Last;
        }

    }
}
