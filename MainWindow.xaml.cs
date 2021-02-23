/************************************
* Author: Alexander Overley         *
* 2/15/2021                         *
* This Class Has Funstions That     *
* Send and Get Info to/from the     *
* OrganizeInfo class by reading the *
* input Text Boxes on the WPF form. *
* It uses events that are detected  *
* Click to send and retrieve data   *         
* ***********************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_NABA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OrganizeInfo info = new OrganizeInfo(); //creates info object for the OrganizeInfo class

        public MainWindow() //initializes window
        {
            InitializeComponent();
        }

        private void submitButtonTxt_Click(object sender, RoutedEventArgs e) //tied to the submit button to send/retrieve from text file
        {

            info.ToTextFile(firstNameText.Text, lasttNameText.Text); //sends input from the first and last name text boxes to the Organize Info class to be placed into a text file
            
            outputText.Text = info.ReadTextFile(); //calls ReadTextFile function from OrganizeInfo to get contents and Text File
        }

        private void submitButtonSQL_Click(object sender, RoutedEventArgs e) //gets and retrieves data from SQL database
        {
            info.InsertSQL(firstNameText.Text, lasttNameText.Text); //sends input to database

            List<string> data = new List<string>();

            data = info.DisplaySQL(); //gets all data in the database and places into list data

            foreach (string dat in data) //loops through and displays data in the database list in a more organized way
            {
                outputSQL.Text = outputSQL.Text + " " + dat;
            }

            //info.ClearSQL(); --clears the database--
        }

        private void submitButtonXl_Click(object sender, RoutedEventArgs e) //gets and retrieves data from Excel file
        {
            info.SetUpExcelFile(firstNameText.Text, lasttNameText.Text); //sends input to Excel File


            outputXl.Text = info.ReadExcel(); //gets info from Excel File
        }
    }
}
