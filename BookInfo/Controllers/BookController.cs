using System;
using Microsoft.AspNetCore.Mvc;
using BookInfo.Repositories;
using BookInfo.Models;
using Microsoft.AspNetCore.Authorization;

namespace BookInfo.Controllers
{
    // This class will be instantiated by the MVC framework or by a unit test

    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private IBookRepository bookRepo;

        public BookController(IBookRepository bookRepo)
        {
            this.bookRepo = bookRepo;
        }

        /* Action Methods that get info from the database */

       [HttpGet]
        public IActionResult GetBooks()
        {
            var books = bookRepo.GetAllBooks();
            if (books.Count == 0) {
                return NotFound();
            }
            else
            {
                return Ok(books);
            }
        }

        /*  // TODO: Provide views for these actions
        public ViewResult AuthorsOfBook(Book book)
        {
            return View(bookRepo.GetAuthorsByBook(book));
        }

        public ViewResult BooksByAuthor(Author author)
        {
            return View(bookRepo.GetBooksByAuthor(author));
        }

        public ViewResult BookByTitle(string title)
        {
            return View(bookRepo.GetBookByTitle(title));
        }
        */
        
        /* Action methods that modify the database */




        [HttpPost]
        public IActionResult AddBook(string title, string date, string author, string birthdate)
        {
            Book book = new Book { Title = title, Date = DateTime.Parse(date) };
            if (author != null)
            {
                book.Authors.Add(new Author { Name = author, Birthday = DateTime.Parse(birthdate)});
                bookRepo.AddBook(book);
                return Ok(book);
            }
            else
            {
                return NotFound();  //  TODO: Use a better error method
            }

        }

        [HttpPut]
        public IActionResult Replace(int bookId, string title, string date, string author, string birthdate)
        {
            Book book = new Book { Title = title, Date = DateTime.Parse(date) };
            book.BookID = bookId;
            if (author != null)
            {
                book.Authors.Add(new Author { Name = author, Birthday = DateTime.Parse(birthdate) });
                bookRepo.Edit(book);
                return Ok(book);
            }
            else
            {
                return NotFound();  //  TODO: Use a better error method
            }

        }

        [HttpPatch]
        public IActionResult UpdateBook(int bookId, string op, string path, string value)
        {
            // TODO: Add support for more ops: remove, copy, move, test

            Book book = bookRepo.GetBookById(bookId);
            switch(path)
            {
                case "title":
                book.Title = value;
                    break;
                case "date":
                    book.Date = Convert.ToDateTime(value);
                    break;
                case "author":
                    book.Authors[0].Name = value;   // TODO: manage author collection
                    break;
                case "birthdate":
                    book.Authors[0].Birthday = Convert.ToDateTime(value);
                    break;
                default:
                    return NotFound();  //  TODO: Use a better error method
            }

            bookRepo.Edit(book);
            return Ok(book);


        }

        /*
        [HttpPost]
        public RedirectToActionResult Edit(Book book)
        {
            bookRepo.Edit(book);
            return RedirectToAction("Index");                
        }

        public RedirectToActionResult Delete(int id)
        {
            bookRepo.Delete(id);
            return RedirectToAction("Index");
        }
        */
    }
}
