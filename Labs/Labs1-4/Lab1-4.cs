using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Transactions;
using System.Dynamic;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks.Sources;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.Tracing;

namespace Labs1_4
{
    class Lab1_4
    {


        public delegate double fDistr(double x, double y, double z);
        public delegate double fCharact(double[] x);

        static private double columnSize = 0.25;

        static private Random r = new Random();

        public static double Rnd(double left = 0, double rigth = 1)
        {
            double rnd = (1 - r.NextDouble());

            return rnd * (rigth - left) + left;
        }

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

        #region Global

        static public double[] GetSample(int amount, fDistr f, double first = 0, double second = 1, double left = 0, double rigth = 1)
        {
            double[] res = new double[amount];

            for (int i = 0; i < amount; i++)
                res[i] = f((rigth - left) * (1 - r.NextDouble()) + left, first, second);

            _qsort(res, 0, amount - 1);

            return res;
        }

        #endregion

        #region Lab1
        static public void PrepareHistogram(fDistr f, fDistr dens, double first, double second, int count, string filename, double left = 0, double rigth = 1)
        {
            double[] values;

            values = GetSample(f, first, second, count, left, rigth);

            _qsort(values, 0, count - 1);

            int columns = (int)Math.Round(Math.Sqrt(count));
           
            //if (count <= 50)
                if (count <= 25)
                    columns = (count + 1) / 2;
                //else
                    //columns = (count + 1) / 3;

            double step = (values[count - 1] - values[0]) / columns;
            double mark = Math.Round(values[0] * 10) / 10;

            try
            {
                StreamWriter sw = new StreamWriter(filename);

                /*
                double[] marks = new double[columns];
                int markCount = 0;

                for(int i = 0; i < count; i++)
                {

                }
                //*/

                for (int i = 0; i < count; i++)
                {
                    sw.Write(values[i]);

                    if (mark < (values[count - 1] + step))
                    {
                        sw.WriteLine("\t" + mark + "\t" + dens(mark, first, second));
                        mark += step;
                    }
                    else
                        sw.WriteLine();
                }

                while (mark < (values[count - 1] + step))
                {
                    sw.WriteLine("\t" + mark);
                    mark += step;
                }

                sw.Close();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
        }

        #endregion


        #region Characteeristics

        static public double xLined(double[] sample)
        {
            double sum = 0;
            double[] res = new double[sample.Length];

            for (int i = 0; i < sample.Length; i++)
                sum += sample[i];

            return sum / sample.Length;
        }

        static public double medX(double[] sample)
        {
            if (sample.Length % 2 == 0)
                return sample[sample.Length / 2];
            else
                return sample[sample.Length / 2 + 1];
        }

        static public double zR(double[] sample)
        {
            return (sample[0] + sample[sample.Length - 1]) / 2;
        }

        static public double QuartileHalfsum(double[] sample)
        {
            if (sample.Length % 4 == 0)
                return (sample[sample.Length / 4] + sample[3 * sample.Length / 4]) / 2;
            else
                return (Math.Round(sample[sample.Length / 4]) + 1 + Math.Round(sample[3 * sample.Length / 4]) + 1) / 2;
        }

        static public double TruncatedAverage(double[] sample)
        {
            int r = sample.Length / 4;
            double sum = 0;

            for (int i = r + 1; i < sample.Length - r; i++)
                sum += sample[i];

            return sum / (sample.Length - 2 * r);
        }

        static public double SampleVariance(double[] sample)
        {
            double sum = 0;
            double lined = xLined(sample);

            for (int i = 0; i < sample.Length; i++)
                sum += (sample[i] - lined) * (sample[i] - lined);

            return sum / sample.Length;
        }

        #endregion

        static public double AverageCharacteristic(int mesAmount, fCharact f, fDistr d, double first, double second, int sampleSize, double left = 0, double rigth = 1)
        {
            double sum = 0;
            double[] sample = null;

            for(int i = 0; i < mesAmount; i++)
            {
                sample = GetSample(sampleSize, d, first, second, left, rigth);
                sum += f(sample);
            }

            if (sample == null)
                return 0;

            return sum / sampleSize;
        }

        static public double AverageSquaredCharacteristic(int mesAmount, fCharact f, fDistr d, double first, double second, int sampleSize, double left = 0, double rigth = 1)
        {
            double sum = 0;
            double[] sample = null;

            for (int i = 0; i < mesAmount; i++)
            {
                double c = f(sample);

                sample = GetSample(sampleSize, d, first, second, left, rigth);
                sum += c * c;
            }

            if (sample == null)
                return 0;

            return sum / sampleSize;
        }

        static public double Dispersion(int mesAmount, fCharact f, fDistr d, double first, double second, int sampleSize, double left = 0, double rigth = 1)
        {
            double avg = AverageCharacteristic(mesAmount, f, d, first, second, sampleSize, left, rigth);

            return AverageSquaredCharacteristic(mesAmount, f, d, first, second, sampleSize, left, rigth) - avg * avg;
        }

        static public double[] GetSample(fDistr f, double first, double second, int sampleSize, double left = 0, double rigth = 1)
        {
            double[] res = new double[sampleSize];

            for (int i = 0; i < sampleSize; i++)
                res[i] = f(Rnd(left, rigth), first, second);

            return res;
        }

        

        #region Lab2

        static private double[] _getAverage(double[,] vals)
        {
            double[] sum = new double[5] { 0, 0, 0, 0, 0 };
            int columns = vals.GetLength(0);

            for (int i = 0; i < columns; i++)
                for (int j = 0; j < 5; j++)
                { 
                    sum[j] += vals[j, i];
                }

            for (int j = 0; j < 5; j++)
                sum[j] /= columns;

            return sum;
        }

        static private double[] _getSquaredAverage(double [,] vals)
        {
            double[] sum = new double[5] { 0, 0, 0, 0, 0 };
            int columns = vals.GetLength(0);

            for (int i = 0; i < columns; i++)
                for (int j = 0; j < 5; j++)
                {
                    sum[j] += vals[j, i] * vals[j, i];
                }

            for (int j = 0; j < 5; j++)
                sum[j] /= columns;

            return sum;
        }

        static private double[] _getDispersion(double[,] vals)
        {
            double[] avg = _getAverage(vals);
            double[] sqAvg = _getSquaredAverage(vals);
            double[] res = new double[5];

            for (int i = 0; i < 5; i++)
                res[i] = sqAvg[i] - avg[i] * avg[i];

            return res;
        }

        static private double[,] _getCharacteristics(fDistr f, double first, double second, int samples, int sampleSize)
        {
            double[,] res = new double[5, samples];

            for(int i = 0; i < samples; i++)
            {
                double[] sample = GetSample(f, first, second, sampleSize);

                res[0, i] = xLined(sample);
                res[1, i] = medX(sample);
                res[2, i] = zR(sample);
                res[3, i] = QuartileHalfsum(sample);
                res[4, i] = TruncatedAverage(sample);
            }

            return res;
        }

        

        static public void Task2(fDistr f, string fileName, double first, double second, int[] count)
        {
            try
            {
                StreamWriter sw = new StreamWriter(fileName);

                for (int i = 0; i < count.Length; i++)
                {

                    double[,] chars = _getCharacteristics(f, first, second, 1000, count[i]);

                    double[] avg = _getAverage(chars);

                    double[] disp = _getDispersion(chars);




                    sw.WriteLine("normal n = " + count[i]);
                    sw.WriteLine("\t\tmed x\tZr\tZq\tZtr");
                    sw.WriteLine("E(z)\t" + avg[0] + "\t" + avg[1] + "\t" + avg[2] + "\t" + avg[3] + "\t" + avg[4]);
                    sw.WriteLine("D(z)\t" + disp[0] + "\t" + disp[1] + "\t" + disp[2] + "\t" + disp[3] + "\t" + disp[4]);
                    sw.WriteLine();
                }

                sw.Close();
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc);
            }
        }

