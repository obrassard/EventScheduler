using System;

namespace EventScheduler.Events
{
    /// <summary>
    /// A DateEvent is a specific type of ScheduledEvent
    /// which is triggered once at a specific DateTime.
    /// </summary>
    public class DateEvent : ScheduledEventBase
    {
        public override DateTime ScheduledTime { get; protected set; }

        /// <summary>
        /// Initializes a new instance of DateEvent with a specific trigger date.
        /// </summary>
        /// <param name="triggerDateTime">The event's trigger date</param>
        /// <exception cref="ArgumentException">If scheduledDateTime is a passed date</exception>
        public DateEvent(DateTime triggerDateTime)
        {
            if (triggerDateTime < DateTime.Now)
            {
                throw new ArgumentException("scheduledDateTime cannot be in the past");
            }
            ScheduledTime = triggerDateTime;
        }
        
        /// <summary>
        /// Initializes a new instance of DateEvent with a specific trigger date
        /// providing an Action delegate which will be invoked on scheduled date.
        /// </summary>
        /// <param name="triggerDateTime">The event's trigger date</param>
        /// <param name="action">The action delegate to invoke on trigger</param>
        public DateEvent(DateTime triggerDateTime, Action action): this(triggerDateTime)
        {
            _action = action;
        }
    }
}