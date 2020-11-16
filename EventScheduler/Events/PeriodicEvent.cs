using System;

namespace EventScheduler.Events
{
    public class PeriodicEvent : ScheduledEventBase
    {
        private readonly TimeSpan _timePeriod;
        public sealed override DateTime ScheduledTime { get; protected set; }

        public PeriodicEvent(TimeSpan timePeriod) : this(timePeriod, DateTime.Now){}

        public PeriodicEvent(TimeSpan timePeriod, Action action) : this (timePeriod, DateTime.Now, action){}

        public PeriodicEvent(TimeSpan timePeriod, DateTime startDate)
        {
            if (startDate < DateTime.Now)
            {
                throw new ArgumentException("startDate cannot be passed");
            }
            _timePeriod = timePeriod;
            ScheduledTime = startDate + timePeriod;
        }
        
        public PeriodicEvent(TimeSpan timePeriod, DateTime startDate, Action action): this(timePeriod, startDate)
        {
            _action = action;
        }

        public override void Trigger(IEventScheduler scheduler)
        {
            base.Trigger(scheduler);
            ScheduledTime = DateTime.Now + _timePeriod;
            scheduler.Schedule(this);
        }
    }
}