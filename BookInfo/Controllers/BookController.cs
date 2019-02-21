using System;
using Microsoft.AspNetCore.Mvc;
using BookInfo.Repositories;
using BookInfo.Models;
using Microsoft.AspNetCore.Authorization;

namespace BookInfo.Controllers
{
    // This class will be instantiated by the MVC framework or by a unit test
    public class BookController : Controller
    {
        private IBookRepository bookRepo;

        public BookController(IBookRepository bookRepo)
        {
            this.bookRepo = bookRepo;
        }

        /* Action Methods that get info from the database */

       
        public IActionResult Index()
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


        [HttpGet]
        public ViewResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(string title, string date, string author, string birthdate)
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
        public IActionResult Edit(int bookId, string title, string date, string author, string birthdate)
        {
            Book book = bookRepo.GetBookById(bookId);
            if (title != "")
                book.Title = title;
            /*
            if (date != "")
              //   book.Date = new DateTime(date);
            if (author != "")
               // book.Authors[0] = author; // TODO: Specify the author to change
            if (birthdate != "")
            //    book.Authors[0].Birthday = birthdate;
            */
            if (author != null)
            {
                bookRepo.Edit(book);
                return Ok(book);
            }
            else
            {
                return NotFound();  //  TODO: Use a better error method
            }

        }

        [HttpGet]
        public ViewResult Edit (int id)
        {
            return View(bookRepo.GetBookById(id));
        }

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
    }
}
