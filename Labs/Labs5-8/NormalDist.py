import numpy as np

def sample(ro, size):
    np.random.multivariate_normal([0, 0], [[1, ro], [ro, 1]], size)

sample(0, 20)
