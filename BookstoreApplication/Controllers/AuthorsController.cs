using BookstoreApplication.Data;
using BookstoreApplication.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookstoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        // GET: api/authors
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(DataStore.Authors);
        }

        // GET api/authors/5
        [HttpGet("{id}")]
        public IActionResult GetOne(int id)
        {
            var author = DataStore.Authors.FirstOrDefault(a => a.Id == id);
            if (author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }

        // POST api/authors
        [HttpPost]
        public IActionResult Post(Author author)
        {
            author.Id = DataStore.GetNewAuthorId();
            DataStore.Authors.Add(author);
            return Ok(author);
        }

        // PUT api/authors/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Author author)
        {
            if (id != author.Id)
            {
                return BadRequest();
            }

            var existingAuthor = DataStore.Authors.FirstOrDefault(a => a.Id == id);
            if (existingAuthor == null)
            {
                return NotFound();
            }

            int index = DataStore.Authors.IndexOf(existingAuthor);
            if (index == -1)
            {
                return NotFound();
                
            }

            DataStore.Authors[index] = author;
            return Ok(author);
        }

        // DELETE api/authors/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var author = DataStore.Authors.FirstOrDefault(a => a.Id == id);
            if (author == null)
            {
                return NotFound();
            }
            DataStore.Authors.Remove(author);

            // kaskadno brisanje svih knjiga obrisanog autora
            DataStore.Books.RemoveAll(b => b.AuthorId == id);

            return NoContent();
        }
    }
}
