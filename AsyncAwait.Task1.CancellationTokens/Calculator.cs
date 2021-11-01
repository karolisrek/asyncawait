using System.Threading;

namespace AsyncAwait.Task1.CancellationTokens
{
    static class Calculator
    {
        public static long Calculate(int n, CancellationToken token)
        {
            long sum = 0;

            for (int i = 0; i < n; i++)
            {
                if (token.IsCancellationRequested)
                {
                    return -1;
                }

                // i + 1 is to allow 2147483647 (Max(Int32)) 
                sum = sum + (i + 1);
                Thread.Sleep(10);
            }

            return sum;
        }
    }
}
