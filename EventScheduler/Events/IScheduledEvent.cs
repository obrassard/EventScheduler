using System;
using EventScheduler.Service;

namespace EventScheduler.Events
{
    /// <summary>
    /// An event that can be scheduled in IEventScheduler
    /// </summary>
    public interface IScheduledEvent : IComparable<IScheduledEvent>
    {
        
        /// <summary>
        /// The next time the event should trigger
        /// </summary>
        DateTime ScheduledTime { get; }
        
        /// <summary>
        /// Trigger this action.
        /// This method is usually invoked by an IEventScheduler and shouldn't be called manually.
        /// </summary>
        /// <param name="scheduler">The scheduler which triggered the event</param>
        void Trigger(IEventScheduler scheduler);
        
        /// <summary>
        /// This event is invoked when the schedule event is triggered by IEventScheduler
        /// </summary>
        event EventHandler<SchedulerEventArgs> EventTriggered;
    }
}