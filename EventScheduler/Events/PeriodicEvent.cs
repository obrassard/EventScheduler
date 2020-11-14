using System;

namespace EventScheduler.Events
{
    public class PeriodicEvent : ScheduledEventBase
    {
        private readonly TimeSpan _timePeriod;
        public sealed override DateTime ScheduledTime { get; protected set; }
        
        public PeriodicEvent(TimeSpan timePeriod)
        {
            _timePeriod = timePeriod;
            ScheduledTime = DateTime.Now + timePeriod;
        }

        public override void Trigger(IEventScheduler scheduler)
        {
            base.Trigger(scheduler);
            ScheduledTime = GetNextScheduleDate();
            scheduler.Schedule(this);
        }

        private DateTime GetNextScheduleDate()
        {
           return DateTime.Now + _timePeriod;
        }
    }
}