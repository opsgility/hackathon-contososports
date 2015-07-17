using System;

namespace ContosoWeb.Utils
{
    public static class DateTimeExtensions
    {
        private static readonly DateTimeOffset EpochUtc = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        public static long ToMillisecondsSinceUtcEpoch(this DateTimeOffset dt)
        {
            return (long) (dt - EpochUtc).TotalMilliseconds;
        }
    }
}