        #endregion Lab2

        #region Lab3

        static public void PrepareBoxplots(fDistr f, double first, double second, string filename)
        {
            try
            {
                StreamWriter sw = new StreamWriter(filename);

                double[] sample;
                double[] hugeSample;
                double[] sortedSample;

                sample = GetSample(f, first, second, 20);
                hugeSample = GetSample(f, first, second, 1000);
                sortedSample = (double[])hugeSample.Clone();

                _qsort(sortedSample, 0, 999);

                for(int i = 0; i < 1000; i++)
                {
                    sw.Write(hugeSample[i]);

                    if (i < 20)
                        sw.Write("\t" + sample[i]);

                    sw.WriteLine();
                }

                sw.Close();
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc);
            }
        }

        static public int GetEmissionsCount(fDistr f, double first, double second, int count)
        {
            double Q1;
            double Q3;
            double X1;
            double X2;
            double[] sample;
            int res;

            sample = GetSample(f, first, second, count);

            _qsort(sample, 0, count - 1);

            if (count % 4 == 0)
            {
                Q1 = sample[count / 4];
                Q3 = sample[count * 3 / 4];
            }
            else
            {
                Q1 = sample[count / 4 + 1];
                Q3 = sample[count * 3 / 4 + 1];
            }

            X1 = Q1 - 3 / 2 * (Q3 - Q1);
            X2 = Q3 + 3 / 2 * (Q3 - Q1);

            res = 0;

            for (int i = 0; sample[i] < X1; i++)
                res++;

            for (int i = count - 1; sample[i] > X2; i--)
                res++;

            return res;
        }

