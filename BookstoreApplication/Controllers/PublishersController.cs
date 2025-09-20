using BookstoreApplication.Data;
using BookstoreApplication.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookstoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        // GET: api/publishers
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(DataStore.Publishers);
        }

        // GET api/publishers/5
        [HttpGet("{id}")]
        public IActionResult GetOne(int id)
        {
            var publisher = DataStore.Publishers.FirstOrDefault(a => a.Id == id);
            if (publisher == null)
            {
                return NotFound();
            }
            return Ok(publisher);
        }

        // POST api/publishers
        [HttpPost]
        public IActionResult Post(Publisher publisher)
        {
            publisher.Id = DataStore.GetNewPublisherId();
            DataStore.Publishers.Add(publisher);
            return Ok(publisher);
        }

        // PUT api/publishers/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Publisher publisher)
        {
            if (id != publisher.Id)
            {
                return BadRequest();
            }

            var existingPublisher = DataStore.Publishers.FirstOrDefault(a => a.Id == id);
            if (existingPublisher == null)
            {
                return NotFound();
            }

            int index = DataStore.Publishers.IndexOf(existingPublisher);
            if (index == -1)
            {
                return NotFound();

            }

            DataStore.Publishers[index] = publisher;
            return Ok(publisher);
        }

        // DELETE api/publishers/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var publisher = DataStore.Publishers.FirstOrDefault(a => a.Id == id);
            if (publisher == null)
            {
                return NotFound();
            }
            DataStore.Publishers.Remove(publisher);

            // kaskadno brisanje svih knjiga obrisanog izdavača
            DataStore.Books.RemoveAll(b => b.PublisherId == id);

            return NoContent();
        }
    }
}
