using System;
using System.Collections.Generic;
using System.Linq;
using EventScheduler.Service;

namespace EventScheduler.Events
{
    /// <summary>
    /// A WeeklyEvent is a specific type of ScheduledEvent
    /// which is triggered weekly given a specific WeeklySchedule
    /// (a set of DaysOfWeek / Time)
    /// </summary>
    public class WeeklyEvent : ScheduledEventBase
    {
        private readonly Queue<WeeklySchedule> _scheduleQueue;
        public override DateTime ScheduledTime { get; protected set; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of WeeklyEvent for a specific day / time.
        /// The event will trigger every week at the provided day and time.
        /// </summary>
        /// <param name="dayOfWeek">Trigger day</param>
        /// <param name="timeOfDay">Trigger time</param>
        public WeeklyEvent(DayOfWeek dayOfWeek, TimeSpan timeOfDay) : 
            this(dayOfWeek, timeOfDay, DateTime.Now){}
        
        /// <summary>
        /// Initializes a new instance of WeeklyEvent for a specific day / time
        /// providing an action delegate to invoke on trigger.
        /// The event will trigger every week at the provided day and time.
        /// </summary>
        /// <param name="dayOfWeek">Trigger day</param>
        /// <param name="timeOfDay">Trigger time</param>
        /// <param name="action">The action to invoke</param>
        public WeeklyEvent(DayOfWeek dayOfWeek, TimeSpan timeOfDay, Action action) :
            this(dayOfWeek, timeOfDay, DateTime.Now, action){}
        
        /// <summary>
        /// Initializes a new instance of WeeklyEvent for a specific day / time, starting at a given date.
        /// The event will trigger every week at the provided day and time.
        /// </summary>
        /// <param name="dayOfWeek">Trigger day</param>
        /// <param name="timeOfDay">Trigger time</param>
        /// <param name="startDate">Date on which the recurrence begins</param>
        public WeeklyEvent(DayOfWeek dayOfWeek, TimeSpan timeOfDay, DateTime startDate) : 
            this(dayOfWeek, timeOfDay, startDate, null){}

        /// <summary>
        /// Initializes a new instance of WeeklyEvent for a specific day / time, starting at a given date,
        /// providing an action delegate to invoke on trigger.
        /// The event will trigger every week at the provided day and time.
        /// </summary>
        /// <param name="dayOfWeek">Trigger day</param>
        /// <param name="timeOfDay">Trigger time</param>
        /// <param name="startDate">Date on which the recurrence begins</param>
        /// <param name="action">The action to invoke</param>
        public WeeklyEvent(DayOfWeek dayOfWeek, TimeSpan timeOfDay, DateTime startDate, Action action)
        {
            var schedule = new WeeklySchedule(dayOfWeek, timeOfDay);
            _scheduleQueue = new Queue<WeeklySchedule>();
            _scheduleQueue.Enqueue(schedule);
            
            ScheduledTime = GetNextScheduleDate(startDate);
            
            if (action != null)
            {
                _action = action;
            }
        }
        
        /// <summary>
        /// Initializes a new instance of WeeklyEvent for a collection of WeeklySchedule.
        /// The event will trigger every week following the given schedule.
        /// </summary>
        /// <param name="schedule">A collection of WeeklyEvent containing the schedule (dayOfWeeks / times) of this event</param>
        public WeeklyEvent(ICollection<WeeklySchedule> schedule) : this(schedule, DateTime.Now) { }
        
        /// <summary>
        /// Initializes a new instance of WeeklyEvent for a collection of WeeklySchedule
        /// providing an action delegate to invoke on trigger.
        /// The event will trigger every week following the given schedule.
        /// </summary>
        /// <param name="schedule">A collection of WeeklyEvent containing the schedule (dayOfWeeks / times) of this event</param>
        /// <param name="action">An action to invoke on trigger</param>
        public WeeklyEvent(ICollection<WeeklySchedule> schedule, Action action): this(schedule, DateTime.Now, action) { }
        
        /// <summary>
        /// Initializes a new instance of WeeklyEvent for a collection of WeeklySchedule, starting at a given date.
        /// The event will trigger every week following the given schedule.
        /// </summary>
        /// <param name="schedule">A collection of WeeklyEvent containing the schedule (dayOfWeeks / times) of this event</param>
        /// <param name="startDate">Date on which the recurrence begins</param>
        public WeeklyEvent(ICollection<WeeklySchedule> schedule, DateTime startDate): this(schedule, startDate, null){}

        /// <summary>
        /// Initializes a new instance of WeeklyEvent for a collection of WeeklySchedule, starting at a given date,
        /// providing an action delegate to invoke on trigger.
        /// The event will trigger every week following the given schedule.
        /// </summary>
        /// <param name="schedule">A collection of WeeklyEvent containing the schedule (dayOfWeeks / times) of this event</param>
        /// <param name="startDate">Date on which the recurrence begins</param>
        /// <param name="action">An action to invoke on trigger</param>
        public WeeklyEvent(ICollection<WeeklySchedule> schedule, DateTime startDate, Action action)
        {
            if (schedule.Count == 0)
                throw new ArgumentException("A WeeklyEvent must contain at least one WeeklySchedule");
            
            _scheduleQueue = new Queue<WeeklySchedule>(schedule.OrderBy(s=>s));
            ScheduledTime = GetNextScheduleDate(startDate);

            if (action != null)
            {
                _action = action;
            }
        }
        #endregion
        
        public override void Trigger(IEventScheduler scheduler)
        {
            base.Trigger(scheduler);
            ScheduledTime = GetNextScheduleDate(null);
            scheduler.Schedule(this);
        }

        /// <summary>
        /// Return the date of the next occurence to schedule
        /// </summary>
        /// <param name="startDate">Date on which the recurrence begins</param>
        /// <returns></returns>
        private DateTime GetNextScheduleDate(DateTime? startDate)
        {
            if (startDate < DateTime.Now) startDate = DateTime.Now;

            WeeklySchedule schedule = GetNextEventInQueue(startDate);
            DateTime scheduleDate = startDate?.Date ?? DateTime.Now;
            
            while(scheduleDate.DayOfWeek != schedule.DayOfWeek)
                scheduleDate = scheduleDate.AddDays(1);

            TimeSpan time = schedule.TimeOfDay;
            scheduleDate = new DateTime(scheduleDate.Year,scheduleDate.Month, scheduleDate.Day, time.Hours, time.Minutes, time.Seconds);

            if (scheduleDate < DateTime.Now)
            {
                scheduleDate = scheduleDate.AddDays(7);
            }

            return scheduleDate;
        }

        /// <summary>
        /// Get the event that should be triggered next in the queue
        /// based on the curent DateTime or startDate
        /// </summary>
        /// <param name="startDate">Date on which the recurrence begins</param>
        /// <returns></returns>
        private WeeklySchedule GetNextEventInQueue(DateTime? startDate)
        {
            if (_scheduleQueue.Count == 1)
            {
                return _scheduleQueue.Peek();
            }
            
            DateTime baseDate = startDate ?? DateTime.Now;
            DayOfWeek today = baseDate.DayOfWeek;
            TimeSpan now = new TimeSpan(0,baseDate.Hour, baseDate.Minute, baseDate.Second, baseDate.Millisecond);

            int i = 0;
            while (i < _scheduleQueue.Count)
            {
                WeeklySchedule schedule = _scheduleQueue.Peek();
                if (schedule.DayOfWeek < today || (schedule.DayOfWeek == today && schedule.TimeOfDay < now))
                {
                    //Push first element at end
                    _scheduleQueue.Enqueue(_scheduleQueue.Dequeue());
                    i++;
                }
                else
                {
                    return schedule;
                }
            }

            return _scheduleQueue.Peek();
        }
    }

