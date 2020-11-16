using System;

namespace EventScheduler.Events
{
    public class YearlyEvent : ScheduledEventBase
    {
        public sealed override DateTime ScheduledTime { get; protected set; }

        public YearlyEvent(DateTime firstOccurence)
        {
            if (firstOccurence < DateTime.Now)
            {
                throw new ArgumentException("firstOccurence cannot be passed");
            }
            ScheduledTime = firstOccurence;
        }
        
        public YearlyEvent(DateTime firstOccurence, Action action): this(firstOccurence)
        {
            _action = action;
        }

        public override void Trigger(IEventScheduler scheduler)
        {
            base.Trigger(scheduler);
            ScheduledTime = ScheduledTime.AddYears(1);
            scheduler.Schedule(this);
        }
    }
}