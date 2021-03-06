﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;
using System.Drawing;
using System.Diagnostics;
using IronPython.Hosting;
using Numpy;


namespace Labs5_8
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

        #region Distributions

        static private double _erf(double x)
        {
            double a1 = 0.254829592;
            double a2 = -0.284496736;
            double a3 = 1.421413741;
            double a4 = -1.453152027;
            double a5 = 1.061405429;
            double p = 0.3275911;
            int sign;

            sign = x < 0 ? -1 : 1;
            x = Math.Abs(x);

            double t = 1.0 / (1.0 + p * x);
            double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

            return sign * y;
        }

        static public double NormalDistribution(double x, double mu, double delta)
        {
            return (1 + _erf((x - mu) / Math.Sqrt(2 * delta * delta))) / 2;
        }

        static public double LaplaceDistribution(double x, double betta, double alpha)
        {
            if (x <= betta)
                return alpha / 2 * Math.Exp(alpha * (x - betta));
            else
                return 1 - alpha / 2 * Math.Exp(-alpha * (x - betta)); ;
        }

        #endregion Disdtributions

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

        static public double NormalTwodimensionalDistributionDensity(double x1, double y1, double x2, double y2, double sigmaX, double sigmaY, double ro)
        {
            return 1 / (2 * Math.PI * sigmaX * sigmaY * Math.Sqrt(1 - ro * ro)) *
                Math.Exp(-1 / (2 * (1 - ro * ro)) * ((x1 - x2) * (x1 - x2) / (sigmaX * sigmaX) - 2 * ro * (x1 - x2) * (y1 - y2) / (sigmaX * sigmaY) + (y1 - y2) * (y1 - y2) / (sigmaY * sigmaY)));
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
            return betta + Math.Log(Math.Abs(x) / (Lab5_8.Rnd())) / alpha;
        }

        static public double PoissonRandom(double k, double lambda, double gap = 0)
        {
            double s = 0;
            int i = -1;

            do
            {
                s += ExpinentialRandom(Lab5_8.Rnd());
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
            return -Math.Log(Lab5_8.Rnd()) / a;
        }

        static public double NormalTwodimensionalrandom()
        {
            return 0;
        }

        static public string run_cmd(string cmd, string args)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "C:\\Users\\serge\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\Python 3.7";
            start.Arguments = string.Format("\"{0}\" \"{1}\"", cmd, args);
            start.UseShellExecute = false;// Do not use OS shell
            start.CreateNoWindow = true; // We don't need new window
            start.RedirectStandardOutput = true;// Any output, generated by application will be redirected back
            start.RedirectStandardError = true; // Any error in standard output will be redirected back (for example exceptions)
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string stderr = process.StandardError.ReadToEnd(); // Here are the exceptions from our Python script
                    string result = reader.ReadToEnd(); // Here is the result of StdOut(for example: print "test")
                    return result;
                }
            }
        }

        /*
        static public string PatchParameter(string parameter, int serviceid)
        {
            var engine = Python.CreateEngine(); // Extract Python language engine from their grasp
            var scope = engine.CreateScope(); // Introduce Python namespace (scope)
            var d = new Dictionary<string, object>
            {
                { "serviceid", serviceid},
                { "parameter", parameter}
            }; // Add some sample parameters. Notice that there is no need in specifically setting the object type, interpreter will do that part for us in the script properly with high probability

            scope.SetVariable("params", d); // This will be the name of the dictionary in python script, initialized with previously created .NET Dictionary
            var source = engine.CreateScriptSourceFromFile("..\\..\\..\\NormalDist.py"); // Load the script
            object result = source.Execute(scope);
            parameter = scope.GetVariable<string>("parameter"); // To get the finally set variable 'parameter' from the python script
            return parameter;
        }
        */

        static public void NtdRandom()
        {
            //instance of python engine
            Microsoft.Scripting.Hosting.ScriptEngine engine = IronPython.Hosting.Python.CreateEngine();
            //reading code from file
            var source = engine.CreateScriptSourceFromFile("..\\..\\..\\NormalDist.py");
            var scope = engine.CreateScope();
            //executing script in scope
            source.Execute(scope);
            var classRandomizer = scope.GetVariable("randomizer");
            //initializing class
            var randomizerInstance = engine.Operations.CreateInstance(classRandomizer);
            Console.WriteLine("From Iron Python");
            Console.WriteLine(randomizerInstance.rand(0, 20));
        }

        #endregion
    }
}
