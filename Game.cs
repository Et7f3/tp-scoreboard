using System;
using System.Collections.Generic;
using System.Threading;

namespace Flappy
{
    public class Game
    {
        /// <summary>
        /// The random number generator
        /// </summary>
        private Random rnd;

        /// <summary>
        /// The x coordinate in the game
        /// </summary>
        public long x;

        /// <summary>
        /// The list of all the birds
        /// </summary>
        private List<Bird> birds;

        /// <summary>
        /// The list of all the managers
        /// </summary>
        private List<Manager> managers;

        /// <summary>
        /// The list of all the pipes
        /// </summary>
        private Deque<Pipe> pipes;

        /// <summary>
        /// The output drawer
        /// </summary>
        private Drawer drawer;

        /// <summary>
        /// The current score
        /// </summary>
        private long score;

        /// <summary>
        /// The time to sleep between two frames (will only impact human players)
        /// </summary>
        private int sleep;

        /// <summary>
        /// The tick to make the game harder
        /// </summary>
        private int tick;

        /// <summary>
        /// The maximum height delta between the center of two following pipes
        /// </summary>
        private int bound;

        /// <summary>
        /// The length between two following pipes
        /// </summary>
        private int step;

        /// <summary>
        /// The height of the free space
        /// </summary>
        private int free;

        /// <summary>
        /// Is true if there is one player alive
        /// </summary>
        public bool Continue
        {
            get
            {
                foreach (Bird bird in this.birds)
                {
                    if (!bird.Dead)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Initialize a game
        /// </summary>
        /// <param name="rnd">The random number generator</param>
        /// <param name="drawer">The output drawer</param>
        public Game(Random rnd, Drawer drawer)
        {
            // Initialize all the fields
            this.rnd = rnd;
            this.x = 0;
            this.birds = new List<Bird>();
            this.pipes = new Deque<Pipe>();
            this.managers = new List<Manager>();
            this.score = 0;
            this.drawer = drawer;
            this.sleep = 150;
            this.step = 20;
            this.tick = 0;
            this.free = this.drawer.Height - (this.drawer.Height / 3) * 2;
            this.bound = 10;
            // Generate the first pipes
            long pos = this.step;
            while (pos < this.drawer.Width)
            {
                this.GeneratePipe(pos);
                pos += this.step;
            }
            this.GeneratePipe(pos);
        }
        
        /// <summary>
        /// Add a new bird if it has not already been added
        /// </summary>
        /// <param name="bird">The bird to add</param>
        public void Add(Bird bird)
        {
            if (!this.birds.Contains(bird))
            {
                bird.Y = this.drawer.Height / 2;
                this.birds.Add(bird);
            }
        }

        /// <summary>
        /// Add a new manager if it has not already been aded
        /// </summary>
        /// <param name="manager">The manager to add</param>
        public void Add(Manager manager)
        {
            if (!this.managers.Contains(manager))
            {
                this.managers.Add(manager);
            }
        }

        /// <summary>
        /// Start all the managers
        /// </summary>
        public void Start()
        {
            foreach (Manager manager in this.managers)
            {
                manager.Start();
            }
        }

        /// <summary>
        /// Stop all the managers
        /// </summary>
        public void Stop()
        {
            foreach (Manager manager in this.managers)
            {
                manager.Stop();
            }
        }

        /// <summary>
        /// Generate a pipe
        /// </summary>
        /// <param name="pos">The x coordinate of the pipe</param>
        /// <returns>The generated pipe</returns>
        private Pipe GeneratePipe(long pos)
        {
            Pipe previous;
            if (this.pipes.Count == 0)
                previous = new Pipe(0, (this.drawer.Height - this.free) / 2, this.free,
                    this.drawer.Height - this.free - (this.drawer.Height - this.free) / 2);
            else
                previous = this.pipes.PeekBack();
            int move = this.rnd.Next(-this.bound, this.bound + 1);

            int free = this.free;
            int top = previous.TopPipeHeight + move * this.drawer.Height / 100;
            if (top + free > this.drawer.Height)
                top = this.drawer.Height - free;
            if (top < 0)
                top = 0;
            int bottom = this.drawer.Height - top - free;
            Pipe pipe = new Pipe(pos, top, free, bottom);
            this.pipes.PushBack(pipe);
            return pipe;
        }

        /// <summary>
        /// Update the game
        /// </summary>
        public void Update()
        {
            try
            {
                // Should we make the game harder ?
                if (this.rnd.Next(Math.Max(100 - this.tick, 0)) == 0)
                {
                    // What should we make harder ?
                    switch (this.rnd.Next(3))
                    {
                        // Decrease the free space
                        case 0:
                            this.free -= 1;
                            if (this.free < this.drawer.Height / 6)
                                this.free = this.drawer.Height / 6;
                            break;
                        // Decrease the step
                        case 1:
                            this.step -= 1;
                            if (this.step < 10)
                                this.step = 10;
                            break;
                        // Decrease the delta
                        case 2:
                            this.bound += 1;
                            if (this.bound >= 40)
                                this.bound = 40;
                            break;
                    }
                    this.tick = 0;
                }
                else
                    this.tick += 1;
                // Add pipes while there is enough space to add one
                while (this.pipes.PeekBack().X + this.step <= this.x + this.drawer.Width)
                {
                    this.GeneratePipe(this.pipes.PeekBack().X + this.step);
                }
                // Remove pipes that are gone
                while (this.pipes.PeekFront().X < this.x - 2)
                {
                    this.pipes.PopFront();
                    // Increase the score
                    this.score += 1;
                    // Decrease the sleep
                    this.sleep -= 1;
                }
                // Take the first pipe
                Pipe first = this.pipes.PeekFront();
                foreach (Bird bird in this.birds)
                {
                    // Don't check for dead birds
                    if (bird.Dead)
                        continue;
                    // Update the bird
                    bird.Update(this.drawer, this.x,
                        this.pipes.DeepCopy(pipe => pipe.DeepCopy()));
                    // Kill the bird if it collides with the first pipe
                    if (first.Collides(this.x + 1, bird.Y))
                    {
                        bird.Kill(this.score);
                    }
                }
                this.x += 1;
            }
            catch(ArgumentOutOfRangeException)
            {
            }
            if (this.sleep < 20)
                this.sleep = 20;
        }

        /// <summary>
        /// Draw the game
        /// </summary>
        public void Draw()
        {
            // Clear the output
            this.drawer.Clear();
            // Draw each alive bird
            foreach (Bird bird in this.birds)
            {
                if (bird.Dead)
                    continue;
                this.drawer.Draw(bird);
            }
            // Draw each pipe
            foreach (Pipe pipe in this.pipes)
            {
                this.drawer.Draw(pipe, x);
            }
            // Draw the score
            this.drawer.Draw(score);
        }

        /// <summary>
        /// Sleep
        /// </summary>
        public void Sleep()
        {
            Thread.Sleep(this.sleep);
        }
    }
}