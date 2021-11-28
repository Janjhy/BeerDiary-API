using Microsoft.AspNetCore.Mvc;
using BeerDiary.DataAccess.Services;
using BeerDiary.Domain.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

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

        private string GetSubject(Microsoft.AspNetCore.Http.HttpRequest req)
        {
            var token = req.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(token);
            var subject = decodedToken.Subject;
            return subject;
        }

        // GET api/<ReviewsController>
        [HttpGet]
        public async Task<ActionResult<List<Review>>> Get()
        {
            var subject = GetSubject(Request);
            int userId;
            try
            {
                userId = int.Parse(subject);
                return await _beerService.GetReviewsByUserId(userId);
            } catch
            {
                System.Diagnostics.Debug.WriteLine("Parsing user id error.");
                return NoContent();
            }
           
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
            var subject = GetSubject(Request);
            int userId;
            try
            {
                userId = int.Parse(subject);
                return await _beerService.GetAllBeersReviewdByUser(userId);
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Parsing user id error.");
                return NoContent();
            }
            
        }

        // POST api/<ReviewsController>
        [HttpPost]
        public async Task<ActionResult<Review>> Post(Review newReview)
        {
            var subject = GetSubject(Request);
            int userId;
            try
            {
                userId = int.Parse(subject);
                var review = await _beerService.CreateReview(newReview, userId);

                return CreatedAtAction(nameof(GetById), new { id = review.Id }, review);
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Parsing user id error.");
                return NoContent();
            }
        }

        // DELETE api/<ReviewsController>/<id>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var subject = GetSubject(Request);
            int userId;
            try
            {
                userId = int.Parse(subject);
                bool isDeleted = await _beerService.DeleteReview(id, userId);
                if (isDeleted)
                {
                    return NoContent();
                }
                return NotFound();
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Parsing user id error.");
                return NotFound();
            }
        }
    }
}
