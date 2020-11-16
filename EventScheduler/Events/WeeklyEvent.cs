using System;
using System.Collections.Generic;
using System.Linq;
using EventScheduler.Collections;

namespace EventScheduler.Events
{
    public class WeeklyEvent : ScheduledEventBase
    {
        private readonly Queue<WeeklySchedule> _scheduleQueue;
        public override DateTime ScheduledTime { get; protected set; }

        #region Constructors

        public WeeklyEvent(DayOfWeek dayOfWeek, TimeSpan timeOfDay) : 
            this(new WeeklySchedule(dayOfWeek, timeOfDay)){}
        
        public WeeklyEvent(DayOfWeek dayOfWeek, TimeSpan timeOfDay, DateTime startDate) : 
            this(new WeeklySchedule(dayOfWeek, timeOfDay), startDate){}
        
        public WeeklyEvent(DayOfWeek dayOfWeek, TimeSpan timeOfDay, DateTime startDate, Action action) : 
            this(new WeeklySchedule(dayOfWeek, timeOfDay), startDate, action){}
        
        public WeeklyEvent(DayOfWeek dayOfWeek, TimeSpan timeOfDay, Action action) : 
            this(new WeeklySchedule(dayOfWeek, timeOfDay), action){}

        public WeeklyEvent(WeeklySchedule schedule) : this(schedule, DateTime.Now) {}
        
        public WeeklyEvent(WeeklySchedule schedule, Action action): this(schedule, DateTime.Now, action) {}
        
        public WeeklyEvent(WeeklySchedule schedule, DateTime startDate)
        {
            _scheduleQueue = new Queue<WeeklySchedule>();
            _scheduleQueue.Enqueue(schedule);
            
            ScheduledTime = GetNextScheduleDate(startDate);
        }
        
        public WeeklyEvent(WeeklySchedule schedule, DateTime startDate, Action action) : this(schedule, startDate)
        {
            _action = action;
        }

        public WeeklyEvent(ICollection<WeeklySchedule> schedule) : this(schedule, DateTime.Now) { }
        
        public WeeklyEvent(ICollection<WeeklySchedule> schedule, Action action): this(schedule, DateTime.Now, action) { }
        
        public WeeklyEvent(ICollection<WeeklySchedule> schedule, DateTime startDate) 
        {
            if (schedule.Count == 0)
                throw new ArgumentException("A WeeklyEvent must contain at least one WeeklySchedule");
            
            _scheduleQueue = new Queue<WeeklySchedule>(schedule.OrderBy(s=>s));
            ScheduledTime = GetNextScheduleDate(startDate);
        }
        
        public WeeklyEvent(ICollection<WeeklySchedule> schedule, DateTime startDate, Action action) : this(schedule, startDate)
        {
            _action = action;
        }
        #endregion
        
        public override void Trigger(IEventScheduler scheduler)
        {
            base.Trigger(scheduler);
            ScheduledTime = GetNextScheduleDate(null);
            scheduler.Schedule(this);
        }

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

    public class WeeklySchedule : IComparable<WeeklySchedule>
    {
        public DayOfWeek DayOfWeek { get; }
        public TimeSpan TimeOfDay { get; }

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