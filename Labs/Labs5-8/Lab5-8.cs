using System;
using System.Collections.Generic;
using System.Text;

namespace Labs5_8
{
    class Lab5_8
    {
        static private Random r = new Random();

        public static double Rnd(double left = 0, double rigth = 1)
        {
            double rnd = (1 - r.NextDouble());

            return rnd * (rigth - left) + left;
        }
    }
}
