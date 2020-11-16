using System;
using System.Diagnostics;
using EventScheduler.Collections;
using EventScheduler.Events;

namespace EventScheduler
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Started");
            
            var schedulerService = new EventSchedulerService();
            
            IScheduledEvent @event = new ScheduledEvent(DateTime.Now.AddSeconds(5));
            @event.EventTriggered += (sender, eventArgs) => Console.WriteLine("Triggered 1");
            schedulerService.Schedule(@event);
            
            @event = new ScheduledEvent(DateTime.Now.AddSeconds(10), () => Console.WriteLine("Triggered in action"));
            schedulerService.Schedule(@event);
            
            @event = new ScheduledEvent(DateTime.Now.AddSeconds(10));
            @event.EventTriggered += (sender, eventArgs) => Console.WriteLine("Triggered 2");
            schedulerService.Schedule(@event);
            // schedulerService.CancelEvent(@event);
            
            @event = new PeriodicEvent(new TimeSpan(0,0,20), DateTime.Now.AddSeconds(10));
            @event.EventTriggered += (sender, eventArgs) => Console.WriteLine("Triggered Periodic");
            schedulerService.Schedule(@event);
            
            @event = new WeeklyEvent(DayOfWeek.Sunday, new TimeSpan(23,0,0), new DateTime(2020,11,19));
            @event.EventTriggered += (sender, eventArgs) =>
            {
                Console.WriteLine("Triggered RecurringEvent");
            };
            schedulerService.Schedule(@event);
            
            schedulerService.Schedule(new DailyEvent(new TimeSpan(23, 02, 0),() =>
            {
                Console.WriteLine("Triggered DailyEvent");
            }));
            
            schedulerService.Schedule(new YearlyEvent(new DateTime(2020,11,16, 10, 51,0),() =>
            {
                Console.WriteLine("YearlyEvent triggered");
            }));
            
            schedulerService.Schedule(new MonthlyEvent(new DateTime(2020,11,16, 10, 12,0),() =>
            {
                Console.WriteLine("MonthlyEvent triggered");
            }));
            
            // WeeklySchedule[] schedules = new[]
            // {
            //     new WeeklySchedule(DayOfWeek.Saturday, new TimeSpan(19, 00, 0)),
            //     new WeeklySchedule(DayOfWeek.Saturday, new TimeSpan(2, 00, 0)),
            //     new WeeklySchedule(DayOfWeek.Monday, new TimeSpan(15, 30, 0)),
            //     new WeeklySchedule(DayOfWeek.Monday, new TimeSpan(0, 53, 10)),
            //     new WeeklySchedule(DayOfWeek.Friday, new TimeSpan(8, 00, 0)),
            //     new WeeklySchedule(DayOfWeek.Wednesday, new TimeSpan(16, 30, 0)),
            // };
            //
            // @event = new WeeklyEvent(schedules, (() => Console.Write("tr")));
            // schedulerService.Schedule(@event);
            
            Console.ReadLine();
        }
    }
}