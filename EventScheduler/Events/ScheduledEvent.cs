using System;

namespace EventScheduler.Events
{
    public class ScheduledEvent : ScheduledEventBase
    {
        public sealed override DateTime ScheduledTime { get; protected set; }

        public ScheduledEvent(DateTime scheduledTime)
        {
            ScheduledTime = scheduledTime;
        }
        
        public ScheduledEvent(DateTime scheduledTime, Action action)
        {
            ScheduledTime = scheduledTime;
            _action = action;
        }
    }
}