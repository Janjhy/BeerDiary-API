using BeerDiary.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BeerDiary.DataAccess.Data
{
    public partial class BeerDiaryContext : DbContext
    {
        public BeerDiaryContext(DbContextOptions<BeerDiaryContext> options) : base(options)
        {
        }

        public DbSet<Beer> Beers {get;set;}
        public DbSet<Review> Reviews {get;set;}
        public DbSet<User> Users {get;set;}
    }
}
