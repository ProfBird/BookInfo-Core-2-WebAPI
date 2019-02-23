using System;
using Microsoft.AspNetCore.Mvc;
using BookInfo.Repositories;
using BookInfo.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace BookInfo.Controllers
{
    // This class will be instantiated by the MVC framework or by a unit test

    [Route("Api/[controller]")]
    public class BookController : Controller
    {
        private IBookRepository bookRepo;

        public BookController(IBookRepository bookRepo)
        {
            this.bookRepo = bookRepo;
        }

        /* Action Methods */

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

        [HttpGet("{id}")]
        public IActionResult GetBook(int id)
        { 
            var book = bookRepo.GetBookById(id);
            if (book == null)
            { 
                return NotFound();
            }
            else
            {
                return Ok(book);
            }
        }

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
                return BadRequest();
            }
        }

        // TODO: Fix author management
        // The author object from the original book will
        // still be pointing to this object
        [HttpPut("{id}")]
        public IActionResult Replace(int id, string title, string date, string author, string birthdate)
        {
            Book book = new Book { Title = title, Date = DateTime.Parse(date) };
            book.BookID = id;
            if (author != null)
            {
                book.Authors.Add(new Author { Name = author, Birthday = DateTime.Parse(birthdate) });
                bookRepo.Edit(book);
                return Ok(book);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateBook(int id, string op, string path, string value)
        {
            // TODO: Add support for more ops: remove, copy, move, test

            Book book = bookRepo.GetBookById(id);
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
                    return BadRequest(); 
            }
            bookRepo.Edit(book);
            return Ok(book);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            Book book = bookRepo.GetBookById(id);
            if (book != null)
            {
                bookRepo.Delete(id);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

    }
}
