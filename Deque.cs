using System;
using System.Collections;

namespace Flappy
{
    public class Deque<T> : IEnumerable
    {
        public class Node
        {
            /// <summary>
            /// A weak reference to the previous
            /// Weak references are used to facilitate the garbage collection
            ///   by preventing circular references
            /// </summary>
            public WeakReference<Node> Previous;

            /// <summary>
            /// The next
            /// </summary>
            public Node Next;

            /// <summary>
            /// The data
            /// </summary>
            public T Element;
        }

        /// <summary>
        /// The head
        /// </summary>
        private Node head;

        /// <summary>
        /// A weak reference to the tail
        /// Weak reference is used to facilitate the garbage collection
        ///   bt preventing circular references
        /// </summary>
        private WeakReference<Node> tail;

        /// <summary>
        /// The number of elements
        /// </summary>
        private int count;
        
        /// <summary>
        /// Initialize a deck
        /// </summary>
        public Deque()
        {
            this.head = null;
            this.tail = null;
            this.count = 0;
        }

        /// <summary>
        /// Get the number of elements
        /// </summary>
        public int Count
        {
            get
            {
                return this.count;
            }
        }

        /// <summary>
        /// Get the element at the front of the list
        /// </summary>
        /// <returns>The element at the front</returns>
        public T PeekFront()
        {
            if (this.head == null)
            {
                throw new ArgumentOutOfRangeException();
            }

            return this.head.Element;
        }

        /// <summary>
        /// Get the element at the back of the list
        /// </summary>
        /// <returns>The element at the back</returns>
        public T PeekBack()
        {
            if (this.tail == null)
            {
                throw new ArgumentOutOfRangeException();
            }

            Node ret;
            this.tail.TryGetTarget(out ret);
            return ret.Element;
        }

        /// <summary>
        /// Remove the element at the front
        /// </summary>
        public void PopFront()
        {
            if (this.head == null)
            {
                throw new ArgumentOutOfRangeException();
            }

            this.head = this.head.Next;
            if (this.head == null)
            {
                this.tail = null;
            }
            else
            {
                this.head.Previous = null;
            }
            --this.count;
        }

        /// <summary>
        /// Remove the element at the back
        /// </summary>
        public void PopBack()
        {
            if (this.tail == null)
            {
                throw new ArgumentOutOfRangeException();
            }

            Node oldTail;
            this.tail.TryGetTarget(out oldTail);
            this.tail = oldTail.Previous;
            if (this.tail == null)
            {
                this.head = null;
            }
            else
            {
                Node newTail;
                this.tail.TryGetTarget(out newTail);
                newTail.Next = null;
            }
            --this.count;
        }

        /// <summary>
        /// Add a new element to the front
        /// </summary>
        /// <param name="element">The element to add</param>
        public void PushFront(T element)
        {
            Node newNode = new Node();
            newNode.Element = element;
            newNode.Previous = null;
            newNode.Next = this.head;
            if (this.head != null)
            {
                
                this.head.Previous = new WeakReference<Node>(newNode);
            }
            else
            {
                this.tail = new WeakReference<Node>(newNode);
            }
            this.head = newNode;
            ++this.count;
        }

        /// <summary>
        /// Add a new element to the back
        /// </summary>
        /// <param name="element">The element to add</param>
        public void PushBack(T element)
        {
            Node newNode = new Node();
            newNode.Element = element;
            newNode.Next = null;
            newNode.Previous = this.tail;
            if (this.tail != null)
            {
                Node oldTail;
                this.tail.TryGetTarget(out oldTail);
                oldTail.Next = newNode;
            }
            else
            {
                this.head = newNode;
            }
            this.tail = new WeakReference<Node>(newNode);
            ++this.count;
        }

        /// <summary>
        /// Get an enumerator to use foreach on the list
        /// </summary>
        /// <returns>An enumerator on all the elements</returns>
        public IEnumerator GetEnumerator()
        {
            for (Node current = this.head; current != null; current = current.Next)
            {
                yield return current.Element;
            }
        }

        /// <summary>
        /// Make a deepcopy of the list with the function given in parameter
        /// </summary>
        /// <param name="copy">The copy function</param>
        /// <returns>A new list</returns>
        public Deque<T> DeepCopy(Func<T, T> copy)
        {
            Deque<T> res = new Deque<T>();
            foreach (T el in this)
                res.PushBack(copy(el));
            return res;
        }

        /// <summary>
        /// Print the whole list nicely
        /// </summary>
        public void Print()
        {
            Console.Write("{0}: ", this.Count);
            Node current = this.head;
            if (current != null)
            {
                Console.Write("[{0}, {1}]: {2}", this.PeekFront().ToString(), this.PeekBack().ToString(),
                    current.Element.ToString());
                for (current = current.Next; current != null; current = current.Next)
                {
                    Console.Write(", {0}", current.Element.ToString());
                }
            }
            Console.WriteLine();
        }
    }
}