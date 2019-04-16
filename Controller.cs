using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flappy
{
    public interface Controller
    {
        /// <summary>
        /// Returns true if the bird should jump, false otherwise
        /// </summary>
        /// <param name="bird">The bird</param>
        /// <param name="drawer">The drawer</param>
        /// <param name="x">The x position</param>
        /// <param name="pipes">The list of pipes</param>
        /// <returns>True if the bird should jump, false otherwise</returns>
        bool ShouldJump(Bird bird, Drawer drawer, long x, Deque<Pipe> pipes);
    }
}
