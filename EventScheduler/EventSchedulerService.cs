using System;
using System.Timers;
using EventScheduler.Collections;
using EventScheduler.Events;

namespace EventScheduler
{
    public class EventSchedulerService : IEventScheduler
    {
        private readonly PriorityQueue<IScheduledEvent> _eventQueue;

        public EventSchedulerService() : this(1000) { }

        public EventSchedulerService(int timerInterval)
        {
            _eventQueue = new PriorityQueue<IScheduledEvent>();

            // Init timer
            var timer = new Timer(timerInterval);
            timer.Elapsed += OnTimerElapsed;
            timer.AutoReset = true;
            timer.Start();
        }

        public void Schedule(IScheduledEvent evt)
        {
            if (evt.ScheduledTime <= DateTime.Now)
                throw new InvalidOperationException("A passed event cannot be scheduled.");
            _eventQueue.Enqueue(evt);
            
            Console.WriteLine($"{DateTime.Now} : Scheduled new {evt.GetType().Name} @ {evt.ScheduledTime}");
        }
        
        public bool CancelEvent(IScheduledEvent evt)
        {
            if (_eventQueue.Remove(evt))
            {
                Console.WriteLine($"{DateTime.Now} : Cancelled {evt.GetType().Name} previously scheduled @ {evt.ScheduledTime}");
                return true;
            }
            return false;
        }
        
        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            // While there is event that should be triggered at this time
            while (_eventQueue.Peek()?.ScheduledTime <= e.SignalTime)
            {
                IScheduledEvent evt = _eventQueue.Dequeue();
                
                Console.WriteLine($"{DateTime.Now} : {evt.GetType().Name} triggered");
                evt.Trigger(this);
            }
        }
    }
}