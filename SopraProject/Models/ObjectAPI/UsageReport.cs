using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using SopraProject.Tools.Extensions.List.Statistics;
using SopraProject.Tools.Extensions.Date;
namespace SopraProject.ObjectApi
{

    /// <summary>
    /// Contains a set of metrics for a given day.
    /// </summary>
    public class DailyMetricSet
    {
        #region XML
        [XmlAttribute("day")]
        public string XMLDay { get { return Day.SerializeDate(); } set { throw new NotImplementedException(); } }
        #endregion

        /// <summary>
        /// Gets the day of this metric set.
        /// </summary>
        [XmlIgnore()]
        public DateTime Day { get; private set; }
        /// <summary>
        /// Gets the occupation rate of the room for the given day.
        /// 
        /// The occupation rate for a given day is computed by the following formula :
        /// - occupation rate = sum(hours occuped) / hours open.
        /// "hour open" is given by the constant RoomOpenDuration. Default value : 12h.
        /// </summary>
        [XmlElement("occupationRate")]
        public float OccupationRate { get; set; }
        /// <summary>
        /// Gets the fill rate per day of the room for the given day.
        /// 
        /// The fill rate for a given booking is computed using the following formula : 
        /// - fill_rate(booking) = number of participants / number of seats
        /// 
        /// The fill rate for a given day is computed by the following formula : 
        /// - fill_rate(day) = sum(fill_rate(bookings)) / count(bookings)
        /// </summary>
        [XmlElement("fillRate")]
        public float FillRate { get; set; }
        /// <summary>
        /// Gets the number of meetings for the given day.
        /// </summary>
        [XmlElement("meetingCount")]
        public int MeetingCount { get; set; }

        /// <summary>
        /// Mandatory constructor for XML serialization.
        /// </summary>
        public DailyMetricSet() { }

        /// <summary>
        /// Creates a new instance of DailyMetricSet.
        /// </summary>
        /// <param name="day"></param>
        public DailyMetricSet(DateTime day)
        {
            Day = day;
            OccupationRate = 0.0f;
            FillRate = 0.0f;
        }
    }
    /// <summary>
    /// Represents a set of statistics for a given metric.
    /// </summary>
    public class Statistics
    {
        [XmlElement("average")]
        public float Average { get; set; }
        [XmlElement("stddev")]
        public float StdDev { get; set; }
        [XmlElement("median")]
        public float Median { get; set; }
        [XmlElement("min")]
        public float Min { get; set; }
        [XmlElement("max")]
        public float Max { get; set; }
        public Statistics() { }
        /// <summary>
        /// Computes the average, standard deviation and median of the given series
        /// of values and stores them in this object.
        /// </summary>
        /// <param name="values"></param>
        public Statistics(List<float> values)
        {
            if (values.Count == 0) return;

            Average = values.Average();
            StdDev = values.StandardDeviation();
            Median = values.Median();
            Min = values.Min();
            Max = values.Max();
        }
    }
    /// <summary>
    /// Represents a report holding statistical information the usage of a certain room.
    /// </summary>
    public class UsageReport
    {

        /// <summary>
        /// Gets the number of hours a room is open.
        /// </summary>
        public static float RoomOpenDuration = 12;

        #region XML
        [XmlAttribute("id")]
        public string XMLRoom { get { return Room.Identifier.Value; } set { throw new NotImplementedException(); } }
        [XmlAttribute("start")]
        public string XMLStartDate { get { return StartDate.SerializeDate(); } set { throw new NotImplementedException(); } }
        [XmlAttribute("end")]
        public string XMLEndDate { get { return EndDate.SerializeDate(); } set { throw new NotImplementedException();  } }
        [XmlArray("metrics")]
        public List<DailyMetricSet> XMLMetricSet { get { return MetricSets; } set { throw new NotImplementedException(); } }
        [XmlElement("occupationRateStats")]
        public Statistics XMLOccupationRate { get { return OccupationRate; } set { throw new NotImplementedException(); } }
        [XmlElement("fillRateStats")]
        public Statistics XMLFillRate { get { return FillRate; } set { throw new NotImplementedException(); } }
        [XmlElement("meetingCountStats")]
        public Statistics XMLMeetingCount { get { return MeetingCount; } set { throw new NotImplementedException(); } }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the room whose statistics are presented in this report.
        /// </summary>
        [XmlIgnore]
        public Room Room { get; private set; }
        /// <summary>
        /// Gets the start date of the observation period.
        /// </summary>
        [XmlIgnore]
        public DateTime StartDate { get; private set; }
        /// <summary>
        /// Gets the end date of the observation period.
        /// </summary>
        [XmlIgnore]
        public DateTime EndDate { get; private set; }
        /// <summary>
        /// Gets the computed metric sets of the report.
        /// There is a metric set for each day of the observation period
        /// (StartDate to EndDate).
        /// 
        /// These metrics are ordered by day (from first to last).
        /// </summary>
        [XmlIgnore]
        public List<DailyMetricSet> MetricSets { get; private set; }

