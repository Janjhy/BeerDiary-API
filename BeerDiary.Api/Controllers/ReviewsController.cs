using Microsoft.AspNetCore.Mvc;
using BeerDiary.DataAccess.Services;
using BeerDiary.Domain.Models;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace BeerDiary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly BeerService _beerService;

        public ReviewsController(BeerService beerService)
        {
            _beerService = beerService;
        }

        // GET api/<ReviewsController>
        [HttpGet]
        public async Task<ActionResult<List<Review>>> Get()
        {
            int userId = 1;
            return await _beerService.GetReviewsByUserId(userId);
        }

        // GET api/<ReviewsController>/<id>
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetById(int id)
        {
            return await _beerService.GetReviewWithId(id);
        }

        // GET api/<ReviewsController>/beers
        [HttpGet("beers")]
        public async Task<ActionResult<List<Beer>>> GetBeers()
        {
            int userId = 1;
            return await _beerService.GetAllBeersReviewdByUser(userId);
        }

        // POST api/<ReviewsController>
        [HttpPost]
        public async Task<ActionResult<Review>> Post(Review newReview)
        {
            int userId = 1;
            var review = await _beerService.CreateReview(newReview, userId);

            return CreatedAtAction(nameof(GetById), new { id = review.Id }, review);
        }

        // PUT api/<ReviewsController>/5
        /*[HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }*/

        // DELETE api/<ReviewsController>/<id>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            int userId = 1;
            bool isDeleted = await _beerService.DeleteReview(id, userId);
            if(isDeleted)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
