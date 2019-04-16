using System;
using System.IO;
using System.Reflection;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

namespace FlappyRunner
{
    internal class Program
    {
        public static Assembly asm;
        public static Type type_bird;
        public static Type type_controller;
        public static Type type_drawer;
        public static Type type_console_drawer;
        public static Type type_pipe;
        public static Type type_deque;

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
            if (args.Length < 2)
                return -1;// usage firstname.lastname.exe save.txt

            if (!File.Exists(args[0]))
                return -2;// file not exist

            asm = Assembly.LoadFrom(args[0]);
            //foreach (Type type in asm.GetTypes())
                //Console.WriteLine(type);
            type_bird = asm.GetType("Flappy.Bird");
            type_controller = asm.GetType("Flappy.BestController");
            type_drawer = asm.GetType("Flappy.Drawer");
            type_console_drawer = asm.GetType("Flappy.ConsoleDrawer");
            type_pipe = asm.GetType("Flappy.Pipe");
            type_deque = asm.GetType("Flappy.Deque`1").MakeGenericType(type_pipe);
            //Console.WriteLine(type_deque == null);
            /*
             * Flappy.BaseController
Flappy.BestController
Flappy.Bird
Flappy.ConsoleDrawer
Flappy.Controller
Flappy.Drawer
Flappy.Game
Flappy.KeyboardController
Flappy.KeyboardManager
Flappy.Manager
Flappy.MaximaxController
Flappy.MaximaxController2
Flappy.Pipe
Flappy.Program
Flappy.BaseController+JumpController
Flappy.BaseController+FallController
Flappy.MaximaxController
Flappy.Deque`1+Node[T]
Flappy.Deque`1+<GetEnumerator>d__13[T]
Flappy.Game+<>c
Flappy.MaximaxController+<>c
Flappy.Deque`1[T]
             */

            if (type_bird == null)
                return -3;// file don't have right class

            if (type_controller == null)
                return -4;// file don't have right class

            // Hide the cursor
            Console.CursorVisible = false;

            // Get one random generator for all the game
            Random rnd = new Random();

            // Initialize the keyboard manager, which will handle the keyboard input
            //KeyboardManager manager = new KeyboardManager();

            // Initialize the console drawer, which will handle the console output
            dynamic drawer = Activator.CreateInstance(type_console_drawer, new object[]{Console.WindowWidth, Console.WindowHeight});

            // Initialize the game with the random generator and the output drawer
            Game game = new Game(rnd, drawer);
            // Add the keyboard manager to the game
            //game.Add(manager);

            // Create a player
            //Bird player = new Bird(new KeyboardController(manager, ConsoleKey.Spacebar));
            // Associate the player with a color in the console drawer
            //drawer.Associate(player, ConsoleColor.Red);

            // Create an AI
            //Console.WriteLine(type_controller.BaseType);

            //MethodInfo mi = my_controller.GetMethod("ShouldJump", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod);

            //MethodInfo mi2 = my_controller.GetTypeInfo().GetDeclaredMethod("ShouldJump");

            //MethodInfo mi3 = my_controller.GetMethod("ShouldJump");

            //MemberInfo mi4 = my_controller.GetMember("ShouldJump")[0];

            //Console.WriteLine($"mi == null: {mi == null}, mi2 == null: {mi2 == null}, mi3 == null: {mi3 == null}, mi4 == null: {mi4 == null}");

            //Console.WriteLine($"my_controller.GetMethod(\"ShouldJump\") == null: " + mi3.GetType());

            //BestController controller = new Flappy.BestController(mi3, Activator.CreateInstance(my_controller));
            //Console.WriteLine("begin");
            //foreach (ConstructorInfo constructorInfo in my_bird.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic))
            //{
            //    Console.WriteLine(constructorInfo.Name);
            //}
            //Bird ai = new Bird(controller);
            dynamic controller = Activator.CreateInstance(type_controller);
            dynamic ai = Activator.CreateInstance(type_bird, new object[]{controller});
            //Console.WriteLine("begin");
            // Associate the ai with a color in the console drawer
            drawer.Associate(ai, ConsoleColor.Blue);

            // Add the player and the ai to the game
            //game.Add(player);
            game.Add(ai);

            // Start the game and draw it once
            game.Start();
            //game.Draw();
            
            // While there is someone alive, continue
            //Thread.Sleep(200000);
            Task.Factory.StartNew(() =>
            {
                while (game.Continue)
                {
                    // Game loop : update, draw and sleep
                    game.Update();
                    //game.Draw();
                    //game.Sleep();
                }
            }).Wait(1000);
            // Stop the game
            game.Stop();
            //Console.Clear();

            // Write the scores
            //Console.WriteLine("Player scored : " + player.Score);
            Console.WriteLine("AI scored : " + ai.Score);
            Console.WriteLine($"AI run : {game.x} pixels");

            // Read a key (for external terminal users)
            //Console.Read();
            Console.CursorVisible = true;
            return 0;// success
        }
    }
}