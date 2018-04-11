using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CognologyFlightBooking.Api.Data;
using CognologyFlightBooking.Api.Data.Interfaces;
using CognologyFlightBooking.Api.Data.Entities;
using CognologyFlightBooking.Api.Models;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace CognologyFlightBooking.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public static IConfigurationRoot Configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddMvcOptions(o => o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter()));

            //var connectionString = @"Server=(localdb)\mssqllocaldb;Database=FlightInfo;Trusted_Connection=True;";
            var connectionString = Startup.Configuration["connectionString:flightInfoDBConnectionString"];
            services.AddDbContext<FlightInfoContext>(o => o.UseSqlServer(connectionString));

            //we created a scope instance for repository to exists for entire cycle of a request
            services.AddScoped<IFlightInfoRepository, FlightInfoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            FlightInfoContext flightInfoContext)
        {
            loggerFactory.AddConsole();

            loggerFactory.AddDebug();

            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // to seed data to db
            flightInfoContext.EnsureSeedDataForContext();

            //to enable status codes from API to have appropriate http status code 
            app.UseStatusCodePages();

            //define mapping between db Entities and model DTO classes returned from API
            AutoMapper.Mapper.Initialize(cgf =>
            {
                cgf.CreateMap<Flight, FlightDto>();
                cgf.CreateMap<Passenger, PassengerDto>();
                cgf.CreateMap<FlightBooking, SearchBookingDto>();
                cgf.CreateMap<FlightBooking, BookingDto>();
                cgf.CreateMap<FlightBooking, MakeBookingDto > ();

                cgf.CreateMap<PassengerDto, Passenger>();
                cgf.CreateMap<FlightDto, Flight>();
                cgf.CreateMap<MakeBookingDto, FlightBooking>();
            });

            // to enable MVC pattern
            app.UseMvc();

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
