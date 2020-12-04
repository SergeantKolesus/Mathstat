import numpy as np
from scipy.stats import chi2, t, norm, moment
import math

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

def GetSample(size):
    return np.random.normal(0, 1, size = size)

def GetMNormal(samples, alpha):
    med = Average(samples)
    n = len(samples)
    s = math.sqrt(Dispersion(samples))
    t_a = t.ppf(1 - alpha / 2, n - 1)
    q_1 = med - s * t_a / math.sqrt(n - 1)
    q_2 = med + s * t_a / math.sqrt(n - 1)
    return q_1, q_2


def GetSigmaNormal(samples, alpha):
    n = len(samples)
    s = math.sqrt(Dispersion(samples))

    q_1 =  s * math.sqrt(n) / math.sqrt(chi2.ppf(1 - alpha / 2, n - 1))
    q_2 = s * math.sqrt(n) / math.sqrt(chi2.ppf(alpha / 2, n - 1))

    return q_1, q_2


def GetMAssim(samples, alpha):
    med = Average(samples)
    n = len(samples)
    s = math.sqrt(Dispersion(samples))

    u = norm.ppf(1 - alpha / 2)

    q_1 = med - s * u / math.sqrt(n)
    q_2 = med + s * u / math.sqrt(n)

    return q_1, q_2


def GetSigmaAssim(samples, alpha):
    med = Average(samples)
    n = len(samples)
    s = np.std(samples)

    u = norm.ppf(1 - alpha / 2)
    m4 = moment(samples, 4)

    e = m4 / (s * s * s * s)
    U = u * math.sqrt((e + 2) / n)
    q_1 = s / math.sqrt(1 + U)
    q_2 = s / math.sqrt(1 - U)

    return q_1, q_2


alpha = 0.05

sample20 = GetSample(20)
sample100 = GetSample(100)

print("Not assim")

print("n = 20, m : ", GetMNormal(sample20, alpha))
print("n = 20, sigma : ", GetSigmaNormal(sample20, alpha))

print("n = 100, m : ", GetMNormal(sample100, alpha))
print("n = 100, sigma : ", GetSigmaNormal(sample100, alpha))

print("Assim")

print("n = 20, m : ", GetMAssim(sample20, alpha))
print("n = 20, sigma : ", GetSigmaAssim(sample20, alpha))

print("n = 100, m : ", GetMAssim(sample100, alpha))
print("n = 100, sigma : ", GetSigmaAssim(sample100, alpha))