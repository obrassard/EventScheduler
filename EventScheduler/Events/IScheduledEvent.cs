using System;

namespace EventScheduler.Events
{
    public interface IScheduledEvent : IComparable<IScheduledEvent>
    {
        DateTime ScheduledTime { get; }
        void Trigger(IEventScheduler scheduler);
        
        event EventHandler<SchedulerEventArgs> EventTriggered;
    }
}