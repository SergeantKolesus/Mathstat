using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Labs5_8
{
    class Lab5_8
    {
        public delegate double fDistr(double x, double y, double z);

        static private Random r = new Random();

        #region QSort

        static private int _divide(double[] vals, int start, int end)
        {
            double temp;
            int marker = start;

            for (int i = start; i < end; i++)
            {
                if (vals[i] < vals[end])
                {
                    temp = vals[marker];
                    vals[marker] = vals[i];
                    vals[i] = temp;
                    marker += 1;
                }
            }
            temp = vals[marker];
            vals[marker] = vals[end];
            vals[end] = temp;
            return marker;
        }

        static private void _qsort(double[] vals, int start, int end)
        {
            if (start >= end)
                return;

            int anchor = _divide(vals, start, end);
            _qsort(vals, start, anchor - 1);
            _qsort(vals, anchor + 1, end);
        }

        #endregion QSort

        public static double Rnd(double left = 0, double rigth = 1)
        {
            double rnd = (1 - r.NextDouble());

            return rnd * (rigth - left) + left;
        }

        static public double[] GetSample(int amount, fDistr f, double first = 0, double second = 1, double left = 0, double rigth = 1)
        {
            double[] res = new double[amount];

            for (int i = 0; i < amount; i++)
                res[i] = f((rigth - left) * (1 - r.NextDouble()) + left, first, second);

            _qsort(res, 0, amount - 1);

            return res;
        }

        static private double _getAvg(double[] x)
        {
            int l = x.Length;
            double s = 0;

            for (int i = 0; i < l; i++)
                s += x[i];

            return s / l;
        }

        #region Lab6

        static private double[] _getX(double min, double max, double step)
        {
            double[] res = new double[(int)Math.Round((max - min) / step) + 1];

            for (double v = min, i = 0; v <= max; v += step, i++)
                res[(int)i] = v;


            return res;
        }

        static private double _getXYAvg(double[] x, double[] y)
        {
            int l = x.Length;
            double s;

            s = 0;

            for (int i = 0; i < l; i++)
                s += x[i] * y[i];

            return s / l;
        }

        static private double _getSqAvg(double[] x)
        {
            int l = x.Length;
            double s = 0;

            for (int i = 0; i < l; i++)
                s += x[i] * x[i];

            return s / l;
        }

        static private double[] _lsm(double[] x, double[] y)
        {
            double xyAvg;
            double xAvg;
            double xSqAvg;
            double yAvg;
            double b1;
            double b2;

            xAvg = _getAvg(x);
            yAvg = _getAvg(y);
            xSqAvg = _getSqAvg(x);
            xyAvg = _getXYAvg(x, y);

            b2 = (xyAvg - xAvg * yAvg) / (xSqAvg - xAvg * xAvg);
            b1 = yAvg - xAvg * b2;

            return new double[] { b1, b2};
        }

        static private double _sign(double x)
        {
            if (x == 0)
                return 0;

            return x > 0 ? 1 : -1;
        }

        static private double[] _lam(double[] x, double[] y)
        {
            double xAvg;
            double yAvg;
            double rQ;
            double qx;
            double qy;
            double k;
            double b1;
            double b2;
            int l;
            int j;
            int n;

            n = x.Length;

            xAvg = _getAvg(x);
            yAvg = _getAvg(y);
            rQ = 0;
            k = 1;


            for (int i = 0; i < n; i++)
                rQ += _sign(x[i] - xAvg) * _sign(y[i] - yAvg);

            rQ /= n;

            if (n % 4 == 0)
                l = n / 4;
            else
                l = n / 4 + 1;

            j = n - l + 1;

            qy = (y[j] - y[l]) / k;
            qx = (x[j] - x[l]) / k;

            b1 = rQ * qy / qx;
            b2 = yAvg - b1 * xAvg;

            return new double[] { b1, b2 };
        }

        static private void _writeLine(double[] x, double[] y, double[] yLsm, double[] yLam, string fileName)
        {
            try
            {
                StreamWriter sw = new StreamWriter(fileName);
                int l = x.Length;
                double lsm;
                double lam;
                double exampleY;

                for (int i = 0; i < l; i++)
                {
                    lsm = yLsm[0] + yLsm[1] * x[i];
                    lam = yLam[0] + yLam[1] * x[i];
                    exampleY = 2 + 2 * x[i];

                    sw.WriteLine(x[i] + "\t" + y[i] + "\t" + exampleY + "\t" + lsm + "\t" + lam);
                }

                sw.Close();
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc);
            }
        }

        static public void Lab6()
        {
            double[] x = _getX(-1.8, 2, 0.2);
            double[] eps = GetSample(20, Distributions.NormalRandom, 0, 1);

            double[] y = new double[20];

            for (int i = 0; i < 20; i++)
                y[i] = 2 + x[i] * 2 + eps[i];

            double[] y2 = (double[])y.Clone();

            y2[0] += 10;
            y2[19] -= 10;

            double[] lsm = _lsm(x, y);
            double[] lam = _lam(x, y);

            _writeLine(x, y, lsm, lam, "NoDist.xls");

            Console.WriteLine("b1 = " + lsm[0] + " and b2 = " + lsm[1]);
            Console.WriteLine("b1 = " + lam[0] + " and b2 = " + lam[1]);

            lsm = _lsm(x, y2);
            lam = _lam(x, y2);

            _writeLine(x, y2, lsm, lam, "Dist.xls");

            Console.WriteLine("b1 = " + lsm[0] + " and b2 = " + lsm[1]);
            Console.WriteLine("b1 = " + lam[0] + " and b2 = " + lam[1]);


        }

        #endregion Lab6

        #region Lab7

        static private void _getMuAlpha()
        {
            double[] sample = GetSample(100, Distributions.NormalRandom, 0, 1);

            double mu = _getAvg(sample);
        }

        static public void Lab7()
        {
            _getMuAlpha();
        }

        #endregion Lab7
    }
}
