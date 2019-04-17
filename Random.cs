using System;

namespace FlappyRunner
{
    public class Random : System.Random
    {
        public override int Next()
        {
            Console.WriteLine("variant 0");
            return base.Next();
        }

        public override int Next(int maxValue)
        {
            Console.WriteLine("variant 1");
            return base.Next(maxValue);
        }

        public override int Next(int minValue, int maxValue)
        {
            Console.WriteLine("variant 2");
            return base.Next(minValue, maxValue);
        }
    }
}