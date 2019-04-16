using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flappy
{
    public class MaximaxController : Controller
    {
        /// <summary>
        /// Returns whether the bird should jump with maximax
        /// </summary>
        /// <param name="bird">The bird</param>
        /// <param name="drawer">The drawer</param>
        /// <param name="x">The x position</param>
        /// <param name="pipes">The list of pipes</param>
        /// <returns>True if the bird should jump, false otherwise</returns>
        public bool ShouldJump(Bird bird, Drawer drawer, long x, Deque<Pipe> pipes)
        {
            return false;
        }
    }
}
