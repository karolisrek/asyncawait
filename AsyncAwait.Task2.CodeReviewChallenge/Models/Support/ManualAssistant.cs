// Remove unnecessary namespaces

// Before
//using System;
//using System.Net.Http;
//using System.Threading;
//using System.Threading.Tasks;
//using CloudServices.Interfaces;

// After
using System;
using System.Net.Http;
using System.Threading.Tasks;
using CloudServices.Interfaces;

namespace AsyncAwait.Task2.CodeReviewChallenge.Models.Support
{
    public class ManualAssistant : IAssistant
    {
        private readonly ISupportService _supportService;

        public ManualAssistant(ISupportService supportService)
        {
            _supportService = supportService ?? throw new ArgumentNullException(nameof(supportService));
        }

        // There is no reason to await the results, that way async keyword can be removed that unnecessary uses memory and lowers performance
        // Using Sleep is not effective and trustworthy way, better use await
        // Code shouldn't be left for debugging purposes
        // Returning message simple message on catch by running a new task doesn't bring any benefit

        // Before
        //public async Task<string> RequestAssistanceAsync(string requestInfo)
        //{
        //    try
        //    {
        //        Task t = _supportService.RegisterSupportRequestAsync(requestInfo);
        //        Console.WriteLine(t.Status); // this is for debugging purposes
        //        Thread.Sleep(5000); // this is just to be sure that the request is registered
        //        return await _supportService.GetSupportInfoAsync(requestInfo)
        //            .ConfigureAwait(false);
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        return await Task.Run(async () => await Task.FromResult($"Failed to register assistance request. Please try later. {ex.Message}"));
        //    }
        //}

        // After
        public async Task<string> RequestAssistanceAsync(string requestInfo)
        {
            try
            {
                await _supportService.RegisterSupportRequestAsync(requestInfo).ConfigureAwait(false);

                return await _supportService.GetSupportInfoAsync(requestInfo).ConfigureAwait(false);
            }
            catch (HttpRequestException ex)
            {
                return $"Failed to register assistance request. Please try later. {ex.Message}";
            }
        }
    }
}
