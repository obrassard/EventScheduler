using System;

namespace EventScheduler.Events
{
    public abstract class ScheduledEventBase : IScheduledEvent
    {
        public event EventHandler<SchedulerEventArgs> EventTriggered;
        protected Action _action;

        public abstract DateTime ScheduledTime { get; protected set; }

        public virtual void Trigger(IEventScheduler scheduler)
        {
            var args = new SchedulerEventArgs(scheduler);
            EventTriggered?.Invoke(this, args);
            _action?.Invoke();
        }

        public int CompareTo(IScheduledEvent other)
        {
            return ScheduledTime.CompareTo(other.ScheduledTime);
        }
    }
}