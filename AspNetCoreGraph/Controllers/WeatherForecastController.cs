using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreGraph.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetCoreGraph.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class WeatherForecastController : ControllerBase
    {
        private UniBlocksDBContext uniBlocksDBContext;
        [ActivatorUtilitiesConstructor]
        public WeatherForecastController(UniBlocksDBContext uniCont)
        {
            uniBlocksDBContext = uniCont;

            //var context = new UniBlocksDBContext();

            //global::System.Console.WriteLine(context.Services);



        }
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            uniBlocksDBContext.Database.EnsureDeleted();
            uniBlocksDBContext.Database.EnsureCreated();

            var aSub = new Subscription() { };

            var aservice =  new AService() { Name ="Fixing Water" };


            uniBlocksDBContext.Add(new AServiceSubscription()
            {
                Subscription = aSub, Service = aservice
            });
            //uniBlocksDBContext.Add(
            //    new ServiceSubscription { Service = aservice, subscription = aSub });

            
            uniBlocksDBContext.SaveChanges();

            //var serviceDBEntry = uniBlocksDBContext.Services.Add(
            //      new Models.Service
            //      {
            //          Name = "test service one"      
            //      }
            //      );
            //uniBlocksDBContext.SaveChanges();

            //uniBlocksDBContext.Subscriptions.Add(
            //        new Models.Subscription
            //        {


            //        });
            //uniBlocksDBContext.SaveChanges();

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
