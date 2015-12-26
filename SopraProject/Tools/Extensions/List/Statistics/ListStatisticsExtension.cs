using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SopraProject.Tools.Extensions.List.Statistics
{
    public static class ListStatisticsExtension
    {
        /// <summary>
        /// Computes the median of the given values.
        /// </summary>
        public static float Median(this List<float> values)
        {
            if (values.Count == 0)
                return 0;
            else if(values.Count % 2 == 0)
                return (values[values.Count / 2 - 1] + values[values.Count / 2]) / 2.0f;
            else
                return values[values.Count / 2];
        }

        /// <summary>
        /// Computes the standard deviation of the given values.
        /// </summary>
        public static float StandardDeviation(this List<float> values)
        {
            if (values.Count == 0)
                return 0;

            float avg = values.Average();
            // Calculates sum((xi - avg)²)
            float sum = values.ConvertAll(val => (float)Math.Pow(val - avg, 2)).Sum();
            return (float)Math.Sqrt(sum / values.Count);
        }
    }
}