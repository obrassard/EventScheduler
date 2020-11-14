namespace EventScheduler
{
    public class SchedulerEventArgs
    {
        public IEventScheduler EventScheduler { get; }

        public SchedulerEventArgs(IEventScheduler eventScheduler)
        {
            EventScheduler = eventScheduler;
        }
    }
}