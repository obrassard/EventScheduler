using System;

namespace EventScheduler.Events
{
    public class MonthlyEvent : ScheduledEventBase
    {
        public sealed override DateTime ScheduledTime { get; protected set; }
        private readonly int _dayOfMonth;

        public MonthlyEvent(DateTime firstOccurence)
        {
            if (firstOccurence < DateTime.Now)
            {
                throw new ArgumentException("firstOccurence cannot be in the past");
            }

            _dayOfMonth = firstOccurence.Day;
            ScheduledTime = firstOccurence;
        }
        
        public MonthlyEvent(DateTime firstOccurence, Action action): this(firstOccurence)
        {
            _action = action;
        }

        public override void Trigger(IEventScheduler scheduler)
        {
            base.Trigger(scheduler);

            DateTime nextMonth = DateTime.Today.AddMonths(1);
            int dayInMonth = DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month);
            int nextDay = dayInMonth < _dayOfMonth ? dayInMonth : _dayOfMonth;
            
            ScheduledTime = new DateTime(nextMonth.Year, nextMonth.Month, nextDay, 
                            ScheduledTime.Hour, ScheduledTime.Minute, ScheduledTime.Second);
            
            scheduler.Schedule(this);
        }
    }
}