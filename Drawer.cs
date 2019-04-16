namespace Flappy
{
    public abstract class Drawer
    {
        /// <summary>
        /// Get the height of the output
        /// </summary>
        public abstract int Height { get; }

        /// <summary>
        /// Get the width of the output
        /// </summary>
        public abstract int Width { get; }

        /// <summary>
        /// Draw a bird
        /// </summary>
        /// <param name="bird">The bird to draw</param>
        public abstract void Draw(Bird bird);

        /// <summary>
        /// Draw a pipe
        /// </summary>
        /// <param name="pipe">The pipe to draw</param>
        /// <param name="x">The current x coordinate</param>
        public abstract void Draw(Pipe pipe, long x);

        /// <summary>
        /// Draw the score
        /// </summary>
        /// <param name="score">The score</param>
        public abstract void Draw(long score);

        /// <summary>
        /// Clear the output
        /// </summary>
        public abstract void Clear();
    }
}