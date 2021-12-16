using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary
{
    public class Book
    {
        public int id { get; set; }
        public string name { get; set; }
        public string author { get; set; }
        public string category { get; set; }
        public string language { get; set; }
        public DateTime publishDate { get; set; }
        public string isbn { get; set; }
        //attributes for taken books
        // is book currently taken
        public bool isTaken { get; set; }
        // when book was taken
        public DateTime whenTaken { get; set; }
        // who book was taken by (person's name)
        public string takenBy { get; set; }
        // how long book was taken for (amount of days)
        public int takenFor { get; set; }

        public Book(string name, string author, string category, string language, DateTime publishDate, string isbn)
        {
            //this.id = id;
            this.name = name;
            this.author = author;
            this.category = category;
            this.language = language;
            this.publishDate = publishDate;
            this.isbn = isbn;
            isTaken = false;
            whenTaken = new DateTime();
            takenBy = "";
            takenFor = 0;
        }
        public Book()
        {
            name = "";
            author = "";
            category = "";
            language = "";
            publishDate = new DateTime();
            isbn = "";
            isTaken = false;
            whenTaken = new DateTime();
            takenBy = "";
            takenFor = 0;
        }
    }
}
