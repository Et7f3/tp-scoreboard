using System;
using System.IO;
using System.Net;
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
        public static int move_count;
        
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
        public static StreamReader sr_seed;
        public static StreamWriter sw_seed;
        public static StreamWriter sw_move;
        public static StreamWriter sw_score;

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
            if (args.Length < 4)
            {
                Console.Error.WriteLine("usage random-seed.txt move.txt move_count firstname.lastname.exe");
                return -1;
                // usage firstname.lastname.exe save.txt
            }

            //Console.WriteLine(args[0] + ".log");
            if (!File.Exists(args[0]))
                File.Create(args[0]);
            sr_seed = new StreamReader(args[0]);
            // can't open in rw mode
            // fuck cdiese
            // sw_seed = new StreamWriter(args[0], true);
            sw_move = new StreamWriter(args[1]);
            sw_score = new StreamWriter(args[3] + ".log");

            if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine("assembly not exist");
                return -2;
                // assembly not exist
            }

            asm = Assembly.LoadFrom(args[3]);

            if (!int.TryParse(args[2], out move_count))
            {
                Console.Error.WriteLine($"move_count {args[2]} non valide");
                return -4;
            }

            type_pipe = asm.GetType("Flappy.Pipe");

            if (type_pipe == null)
            {
                Console.Error.WriteLine("Flappy.Pipe not available");
                return -3;
                // file don't have right class
            }

            // since now Activator.CreateInstance
            // should always work because all value are defined

            type_pipe = Activator.CreateInstance(type_pipe, new object[]{0, 0, 0, 0});

            type_deque = asm.GetType("Flappy.Deque`1");

            if (type_deque == null)
            {
                Console.Error.WriteLine("Flappy.Deque`1 not available");
                return -3;
                // file don't have right class
            }

            type_deque = Activator.CreateInstance(type_deque.MakeGenericType(type_pipe.GetType()), new object[]{});

            type_console_drawer = asm.GetType("Flappy.ConsoleDrawer");

            if (type_console_drawer == null)
            {
                Console.Error.WriteLine("Flappy.ConsoleDrawer not available");
                return -3;
                // file don't have right class
            }

            type_console_drawer = Activator.CreateInstance(type_console_drawer, new object[]{Console.WindowWidth, Console.WindowHeight});

            // Get one random generator for all the game
            Random rnd = new FlappyRunner.Random();

            // Initialize the console drawer, which will handle the console output
            ConsoleDrawer drawer = new ConsoleDrawer(Console.WindowWidth, Console.WindowHeight);
            TransmuteToGC(drawer, type_console_drawer);

            // Initialize the game with the random generator and the output drawer
            Game game = new Game(rnd, drawer);

            dynamic best_controller_orig = asm.GetType("Flappy.BestController");

            if (best_controller_orig == null)
            {
                Console.Error.WriteLine("Flappy.ConsoleDrawer not available");
                return -3;
                // file don't have right class
            }

            // Create an AI
            best_controller_orig = Activator.CreateInstance(best_controller_orig, new object[]{});
            BestController best_controller = new BestController();
            //TransmuteToGC(best_controller_orig, best_controller);
            Bird ai = new Bird(best_controller_orig);

            type_bird = asm.GetType("Flappy.Bird");

            if (type_bird == null)
            {
                Console.Error.WriteLine("Flappy.Bird not available");
                return -3;
                // file don't have right class
            }
            type_bird = Activator.CreateInstance(type_bird, new object[] {best_controller_orig});
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

            sr_seed.Close();

            // hopefully we don't want to read will writting
            sw_seed = new StreamWriter(args[0], true);

            Random.seeds.ForEach(sw_seed.WriteLine);

            sw_seed.Flush();
            sw_seed.Close();

            sw_move.Flush();
            sw_move.Close();

            // Write the scores
            sw_score.WriteLine(game.x);
            // we write score to file instead of return it
            sw_score.Flush();
            sw_score.Close();

            return 0;// success
        }
    }
}