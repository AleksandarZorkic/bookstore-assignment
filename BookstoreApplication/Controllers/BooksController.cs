using BookstoreApplication.Data;
using BookstoreApplication.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookstoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        // GET: api/books
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(DataStore.Books);
        }

        // GET api/books/5
        [HttpGet("{id}")]
        public IActionResult GetOne(int id)
        {
            var book = DataStore.Books.FirstOrDefault(a => a.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        // POST api/books
        [HttpPost]
        public IActionResult Post(Book book)
        {
            // kreiranje knjige je moguće ako je izabran postojeći autor
            var author = DataStore.Authors.FirstOrDefault(a => a.Id == book.AuthorId);
            if (author == null)
            {
                return BadRequest();
            }

            // kreiranje knjige je moguće ako je izabran postojeći izdavač
            var publisher = DataStore.Publishers.FirstOrDefault(a => a.Id == book.PublisherId);
            if (publisher == null)
            {
                return BadRequest();
            }

            book.Id = DataStore.GetNewBookId();
            book.Author = author;
            book.Publisher = publisher;
            DataStore.Books.Add(book);
            return Ok(book);
        }

        // PUT api/books/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            var existingBook = DataStore.Books.FirstOrDefault(a => a.Id == id);
            if (existingBook == null)
            {
                return NotFound();
            }

            // izmena knjige je moguca ako je izabran postojeći autor
            var author = DataStore.Authors.FirstOrDefault(a => a.Id == book.AuthorId);
            if (author == null)
            {
                return BadRequest();
            }

            // izmena knjige je moguca ako je izabran postojeći izdavač
            var publisher = DataStore.Publishers.FirstOrDefault(a => a.Id == book.PublisherId);
            if (publisher == null)
            {
                return BadRequest();
            }

            int index = DataStore.Books.IndexOf(existingBook);
            if (index == -1)
            {
                return NotFound();

            }

            book.Author = author;
            book.Publisher = publisher;
            DataStore.Books[index] = book;
            return Ok(book);
        }

        // DELETE api/books/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var book = DataStore.Books.FirstOrDefault(a => a.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            DataStore.Books.Remove(book);
            return NoContent();
        }
    }
}
