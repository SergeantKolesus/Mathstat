using System;
using System.IO;
using System.Security.Cryptography;

namespace Labs1_4
{
    class Program
    {
        #region Lab1

        static private void _prepareDistribution(Lab1_4.fDistr f, Lab1_4.fDistr dens, double first, double second, string name)
        {
            Console.WriteLine("################################");
            Console.WriteLine(name + " distribution");
            Console.WriteLine();

            Console.WriteLine("Preparing 10 elements file");
            Lab1_4.PrepareHistogram(f, dens, first, second, 10, (string)("10" + name + "Hist.xls"));
            Console.WriteLine();

            Console.WriteLine("Preparing 50 elements file");
            Lab1_4.PrepareHistogram(f, dens, first, second, 50, (string)("50" + name + "Hist.xls"));
            Console.WriteLine();

            Console.WriteLine("Preparing 1000 elements file");
            Lab1_4.PrepareHistogram(f, dens, first, second, 1000, (string)("1000" + name + "Hist.xls"));
            Console.WriteLine();

            Console.WriteLine("Completed " + name);
            Console.WriteLine("################################");
            Console.WriteLine();
        }

        static private void _lab1()
        {
            Console.WriteLine("################################################################");
            Console.WriteLine("Lab1");
            Console.WriteLine();

            _prepareDistribution(Distributions.NormalRandom, Distributions.NormalDistributionDensity, 0, 1, "Normal");

            _prepareDistribution(Distributions.CauchyRandom, Distributions.CauchyDistributionDensity, 0, 1, "Cauchy");

            _prepareDistribution(Distributions.LaplaceRandom, Distributions.LaplaceDistributionDensity, 0, 1 / Math.Sqrt(2), "Laplace");

            _prepareDistribution(Distributions.PoissonRandom, Distributions.PoissonDistributionDensity, 10, 0, "Poisson");

            _prepareDistribution(Distributions.UniformRandom, Distributions.UniformDistributionDensity, -Math.Sqrt(3), Math.Sqrt(3), "Uniform");

            Console.WriteLine("Completed Lab1");
            Console.WriteLine("################################################################");
        }

        #endregion

        #region Lab2

        static private void _prepareChars(Lab1_4.fDistr f, double first, double second, string filename, string name)
        {
            Console.WriteLine("################################");
            Console.WriteLine(name + " distribution");
            Console.WriteLine();

            Console.WriteLine("Preparing file with characteristics");
            Console.WriteLine();

            Lab1_4.Task2(f, filename, first, second, new int[3] { 10, 100, 1000 });

            Console.WriteLine("Completed " + name);
            Console.WriteLine("################################");
            Console.WriteLine();

            
        }

        static private void _lab2()
        {
            Console.WriteLine("################################################################");
            Console.WriteLine("Lab2");
            Console.WriteLine();

            _prepareChars(Distributions.NormalRandom, 0, 1, "NormalCharacteristics.xls", "Normal");

            _prepareChars(Distributions.CauchyRandom, 0, 1, "CauchyCharacteristics.xls", "Cauchy");

            _prepareChars(Distributions.LaplaceRandom, 0, 1 / Math.Sqrt(2), "LaplaceCharacteristics.xls", "Laplace");

            _prepareChars(Distributions.PoissonRandom, 10, 0, "PoissonCharacteristics.xls", "Poisson");

            _prepareChars(Distributions.UniformRandom, -Math.Sqrt(3), Math.Sqrt(3), "UniformCharacteristics.xls", "Uniform");


            Console.WriteLine("Completed Lab2");
            Console.WriteLine("################################################################");
        }

        #endregion

        #region Lab3

        static private void _prepareBoxplot(Lab1_4.fDistr f, double first, double second, string filename, string name)
        {
            Console.WriteLine("################################");
            Console.WriteLine(name + " distribution");
            Console.WriteLine();

            Console.WriteLine("Preparing boxplot");
            Console.WriteLine();

            Lab1_4.PrepareBoxplots(f, first, second, filename);

            Console.WriteLine("Completed " + name);
            Console.WriteLine("################################");
            Console.WriteLine();
        }

