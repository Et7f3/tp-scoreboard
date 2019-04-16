using System;
using System.IO;
using System.Reflection;

namespace Flappy
{
    internal class Program
    {
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

            Assembly asm = Assembly.LoadFrom(args[0]);
            //foreach (Type type in asm.GetTypes())
            //{
            //    Console.WriteLine(type);
            //}
            Type my_bird = asm.GetType("Flappy.Bird");
            Type my_controller = asm.GetType("Flappy.BestController");

            if (my_bird == null)
                return -3;// file don't have right class

            if (my_controller == null)
                return -4;// file don't have right class

            // Hide the cursor
            Console.CursorVisible = false;

            // Get one random generator for all the game
            Random rnd = new Random();

            // Initialize the keyboard manager, which will handle the keyboard input
            //KeyboardManager manager = new KeyboardManager();

            // Initialize the console drawer, which will handle the console output
            ConsoleDrawer drawer = new ConsoleDrawer(Console.WindowWidth, Console.WindowHeight);

            // Initialize the game with the random generator and the output drawer
            Game game = new Game(rnd, drawer);
            // Add the keyboard manager to the game
            //game.Add(manager);

            // Create a player
            //Bird player = new Bird(new KeyboardController(manager, ConsoleKey.Spacebar));
            // Associate the player with a color in the console drawer
            //drawer.Associate(player, ConsoleColor.Red);

            // Create an AI
            Console.WriteLine(my_controller.BaseType);

            MethodInfo mi = my_controller.GetMethod("ShouldJump"
                , BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod
            );

            MethodInfo mi2 = my_controller.GetTypeInfo().GetDeclaredMethod("ShouldJump");

            MethodInfo mi3 = my_controller.GetMethod("ShouldJump");

            MemberInfo mi4 = my_controller.GetMember("ShouldJump")[0];

            Console.WriteLine($"mi == null: {mi == null}, mi2 == null: {mi2 == null}, mi3 == null: {mi3 == null}, mi4 == null: {mi4 == null}");

            Console.WriteLine($"my_controller.GetMethod(\"ShouldJump\") == null: " + mi3.GetType());

            BestController controller = new Flappy.BestController(mi3, Activator.CreateInstance(my_controller));
            //Console.WriteLine("begin");
            //foreach (ConstructorInfo constructorInfo in my_bird.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic))
            //{
            //    Console.WriteLine(constructorInfo.Name);
            //}
            Bird ai = new Bird(controller);
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
            while (game.Continue)
            {
                // Game loop : update, draw and sleep
                game.Update();
                game.Draw();
                game.Sleep();
            }
            // Stop the game
            game.Stop();
            //Console.Clear();

            // Write the scores
            //Console.WriteLine("Player scored : " + player.Score);
            Console.WriteLine("AI scored : " + ai.Score);

            // Read a key (for external terminal users)
            //Console.Read();
            Console.CursorVisible = true;
            return 0;// success
        }
    }
}