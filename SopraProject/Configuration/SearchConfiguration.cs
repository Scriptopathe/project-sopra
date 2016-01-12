using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SopraProject.Configuration
{
    /// <summary>
    /// Configuration class of the search algorithm.
    /// </summary>
    public class SearchConfiguration
    {
        public event ConfStateChangedDelegate StateChanged;
        private int _minMeetingDuration = 15;
        private int _dayStart = 7;
        private int _dayEnd = 23;

        /// <summary>
        /// Minimal meeting duration, expressed in minutes.
        /// </summary>
        public int MinMeetingDuration
        {
            get { return _minMeetingDuration; }
            set
            {
                _minMeetingDuration = value;
                if(StateChanged != null)
                    StateChanged();
            }
        }
        /// <summary>
        /// Indicates the hour at which the algorithm considers a new day starts.
        /// </summary>
        public int DayStart
        {
            get { return _dayStart; }
            set
            {
                _dayStart = value;
                if (StateChanged != null)
                    StateChanged();
            }
        }
        /// <summary>
        /// Indicates the hour at which the algorithm considers a day ends.
        /// </summary>
        public int DayEnd
        {
            get { return _dayEnd; }
            set
            {
                _dayEnd = value;
                if (StateChanged != null)
                    StateChanged();
            }
        }


        /// <summary>
        /// Creates a new instance of Search Configuration with default value.
        /// </summary>
        public SearchConfiguration() { }
    }
}