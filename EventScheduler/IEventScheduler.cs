using EventScheduler.Events;

namespace EventScheduler
{
    /// <summary>
    /// The minimal interface of an event scheduler implementation
    /// </summary>
    public interface IEventScheduler
    {
        void Schedule(IScheduledEvent evt);

        bool CancelEvent(IScheduledEvent evt);
    }
}