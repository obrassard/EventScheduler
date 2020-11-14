using System;
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
            
            @event = new PeriodicEvent(new TimeSpan(0,0,15));
            @event.EventTriggered += (sender, eventArgs) => Console.WriteLine("Triggered Periodic");
            schedulerService.Schedule(@event);
            
            @event = new RecurringEvent(DayOfWeek.Saturday, new TimeSpan(15,35,30));
            @event.EventTriggered += (sender, eventArgs) =>
            {
                Console.WriteLine("Triggered RecurringEvent");
            };
            schedulerService.Schedule(@event);
            
            Console.ReadLine();
        }
    }
}