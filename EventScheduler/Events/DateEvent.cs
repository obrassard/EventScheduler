using System;

namespace EventScheduler.Events
{
    public class ScheduledEvent : ScheduledEventBase
    {
        public sealed override DateTime ScheduledTime { get; protected set; }

        public ScheduledEvent(DateTime scheduledDateTime)
        {
            if (scheduledDateTime < DateTime.Now)
            {
                throw new ArgumentException("scheduledTime cannot be passed");
            }
            ScheduledTime = scheduledDateTime;
        }
        
        public ScheduledEvent(DateTime scheduledDateTime, Action action): this(scheduledDateTime)
        {
            _action = action;
        }
    }
}