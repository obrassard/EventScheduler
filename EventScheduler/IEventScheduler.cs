using EventScheduler.Events;

namespace EventScheduler
{
    public interface IEventScheduler
    {
        void Schedule(IScheduledEvent evt);
    }
}