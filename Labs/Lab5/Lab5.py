import numpy as np
import math

def Rang(x, row):
    r = 0

    for i in row:
        if(i <= x):
            r += 1

    return r - 1

def Sample(ro, size):
    return np.random.multivariate_normal([0, 0], [[1, ro], [ro, 1]], size)

def Covariation(sample):
    xAvg = Average(sample[:, 0])
    yAvg = Average(sample[:, 1])
    s = 0

    for i in sample:
        s += (i[0] - xAvg) * (i[1] - yAvg)

    return s / len(sample)

def Pirson(sample):
    xDis = Dispersion(sample[:, 0])
    yDis = Dispersion(sample[:, 1])

    return Covariation(sample) / math.sqrt(xDis * yDis)

def Spearmen(sample):
    rAvg = math.floor((len(sample) + 1) / 2);
    avg = []
    sqAvgX = []
    sqAvgY = []

    for i in range(len(sample)):
        avg.append((Rang(sample[i, 0], sample[:, 0]) - rAvg) * (Rang(sample[i, 1], sample[:, 1]) - rAvg))
        sqAvgY.append(Rang(sample[i, 1], sample[:, 1]) - rAvg)
        sqAvgX.append(Rang(sample[i, 0], sample[:, 0]) - rAvg)

    return Average(avg) / math.sqrt(SqAverage(sqAvgX) * SqAverage(sqAvgY))


def SquaredCorrel(sample):
    medX = np.median(sample[:, 0])
    medY = np.median(sample[:, 1])

    n1 = 0
    n2 = 0
    n3 = 0
    n4 = 0

    for i in sample:
        if(i[0] < medX):
            if(i[1] < medY):
                n3 += 1
            else:
                n2 += 1
        else:
            if(i[1] < medY):
                n4 += 1
            else:
                n1 += 1

    return (n1 + n3 - n2 - n4) / len(sample)

def Combined(size):
    return 0.9 * Sample(0.9, size) + 0.1 * np.random.multivariate_normal([0, 0], [[10, -0.9], [-0.9, 10]], size)

def Average(row):
    s = 0

    for i in row:
        s += i

    s /= len(row)

    return s

def SqAverage(row):
    s = 0

    for i in row:
        s += i * i

    s /= len(row)

    return s

def Dispersion(row):
    t = Average(row)
    return SqAverage(row) - t * t

ro = [0, 0.5, 0.9]
n = [20, 60, 100]

pir = []
spe = []
sqc = []
print()

# for k in range(1000):
#     sample = Combined(60)
#     #sample = Sample(0.5, 60)
#     pir.append(Pirson(sample))
#     spe.append(Spearmen(sample))
#     sqc.append(SquaredCorrel(sample))
#
# print("Average: ", Average(pir), " ", SqAverage(pir), " ", Dispersion(pir))
# print("Average: ", Average(spe), " ", SqAverage(spe), " ", Dispersion(spe))
# print("Average: ", Average(sqc), " ", SqAverage(sqc), " ", Dispersion(sqc))


for i in ro:
    for j in n:
        pir = []
        spe = []
        sqc = []

        for k in range(1000):
            sample = Sample(i, j)
            pir.append(Pirson(sample))
            spe.append(Spearmen(sample))
            sqc.append(SquaredCorrel(sample))

        avgPir = Average(pir)
        avgSpe = Average(spe)
        avgSqc = Average(sqc)

        sqAvgPir = SqAverage(pir)
        sqAvgSpe = SqAverage(spe)
        sqAvgSqc = SqAverage(sqc)

        dispPir = Dispersion(pir)
        dispSpe = Dispersion(spe)
        dispSqc = Dispersion(sqc)

        print("ro = ", i, " ; n = ", j)
        print("Average: ", avgPir, " ", avgSpe, " ", avgSqc)
        print("Squared average: ", sqAvgPir, " ", sqAvgSpe, " ", sqAvgSqc)
        print("Dispersion: ", dispPir, " ", dispSpe, " ", dispSqc)