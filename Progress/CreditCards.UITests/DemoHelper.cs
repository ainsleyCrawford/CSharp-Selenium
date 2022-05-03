using System.Threading;

namespace CreditCards.UITests
{
    internal static class DemoHelper
    {
        // Brief delay to slow down browser interactions for clarity
        public static void Pause(int secondsToPause = 3000)
        {
            Thread.Sleep(secondsToPause);
        }
    }
}
