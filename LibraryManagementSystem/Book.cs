using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    public class Book
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string publicationDate { get; set; }
        public string Pages { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public string CurrentBorrower { get; set; }
        public string historyOfBorrowers { get; set; }

        public static Book FromCsv(string csvLine) {
            string[] values = csvLine.Split(',');
            Book book = new Book();

            book.ID = values[0];
            book.Title = values[1];
            book.Author = values[2];
            book.Genre = values[3];
            book.publicationDate = values[4];
            book.Pages = values[5];
            book.ISBN = values[6];
            book.Description = values[7];
            book.CurrentBorrower = values[8];
            book.historyOfBorrowers = values[9];

            return book;
        }

        public static Book FromCsvFilter(string csvLine, string Genre, string Author, string Title)
        {
            string[] values = csvLine.Split(',');
            Book book = new Book();

            if ((values[3].Contains(Genre) || values[2].Contains(Author) || values[1].Contains(Title))
                 || (Genre.Contains("-1_NOT_FILTERING") && Author.Contains("-1_NOT_FILTERING") && Title.Contains("-1_NOT_FILTERING")))
            {
                book.ID = values[0];
                book.Title = values[1];
                book.Author = values[2];
                book.Genre = values[3];
                book.publicationDate = values[4];
                book.Pages = values[5];
                book.ISBN = values[6];
                book.Description = values[7];
                book.CurrentBorrower = values[8];
                book.historyOfBorrowers = values[9];
            }
            else { book.ID = "INVALID"; }

            return book;
        }
    }
}
