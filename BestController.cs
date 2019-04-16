using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Flappy
{
    class BestController: Controller
    {
        private object _con;
        private MethodInfo _mi;

        public BestController(MethodInfo mi, object con)
        {
            _con = con;
            _mi = mi;
            Console.WriteLine($"mi == null: {mi == null}, con == null: {con == null}");
        }

        public bool ShouldJump(Bird bird, Drawer drawer, long x, Deque<Pipe> pipes)
        {
            Console.WriteLine("ok doit sauter");
            Thread.Sleep(2000);
            var ret = _mi.Invoke(_con, new object[]{bird, drawer, x, pipes});

            return (bool) ret;
        }
    }
}
