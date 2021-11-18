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
			List<Review> userReviews = await _context.Reviews.AsNoTracking().Where(review => review.UserId == userId).ToListAsync();
			
			return userReviews;
		}
		

		//Return list of all beers in database
		public async Task<List<Beer>> GetAllBeers()
		{
			//Using AsNoTracking() to not track beers and have better performance
			List<Beer> beers = await _context.Beers.AsNoTracking()
				//Set the beers in alphabetical order
				.OrderByDescending(beer => beer.BeerName).ToListAsync();
			 //Todo is this needed ?.ToListAsync();
			return beers;
		}

		public async Task<List<Beer>> GetAllBeersReviewdByUser(int userId) 
		{
			List<Review> reviews = await GetReviewsByUserId(userId);
			var idList = reviews.Select(review => review.BeerId).ToList();
			List<Beer> beers = await _context.Beers.AsNoTracking()
				.Where(beer => idList.Contains(beer.Id)).ToListAsync();
			return beers;
		}

		private IQueryable<Beer> GetBeerByInfo(Beer newBeer)
		{
			return _context.Beers.AsNoTracking().Where(beer => beer.BeerName == newBeer.BeerName && beer.BeerBrewer == newBeer.BeerBrewer && beer.BeerSize == newBeer.BeerSize); 
		}

		private IQueryable<Beer> GetBeerById(int beerId)
		{
			return _context.Beers.AsNoTracking().Where(beer => beer.Id == beerId);
		}

		private IQueryable<Review> GetReviewById(int reviewId)
        {
			return _context.Reviews.AsNoTracking().Where(review => review.Id == reviewId);
        }

		public async Task<Review> GetReviewWithId(int reviewId)
		{
			Review review = await GetReviewById(reviewId).FirstOrDefaultAsync();
			return review;
		}

		public async Task<Beer> GetBeerWithId(int beerId)
        {
			Beer beer = await GetBeerById(beerId).FirstOrDefaultAsync();
			return beer;
        }

		//Add new beer if it does not exist already
		public async Task<Beer> CreateBeer(Beer newBeer) 
		{
			Beer beer = await GetBeerByInfo(newBeer).FirstOrDefaultAsync();
			if (beer != null) return beer;

			_context.Beers.Add(newBeer);
			await _context.SaveChangesAsync();
			return newBeer;
		} 

		//Add new review, but check that beer exists
		public async Task<Review> CreateReview(Review newReview, int userId)
		{
			Beer beer = await GetBeerById(newReview.BeerId).FirstOrDefaultAsync();
			if (beer == null) return null;	
			
			newReview.ReviewDone = DateTime.UtcNow;
			newReview.UserId = userId;
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
				//beer.BeerReviews TODO: do I need to modify reviews list?
				_context.Entry(beer).State = EntityState.Modified;
				await _context.SaveChangesAsync();
				return true;
			}
			return false;
		}

		public async Task<bool> DeleteReview(int reviewId, int userId)
        {
			Review review = await GetReviewById(reviewId).FirstOrDefaultAsync();

			if(review != null && review.UserId == userId)
            {
				_context.Remove(review);
				await _context.SaveChangesAsync();
				return true;
            }
			return false;
        }
	}
}