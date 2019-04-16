using System;
using System.Collections.Generic;

namespace Flappy
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
        private Dictionary<Bird, ConsoleColor> colors;
        
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
            this.colors = new Dictionary<Bird, ConsoleColor>();
        }

        /// <summary>
        /// Associate a bird and a color
        /// </summary>
        /// <param name="bird">the bird</param>
        /// <param name="color">Its color</param>
        public void Associate(Bird bird, ConsoleColor color)
        {
            this.colors[bird] = color;
        }

        /// <summary>
        /// Draw a bird
        /// </summary>
        /// <param name="bird">The bird to draw</param>
        public override void Draw(Bird bird)
        {
            if (bird.Dead)
            {
                return;
            }
            Console.SetCursorPosition(1, bird.Y);
            Console.ForegroundColor = this.colors[bird];
            if (bird.VerticalSpeed < 0)
            {
                Console.Write('^');
            }
            else
            {
                Console.Write('v');
            }
            Console.ResetColor();
        }

        /// <summary>
        /// Draw a pipe
        /// </summary>
        /// <param name="pipe">The pipe to draw</param>
        /// <param name="x">The current x position</param>
        public override void Draw(Pipe pipe, long x)
        {
            // Note : here we aren't using width but window width
            //   to ensure it hasn't been resized
            int bottom = pipe.TopPipeHeight + pipe.FreeHeight;
            int drawPos = (int) (pipe.X - x + 1);
            // Draw the first vertical ligne if withhin the bound of the console
            if (0 <= drawPos && drawPos < Console.WindowWidth)
            {
                if (pipe.TopPipeHeight > 0)
                {
                    Console.SetCursorPosition(drawPos, 0);
                    for (int i = 1; i < pipe.TopPipeHeight; ++i)
                    {
                        Console.Write('│');
                        Console.SetCursorPosition(drawPos, i);
                    }
                    Console.Write('└');
                }
                if (bottom < this.Height)
                {
                    Console.SetCursorPosition(drawPos, bottom);
                    Console.Write('┌');
                    for (int i = 1; i < pipe.BottomPipeHeight; ++i)
                    {
                        Console.SetCursorPosition(drawPos, bottom + i);
                        Console.Write('│');
                    }
                }
            }
            // Draw the horizontal ligne if withhin the bound of the console
            drawPos += 1;
            if (0 <= drawPos && drawPos < Console.WindowWidth)
            {
                if (pipe.TopPipeHeight > 0)
                {
                    Console.SetCursorPosition(drawPos, pipe.TopPipeHeight - 1);
                    Console.Write('─');
                }
                if (bottom < this.Height)
                {
                    Console.SetCursorPosition(drawPos, bottom);
                    Console.Write('─');
                }
            }
            // Draw the second vertical ligne if withhin the bound of the console
            drawPos += 1;
            if (0 <= drawPos && drawPos < Console.WindowWidth)
            {
                if (pipe.TopPipeHeight > 0)
                {
                    Console.SetCursorPosition(drawPos, 0);
                    for (int i = 1; i < pipe.TopPipeHeight; ++i)
                    {
                        Console.Write('│');
                        Console.SetCursorPosition(drawPos, i);
                    }
                    Console.Write('┘');
                }
                if (bottom < this.Height)
                {
                    Console.SetCursorPosition(drawPos, bottom);
                    Console.Write('┐');
                    for (int i = 1; i < pipe.BottomPipeHeight; ++i)
                    {
                        Console.SetCursorPosition(drawPos, bottom + i);
                        Console.Write('│');
                    }
                }
            }
        }

        /// <summary>
        /// Draw the score
        /// </summary>
        /// <param name="score">The score</param>
        public override void Draw(long score)
        {
            string str = "Score : " + score.ToString();
            Console.SetCursorPosition(this.Width - str.Length - 1, 0);
            Console.Write(str);
        }

        /// <summary>
        /// Clear the console
        /// </summary>
        public override void Clear()
        {
            Console.Clear();
        }
    }
}