using System;
using System.Timers;
using EventScheduler.Events;
using EventScheduler.Queue;

namespace EventScheduler.Service
{
    /// <summary>
    /// EventSchedulerService is the main implementation of IEventScheduler.
    /// It register and trigger the scheduled events.
    /// </summary>
    public class EventSchedulerService : IEventScheduler
    {
        private readonly PriorityQueue<IScheduledEvent> _eventQueue;

        /// <summary>
        /// Initializes a new instance of EventSchedulerService with a
        /// default update interval of 1 second.
        /// </summary>
        public EventSchedulerService() : this(1000) { }

        /// <summary>
        /// Initializes a new instance of EventSchedulerService with a custom update interval.
        /// (The update interval, is the amount of time between each check of the event queue
        /// to see if a new event should be triggered.) 
        /// </summary>
        /// <param name="updateInterval">custom update interval (in ms)</param>
        public EventSchedulerService(int updateInterval)
        {
            _eventQueue = new PriorityQueue<IScheduledEvent>();

            // Init timer
            var timer = new Timer(updateInterval);
            timer.Elapsed += OnTimerElapsed;
            timer.AutoReset = true;
            timer.Start();
        }

        /// <summary>
        /// Schedule a new event by adding it to the event queue.
        /// </summary>
        /// <param name="evt">The event to schedule</param>
        /// <exception cref="InvalidOperationException">thrown if evt is in the past</exception>
        public void Schedule(IScheduledEvent evt)
        {
            if (evt.ScheduledTime <= DateTime.Now)
                throw new InvalidOperationException("A passed event cannot be scheduled.");
            _eventQueue.Enqueue(evt);
            
            Console.WriteLine($"{DateTime.Now} : Scheduled new {evt.GetType().Name} @ {evt.ScheduledTime}");
        }
        
        /// <summary>
        /// Cancel a previously scheduled event.
        /// </summary>
        /// <param name="evt">The event to cancel</param>
        /// <returns>Returns true if the given event was successfully removed form the event queue</returns>
        public bool CancelEvent(IScheduledEvent evt)
        {
            if (_eventQueue.Remove(evt))
            {
                Console.WriteLine($"{DateTime.Now} : Cancelled {evt.GetType().Name} previously scheduled @ {evt.ScheduledTime}");
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// Called when update interval is elapsed.
        /// Check the queue to trigger the next event if required.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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