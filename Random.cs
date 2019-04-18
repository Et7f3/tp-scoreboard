using System;
using System.Collections.Generic;
using System.IO;

namespace FlappyRunner
{
    public class Random : System.Random
    {
        private int used = 0;

        public static List<int> seeds = new List<int>();

        System.Random my_rnd;

        public override int Next()
        {
            string str;
            int next_seed;

            if (used-- == 0)
            {
                if ((str = Program.sr_seed.ReadLine()) != null)
                    my_rnd = new System.Random(parse(str));
                else
                {
                    next_seed = (int) System.DateTime.Now.Ticks;
                    my_rnd = new System.Random(next_seed);
                    seeds.Add(next_seed);
                }
                used = 100;
            }

            //Console.WriteLine("variant 0");
            return my_rnd.Next();
        }

        int parse(string s)
        {
            long ret = 0;
            int i = 0;
            int l = s.Length;

            // '0' = '/' + 1
            // '0' > '\r' && '0' > '\n'
            // we assume valid data so we won't have > '9'
            while (i < l)
                ret = ret * 10 + s[i++] - '0';

            return (int) ret;
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