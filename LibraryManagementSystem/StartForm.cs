using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

/*
    >>> Library Management System <<<
    
    >> the list of books is retrieved from a sql database
    >> we can / edit / remove the books present in our database
    >> we can search if a book exists in the library
    >> we can check if we can borrow the book
    >> we can borrow / return the book
    >> the changes done in the form are saved in the database

 */

/*
    >>> Sql implementation

    >> In order to implement an sql connection we have to create a new data connection to our database

    >> We connect to the database by using the following lines
            string connect = @"Data Source=(localDb)\MSSQLLOCALDB;Initial Catalog=Library;Integrated Security=True;Pooling=False";
            SqlConnection con = new SqlConnection(connect);
    
    >> We open the connection
             con.Open();

    >> An we retrieve the data
             string query1 = "SELECT * FROM Books WHERE TITLE = '" + Title + "' AND GENRE = '" + Genre + "' AND AUTHOR = '" + Author + "'";
             SqlCommand cmd = new SqlCommand(query1, con);
             SqlDataAdapter adpt = new SqlDataAdapter(cmd);
             DataTable dt = new DataTable();
             adpt.Fill(dt);

    >> Then, in designer mode, we bind our data to the form

    >> The sql implementation works, but we preffered to let it commented out in order to not interfere with out csv reading from the files
    >> It can easily be put back by adding the connection to our classes and re-mapping the values along side with some basic sql queries

 */

namespace LibraryManagementSystem
{
    public partial class StartForm : Form
    {
        List<Book> books;

        public static string Author = string.Empty;
        public static string Genre = string.Empty;
        public static string Title = string.Empty;
        public static string Borrower = string.Empty;
        public static DateTime date = DateTime.Now;
        
        //path to our csv file, it is bin tho
        string path = @"./MOCK_BOOKS.csv";

        //to check if we have filtering on
        public static int Filtered = 0; 


        public StartForm()
        {
            InitializeComponent();
            showData();
        }

        public void showData() {
            createColumns();

            if (Filtered == 0)
                noFiltering();
            else
                withFiltering();

            foreach (var x in books.Select((value, i) => new { i, value }))
            {
                var index = x.i;
                if (books[index].ID != "INVALID" && books[index].ID != "")
                {
                    dataGridView1.Rows.Add(
                        books[index].ID,
                        books[index].Title,
                        books[index].Author,
                        books[index].Genre,
                        books[index].publicationDate,
                        books[index].Pages,
                        books[index].ISBN,
                        books[index].Description,
                        books[index].CurrentBorrower
                        );
                }
            }
        }

        public void createColumns() {
            dataGridView1.Columns.Add("ID","ID");
            dataGridView1.Columns.Add("Title", "Title");
            dataGridView1.Columns.Add("Author","Author");
            dataGridView1.Columns.Add("Genre","Genre");
            dataGridView1.Columns.Add("Publication Date","Publication Date");
            dataGridView1.Columns.Add("Pages","Pages");
            dataGridView1.Columns.Add("ISBN","ISBN");
            dataGridView1.Columns.Add("Description","Description");
            dataGridView1.Columns.Add("Current Borrower", "Current Borrower");
        }

        private void clear()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
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
            Genre = textBox1.Text;
            Author = textBox2.Text;
            Title = textBox3.Text;

            int tmp = 0;

            if (Genre == "") { Genre = "-1_NOT_FILTERING"; ++tmp; }
            if (Author == "") { Author = "-1_NOT_FILTERING"; ++tmp; }
            if (Title == "") { Title = "-1_NOT_FILTERING"; ++tmp; } 

            MessageBox.Show("Search completed!");

            if (tmp == 3)
                Filtered = 0;
            else
                Filtered = 1;

            clear();
            showData();
        }

        public void noFiltering() {
            books = File.ReadAllLines("./MOCK_BOOKS.csv")
                        .Skip(1)
                        .Select(b => Book.FromCsv(b))
                        .ToList();
        }

        public void withFiltering() {
            books = File.ReadAllLines("./MOCK_BOOKS.csv")
                        .Skip(1)
                        .Select(b => Book.FromCsvFilter(b, Genre, Author, Title))
                        .ToList();
        }

        // adding the new borrower to the list
        private void button1_Click_1(object sender, EventArgs e)
        {
            int bookIndex = dataGridView1.CurrentCell.RowIndex;
            Borrower = textBox4.Text;

            if (books[bookIndex].CurrentBorrower != "")
            {
                MessageBox.Show("We're sorry, this book was borrowed by " + books[bookIndex].CurrentBorrower);
            }
            else {
                List<string> lines = new List<string>();

                using (StreamReader reader = new StreamReader(path))
                {
                    String line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains(books[bookIndex].Title))
                        {
                            line = books[bookIndex].ID + ',' + books[bookIndex].Title + ',' + books[bookIndex].Author + ',' + books[bookIndex].Genre + ',' + books[bookIndex].publicationDate + ',' + books[bookIndex].Pages + ',' + books[bookIndex].ISBN + ',' + books[bookIndex].Description + ',' + Borrower;
                        }
                        lines.Add(line);
                    }

                }

                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    foreach (String line in lines)
                        writer.WriteLine(line);
                }

                MessageBox.Show("Congrats!! You borrowed this book.");

                clear();
                showData();
            }
        }

        // returning the book
        private void button2_Click_1(object sender, EventArgs e)
        {
            int bookIndex = dataGridView1.CurrentCell.RowIndex;

            if (books[bookIndex].CurrentBorrower == "")
            {
                MessageBox.Show("We're sorry, this book was not borrowed by anyone!");
            }
            else
            {
                List<string> lines = new List<string>();

                using (StreamReader reader = new StreamReader(path))
                {
                    String line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains(books[bookIndex].Title))
                        {
                            line = books[bookIndex].ID + ',' + books[bookIndex].Title + ',' + books[bookIndex].Author + ',' + books[bookIndex].Genre + ',' + books[bookIndex].publicationDate + ',' + books[bookIndex].Pages + ',' + books[bookIndex].ISBN + ',' + books[bookIndex].Description + ',' + "";
                        }
                        lines.Add(line);
                    }

                }

                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    foreach (String line in lines)
                        writer.WriteLine(line);
                }

                MessageBox.Show("Book returned!");

                clear();
                showData();
            }

        }
    }
}