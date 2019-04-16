using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flappy
{
    public abstract class Manager
    {
        /// <summary>
        /// Start the manager
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Stop the manager
        /// </summary>
        public abstract void Stop();
    }
}
