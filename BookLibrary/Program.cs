using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace BookLibrary
{
    class Program
    {
        public static List<Book> books;
        public static LibraryManagement manager;
        static void Main(string[] args)
        {
            manager = new LibraryManagement();
            while (true)
            {
                Console.WriteLine("Hello! Please enter what you'd like to do in our library. Options: add, delete, borrow, return, filter, reset");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "add":
                        //if person wishes to add a book to the library, ask them to enter all the required information
                        Console.WriteLine("Please enter the book's title:");
                        string title = Console.ReadLine();
                        Console.WriteLine("Please enter the book's author:");
                        string author = Console.ReadLine();
                        Console.WriteLine("Please enter the book's category:");
                        string category = Console.ReadLine();
                        Console.WriteLine("Please enter the book's language:");
                        string language = Console.ReadLine();
                        Console.WriteLine("Please enter the book's publish date:");
                        string date = Console.ReadLine();
                        Console.WriteLine("Please enter the book's ISBN:");
                        string isbn = Console.ReadLine();
                        // once the information has been entered, pass it to the method to create the book and add it to our library
                        manager.AddBook(title, author, category, language, date, isbn);
                        break;
                    case "delete":
                        // if person wishes to delete a book, ask them for the ISBN to identify it. if there is more than one copy, the one encountered first (the lower id/older entry) will be removed.
                        Console.WriteLine("Please enter the book's ISBN:");
                        isbn = Console.ReadLine();
                        // check returned result. true - book deleted. false - book not found
                        bool res = manager.DeleteBook(isbn);
                        if (res)
                            Console.WriteLine("Book has been deleted successfully.");
                        else Console.WriteLine("Deletion unsuccessful. Could not find specified book in library.");
                        break;
                    case "borrow":
                        // if person wishes to borrow a book, ask for their name, how long they wish to have it and the isbn to identify the book
                        Console.WriteLine("Please enter your name:");
                        string person = Console.ReadLine();
                        Console.WriteLine("Please enter how many days you'd like to borrow the book for:");
                        string days = Console.ReadLine();
                        Console.WriteLine("Please enter the book's ISBN:");
                        isbn = Console.ReadLine();
                        // check returned result. true - book borrowed successfully. false - one of the failure reasons: book not found (or currently borrowed), borrow time was over 90 days, or person has already borrowed 3 books
                        res = manager.BorrowBook(isbn,person, days);
                        if (res)
                            Console.WriteLine("Book has been borrowed successfully.");
                        else Console.WriteLine("Borrowing unsuccessful. Could not find specified book in library or it is currently borrowed.");
                        break;
                    case "return":
                        // if person wishes to return a book, ask for their name and the isbn to identify the book
                        Console.WriteLine("Please enter the book's ISBN:");
                        isbn = Console.ReadLine();
                        Console.WriteLine("Please enter your name:");
                        person = Console.ReadLine();
                        // check returned result. true - book returned successfully. false - there is no record of a book with this isbn borrowed by this person
                        res = manager.ReturnBook(isbn, person);
                        if (res)
                            Console.WriteLine("Book has been returned successfully.");
                        else Console.WriteLine("Return unsuccessful. Could not find specified borrowed book in records.");
                        break;
                    case "filter":
                        // if person wishes to see a filtered list of books, ask them for the type of filter, and a keyword (that will be used unless the filter type is taken or available)
                        Console.WriteLine("Please enter how you'd like to filter the books. Options: all, author, category, language, isbn, name, taken, available");
                        string type = Console.ReadLine();
                        Console.WriteLine("Please enter a keyword to search by (required for all filters except taken and available): ");
                        string keyword = Console.ReadLine();
                        // a list with only the corresponding filtered items is returned
                        List<Book> list = manager.FilterBooks(type, keyword);
                        // print the returned list
                        manager.PrintList(list);
                        break;
                    case "reset":
                        // allow person to clear the text in the console once it's full
                        Console.Clear();
                        break;
                    default:
                        break;
                }
            }
            
        }
        
    }
}
