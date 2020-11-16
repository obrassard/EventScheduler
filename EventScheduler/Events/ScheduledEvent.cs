using System;

namespace EventScheduler.Events
{
    public class ScheduledEvent : ScheduledEventBase
    {
        public sealed override DateTime ScheduledTime { get; protected set; }

        public ScheduledEvent(DateTime scheduledTime)
        {
            if (scheduledTime < DateTime.Now)
            {
                throw new ArgumentException("scheduledTime cannot be passed");
            }
            ScheduledTime = scheduledTime;
        }
        
        public ScheduledEvent(DateTime scheduledTime, Action action): this(scheduledTime)
        {
            _action = action;
        }
    }
}