        static public double[] GetAverageEmissionsCount(fDistr f, double first, double second)
        {
            double[] res = new double[2];
            int temp;

            res[0] = 0;
            res[1] = 0;

            for(int i = 0; i < 1000; i++)
            {
                temp = GetEmissionsCount(f, first, second, 20);

                res[0] += (double)temp / 20;
            }

            res[0] /= 1000;

            for (int i = 0; i < 1000; i++)
            {
                temp = GetEmissionsCount(f, first, second, 100);

                res[1] += (double)temp / 100;
            }

            res[1] /= 1000;

            return res;
        }

        static public void PrepareEmissions(fDistr[] f, double[] first, double[] second, string filename)
        {
            try
            {
                StreamWriter sw = new StreamWriter(filename);
                double[] t;

                for (int i = 0; i < f.Length; i++)
                {
                    t = GetAverageEmissionsCount(f[i], first[i], second[i]);

                    sw.WriteLine(f[i].ToString() + "\t" + t[0] + "\t" + t[1]);
                }

                sw.Close();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
        }

        #endregion

        #region Lab4

        static public double[,] GetEmpiricDist(fDistr f, double first, double second, int count, string filename, double left = -1, double rigth = 1)
        {
            double[] sample;
            double[,] res;
            double[,] temp;
            int columns = 0;
            double t;

            sample = GetSample(f, first, second, count, left, rigth);

            _qsort(sample, 0, count - 1);

            sample[0] = Math.Floor(sample[0] * 100) / 100;
            t = sample[0];

            for (int i = 1; i < count; i++)
            {
                sample[i] = Math.Floor(sample[i] * 100) / 100;
                if (t != sample[i])
                {
                    t = sample[i];
                    columns++;
                }
            }

            temp = new double[2, columns];

            t = sample[0];
            int column = 0;
            int n = 1;

            for (int i = 1; i < count; i++)
            {
                n++;

                if (t != sample[i])
                {
                    t = sample[i];
                    temp[0, column] = sample[i - 1];
                    temp[1, column] = n;
                    n = 0;
                    column++;
                }
            }

            res = new double[2, columns];

            res[0, 0] = temp[0, 0];
            res[1, 0] = temp[1, 0] / count;

            for (int i = 1; i < columns; i++)
            {
                res[0, i] = temp[0, i];

                res[1, i] = res[1, i - 1] + temp[1, i] / count;
            }

            try
            {
                StreamWriter sw = new StreamWriter(filename);

                for (int i = 0; i < columns; i++)
                {
                    sw.WriteLine(res[0, i] + "\t" + res[1, i]);
                }

                sw.Close();
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc);
            }

            return res;
        }

        //Gauss ker
        static private double _ker(double u)
        {
            return Math.Exp(-0.5 * u * u) / Math.Sqrt(2 * Math.PI);
        }

        static public double KerDens(double[] sample, double x)
        {

            double sum = 0;
            double h = 1;

            for(int i = 0; i < sample.Length; i++)
            {
                sum += _ker((x - sample[i]) / h);
            }

            return sum / (sample.Length * h);
        }

        static private void _compressSample(double[] sample)
        {
            for (int i = 0; i < sample.Length; i++)
            {
                sample[i] = Math.Floor(sample[i] * 100) / 100;
            }
        }

        static public void PrepareKerDens(fDistr f, fDistr dens, double first, double second, int count, string filename)
        {
            try
            {
                StreamWriter sw = new StreamWriter(filename);
                double[] sample;
                double t;
                
                sample = GetSample(f, first, second, count);

                _qsort(sample, 0, count - 1);

                _compressSample(sample);

                t = sample[0];
                sw.WriteLine("Density\tKer\tx");
                sw.WriteLine(dens(t, first, second) + "\t" + KerDens(sample, t) + "\t" + t);

                for(int i = 1; i < count; i++)
                {
                    if(t != sample[i])
                    {
                        t = sample[i];
                        sw.WriteLine(dens(t, first, second) + "\t" + KerDens(sample, t) + "\t" + t);
                    }
                }

                sw.Close();
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc);
            }
        }

        #endregion
    }


}
