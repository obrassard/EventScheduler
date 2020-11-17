using System;
using EventScheduler.Service;

namespace EventScheduler.Events
{
    /// <summary>
    /// A YearlyEvent is a specific type of ScheduledEvent
    /// which is triggered every year at a specific date.
    /// </summary>
    public class YearlyEvent : ScheduledEventBase
    {
        public sealed override DateTime ScheduledTime { get; protected set; }

        /// <summary>
        /// Initializes a new instance of YearlyEvent with the date of the first occurence.
        /// The event will trigger at this date every year, starting from the provided DateTime.
        /// </summary>
        /// <param name="firstOccurence">The date of the first occurence</param>
        /// <exception cref="ArgumentException">If firstOccurence is a past date</exception>
        public YearlyEvent(DateTime firstOccurence)
        {
            if (firstOccurence < DateTime.Now)
            {
                throw new ArgumentException("firstOccurence cannot be in the past");
            }
            ScheduledTime = firstOccurence;
        }
        
        /// <summary>
        /// Initializes a new instance of YearlyEvent with the date of the first occurence, providing
        /// a specific Action delegate to invoke on trigger.
        /// The event will trigger at this date every year, starting from the provided DateTime.
        /// </summary>
        /// <param name="firstOccurence">The date of the first occurence</param>
        /// <param name="action">An action to invoke on trigger</param>
        /// <exception cref="ArgumentException">If firstOccurence is a past date</exception>
        public YearlyEvent(DateTime firstOccurence, Action action): this(firstOccurence)
        {
            _action = action;
        }

        public override void Trigger(IEventScheduler scheduler)
        {
            base.Trigger(scheduler);
            ScheduledTime = ScheduledTime.AddYears(1);
            scheduler.Schedule(this);
        }
    }
}