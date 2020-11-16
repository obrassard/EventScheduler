using System;

namespace EventScheduler.Events
{
    public class DailyEvent : ScheduledEventBase
    {
        private readonly TimeSpan _scheduledHour;
        public sealed override DateTime ScheduledTime { get; protected set; }

        public DailyEvent(TimeSpan timeOfDay) : this(timeOfDay, DateTime.Today){ }

                
        public DailyEvent(TimeSpan timeOfDay, Action action) : this(timeOfDay, DateTime.Today)
        {
            _action = action;
        }
        public DailyEvent(TimeSpan timeOfDay, DateTime startDay)
        {
            if (timeOfDay.Hours > 23 || timeOfDay.Minutes > 59 || timeOfDay.Seconds > 59)
                throw new ArgumentException("timeOfDay is not a valid hour");
            
            if (startDay < DateTime.Today)
                throw new ArgumentException("startDay cannot be passed");

            _scheduledHour = timeOfDay;
            ScheduledTime = GetNextScheduleDate(startDay);
        }

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