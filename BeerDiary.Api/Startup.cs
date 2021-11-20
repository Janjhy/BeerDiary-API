using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using BeerDiary.DataAccess.Data;
using BeerDiary.DataAccess.Services;

namespace BeerDiary.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddScoped<BeerService>();

            //var builder = new SqlConnectionStringBuilder(Configuration.GetConnectionString("BeerDiary"));
            var builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = "Data Source=DESKTOP-AFVT8DP\\SQLEXPRESS02;Initial Catalog=master;Integrated Security=True";

            IConfigurationSection beerDiaryCred = Configuration.GetSection("BeerDiaryCred");

            //builder.UserID = beerDiaryCred["UserId"];
            //builder.UserID = "DESKTOP-AFVT8DP\\proje";
            //builder.Password = "";
            //builder.Password = beerDiaryCred["Password"];

            services.AddDbContext<BeerDiaryContext>(opt => opt.UseSqlServer(builder.ConnectionString));
            
            services.AddControllers();

            services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
            {
                options.Audience = "beerdiary-api";
                options.Authority = "https://localhost:7226"; //TODO: Extract string
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
