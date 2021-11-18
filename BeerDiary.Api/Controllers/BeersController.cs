using Microsoft.AspNetCore.Mvc;
using BeerDiary.DataAccess.Services;
using BeerDiary.Domain.Models;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace BeerDiary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeersController : ControllerBase
    {
        private readonly BeerService _beerService;

        public BeersController(BeerService beerService)
        {
            _beerService = beerService;
        }

        // GET: api/<BeersController>
        [HttpGet]
        public async Task<ActionResult<List<Beer>>> Get()
        {
            return await _beerService.GetAllBeers();
        }

        // GET api/<BeersController>/<id>
        [HttpGet("{id}")]
        public async Task<ActionResult<Beer>> GetById(int id)
        {
            return await _beerService.GetBeerWithId(id);
        }

        // POST api/<BeersController>
        [HttpPost]
        public async Task<ActionResult<Beer>> Post(Beer newBeer)
        {
            var beer = await _beerService.CreateBeer(newBeer);

            return CreatedAtAction(nameof(GetById), new { id = beer.Id }, beer);
        }
    }
}
