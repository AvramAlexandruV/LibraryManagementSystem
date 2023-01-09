using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    public class Book
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string publicationDate { get; set; }
        public int Pages { get; set; }
        public int ISBN { get; set; }
        public string Description { get; set; }
        public string CurrentBorrower { get; set; }
    }
}
