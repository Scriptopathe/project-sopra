using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SopraProject.Tools.Extensions.Date
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Serializes a Date to a string.
        /// This method ensure that the given date time will be serialized the same
        /// way under different cultures and different time zones.
        /// </summary>
        public static string SerializeDate(this DateTime time)
        {
            try
            { 
                var utcTime = time.ToLocalTime();
                return utcTime.ToString(@"MM\/dd\/yyyy");
            }
            catch
            {
                throw new ArgumentException("Invalid date format.");
            }
        }

        /// <summary>
        /// Serializes the date time.
        /// </summary>
        /// <returns>The date time.</returns>
        /// <param name="time">Time.</param>
        public static string SerializeDateTime(this DateTime time)
        {
            try
            {
                var utcTime = time.ToLocalTime();
                return utcTime.ToString(@"MM\/dd\/yyyy-HH:mm:ss");
            }
            catch
            {
                throw new ArgumentException("Invalid date format.");
            }
        }
        /// <summary>
        /// Serializes a Date to a string.
        /// This method ensure that the given date time will be serialized the same
        /// way under different cultures and different time zones.
        /// </summary>
        public static DateTime DeserializeDate(this string time)
        {
            try
            {
                string[] values = time.Split('/');
                return new DateTime(Int32.Parse(values[2]), Int32.Parse(values[0]), Int32.Parse(values[1]), 0, 0, 0, DateTimeKind.Local);
            }
            catch
            {
                throw new ArgumentException("Invalid time format.");
            }
        }


        /// <summary>
        /// Serializes a DateTime to a string.
        /// This method ensure that the given date time will be serialized the same
        /// way under different cultures and different time zones.
        /// </summary>
        public static DateTime DeserializeDateTime(this string time)
        {
            try
            {
                string[] parts = time.Split('-');
                List<int> timeValues = parts[1].Split(':').ToList().ConvertAll(s => Int32.Parse(s));
                string[] values = parts[0].Split('/');
                return new DateTime(Int32.Parse(values[2]), Int32.Parse(values[0]), Int32.Parse(values[1]),
                    timeValues[0], timeValues[1], timeValues[2], DateTimeKind.Local);
            }
            catch
            {
                throw new ArgumentException("Invalid time format.");
            }
        }
    }
}