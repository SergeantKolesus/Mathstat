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

        static private double _stdDevi(double[] sample)
        {
            double s;
            double avg;
            int l;

            s = 0;
            l = sample.Length;
            avg = _getAvg(sample);

            for (int i = 0; i < l; i++)
            {
                double t = sample[i];
                s += t * t;
            }

            return Math.Sqrt(s / l);
        }

        static private double _getDispersion(double[] sample)
        {
            double avg = _getAvg(sample);

            return _getSqAvg(sample) - avg * avg;
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

        static private double[] _getMuSigma(double[] sample)
        {
            double mu = _getAvg(sample);
            double sigma = _stdDevi(sample);

            Console.WriteLine("mu = " + mu + " and sigma = " + sigma);

            return new double[] { mu, sigma };
        }

        static private double[] _getBorders(double left, double rigth, int count)
        {
            double[] res = new double[count + 1];
            double step = (rigth - left) / count;
            double val = left + step;

            res[0] = left;

            for (int i = 0; i < count; i++)
            {
                res[i + 1] = val;
                val += step;
            }

            
            //res[count + 1] = rigth;

            return res;
        }

        static private double[] _getDiapsCounts(double[] sample, double[] borders)
        {
            double[] res = new double[6];
            int l = sample.Length;

            for(int i = 0; i < l; i++)
            {
                if (sample[i] < borders[0])
                    res[0]++;

                if (sample[i] >= borders[4])
                    res[5]++;

                for (int j = 0; j < 4; j++)
                    if ((sample[i] >= borders[j]) && (sample[i] < borders[j + 1]))
                        res[j + 1]++;
            }

            return res;
        }

        static private void _getDiapasonsInfo(double[] sample, double[] ni)
        {
            int diapSum = 0;

            for (int i = 0; i < 6; i++)
            {
                diapSum += (int)ni[i];
                Console.WriteLine("int diap number " + i + "are " + ni[i] + " vals");
            }

            Console.WriteLine("total elements count " + diapSum);
        }

        static private void _getNPsInfo(double[] sample, double[] p, int n)
        {
            Console.WriteLine("p\tnp");

            double sumP = 0;

            for (int i = 0; i < 6; i++)
            {
                Console.WriteLine(p[i] + "\t" + (n * p[i]));

                sumP += p[i];
            }

            Console.WriteLine("p sum = " + sumP);
        }

        static private void _getNNPs(double[] p, double[] ni, int n)
        {
            double sum = 0;

            Console.WriteLine("Ni - N * Pi");

            for(int i = 0; i < 6; i++)
            {
                double t = ni[i] - n * p[i];
                sum += t;
                Console.WriteLine(i + " : " + t);
            }

            Console.WriteLine("total sum " + sum);
        }

        static private void _getNNPsSq(double[] p, double[] ni, int n)
        {
            double sum = 0;

            Console.WriteLine("(Ni - N * Pi)^2/(N * Pi)");

            for (int i = 0; i < 6; i++)
            {
                double t1 = ni[i] - n * p[i];
                double t = t1 * t1 / (n * p[i]);
                sum += t;
                Console.WriteLine(i + " : " + t);
            }

            Console.WriteLine("total sum " + sum);
        }

        static private void _getKsi(double[] sample, fDistr f, double fFirst, double fSecond, int count)
        {
            double mu = _getAvg(sample);
            double left = mu - 1.5;
            double rigth = mu + 1.5;

            double[] borders = _getBorders(left, rigth, 4);

            Console.Write("Borders: " + left + " ");

            for (int i = 0; i < 4; i++)
                Console.Write(borders[i + 1] + " ");

            Console.WriteLine();

            double[] p = new double[6];

            p[0] = f(left, fFirst, fSecond);

            for (int i = 0; i < 4; i++)
                p[i + 1] = f(borders[i + 1], fFirst, fSecond) - f(borders[i], fFirst, fSecond);

            p[5] = 1 - f(rigth, fFirst, fSecond);

            double[] ni = _getDiapsCounts(sample, borders);

            _getDiapasonsInfo(sample, ni);

            _getNPsInfo(sample, p, count);

            _getNNPs(p, ni, count);

            _getNNPsSq(p, ni, count);
        }

        static public void Lab7()
        {
            double[] sample = GetSample(100, Distributions.NormalRandom, 0, 1);

            _getMuSigma(sample);

            _getKsi(sample, Distributions.NormalDistribution, 0, 1, 100);

            //_maximalPlausibilityMethod(sample);

            sample = GetSample(20, Distributions.LaplaceRandom, 0, 1);

            double[] t = _getMuSigma(sample);

            Console.WriteLine("Sensitivity Laplace n=20");

            _getKsi(sample, Distributions.LaplaceDistribution, t[0], t[1] / Math.Sqrt(2), 20);

            sample = GetSample(20, Distributions.NormalRandom, 0, 1);

            _getMuSigma(sample);

            Console.WriteLine("Sensitivity Normal n=20");

            _getKsi(sample, Distributions.NormalDistribution, 0, 1, 20);
        }

        #endregion Lab7

        #region Lab8

        static private double[] _getM(double[] sample)
        {
            double avg;
            double s;
            double t;
            double min;
            double max;
            double t1;
            double t2; 
            int l;
            
            avg = _getAvg(sample);
            s = _getDispersion(sample);
            l = sample.Length;

            t = s * sample[0] / Math.Sqrt(l - 1);

            min = avg - t;
            max = avg + t;

            for (int i = 1; i < l; i++)
            {
                t = s * sample[i] / Math.Sqrt(l - 1);

                t1 = avg - t;
                t2 = avg + t;

                if (t1 < min)
                    min = t1;

                if (t2 > max)
                    max = t2;
            }

            return new double[] { min, max};
        }

        static private double[] _getSimga(double[] sample)
        {
            double avg;
            double s;
            double t;
            double min;
            double max;
            double t1;
            double t2;
            int l;

            avg = _getAvg(sample);
            s = _getDispersion(sample);
            l = sample.Length;

            t = s * Math.Sqrt((double)l / (l - 1.0));

            min = avg - t;
            max = avg + t;

            for (int i = 1; i < l; i++)
            {
                t = s * sample[i] / Math.Sqrt(l - 1);

                t1 = avg - t;
                t2 = avg + t;

                if (t1 < min)
                    min = t1;

                if (t2 > max)
                    max = t2;
            }

            return new double[] { min, max };
        }

        static private double[] _getAssM(double[] sample)
        {
            double avg;
            double s;
            double t;
            double min;
            double max;
            double t1;
            double t2;
            int l;

            avg = _getAvg(sample);
            s = _getDispersion(sample);
            l = sample.Length;

            t = s * sample[0] / Math.Sqrt(l - 1);

            min = avg - t;
            max = avg + t;

            for (int i = 1; i < l; i++)
            {
                t = s * sample[i] / Math.Sqrt(l - 1);

                t1 = avg - t;
                t2 = avg + t;

                if (t1 < min)
                    min = t1;

                if (t2 > max)
                    max = t2;
            }

            return new double[] { min, max };
        }

        static private double[] _geAsstSimga(double[] sample)
        {
            double avg;
            double s;
            double t;
            double min;
            double max;
            double t1;
            double t2;
            int l;

            avg = _getAvg(sample);
            s = _getDispersion(sample);
            l = sample.Length;

            t = s * Math.Sqrt((double)l / (l - 1.0));

            min = avg - t;
            max = avg + t;

            for (int i = 1; i < l; i++)
            {
                t = s * sample[i] / Math.Sqrt(l - 1);

                t1 = avg - t;
                t2 = avg + t;

                if (t1 < min)
                    min = t1;

                if (t2 > max)
                    max = t2;
            }

            return new double[] { min, max };
        }

        static public void Lab8()
        {
            double alpha = 0.05;
            double[] sample20 = GetSample(20, Distributions.NormalRandom);
            double[] sample100 = GetSample(100, Distributions.NormalRandom);

            double[] m = _getM(sample20);

            Console.WriteLine(m[0] + " " + m[1]);

            m = _getM(sample100);

            Console.WriteLine(m[0] + " " + m[1]);

        }

        #endregion Lab8
    }
}
