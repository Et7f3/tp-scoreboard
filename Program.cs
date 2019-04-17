using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace FlappyRunner
{
    internal class Program
    {
        public static Assembly asm;
        public static Type type_bird;
        public static Type type_controller;
        //public static Type type_drawer;
        public static Type type_console_drawer;
        public static Type type_pipe;
        public static Type type_deque;
        public static StreamWriter sw;

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
            if (args.Length < 1)
            {
                Console.Error.WriteLine("usage firstname.lastname.exe");
                return -1;
                // usage firstname.lastname.exe save.txt
            }
            //Console.WriteLine(args[0] + ".log");
            sw = new StreamWriter(args[0] + ".log");

            if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine("assembly not exist");
                return -2;
                // assembly not exist
            }

            asm = Assembly.LoadFrom(args[0]);
            //foreach (Type type in asm.GetTypes())
                //Console.WriteLine(type);
            type_bird = asm.GetType("Flappy.Bird");
            type_controller = asm.GetType("Flappy.BestController");
            //type_drawer = asm.GetType("Flappy.Drawer");
            type_console_drawer = asm.GetType("Flappy.ConsoleDrawer");
            type_pipe = asm.GetType("Flappy.Pipe");
            type_deque = asm.GetType("Flappy.Deque`1").MakeGenericType(type_pipe);

            if (type_bird == null)
            {
                Console.Error.WriteLine("Flappy.Bird not available");
                return -3;
                // file don't have right class
            }

            if (type_controller == null)
            {
                Console.Error.WriteLine("Flappy.BestController not available");
                return -3;
                // file don't have right class
            }

            if (type_console_drawer == null)
            {
                Console.Error.WriteLine("Flappy.ConsoleDrawer not available");
                return -3;
                // file don't have right class
            }

            if (type_pipe == null)
            {
                Console.Error.WriteLine("Flappy.Pipe not available");
                return -3;
                // file don't have right class
            }

            if (type_deque == null)
            {
                Console.Error.WriteLine("Flappy.Deque`1 not available");
                return -3;
                // file don't have right class
            }

            // Get one random generator for all the game
            Random rnd = new Random();

            // Initialize the console drawer, which will handle the console output
            dynamic drawer = Activator.CreateInstance(type_console_drawer, new object[]{Console.WindowWidth, Console.WindowHeight});

            // Initialize the game with the random generator and the output drawer
            Game game = new Game(rnd, drawer);

            // Create an AI
            //Console.WriteLine(type_controller.BaseType);

            //MethodInfo mi = my_controller.GetMethod("ShouldJump", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod);

            //MethodInfo mi2 = my_controller.GetTypeInfo().GetDeclaredMethod("ShouldJump");

            //MethodInfo mi3 = my_controller.GetMethod("ShouldJump");

            //MemberInfo mi4 = my_controller.GetMember("ShouldJump")[0];

            //Console.WriteLine($"mi == null: {mi == null}, mi2 == null: {mi2 == null}, mi3 == null: {mi3 == null}, mi4 == null: {mi4 == null}");

            //Console.WriteLine($"my_controller.GetMethod(\"ShouldJump\") == null: " + mi3.GetType());

            //BestController controller = new Flappy.BestController(mi3, Activator.CreateInstance(my_controller));
            //foreach (ConstructorInfo constructorInfo in my_bird.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic))
            //{
            //    Console.WriteLine(constructorInfo.Name);
            //}
            //Bird ai = new Bird(controller);
            dynamic controller = Activator.CreateInstance(type_controller);
            dynamic ai = Activator.CreateInstance(type_bird, new object[]{controller});

            // Associate the ai with a color in the console drawer
            drawer.Associate(ai, ConsoleColor.Blue);

            // Add the ai to the game
            game.Add(ai);

            // Start the game and draw it once
            game.Start();
            //game.Draw();
            
            // While there is someone alive, continue or timeout
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(40 * 1000);
                game.Continue = false;
            });

            while (game.Continue)
            {
                // Game loop : update
                game.Update();
                if (game.x > int.MaxValue)
                {
                    Console.WriteLine("score overflow");
                    break;
                }
            }
            // Stop the game
            game.Stop();

            // Write the scores
            sw.WriteLine(game.x);
            // we write score to file instead of return it
            sw.Flush();
            sw.Close();

            // Read a key (for external terminal users)
            //Console.Read();
            return 0;// success
        }
    }
}