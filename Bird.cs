using System;
using System.Reflection;
using System.Linq;

namespace FlappyRunner
{
    public struct s_move
    {
        private long _position;
        private bool _state;

        public s_move(long position, bool state)
        {
            _position = position;
            _state = state;
        }

        public override string ToString()
        {
            return $"{{\"state\":\"{(_state ? "up" : "down")}\",\"pos\":{_position}}}";
        }
    }

    public class Bird
    {
        /// <summary>
        /// The gravity acceleration
        /// </summary>
        private static double GRAVITY = 0.04;

        /// <summary>
        /// The jump acceleration
        /// </summary>
        private static double JUMP = -0.08;

        /// <summary>
        /// The maximal up speed
        /// </summary>
        private static double MAXIMAL_UP= 2 * JUMP;

        /// <summary>
        /// The maximal down speed
        /// </summary>
        private static double MAXIMAL_DOWN = 3 * GRAVITY;

        /// <summary>
        /// The y position
        /// </summary>
        private int y;

        /// <summary>
        /// The vertical speed
        /// </summary>
        private double verticalSpeed;

        /// <summary>
        /// Whether the bird is dead
        /// </summary>
        private bool dead;

        /// <summary>
        /// The associate controller
        /// </summary>
        private dynamic controller;
        private long score;

        /// <summary>
        /// Returns true if the bird is dead, false otherwise
        /// </summary>
        public bool Dead
        {
            get
            {
                return this.dead;
            }
        }
        
        /// <summary>
        /// Return the bird's score
        /// </summary>
        public long Score
        {
            get
            {
                return this.score;
            }
        }

        /// <summary>
        /// Return the y coordinate of the bird, and set it if it has never been set
        /// </summary>
        public int Y
        {
            get
            {
                return this.y;
            }
            set
            {
                if (this.y == -1)
                    this.y = value;
            }
        }

        /// <summary>
        /// Return the vertical speed of the bird
        /// </summary>
        public double VerticalSpeed
        {
            get
            {
                return this.verticalSpeed;
            }
        }

        /// <summary>
        /// Initialize a bird
        /// </summary>
        /// <param name="controller">The associate controller</param>
        public Bird(dynamic controller)
        {
            this.y = -1;
            this.dead = false;
            this.verticalSpeed = 0;
            this.controller = controller;
            this.score = -1;
        }

        /// <summary>
        /// Kill a bird and set its score
        /// </summary>
        /// <param name="score">The bird's score</param>
        public void Kill(long score)
        {
            this.dead = true;
            this.score = score;
        }

        /// <summary>
        /// Make a deep copy of a bird
        /// </summary>
        /// <param name="controller">The new controller</param>
        /// <returns>A new bird</returns>
        public dynamic DeepCopy(dynamic controller)
        {
            Bird res = new Bird(controller);
            res.y = this.y;
            res.verticalSpeed = this.verticalSpeed;
            return res;
        }

        /// <summary>
        /// Update a bird
        /// </summary>
        /// <param name="drawer">The drawer</param>
        /// <param name="x">The x coordinate</param>
        /// <param name="pipes">The list of pipes</param>
        public void Update(dynamic drawer, long x, dynamic pipes)
        {
            // Get the maximal y value
            int max = drawer.Height - 1;
            // Call the controller and either jump or fall accordingly
            Program.TransmuteToGC(pipes, Program.type_deque);
            //Program.TransmuteToGC(this, Program.type_bird);
            //Program.TransmuteToGC(x, (long)0);
            dynamic mutant = this.DeepCopy(this.controller);
            Program.TransmuteToGC(mutant, Program.type_bird);
            bool state = this.controller.ShouldJump(mutant, drawer, x, pipes);
            
            Game.last_moves[Game.last_move_added] = new s_move(x, state);
            Game.last_move_added = (Game.last_move_added + 1) % Program.move_count;

            if (state)
                this.verticalSpeed = JUMP;
            else
                this.verticalSpeed += GRAVITY;
            Program.TransmuteToGC(this, Program.type_bird_orig);
            // Bounds checking of the speed
            if (this.verticalSpeed < MAXIMAL_UP)
                this.verticalSpeed = MAXIMAL_UP;
            if (this.verticalSpeed > MAXIMAL_DOWN)
                this.verticalSpeed = MAXIMAL_DOWN;

            // Place the bird within the bounds of the drawer
            this.y = (int)(this.y + this.verticalSpeed * drawer.Height);
            if (y > max)
            {
                this.verticalSpeed = 0;
                y = max;
            }
            if (y < 0)
            {
                this.verticalSpeed = 0;
                y = 0;
            }
        }
    }
}