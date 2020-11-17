using EventScheduler.Events;

namespace EventScheduler.Service
{
    /// <summary>
    /// Represent a service object that can schedule and trigger IScheduledEvent
    /// </summary>
    public interface IEventScheduler
    {
        /// <summary>
        /// Schedule a new event by adding it to the event queue.
        /// </summary>
        /// <param name="evt">The event to schedule</param>
        void Schedule(IScheduledEvent evt);

        /// <summary>
        /// Cancel a previously scheduled event.
        /// </summary>
        /// <param name="evt">The event to cancel</param>
        /// <returns>Returns true if the given event was successfully cancelled</returns>
        bool CancelEvent(IScheduledEvent evt);
    }
}