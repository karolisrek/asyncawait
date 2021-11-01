// Remove unncessary namespaces

// Before
//using System;
//using System.Threading;
//using System.Threading.Tasks;
//using AsyncAwait.Task2.CodeReviewChallenge.Headers;
//using CloudServices.Interfaces;
//using Microsoft.AspNetCore.Http;

// After
using System;
using System.Threading.Tasks;
using AsyncAwait.Task2.CodeReviewChallenge.Headers;
using CloudServices.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AsyncAwait.Task2.CodeReviewChallenge.Middleware
{
    public class StatisticMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly IStatisticService _statisticService;

        public StatisticMiddleware(RequestDelegate next, IStatisticService statisticService)
        {
            _next = next;
            _statisticService = statisticService ?? throw new ArgumentNullException(nameof(statisticService));
        }

        // Don't rely on Sleep, simply await for the task
        // Threr is no need to start a new task on async function
        // Instead of GetAwaiter use await which uses GetAwaiter internally
        // Code for debugging purposes shouldn't stay
        // UpdateHeaders can be a private method no need for nested one
        // UpdateHandler dont need to use GetAwaiter too, simply use await

        // Before
        //public async Task InvokeAsync(HttpContext context)
        //{   
        //    string path = context.Request.Path;

        //    Task staticRegTask = Task.Run(
        //        () => _statisticService.RegisterVisitAsync(path)
        //        .ConfigureAwait(false)
        //        .GetAwaiter().OnCompleted(UpdateHeaders));
        //    Console.WriteLine(staticRegTask.Status); // just for debugging purposes
            
        //    void UpdateHeaders()
        //    {
        //        context.Response.Headers.Add(
        //            CustomHttpHeaders.TotalPageVisits,
        //            _statisticService.GetVisitsCountAsync(path).GetAwaiter().GetResult().ToString());
        //    }

        //    Thread.Sleep(3000); // without this the statistic counter does not work
        //    await _next(context);
        //}

        // After
        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path;

            await _statisticService.RegisterVisitAsync(path);
            await UpdateHeaders(context, path);

            await _next(context);
        }

        private async Task UpdateHeaders(HttpContext context, string path)
        {
            var visitsCount = await _statisticService.GetVisitsCountAsync(path);
            context.Response.Headers.Add(CustomHttpHeaders.TotalPageVisits, visitsCount.ToString());
        }
    }
}
