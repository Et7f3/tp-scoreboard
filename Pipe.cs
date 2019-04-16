using System;
using System.ComponentModel;
using System.Xml;

namespace Flappy
{
    public class Pipe
    {
        /// <summary>
        /// The x position of the pipe
        /// </summary>
        private long x;

        /// <summary>
        /// The height of the top pipe
        /// </summary>
        private int topPipeHeight;

        /// <summary>
        /// The height of the free part, which is the part between the pipes
        /// </summary>
        private int freeHeight;

        /// <summary>
        /// The height of the bottom pipe
        /// </summary>
        private int bottomPipeHeight;

        /// <summary>
        /// Returns the x position of the pipe
        /// </summary>
        public long X
        {
            get
            {
                return this.x;
            }
        }

        /// <summary>
        /// Returns the height of the top pipe
        /// </summary>
        public int TopPipeHeight
        {
            get
            {
                return this.topPipeHeight;
            }
        }

        /// <summary>
        /// Returns the free height
        /// </summary>
        public int FreeHeight
        {
            get
            {
                return this.freeHeight;
            }
        }

        /// <summary>
        /// Returns the height of the bottom pipe
        /// </summary>
        public int BottomPipeHeight
        {
            get
            {
                return this.bottomPipeHeight;
            }
        }

        /// <summary>
        /// Initialize a pipe
        /// </summary>
        /// <param name="x">The x position</param>
        /// <param name="topPipeHeight">The height of the top pipe</param>
        /// <param name="freeHeight">The free height</param>
        /// <param name="bottomPipeHeight">The height of the bottom pipe</param>
        public Pipe(long x, int topPipeHeight, int freeHeight, int bottomPipeHeight)
        {
            this.x = x;
            this.topPipeHeight = topPipeHeight;
            this.freeHeight = freeHeight;
            this.bottomPipeHeight = bottomPipeHeight;
        }

        /// <summary>
        /// Check whether a position is within a pipe
        /// </summary>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        /// <returns>True if (x, y) is inside the pipe</returns>
        public bool Collides(long x, int y)
        {
            if (this.x <= x && x < this.x + 3)
            {
                if (y < this.TopPipeHeight)
                {
                    return true;
                }
                else if (y >= this.TopPipeHeight + this.FreeHeight)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Make a deep copy of the pipe
        /// </summary>
        /// <returns>A new pipe</returns>
        public Pipe DeepCopy()
        {
            return new Pipe(this.X, this.TopPipeHeight, this.FreeHeight, this.BottomPipeHeight);
        }
    }
}