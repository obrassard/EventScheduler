using System;

namespace EventScheduler.Events
{
    public class RecurringEvent : ScheduledEventBase
    {
        private readonly RecurringSchedule _schedule;
        public override DateTime ScheduledTime { get; protected set; }

        public RecurringEvent(DayOfWeek dayOfWeek, TimeSpan timeOfDay) : 
            this(new RecurringSchedule(dayOfWeek, timeOfDay)){ }
        
        public RecurringEvent(DayOfWeek dayOfWeek, TimeSpan timeOfDay, Action action) : 
            this(new RecurringSchedule(dayOfWeek, timeOfDay), action){ }

        public RecurringEvent(RecurringSchedule schedule, Action action) : this(schedule)
        {
            _action = action;
        }
        public RecurringEvent(RecurringSchedule schedule)
        {
            _schedule = schedule;
            ScheduledTime = GetNextScheduleDate();
        }
        
        public override void Trigger(IEventScheduler scheduler)
        {
            base.Trigger(scheduler);
            ScheduledTime = GetNextScheduleDate();
            scheduler.Schedule(this);
        }

        private DateTime GetNextScheduleDate()
        {
            DateTime scheduleDate = DateTime.Today;
            while(scheduleDate.DayOfWeek != _schedule.DayOfWeek)
                scheduleDate = scheduleDate.AddDays(1);

            TimeSpan time = _schedule.TimeOfDay;
            scheduleDate = new DateTime(scheduleDate.Year,scheduleDate.Month, scheduleDate.Day, time.Hours, time.Minutes, time.Seconds);

            if (scheduleDate < DateTime.Now)
            {
                scheduleDate = scheduleDate.AddDays(7);
            }

            return scheduleDate;
        }
    }

    public class RecurringSchedule
    {
        public DayOfWeek DayOfWeek { get; }
        public TimeSpan TimeOfDay { get; }

        public RecurringSchedule(DayOfWeek dayOfWeek, TimeSpan timeOfDay)
        {
            DayOfWeek = dayOfWeek;
            TimeOfDay = timeOfDay;
        }
    }
}