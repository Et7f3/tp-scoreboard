using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flappy
{
    class KeyboardController: Controller
    {
        /// <summary>
        /// The associated keyboard manager
        /// </summary>
        private KeyboardManager manager;

        /// <summary>
        /// The associated key
        /// </summary>
        private ConsoleKey key;

        /// <summary>
        /// Whether the key has been pressed
        /// </summary>
        private bool pressed;

        /// <summary>
        /// Initialize a keyboard controller
        /// </summary>
        /// <param name="manager">The keyboard manager</param>
        /// <param name="key">The key</param>
        public KeyboardController(KeyboardManager manager, ConsoleKey key)
        {
            this.key = key;
            this.pressed = false;
            this.manager = manager;
            // Register the controller to the manager
            this.manager.Register(key, this);
        }

        /// <summary>
        /// Sets pressed to true
        /// </summary>
        public void Pressed()
        {
            this.pressed = true;
        }

        /// <summary>
        /// Returns whether the key has been pressed and resets it
        /// </summary>
        /// <param name="bird">The bird</param>
        /// <param name="drawer">The drawer</param>
        /// <param name="x">The x position</param>
        /// <param name="pipes">The list of pipes</param>
        /// <returns>True if the key has been pressed</returns>
        public bool ShouldJump(Bird bird, Drawer drawer, long x, Deque<Pipe> pipes)
        {
            bool pressed = this.pressed;
            this.pressed = false;
            return pressed;
        }
    }
}
