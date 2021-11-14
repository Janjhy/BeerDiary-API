using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BeerDiary.DataAccess.Data;

namespace BeerDiary.DataAccess.Services
{
	public class BeerService
	{
		private readonly BeerDiaryContext _context;

		public BeerService(BeerDiaryContext context)
        {
			_context = context;
        }
	}

	public class ReviewService
	{
	}
}