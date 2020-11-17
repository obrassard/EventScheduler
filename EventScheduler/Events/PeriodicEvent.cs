using System;
using EventScheduler.Service;

namespace EventScheduler.Events
{
    /// <summary>
    /// A PeriodicEvent is a specific type of ScheduledEvent
    /// which is triggered periodically given a specific interval.
    /// </summary>
    public class PeriodicEvent : ScheduledEventBase
    {
        private readonly TimeSpan _timePeriod;
        public sealed override DateTime ScheduledTime { get; protected set; }

        /// <summary>
        /// Initializes a new instance of PeriodicEvent with a specific time interval.
        /// </summary>
        /// <param name="timePeriod">The time interval between each trigger</param>
        public PeriodicEvent(TimeSpan timePeriod) : this(timePeriod, DateTime.Now){}

        /// <summary>
        /// Initializes a new instance of PeriodicEvent with a specific time interval
        /// providing an Action delegate to invoke on trigger.
        /// </summary>
        /// <param name="timePeriod">The time interval between each trigger</param>
        /// <param name="action">The action to invoke</param>
        public PeriodicEvent(TimeSpan timePeriod, Action action) : this (timePeriod, DateTime.Now, action){}

        /// <summary>
        /// Initializes a new instance of PeriodicEvent with a time interval
        /// and a specific startDate.
        /// </summary>
        /// <param name="timePeriod">The time interval between each trigger</param>
        /// <param name="startDate">The date at which the recurring event must start</param>
        public PeriodicEvent(TimeSpan timePeriod, DateTime startDate)
        {
            if (startDate < DateTime.Now)
            {
                throw new ArgumentException("startDate cannot be in the past");
            }
            _timePeriod = timePeriod;
            ScheduledTime = startDate + timePeriod;
        }
        
        /// <summary>
        /// Initializes a new instance of PeriodicEvent with a time interval
        /// and a specific startDate, providing an Action delegate to invoke on trigger
        /// </summary>
        /// <param name="timePeriod">The time interval between each trigger</param>
        /// <param name="startDate">The date at which the recurring event must start</param>
        /// <param name="action">The action to invoke</param>
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