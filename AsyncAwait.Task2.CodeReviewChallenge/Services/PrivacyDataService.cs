using System.Threading.Tasks;

namespace AsyncAwait.Task2.CodeReviewChallenge.Services
{
    public class PrivacyDataService : IPrivacyDataService
    {
        // Move message to const, simplify function
        // Converting ValueTask to task brings no performance benefit and adds complexity, therefore can be simplified

        // Before
        //public Task<string> GetPrivacyDataAsync()
        //{
        //    return new ValueTask<string>("This Policy describes how async/await processes your personal data," +
        //                                    "but it may not address all possible data processing scenarios.").AsTask();
        //}

        // After
        const string PrivacyDataMessage = "This Policy describes how async/await processes your personal data, but it may not address all possible data processing scenarios.";
        public Task<string> GetPrivacyDataAsync()
        {
            return Task.FromResult(PrivacyDataMessage);
        }
    }
}
