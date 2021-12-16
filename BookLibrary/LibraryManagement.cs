using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;


namespace BookLibrary
{
    public class LibraryManagement
    {
        public List<Book> books;
        public const string path = @"C:\Users\ievus\source\repos\BookLibrary\BookLibrary\books.json";
        /// <summary>
        /// Constructor method
        /// </summary>
        public LibraryManagement()
        {
            //set path for json file, read the list last saved there
            string json = File.ReadAllText(path);
            books = JsonSerializer.Deserialize<List<Book>>(json);
        }

        /// <summary>
        /// Method to add book to library
        /// </summary>
        /// <param name="name">name of book</param>
        /// <param name="author">author of book</param>
        /// <param name="category">category of book</param>
        /// <param name="language">language of book</param>
        /// <param name="date">publication date of book</param>
        /// <param name="isbn">isbn of book</param>
        /// <returns></returns>
        public bool AddBook(string name, string author, string category, string language, string date, string isbn)
        {
            DateTime publishDate;
            try
            {
                publishDate = DateTime.Parse(date);
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid date entered. Make sure to use a format with dashes, dots or /.");
                return false;
            }
            Book b = new Book(name, author, category, language, publishDate, isbn);
            if (books.Count > 0)
                b.id = books.Last().id + 1;
            else b.id = 1;
            books.Add(b);            
            //update json file with correct list after changes
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(books, options);
            File.WriteAllText(path, json);
            return true;
        }
        /// <summary>
        /// Method to remove a book from a library
        /// </summary>
        /// <param name="isbn">isbn number of book to remove (if isbns of two books match, in other words, there are two copies of the same book, the one added first will be removed)</param>
        /// <returns>true if removal was successful, false if not</returns>
        public bool DeleteBook(string isbn)
        {
            int index = FindBook(isbn);
            // if book was not found, -1 was returned. check if index is not -1
            if (index >= 0)
            {
                books.RemoveAt(index);
                //update json file with correct list after changes
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(books, options);
                File.WriteAllText(path, json);
                return true;
            }
            else return false;
        }
        /// <summary>
        /// Method to borrow book.
        /// </summary>
        /// <param name="isbn">isbn of the book that's being borrowed</param>
        /// <param name="person"> person borrowing book (name)</param>
        /// <param name="days">amount of time book is borrowed for</param>
        /// <returns>true if book has been borrowed successfully, false if not</returns>
        public bool BorrowBook(string isbn, string person, string days)
        {
            if(HowManyBooksBorrowedByPerson(person) >= 3)
            {
                Console.WriteLine("You cannot borrow more than 3 books. Please return a book first. Sorry!");
                return false;
            }
            int d = -1;
            try 
            { 
                d = Int32.Parse(days);
                if (d > 90)
                {
                    Console.WriteLine("You cannot borrow a book for more than 3 months. Sorry!");
                    return false;
                }
                else if (d < 0)
                {
                    Console.WriteLine("Please enter a positive number.");
                    return false;
                }
            }
            catch (Exception e) 
            { 
                Console.WriteLine("Not a valid number of days entered. Make sure it is a number.");
                return false;
            }
            
            foreach (Book b in books)
            {
                //check if book is not borrowed and isbn matches 
                if (!b.isTaken && b.isbn == isbn)
                {
                    b.isTaken = true;
                    b.whenTaken = DateTime.Now.Date;
                    b.takenBy = person;
                    b.takenFor = d;
                    //update json file with correct list after changes
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string json = JsonSerializer.Serialize(books, options);
                    File.WriteAllText(path, json);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Method to return borrowed book. Will print message if return is late
        /// </summary>
        /// <param name="isbn">isbn of the returned book</param>
        /// <param name="person">name of person returning the book</param>
        /// <returns>true if return has been successful, false if specified book wasn't found</returns>
        public bool ReturnBook(string isbn, string person)
        {
           
            foreach(Book b in books)
            {
                // check if current book is borrowed, if it is borrowed by the person trying to return, and if the isbn matches
                if (b.isTaken && b.takenBy == person && b.isbn == isbn)
                {
                    if (DateTime.Now.Date > b.whenTaken.AddDays(b.takenFor))
                    {
                        Console.WriteLine("Book returned late.");
                    }
                    b.isTaken = false;
                    b.whenTaken = new DateTime();
                    b.takenBy = "";
                    b.takenFor = 0;
                    //update json file with correct list after changes
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string json = JsonSerializer.Serialize(books, options);
                    File.WriteAllText(path, json);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Method to filter book list based on criteria
        /// </summary>
        /// <param name="type">type of filtering that will be given with command - by author, by category, by borrowed status, etc.</param>
        /// <param name="keyword">if the type of filtering is by name, author, etc., we need a keyword to filter by (like "Journey").</param>
        /// <returns>filtered list of books</returns>
        public List<Book> FilterBooks(string type, string keyword)
        {
            List<Book> filtered = new List<Book>();
            switch (type)
            {
                case "all":
                    filtered = books;
                    break;
                case "author":
                    foreach (Book b in books)
                    {
                        if (b.author.ToLower().Contains(keyword.ToLower()))
                            filtered.Add(b);
                    }
                    break;
                case "category":
                    foreach (Book b in books)
                    {
                        if (b.category.ToLower().Contains(keyword.ToLower()))
                            filtered.Add(b);
                    }
                    break;
                case "language":
                    foreach (Book b in books)
                    {
                        if (b.language.ToLower().Contains(keyword.ToLower()))
                            filtered.Add(b);
                    }
                    break;
                case "isbn":
                    foreach (Book b in books)
                    {
                        if (b.isbn.ToLower().Contains(keyword.ToLower()))
                            filtered.Add(b);
                    }
                    break;
                case "name":
                    foreach (Book b in books)
                    {
                        if (b.name.ToLower().Contains(keyword.ToLower()))
                            filtered.Add(b);
                    }
                    break;
                case "taken":
                    foreach (Book b in books)
                    {
                        if (b.isTaken)
                            filtered.Add(b);
                    }
                    break;
                case "available":
                    foreach(Book b in books)
                    {
                        if (!b.isTaken)
                            filtered.Add(b);
                    }
                    break;
                default:
                    Console.WriteLine("Unknown filter type.");
                    break;
            }
            return filtered;
        }
        /// <summary>
        /// Find book via it's isbn
        /// </summary>
        /// <param name="isbn">isbn of the book to look for</param>
        /// <returns>index of book in list, -1 if it can't be found</returns>
        public int FindBook(string isbn)
        {
            for(int i = 0; i < books.Count; i++)
            {
                if (books.ElementAt(i).isbn == isbn)
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// Count how many books a person currently has borrowed
        /// </summary>
        /// <param name="person">person's name</param>
        /// <returns>amount of books the person has borrowed</returns>
        public int HowManyBooksBorrowedByPerson(string person)
        {
            int n = 0;
            foreach (Book b in books)
            {
                if (b.isTaken && b.takenBy == person)
                {
                    n++;
                }
            }
            return n;
        }
        /// <summary>
        /// Print list to console (for filtering)
        /// </summary>
        /// <param name="list">list to print</param>
        public void PrintList(List<Book> list)
        {
            Console.WriteLine("{0,-18} {1,-50} {2, -15} {3, -12} {4, -15} {5, -12}, {6, -7}", "Author", "Name", "Category", "Language", "Publish date", "ISBN", "Borrowed?");
            foreach(Book b in list)
            {
                Console.WriteLine("{0,-18} {1,-50} {2, -15} {3, -12} {4, -15} {5, -12} {6, -7}", b.author, b.name, b.category, b.language, b.publishDate.ToShortDateString(), b.isbn, b.isTaken);
            }
        }
        
    }
}
