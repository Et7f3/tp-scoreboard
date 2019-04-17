using System;
using System.Collections.Generic;

namespace FlappyRunner
{
    public class ConsoleDrawer: Drawer
    {
        /// <summary>
        /// The height of the console
        /// </summary>
        private int height;

        /// <summary>
        /// The width of the console
        /// </summary>
        private int width;

        /// <summary>
        /// The dictionary associating the birds and their colors
        /// </summary>
        private Dictionary<dynamic, ConsoleColor> colors;
        
        /// <summary>
        /// Returns the height of the console
        /// </summary>
        public override int Height
        {
            get
            {
                return this.height;
            }
        }

        /// <summary>
        /// Returns the width of the console
        /// </summary>
        public override int Width
        {
            get
            {
                return this.width;
            }
        }
        
        /// <summary>
        /// Initialize a new console drawer
        /// </summary>
        /// <param name="width">The width of the console</param>
        /// <param name="height">The height of the console</param>
        public ConsoleDrawer(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.colors = new Dictionary<dynamic, ConsoleColor>();
        }

        /// <summary>
        /// Associate a bird and a color
        /// </summary>
        /// <param name="bird">the bird</param>
        /// <param name="color">Its color</param>
        public void Associate(dynamic bird, ConsoleColor color)
        {
        }

        /// <summary>
        /// Draw a bird
        /// </summary>
        /// <param name="bird">The bird to draw</param>
        public override void Draw(dynamic bird)
        {
        }

        /// <summary>
        /// Draw a pipe
        /// </summary>
        /// <param name="pipe">The pipe to draw</param>
        /// <param name="x">The current x position</param>
        public override void Draw(Pipe pipe, long x)
        {
        }

        /// <summary>
        /// Draw the score
        /// </summary>
        /// <param name="score">The score</param>
        public override void Draw(long score)
        {
        }

        /// <summary>
        /// Clear the console
        /// </summary>
        public override void Clear()
        {
        }
    }
}