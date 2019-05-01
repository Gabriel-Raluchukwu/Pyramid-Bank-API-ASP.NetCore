using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTwoCoreAPI.UtilityLogic
{
    public class UnixDateTime
    {
        public static long ConvertToUnixTime(DateTime dateTimeToConvert)
        {
            var dateTimeOffset = new DateTimeOffset(dateTimeToConvert);
            var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
            return unixDateTime;
        }

        public static DateTime ConvertToDateTime(long unixTime)
        {
            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTime);
            var localTime = dateTimeOffset.DateTime.ToLocalTime();
            return localTime;
        }
    }
}
