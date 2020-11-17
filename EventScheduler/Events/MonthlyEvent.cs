using System;
using EventScheduler.Service;

namespace EventScheduler.Events
{
    /// <summary>
    /// A MonthlyEvent is a specific type of ScheduledEvent
    /// which is triggered every month at a specific day.
    /// </summary>
    public class MonthlyEvent : ScheduledEventBase
    {
        public sealed override DateTime ScheduledTime { get; protected set; }
        private readonly int _dayOfMonth;

        /// <summary>
        /// Initializes a new instance of MonthlyEvent with the date of the first occurence.
        /// The event will trigger at the same day each months, starting from the provided DateTime.
        /// </summary>
        /// <param name="firstOccurence">The date of the first occurence</param>
        /// <exception cref="ArgumentException">If firstOccurence is a past date</exception>
        public MonthlyEvent(DateTime firstOccurence)
        {
            if (firstOccurence < DateTime.Now)
            {
                throw new ArgumentException("firstOccurence cannot be in the past");
            }

            _dayOfMonth = firstOccurence.Day;
            ScheduledTime = firstOccurence;
        }
        
        /// <summary>
        /// Initializes a new instance of MonthlyEvent with the date of the first occurence, providing
        /// a specific action delegate to invoke on trigger.
        /// The event will trigger at the same day each months, starting from the provided DateTime.
        /// </summary>
        /// <param name="firstOccurence">The date of the first occurence</param>
        /// <exception cref="ArgumentException">If firstOccurence is a past date</exception>
        /// <param name="action">The action to invoke on trigger</param>
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