    /// <summary>
    /// Represents a moment at which to trigger a WeeklyEvent.
    /// </summary>
    public class WeeklySchedule : IComparable<WeeklySchedule>
    {
        /// <summary>
        /// The event trigger day 
        /// </summary>
        public DayOfWeek DayOfWeek { get; }
        
        /// <summary>
        /// The event trigger time 
        /// </summary>
        public TimeSpan TimeOfDay { get; }

        /// <summary>
        /// Create a new instance of WeeklySchedule
        /// </summary>
        /// <param name="dayOfWeek">Trigger day</param>
        /// <param name="timeOfDay">Trigger time</param>
        /// <exception cref="ArgumentException">If timeOfDay is not a valid 24h time</exception>
        public WeeklySchedule(DayOfWeek dayOfWeek, TimeSpan timeOfDay)
        {
            if (timeOfDay.Hours > 23 || timeOfDay.Minutes > 59 || timeOfDay.Seconds > 59)
                throw new ArgumentException("timeOfDay is not a valid hour");
            
            DayOfWeek = dayOfWeek;
            TimeOfDay = timeOfDay;
        }

        public int CompareTo(WeeklySchedule other)
        {
            if (DayOfWeek == other.DayOfWeek)
            {
                return TimeOfDay.CompareTo(other.TimeOfDay);
            }

            return DayOfWeek.CompareTo(other.DayOfWeek);
        }
    }
}