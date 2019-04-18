using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Threading;
using System.Threading.Tasks;

namespace FlappyRunner
{
    internal unsafe class Program
    {
        public static dynamic type_bird;
        public static dynamic type_bird_orig;
        public static dynamic type_pipe;
        public static dynamic type_deque;
        public static dynamic type_console_drawer;
        
        public static unsafe void* GetObjectAddress(object obj)
        {
            return *(void**)Unsafe.AsPointer(ref obj);
        }
        
        public static unsafe void TransmuteTo(object target, object source)
        {
            var s = (void**)GetObjectAddress(source);
            var t = (void**)GetObjectAddress(target);
            *t = *s;
        }
        
        public static unsafe void TransmuteToGC(object target, object source)
        {
            var handle_target = GCHandle.Alloc(target, GCHandleType.Pinned);
            var handle_source = GCHandle.Alloc(source, GCHandleType.Pinned);
            TransmuteTo(target, source);
            /* do our magic here */
            handle_source.Free();
            handle_target.Free();
        }

        public static Assembly asm;
        public static StreamWriter sw_score;
        public static StreamWriter sw_seed;
        public static StreamWriter sw_move;
        /// <summary>
        /// The main function:
        /// - Register the managers
        /// - Register the drawers
        /// - Register the birds
        /// - Initialize the game
        /// - Play
        /// </summary>
        public static int Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.Error.WriteLine("usage firstname.lastname.exe random-seed.txt move.txt");
                return -1;
                // usage firstname.lastname.exe save.txt
            }
            //Console.WriteLine(args[0] + ".log");
            sw_score = new StreamWriter(args[0] + ".log");
            sw_seed = new StreamWriter(args[1]);
            sw_move = new StreamWriter(args[2]);

            if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine("assembly not exist");
                return -2;
                // assembly not exist
            }

            asm = Assembly.LoadFrom(args[0]);

            type_pipe = Activator.CreateInstance(asm.GetType("Flappy.Pipe"), new object[]{0, 0, 0, 0});
            type_deque = Activator.CreateInstance(asm.GetType("Flappy.Deque`1").MakeGenericType(type_pipe.GetType()), new object[]{});
            type_console_drawer = Activator.CreateInstance(asm.GetType("Flappy.ConsoleDrawer"), new object[]{Console.WindowWidth, Console.WindowHeight});

            // Get one random generator for all the game
            Random rnd = new Random();

            // Initialize the console drawer, which will handle the console output
            ConsoleDrawer drawer = new ConsoleDrawer(Console.WindowWidth, Console.WindowHeight);
            TransmuteToGC(drawer, type_console_drawer);

            // Initialize the game with the random generator and the output drawer
            Game game = new Game(rnd, drawer);

            // Create an AI
            dynamic best_controller_orig = Activator.CreateInstance(asm.GetType("Flappy.BestController"), new object[]{});
            BestController best_controller = new BestController();
            //TransmuteToGC(best_controller_orig, best_controller);
            Bird ai = new Bird(best_controller_orig);
            type_bird = Activator.CreateInstance(asm.GetType("Flappy.Bird"), new object[] {best_controller_orig});
            type_bird_orig = new Bird(new BestController());

 
            // Associate the ai with a color in the console drawer
            drawer.Associate(ai, ConsoleColor.Blue);

            // Add the ai to the game
            game.Add(ai);

            // While there is someone alive, continue or timeout
            Task.Factory.StartNew(() =>
            {
#if DEBUG
                Thread.Sleep(10 * 1000);
#else
                Thread.Sleep(4 * 40 * 1000);
#endif
                game.Continue = false;
            });

            // While there is someone alive, continue
            while (game.Continue)
            {
                // Game loop : update
                game.Update();
            }

            // Write the scores
            sw_score.WriteLine(game.x);
            // we write score to file instead of return it
            sw_score.Flush();
            sw_score.Close();

            sw_seed.Flush();
            sw_seed.Close();

            sw_move.Flush();
            sw_move.Close();

            return 0;// success
        }
    }
}