        /// <summary>
        /// Gets statistics about the occupation rate of the room for this observation period.
        /// </summary>
        [XmlIgnore]
        public Statistics OccupationRate { get; private set; }
        /// <summary>
        /// Gets statistics about the fill rate of the room for this observation period.
        /// </summary>
        [XmlIgnore]
        public Statistics FillRate { get; private set; }
        /// <summary>
        /// Gets statistics about the meeting count of the room for this observation period.
        /// </summary>
        [XmlIgnore]
        public Statistics MeetingCount { get; private set; }
        
        #endregion

        /// <summary>
        /// Creates a new instance of UsageReport and calculates the metrics.
        /// </summary>
        public UsageReport(Room room, DateTime startDate, DateTime endDate)
        {
            Room = room;
            StartDate = startDate;
            EndDate = endDate;
            ComputeMetrics();
        }


        /// <summary>
        /// Mandatory constructor for XML serialization. Dont use this.
        /// </summary>
        public UsageReport() { }


        /// <summary>
        /// Computes the metrics for this report.
        /// </summary>
        void ComputeMetrics()
        {
            // Gets the bookings corresponding to this period.
            List<Booking> bookings = ObjectApiProvider.Instance.BookingsApi.GetBookings(Room.Identifier, StartDate, EndDate)
                                     .ConvertAll(bookingId => new Booking(bookingId));

            if (bookings.Count != 0)
            {


                int firstday = GetDay(bookings.First().StartDate);
                int lastday = GetDay(bookings.Last().StartDate);

                // Computes the occupation rates indexed by day.
                Dictionary<int, float> occupationRates = new Dictionary<int, float>();
                Dictionary<int, float> fillRate = new Dictionary<int, float>();
                Dictionary<int, int> meetingCount = new Dictionary<int, int>();

                for (int i = firstday; i <= lastday; i++)
                {
                    occupationRates.Add(i, 0);
                    fillRate.Add(i, 0);
                    meetingCount.Add(i, 0);
                }

                foreach (Booking booking in bookings)
                {
                    int day = GetDay(booking.StartDate);
                    occupationRates[day] += booking.Duration / RoomOpenDuration;
                    fillRate[day] += (float)booking.ParticipantsCount / booking.Room.Capacity;
                    meetingCount[day]++;
                }

                // Aggregates data and puts it into the field.
                MetricSets = new List<DailyMetricSet>();
                for (int day = firstday; day < lastday; day++)
                {
                    DailyMetricSet metricSet = new DailyMetricSet(StartDate.AddDays(day));
                    metricSet.MeetingCount = meetingCount[day];
                    metricSet.OccupationRate = occupationRates[day];
                    metricSet.FillRate = fillRate[day];
                    MetricSets.Add(metricSet);
                }
            }
            else
            {
                MetricSets = new List<DailyMetricSet>();
            }

            // Compute statistics.
            OccupationRate = new Statistics(MetricSets.ConvertAll(metricset => metricset.OccupationRate));
            FillRate = new Statistics(MetricSets.ConvertAll(metricset => metricset.FillRate));
            MeetingCount = new Statistics(MetricSets.ConvertAll(metricset => (float)metricset.MeetingCount));
        }

        /// <summary>
        /// Gets the day number corresponding to the given date.
        /// The day number of the start date is 0.
        /// </summary>
        int GetDay(DateTime src)
        {
            DateTime troncatedStartDate = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day);
            return (int)(src - troncatedStartDate).TotalDays;
        }
    }
}