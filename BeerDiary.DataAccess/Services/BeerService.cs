using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BeerDiary.DataAccess.Data;
using BeerDiary.Domain.Models;

namespace BeerDiary.DataAccess.Services
{
	public class BeerService
	{
		private readonly BeerDiaryContext _context;

		public BeerService(BeerDiaryContext context)
    {
			_context = context;
		}

		public async Task<List<Review>> GetReviewsByUserId(int userId)
		{
			List<Review> userReviews = await _context.Reviews.AsNoTracking().Where(review => review.UserId == userId);
			
			return userReviews();
		}
		

		//Return list of all beers in database
		public async Task<List<Beer>> GetAllBeers()
		{
			//Using AsNoTracking() to not track beers and have better performance
			List<Beer> beers = await _context.Beers.AsNoTracking()
				//Set the beers in alphabetical order
				.OrderByDescending(beer => beer.BeerName);
			 //Todo is this needed ?.ToListAsync();
			if (!beers.Any()) {
				return NotFound();
			}
			return beers;
		}

		public async Task<List<Beer>> GetAllBeersReviewdByUser(int userId) 
		{
			List<Reviews> reviews = await GetReviewsByUserId(userId);
			List<Beer> beers = await _context.Beers.AsNoTracking()
				.Where(beer => reviews.Contains(beer.BeerId));
			return beers;
		}

		private IQueryable<Beer> GetBeerByInfo(Beer newBeer)
		{
			_context.Beers.AsNoTracking().Where(beer => beer.BeerName == newBeer.BeerName && beer.BeerBrewer == newBeer.BeerBrewer && beer.BeerSize == newBeer.BeerSize)) 
		}

		private IQueryable<Beer> GetBeerById(int beerId)
		{
			_context.Beers.AsNoTracking().Where(beer => beer.BeerId == beerId);
		}

		//Add new beer if it does not exist already
		public async Task<Beer> Create(Beer newBeer) 
		{
			if(GetBeerByInfo(newBeer)) return;

			_context.Beer.Add(newBeer);
			await _context.SaveChangesAsync();
			return newBeer;
		} 

		//Add new beer, but check that beer exists
		public async Task<Review> Create(Review newReview)
		{
			if(GetBeerById(newReview.BeerId)) return;	
			
			newReview.ReviewDone = DateTime.UtcNow;
			_context.Reviews.Add(newReview);
			await _context.SaveChangesAsync();


			return newReview;
		}
		public async Task<bool> UpdateBeerCountAndReview(int beerId, Review newReview)
		{
			Beer beer = await GetBeerById(beerId).FirstOrDefaultAsync();

			if(beer != null)
			{
				beer.BeerReviewCount++;
				//beer.BeerReviews
				_context.Entry(beer).State = EntityState.Modified;
				await _context.SaveChangesAsync();
				return true;
			}
			return false;
		}
	}
}