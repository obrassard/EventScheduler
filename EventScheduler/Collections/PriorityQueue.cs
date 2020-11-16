using System;
using System.Collections.Generic;

namespace EventScheduler.Collections
{
    /// <summary>
    /// Heap priority queue implementation
    /// </summary>
    /// <typeparam name="T">The type of object stored in this queue</typeparam>
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private List<T> _data;

        /// <summary>
        /// Initialize an new instance of the PriorityQueue class.
        /// </summary>
        public PriorityQueue()
        {
            _data = new List<T>();
        }

        /// <summary>
        /// Adds a new element in the Queue
        /// </summary>
        /// <param name="element">The element to add</param>
        public void Enqueue(T element)
        {
            _data.Add(element);
            int ci = _data.Count - 1; // child index; start at end
            while (ci > 0)
            {
                int pi = (ci - 1) / 2; // parent index
                if (_data[ci].CompareTo(_data[pi]) >= 0)
                    break; // child item is larger than (or equal) parent so we're done
                T tmp = _data[ci];
                _data[ci] = _data[pi];
                _data[pi] = tmp;
                ci = pi;
            }
        }

        /// <summary>
        /// Remove and returns the first element of the priority queue
        /// (the element with the higher priority)
        /// </summary>
        /// <returns>The first element of the priority queue</returns>
        public T Dequeue()
        {
            // assumes pq is not empty; up to calling code
            int li = _data.Count - 1; // last index (before removal)

            if (li < 0) return default;

            T frontItem = _data[0]; // fetch the front
            _data[0] = _data[li];
            _data.RemoveAt(li);

            --li; // last index (after removal)
            int pi = 0; // parent index. start at front of pq
            while (true)
            {
                int ci = pi * 2 + 1; // left child index of parent
                if (ci > li) break; // no children so done
                int rc = ci + 1; // right child
                if (rc <= li && _data[rc].CompareTo(_data[ci]) < 0
                ) // if there is a rc (ci + 1), and it is smaller than left child, use the rc instead
                    ci = rc;
                if (_data[pi].CompareTo(_data[ci]) <= 0)
                    break; // parent is smaller than (or equal to) smallest child so done
                T tmp = _data[pi];
                _data[pi] = _data[ci];
                _data[ci] = tmp; // swap parent and child
                pi = ci;
            }

            return frontItem;
        }

        /// <summary>
        /// Returns the first element of the queue (the one with the highest priority)
        /// </summary>
        /// <returns>The first element of the queue</returns>
        public T Peek()
        {
            if (_data.Count == 0)
                return default;
            
            T frontItem = _data[0];
            return frontItem;
        }

        /// <summary>
        /// Remove a specific element from the priority queue
        /// </summary>
        /// <param name="element">The element to remove</param>
        /// <returns>True if an element was removed</returns>
        public bool Remove(T element)
        {
            // assumes pq is not empty; up to calling code
            int li = _data.Count - 1; // last index (before removal)
            int iToRemove = _data.IndexOf(element); //find item to remove

            if (iToRemove == -1) return false;

            _data[iToRemove] = _data[li]; // replace element to remove by last element
            _data.RemoveAt(li);

            --li; // last index (after removal)
            int pi = 0; // parent index. start at front of pq
            while (true)
            {
                int ci = pi * 2 + 1; // left child index of parent
                if (ci > li) break; // no children so done
                int rc = ci + 1; // right child
                if (rc <= li && _data[rc].CompareTo(_data[ci]) < 0
                ) // if there is a rc (ci + 1), and it is smaller than left child, use the rc instead
                    ci = rc;
                if (_data[pi].CompareTo(_data[ci]) <= 0)
                    break; // parent is smaller than (or equal to) smallest child so done
                T tmp = _data[pi];
                _data[pi] = _data[ci];
                _data[ci] = tmp; // swap parent and child
                pi = ci;
            }

            return true;
        }

        /// <summary>
        /// Get the number of elements contained in the priority queue
        /// </summary>
        /// <returns>the number of elements</returns>
        public int Count()
        {
            return _data.Count;
        }

        /// <summary>
        /// Returns a string that represent this queue.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = "";
            foreach (T t in _data) s += t.ToString() + " ";

            s += "count = " + _data.Count;
            return s;
        }
        
        private bool IsConsistent()
        {
            // is the heap property true for all data?
            if (_data.Count == 0) return true;
            int li = _data.Count - 1; // last index
            for (int pi = 0; pi < _data.Count; ++pi) // each parent index
            {
                int lci = 2 * pi + 1; // left child index
                int rci = 2 * pi + 2; // right child index

                if (lci <= li && _data[pi].CompareTo(_data[lci]) > 0)
                    return false; // if lc exists and it's greater than parent then bad.
                if (rci <= li && _data[pi].CompareTo(_data[rci]) > 0) return false; // check the right child too.
            }

            return true; // passed all checks
        } // IsConsistent
    } // PriorityQueue
}