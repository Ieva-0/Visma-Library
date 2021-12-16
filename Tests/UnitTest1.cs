using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookLibrary;
using System;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        LibraryManagement man = new LibraryManagement();
        [TestMethod]
        public void TestAddSuccess()
        {
            string[,] attr = { { "TestName", "author", "drama", "polish", "2020-11-12", "12345" },
            { "TestName", "author", "drama", "polish", "2000/11/11", "12398" },
            { "TestName", "author", "drama", "polish", "1998.05.5", "12378" },
            { "TestName", "author", "drama", "polish", "2013-1-2", "12356" }};

            for (int i = 0; i < 4; i++)
            {
                bool result = man.AddBook(attr[i, 0], attr[i, 1], attr[i, 2], attr[i, 3], attr[i, 4], attr[i, 5]);
                Assert.IsTrue(result,
                       String.Format("Expected for combination '{0}': true; Actual: {1}",
                                     i, result));
            }

        }
        [TestMethod]
        public void TestAddFailure()
        {
            string[,] attr = { { "TestName1", "author", "drama", "polish", "2020-15-11", "12345"},
            { "TestName2", "author", "drama", "polish", "haha", "12398"},
            { "TestName3", "author", "drama", "polish", "ęėįšęėš", "12378"},
            { "TestName4", "author", "drama", "polish", "222222", "12356"}};
            for (int i = 0; i < 4; i++)
            {
                bool result = man.AddBook(attr[i, 0], attr[i, 1], attr[i, 2], attr[i, 3], attr[i, 4], attr[i, 5]);
                Assert.IsFalse(result,
                       String.Format("Expected for combination '{0}': false; Actual: {1}",
                                     i, result));
            }
        }

        [TestMethod]
        public void TestDeleteSuccess()
        {
            string[] isbn = { "12345", "12398", "12378", "12356" };
            foreach (string i in isbn)
            {
                bool result = man.DeleteBook(i);
                Assert.IsTrue(result,
                       String.Format("Expected for '{0}': true; Actual: {1}",
                                     i, result));
            }

        }
        [TestMethod]
        public void TestDeleteFailure()
        {
            string[] isbn = { "12121212-0", "word", "Москва", "ęėįšęėš" };
            foreach (string i in isbn)
            {
                bool result = man.DeleteBook(i);
                Assert.IsFalse(result,
                       String.Format("Expected for '{0}': false; Actual: {1}",
                                     i, result));
            }
        }
        [TestMethod]
        public void TestBorrowSuccess()
        {
            string[,] args = { { "633775170-1", "Tom", "15" }, { "804783608-0", "Fabian", "20" }, { "433183196-5", "Other person", "89" }, { "229333912-2", "Anna", "22" } };
            for (int i = 0; i < 4; i++)
            {
                bool result = man.BorrowBook(args[i, 0], args[i, 1], args[i, 2]);
                Assert.IsTrue(result,
                       String.Format("Expected for combination '{0}': true; Actual: {1}",
                                     i, result));
            }
        }
        [TestMethod]
        public void TestBorrowFailure()
        {
            string[,] args = { { "080408190-5", "Tom", "10" }, { "804783608-0", "Fabian", "95" }, { "11111", "Other person", "text" }, { "047865969-5", "Ieva", "22" } };
            //                  book already taken              not allowed to take for over 90 days      not a vlid number            this person has already borrowed 3 books
            for (int i = 0; i < 4; i++)
            {
                bool result = man.BorrowBook(args[i, 0], args[i, 1], args[i, 2]);
                Assert.IsFalse(result,
                       String.Format("Expected for combination '{0}': false; Actual: {1}",
                                     i, result));
            }
        }
        [TestMethod]
        public void TestReturnSuccess()
        {
            string[,] args = { { "633775170-1", "Tom" }, { "804783608-0", "Fabian" }, { "433183196-5", "Other person"}, { "229333912-2", "Anna" } };

            //string[,] args = { { "512425967-1", "Tom" }, { "631657126-7", "Fabian"}, { "433183196-5", "Other person"}, { "229333912-2", "Anna"} };
            for (int i = 0; i < 4; i++)
            {
                bool result = man.ReturnBook(args[i, 0], args[i, 1]);
                Assert.IsTrue(result,
                       String.Format("Expected for combination '{0}': true; Actual: {1}",
                                     i, result));
            }
        }
        [TestMethod]
        public void TestReturnFailure()
        {
            string[,] args = { { "1451", "Tom"}, { "804783608-0", "Tim" }, { "43396-5", "Other person" }, { "2912-2", "Anna" } };
            //                book not borrowed             wrong person name        wrong isbn                     
            for (int i = 0; i < 4; i++)
            {
                bool result = man.ReturnBook(args[i, 0], args[i, 1]);
                Assert.IsFalse(result,
                       String.Format("Expected for combination '{0}': false; Actual: {1}",
                                     i, result));
            }
        }
        [TestMethod]
        public void TestFilter1()
        {
            string[,] args = { { "taken", "key" }, { "category", "Horror" }, { "available", "key" }, { "isbn", "1" } };
            for (int i = 0; i < 4; i++)
            {
                List<Book> result = man.FilterBooks(args[i, 0], args[i, 1]);
                Assert.AreNotEqual(result.Count, 0,
                       String.Format("Expected for combination '{0}': >0; Actual: {1}",
                                     i, result.Count));
            }
        }
        [TestMethod]
        public void TestFilter2()
        {
            string[,] args = { { "random", "key" }, { "category", "11111" }, { "taken00", "" }, { "name", "555" } };
            for (int i = 0; i < 4; i++)
            {
                List<Book> result = man.FilterBooks(args[i, 0], args[i, 1]);
                Assert.AreEqual(result.Count, 0,
                       String.Format("Expected for combination '{0}': 0; Actual: {1}",
                                     i, result.Count));
            }
        }
        [TestMethod]
        public void TestBorrowedBooksCount1()
        {
            string[] names = { "Ieva", "Other person", "Tom", "Fabian" };
            foreach (string s in names)
            {
                int result = man.HowManyBooksBorrowedByPerson(s);
                Assert.AreNotEqual(result, 0,
                      String.Format("Expected for name '{0}': >0; Actual: {1}",
                                    s, result));
            }
        }
        [TestMethod]
        public void TestBorrowedBooksCount2()
        {
            string[] names = { "Name", "123", "čęėėęš", "" };
            foreach(string s in names)
            {
                int result = man.HowManyBooksBorrowedByPerson(s);
                Assert.AreEqual(result, 0,
                    String.Format("Expected for name '{0}': 0; Actual: {1}",
                                     s, result));
            }
        }
        [TestMethod]
        public void TestFindBookSuccess()
        {
            string[] names = { "838386008-0", "1452", "512425967-1", "415291608-7" };
            foreach (string s in names)
            {
                int result = man.HowManyBooksBorrowedByPerson(s);
                Assert.AreNotEqual(result, -1,
                    String.Format("Expected for name '{0}': 0; Actual: {1}",
                                     s, result));
            }
        }
        [TestMethod]
        public void TestFindBookFailure()
        {
            string[] isbn = { "isbn", "123", "čęėėęš", "" };
            foreach (string s in isbn)
            {
                int result = man.FindBook(s);
                Assert.AreEqual(result, -1,
                    String.Format("Expected for ISBN '{0}': -1; Actual: {1}",
                                     s, result));
            }
        }
    }
}
