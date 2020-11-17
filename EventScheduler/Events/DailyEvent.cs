using System;
using EventScheduler.Service;

namespace EventScheduler.Events
{
    /// <summary>
    /// A DailyEvent is a specific type of ScheduledEvent
    /// which is triggered every day at a specific time.
    /// </summary>
    public class DailyEvent : ScheduledEventBase
    {
        private readonly TimeSpan _scheduledHour;
        public sealed override DateTime ScheduledTime { get; protected set; }

        /// <summary>
        /// Initializes a new instance of DailyEvent with a specific trigger time.
        /// </summary>
        /// <param name="timeOfDay">The hour at which the event will be triggered every day</param>
        public DailyEvent(TimeSpan timeOfDay) : this(timeOfDay, DateTime.Today){ }
        
        /// <summary>
        /// Initializes a new instance of DailyEvent with a specific trigger time
        /// providing an Action delegate to invoke.
        /// </summary>
        /// <param name="timeOfDay">The hour at which the event will be triggered every day</param>
        /// <param name="action">The action delegate to invoke on trigger</param>
        public DailyEvent(TimeSpan timeOfDay, Action action) : this(timeOfDay, DateTime.Today)
        {
            _action = action;
        }
        
        /// <summary>
        /// Initializes a new instance of DailyEvent with a specific trigger time,
        /// starting at a given date. 
        /// </summary>
        /// <param name="timeOfDay">The hour at which the event will be triggered every day</param>
        /// <param name="startDay">The date at which the recurring event must start</param>
        public DailyEvent(TimeSpan timeOfDay, DateTime startDay)
        {
            if (timeOfDay.Hours > 23 || timeOfDay.Minutes > 59 || timeOfDay.Seconds > 59)
                throw new ArgumentException("timeOfDay is not a valid hour");
            
            if (startDay < DateTime.Today)
                throw new ArgumentException("startDay cannot be in the past");

            _scheduledHour = timeOfDay;
            ScheduledTime = GetNextScheduleDate(startDay);
        }

        /// <summary>
        /// Initializes a new instance of DailyEvent with a specific trigger time,
        /// starting at a given date, providing an Action delegate to invoke. 
        /// </summary>
        /// <param name="timeOfDay">The hour at which the event will be triggered every day</param>
        /// <param name="startDay">The date at which the recurring event must start</param>
        /// <param name="action">The action delegate to invoke on trigger</param>
        public DailyEvent(TimeSpan timeOfDay, DateTime startDay, Action action) : this(timeOfDay, startDay)
        {
            _action = action;
        }
        
        public override void Trigger(IEventScheduler scheduler)
        {
            base.Trigger(scheduler);
            ScheduledTime = GetNextScheduleDate(DateTime.Today.AddDays(1));
            scheduler.Schedule(this);
        }

        private DateTime GetNextScheduleDate(DateTime fromDate)
        {
            var schedule = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 
                _scheduledHour.Hours, _scheduledHour.Minutes, _scheduledHour.Seconds);
            if (schedule < DateTime.Now)
                schedule = schedule.AddDays(1);
            return schedule;
        }
    }
}