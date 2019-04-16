using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Flappy
{
    class KeyboardManager: Manager
    {
        /// <summary>
        /// Associate the keys and a list of keyboard controllers
        /// </summary>
        private Dictionary<ConsoleKey, List<KeyboardController>> keyboards;

        /// <summary>
        /// The input thread
        /// </summary>
        Thread thread;

        /// <summary>
        /// Whether the thread is running
        /// </summary>
        bool run;

        /// <summary>
        /// Initialize a keyboard manager
        /// </summary>
        public KeyboardManager()
        {
            this.keyboards = new Dictionary<ConsoleKey, List<KeyboardController>>();
            this.thread = null;
            this.run = false;
        }

        /// <summary>
        /// Register a new key / controller association
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="controller">The keyboard controller</param>
        public void Register(ConsoleKey key, KeyboardController controller)
        {
            if (!this.keyboards.ContainsKey(key))
                this.keyboards[key] = new List<KeyboardController>();
            if (!this.keyboards[key].Contains(controller))
                this.keyboards[key].Add(controller);
        }

        /// <summary>
        /// Start the thread
        /// </summary>
        public override void Start()
        {
            if (this.run)
            {
                return;
            }

            this.run = true;
            this.thread = new Thread(Read);
            this.thread.Start();
        }

        /// <summary>
        /// The thread loop
        /// </summary>
        public void Read()
        {
            while (this.run)
            {
                ConsoleKey read = Console.ReadKey(true).Key;
                if (this.keyboards.ContainsKey(read))
                {
                    foreach (KeyboardController controller in this.keyboards[read])
                        controller.Pressed();
                }
            }
        }

        /// <summary>
        /// Stop the thread
        /// </summary>
        public override void Stop()
        {
            this.run = false;
        }
    }
}
