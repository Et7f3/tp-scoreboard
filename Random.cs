using System;

namespace FlappyRunner
{
    public class Random : System.Random
    {
        private int used = 0;

        System.Random my_rnd;

        public override int Next()
        {
            if (used-- == 0)
            {
                my_rnd = new System.Random((int)System.DateTime.Now.Ticks);
                used = 100;
            }

            //Console.WriteLine("variant 0");
            return my_rnd.Next();
        }

        public override int Next(int maxValue)
        {
            return Next() % maxValue;
        }

        public override int Next(int minValue, int maxValue)
        {
            return Next(maxValue - minValue) + minValue;
        }
    }
}