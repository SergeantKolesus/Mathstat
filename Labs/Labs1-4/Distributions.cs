using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;
using System.Drawing;

namespace Labs1_4
{
    class Distributions
    {
        static private double _factorial(int n)
        {
            if (n < 2)
                return 1;

            double res = 1;

            for (int i = 2; i <= n; i++)
            {
                res *= i;
            }

            return res;
        }

        #region Densities

        static public double NormalDistributionDensity(double x, double mu, double delta)
        {
            return Math.Exp(-(x - mu) * (x - mu) / (2 * delta * delta)) / (delta * Math.Sqrt(2 * Math.PI));
        }

        static public double CauchyDistributionDensity(double x, double x0, double gamma)
        {
            return 1 / (Math.PI * gamma * (1 + (x - x0) * (x - x0) / (gamma * gamma)));
        }

        static public double LaplaceDistributionDensity(double x, double betta, double alpha)
        {
            return alpha / 2 * Math.Exp(-alpha * Math.Abs(x - betta));
        }

        static public double PoissonDistributionDensity(double k, double lambda, double gap = 0)
        {
            return Math.Pow(lambda, Math.Round(k)) / _factorial((int)Math.Round(k)) * Math.Exp(-lambda);
        }

        static public double UniformDistributionDensity(double x, double a, double b)
        {
            if ((x >= a) && (x <= b))
                return 1 / (b - a);
            else
                return 0;
        }

        #endregion

        #region Random values

        static public double NormalRandom(double x, double mu, double delta)
        {
            Random r = new Random();
            double sum = 0;

            for (int i = 0; i < 12; i++)
                sum += r.NextDouble();

            return sum - 6;
        }

        static public double CauchyRandom(double x, double x0, double gamma)
        {
            double res;
            /*
              do
              {
                  res = x0 + gamma * Math.Tan(2 * Math.PI * x);
                  if (x > 0)
                      x = Math.Sqrt(x);
                  else
                      x = -Math.Sqrt(-x);
              } while (Math.Abs(res) >= 4);
              //*/

            res = x0 + gamma * Math.Tan(2 * Math.PI * x);

            return res;
        }

        static public double LaplaceRandom(double x, double betta, double alpha)
        {
            return betta + Math.Log(Math.Abs(x) / (Lab1_4.Rnd())) / alpha;
        }

        static public double PoissonRandom(double k, double lambda, double gap = 0)
        {
            double s = 0;
            int i = -1;

            do
            {
                s += ExpinentialRandom(Lab1_4.Rnd());
                i++;
            } while (s < lambda);

            return i;
        }

        static public double UniformRandom(double x, double a, double b)
        {
            return (b - a) * x + a;
        }

        static public double ExpinentialRandom(double x, double a = 1, double b = 0)
        {
            return -Math.Log(Lab1_4.Rnd()) / a;
        }

        #endregion
    }
}
