using System;
using EventScheduler.Service;

namespace EventScheduler.Events
{
    public abstract class ScheduledEventBase : IScheduledEvent
    {
        public event EventHandler<SchedulerEventArgs> EventTriggered;
        protected Action _action;

        /// <summary>
        /// The next time the event should trigger
        /// </summary>
        public abstract DateTime ScheduledTime { get; protected set; }

        /// <summary>
        /// Trigger this action.
        /// This method is usually invoked by an IEventScheduler and shouldn't be called manually.
        /// </summary>
        /// <param name="scheduler">The scheduler which triggered the event</param>
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