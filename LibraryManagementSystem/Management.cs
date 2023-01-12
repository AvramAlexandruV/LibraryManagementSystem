﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LibraryManagementSystem
{
    public partial class Management : Form
    {
        List<Book> books;

        //saving the last id
        public int lastId;

        //path to data
        string path = @"./MOCK_BOOKS.csv";

        public string title;
        public string author;
        public string genre;
        public string publicationDate;
        public string pages;
        public string isbn;
        public string description;


        public Management()
        {
            InitializeComponent();
            showData();
        }

        public void showData()
        {
            createColumns();

            books = File.ReadAllLines("./MOCK_BOOKS.csv")
                        .Skip(1)
                        .Select(b => Book.FromCsv(b))
                        .ToList();

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

                lastId = Convert.ToInt32(books[index].ID);
            }
        }

        public void createColumns()
        {
            dataGridView1.Columns.Add("ID", "ID");
            dataGridView1.Columns.Add("Title", "Title");
            dataGridView1.Columns.Add("Author", "Author");
            dataGridView1.Columns.Add("Genre", "Genre");
            dataGridView1.Columns.Add("Publication Date", "Publication Date");
            dataGridView1.Columns.Add("Pages", "Pages");
            dataGridView1.Columns.Add("ISBN", "ISBN");
            dataGridView1.Columns.Add("Description", "Description");
            dataGridView1.Columns.Add("Current Borrower", "Current Borrower");
        }

        private void clear()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            title = titleTextBox.Text;
            author = authorTextBox.Text;
            genre = genreTextBox.Text;
            publicationDate = publicationDateTextBox.Text;
            pages = pagesTextBox.Text;
            isbn = iSBNTextBox.Text;
            description = descriptionTextBox.Text;

            List<string> lines = new List<string>();

            using (StreamReader reader = new StreamReader(path))
            {
                String line;

                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }

                line = Convert.ToString(++lastId) + ',' + title + ',' + author + ',' + genre + ',' + publicationDate + ',' + pages + ',' + isbn + ',' + description + ',' + "";
                lines.Add(line);
            }

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                foreach (String line in lines)
                    writer.WriteLine(line);
            }

            MessageBox.Show("Book added!");

            clear();
            showData();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            int bookIndex = dataGridView1.CurrentCell.RowIndex;

            List<string> lines = new List<string>();

            using (StreamReader reader = new StreamReader(path))
            {
                String line;

                while ((line = reader.ReadLine()) != null)
                {
                    if (!line.Contains(books[bookIndex].Title))
                    {
                        lines.Add(line);
                    }
                    
                }
            }

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                foreach (String line in lines)
                    writer.WriteLine(line);
            }

            MessageBox.Show("Book deleted!");

            clear();
            showData();
        }
    }
}