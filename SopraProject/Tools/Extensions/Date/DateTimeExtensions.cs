using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SopraProject.Tools.Extensions.Date
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Serializes a DateTime to a string.
        /// This method ensure that the given date time will be serialized the same
        /// way under different cultures and different time zones.
        /// </summary>
        public static string SerializeDate(this DateTime time)
        {
            var utcTime = time.ToLocalTime();
            return utcTime.ToString(@"MM\/dd\/yyyy");
        }

        /// <summary>
        /// Serializes a DateTime to a string.
        /// This method ensure that the given date time will be serialized the same
        /// way under different cultures and different time zones.
        /// </summary>
        public static DateTime DeserializeDate(this string time)
        {
            string[] values = time.Split('/');
            return new DateTime(Int32.Parse(values[2]), Int32.Parse(values[0]), Int32.Parse(values[1]), 0, 0, 0, DateTimeKind.Local);
        }
    }
}