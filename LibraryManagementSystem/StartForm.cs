using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
    >>> Library Management System <<<
    
    >> the list of books is retrieved from a sql database
    >> we can / edit / remove the books present in our database
    >> we can search if a book exists in the library
    >> we can check if we can borrow the book
    >> we can borrow / return the book
    >> the changes done in the form are saved in the database

 */

namespace LibraryManagementSystem
{
    public partial class StartForm : Form
    {
        //  a book variable using the Book.cs interface we created
        public Book searchedBook { get; set; }

        //  initial state of our variables
        public static string Author = string.Empty;
        public static string Genre = string.Empty;
        public static string Title = string.Empty;
        public static string Borrower = string.Empty;
        public static DateTime date = DateTime.Now;

       

        public StartForm()
        {
            // when we initialize the form we want our option buttons to have the Enable property set to false
            InitializeComponent();
            borrowBook.Enabled = false;
            textBox4.Enabled = false;
            returnBook.Enabled = false;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        // by clicking the "Management" button we open a new form
        private void button2_Click(object sender, EventArgs e)
        {
            var myForm = new Management();
            myForm.Show();
        }

        // checking if the book exists and if it can be borrowed
        private void button1_Click(object sender, EventArgs e)
        {
            // we retrieve the info from the user input
            Genre = textBox1.Text;
            Author = textBox2.Text;
            Title = textBox3.Text;

            // creating our connection to the local mssql database
            string connect = @"Data Source=(localDb)\MSSQLLOCALDB;Initial Catalog=Library;Integrated Security=True;Pooling=False";
            SqlConnection con = new SqlConnection(connect);

            try
            {
                con.Open();

                if (Title != "" && Genre != "" && Author != "")
                {
                    string query = "SELECT * FROM Books WHERE TITLE = '" + Title + "' AND GENRE = '" + Genre + "' AND AUTHOR = '" + Author + "'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adpt.Fill(dt);

                    if (dt.Rows.Count != 0)
                    {
                        MessageBox.Show("Found!");
                        if (dt.Rows[0][8].ToString() == "")
                        {
                            MessageBox.Show("You can borrow this book!");
                            borrowBook.Enabled = true;
                            textBox4.Enabled = true;
                        }
                        else {
                            MessageBox.Show("We're sorry, this book was borrowed by " + dt.Rows[0][8] );
                            returnBook.Enabled = true;
                        }
                        
                        dataGridView1.DataSource = dt;
                    }
                    else
                    {
                        MessageBox.Show("Not Found. Make sure the Title, Genre and Author are written correctly!");
                    }
                }
            }   
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // adding the new borrower to the list
        private void button1_Click_1(object sender, EventArgs e)
        {
            string connect = @"Data Source=(localDb)\MSSQLLOCALDB;Initial Catalog=Library;Integrated Security=True;Pooling=False";
            SqlConnection con = new SqlConnection(connect);

            Borrower = textBox4.Text;

            // we update the Borrower field of our book

            try {
                con.Open();

                string query1 = "SELECT * FROM Books WHERE TITLE = '" + Title + "' AND GENRE = '" + Genre + "' AND AUTHOR = '" + Author + "'";
                SqlCommand cmd = new SqlCommand(query1, con);
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adpt.Fill(dt);

                string query2 = "UPDATE BOOKS SET CurrentBorrower = '" + Borrower + " " + date + "' WHERE Title='" + Title + "'";
                SqlCommand cmd2 = new SqlCommand(query2, con);
                adpt = new SqlDataAdapter(cmd2);
                dt = new DataTable();
                adpt.Fill(dt);


                borrowBook.Enabled = false;
                textBox4.Enabled = false;
                MessageBox.Show("Congrats!! You borrowed this book.");

            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // returning the book
        private void button2_Click_1(object sender, EventArgs e)
        {
            string connect = @"Data Source=(localDb)\MSSQLLOCALDB;Initial Catalog=Library;Integrated Security=True;Pooling=False";
            SqlConnection con = new SqlConnection(connect);

            // we update the HistoryOfBorrowers field by adding the last borrower and then we clear the borrower field by assigning
            // it an empty string

            try {
                con.Open();

                string query1 = "SELECT * FROM Books WHERE TITLE = '" + Title + "' AND GENRE = '" + Genre + "' AND AUTHOR = '" + Author + "'";
                SqlCommand cmd = new SqlCommand(query1, con);
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adpt.Fill(dt);

                string query2 = "UPDATE BOOKS SET HistoryOfBorrowers = '" + dt.Rows[0][9] + " " + dt.Rows[0][8] + " " + date + "' WHERE Title='" + Title + "'";
                SqlCommand cmd2 = new SqlCommand(query2, con);
                adpt = new SqlDataAdapter(cmd2);
                dt = new DataTable();
                adpt.Fill(dt);

                string query3 = "UPDATE BOOKS SET CurrentBorrower = '' WHERE Title='" + Title + "'";
                SqlCommand cmd3 = new SqlCommand(query3, con);
                adpt = new SqlDataAdapter(cmd3);
                dt = new DataTable();
                adpt.Fill(dt);

                borrowBook.Enabled = true;
                textBox4.Enabled = true;
                returnBook.Enabled = false;


                MessageBox.Show("Book returned!");
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