        static private void _lab3()
        {
            Console.WriteLine("################################################################");
            Console.WriteLine("Lab3");
            Console.WriteLine();

            _prepareBoxplot(Distributions.NormalRandom, 0, 1, "NormalBoxplot.xls", "Normal");

            _prepareBoxplot(Distributions.CauchyRandom, 0, 1, "CauchyBoxplot.xls", "Cauchy");

            _prepareBoxplot(Distributions.LaplaceRandom, 0, 1 / Math.Sqrt(2), "LaplaceBoxplot.xls", "Laplace");

            _prepareBoxplot(Distributions.PoissonRandom, 10, 0, "PoissonBoxplot.xls", "Poisson");

            _prepareBoxplot(Distributions.UniformRandom, -Math.Sqrt(3), Math.Sqrt(3), "UniformBoxplot.xls", "Uniform");

            Console.WriteLine("################################");

            Console.WriteLine("Preparing emissions");
            Console.WriteLine();

            Lab1_4.PrepareEmissions(new Lab1_4.fDistr[5] { Distributions.NormalRandom, Distributions.CauchyRandom, Distributions.LaplaceRandom, Distributions.PoissonRandom, Distributions.UniformRandom },
                new double[5] { 0, 0, 0, 10, -Math.Sqrt(3) },
                new double[5] { 1, 1, 1, 0, Math.Sqrt(3) },
                "Emissions.xls");

            Console.WriteLine("Completed");
            Console.WriteLine("################################");
            Console.WriteLine();

            Console.WriteLine("Completed Lab3");
            Console.WriteLine("################################################################");
        }

        #endregion

        #region Lab4

        static private void _prepareKer(Lab1_4.fDistr f, Lab1_4.fDistr dens, double first, double second, string name)
        {
            Console.WriteLine("################################");
            Console.WriteLine(name + " distribution");
            Console.WriteLine();

            Console.WriteLine("Preparing boxplot");
            Console.WriteLine();
            Console.WriteLine("20");

            Lab1_4.PrepareKerDens(f, dens, first, second, 20, "20" + name + "KerDens.xls");

            Console.WriteLine("60");

            Lab1_4.PrepareKerDens(f, dens, first, second, 60, "60" + name + "KerDens.xls");

            Console.WriteLine("100");

            Lab1_4.PrepareKerDens(f, dens, first, second, 100, "100" + name + "KerDens.xls");

            Console.WriteLine("Completed " + name);
            Console.WriteLine("################################");
            Console.WriteLine();
        }

        static private void _prepareEmpiric(Lab1_4.fDistr f, double first, double second, string name)
        {
            Console.WriteLine("################################");
            Console.WriteLine(name + " distribution");
            Console.WriteLine();

            Console.WriteLine("Preparing boxplot");
            Console.WriteLine();
            Console.WriteLine("20");

            Lab1_4.GetEmpiricDist(f, first, second, 20, "20"+name+"Empiric.xls");

            Console.WriteLine("60");

            Lab1_4.GetEmpiricDist(f, first, second, 60, "60" + name + "Empiric.xls");

            Console.WriteLine("100");

            Lab1_4.GetEmpiricDist(f, first, second, 100, "100" + name + "Empiric.xls");

            Console.WriteLine("Completed " + name);
            Console.WriteLine("################################");
            Console.WriteLine();
            
        }

        static private void _lab4()
        {
            Console.WriteLine("################################################################");
            Console.WriteLine("Lab2");
            Console.WriteLine();

            _prepareEmpiric(Distributions.NormalRandom, 0, 1, "Normal");

            _prepareEmpiric(Distributions.CauchyRandom, 0, 1, "Cauchy");

            _prepareEmpiric(Distributions.LaplaceRandom, 0, 1 / Math.Sqrt(2), "Laplace");

            _prepareEmpiric(Distributions.PoissonRandom, 10, 0, "Poisson");

            _prepareEmpiric(Distributions.UniformRandom, -Math.Sqrt(3), Math.Sqrt(3), "Uniform");

            _prepareKer(Distributions.NormalRandom, Distributions.NormalDistributionDensity, 0, 1, "Normal");

            _prepareKer(Distributions.CauchyRandom, Distributions.CauchyDistributionDensity, 0, 1, "Cauchy");

            _prepareKer(Distributions.LaplaceRandom, Distributions.LaplaceDistributionDensity, 0, 1 / Math.Sqrt(2), "Laplace");

            _prepareKer(Distributions.PoissonRandom, Distributions.PoissonDistributionDensity, 10, 0, "Poisson");

            _prepareKer(Distributions.UniformRandom, Distributions.UniformDistributionDensity, -Math.Sqrt(3), Math.Sqrt(3), "Uniform");



            Console.WriteLine("Completed Lab2");
            Console.WriteLine("################################################################");
        }

        #endregion Lab4

        static void Main(string[] args)
        {
            _lab1();

            _lab2();

            _lab3();

            _lab4();
        }
    }
}
