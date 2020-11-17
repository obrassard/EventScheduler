namespace EventScheduler.Service
{
    public class SchedulerEventArgs
    {
        /// <summary>
        /// The EventScheduler which triggered this event.
        /// </summary>
        public IEventScheduler EventScheduler { get; }

        /// <summary>
        /// Create a new instance of SchedulerEventArgs
        /// </summary>
        /// <param name="eventScheduler">A reference to the EventScheduler which triggered the event</param>
        public SchedulerEventArgs(IEventScheduler eventScheduler)
        {
            EventScheduler = eventScheduler;
        }
    }
}