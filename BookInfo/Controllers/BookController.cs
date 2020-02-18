using System;
using Microsoft.AspNetCore.Mvc;
using BookInfo.Repositories;
using BookInfo.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace BookInfo.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class BookController : ControllerBase // ControllerBase doesn't include view support so it's apropos for WebAPIs
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
            if (books.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(books);
            }
        }

        // Api/Book -- default request
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
        public IActionResult AddBook([FromBody]BookViewModel bookVm)
        {
            if (bookVm != null)
            {
                Book book = new Book { Title = bookVm.Title, Date = DateTime.Parse(bookVm.Date) };
                book.Authors.Add(new Author { Name = bookVm.Name, Birthday = DateTime.Parse(bookVm.Birthday) });
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
        public IActionResult Replace(int id, [FromBody]BookViewModel bookVm)
        {
            if (bookVm != null) 
            { 
                Book book = bookRepo.GetBookById(id);
                book.Title = bookVm.Title;
                book.Date = DateTime.Parse(bookVm.Date);
                book.Authors[0].Name = bookVm.Name;  // TODO: Find a better way to manage authors
                book.Authors[0].Birthday = DateTime.Parse(bookVm.Birthday);
                bookRepo.Edit(book);
                return Ok(book);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateBook(int id, [FromBody]PatchViewModel patchVm)
        {
            // TODO: Add support for more ops: remove, copy, move, test

            Book book = bookRepo.GetBookById(id);
            switch (patchVm.Path)
            {
                case "title":
                    book.Title = patchVm.Value;
                    break;
                case "date":
                    book.Date = Convert.ToDateTime(patchVm.Value);
                    break;
                case "author":
                    book.Authors[0].Name = patchVm.Value;   // TODO: manage author collection
                    break;
                case "birthdate":
                    book.Authors[0].Birthday = Convert.ToDateTime(patchVm.Value);
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
                return NoContent();  // Successfully completed, no data to send back
            }
            else
            {
                return NotFound();
            }
        }

    }
}
