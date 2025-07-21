import numpy as np
import pandas as pd
from pandas.core import apply

# Q, l starts from 0
# params
L=[0,1,5,10,19,9999]
K1 = 1
K2 = 1
A = 2
S = 31000 # k= 31000 ( 链式霰弹枪)
K3=1
CM=1.0
CT=0

u = lambda RQ,work,Q: (K1*A**RQ + K2*np.log(work)/np.log(S)+CM ) if RQ > 0 else 0
T = lambda RQ,work,Q: (K3*np.log(work)/np.log(S) * np.log((RQ+1))+CT) if RQ > 0 else 0

def getRQ(l,Q):
    for i in range(len(L)):
        if(L[i]>l):
            return Q - (i-1)

def tab(s):
    print("s = ", s)
    mood_buffs = [[ u(RQ=getRQ(l,Q),work=s,Q=Q) for Q in range(0,7) ] for l in range(0,21)]
    durations = [[ T(RQ=getRQ(l,Q),work=s,Q=Q) for Q in range(0,7) ] for l in range(0,21)]

    # P = 0
    mood_buffs[0][4] = 0
    mood_buffs[0][5] = 0
    mood_buffs[1][5] = 0
    mood_buffs[2][5] = 0
    mood_buffs[3][5] = 0

    durations[0][4] = 0
    durations[0][5] = 0
    durations[1][5] = 0
    durations[2][5] = 0
    durations[3][5] = 0


    t = [ [f"{round(mood_buffs[i][j],1)}x{round(durations[i][j],1)}" for j in range(7)] for i in range(21) ]

    df = pd.DataFrame(t)

    print(df)

tab(31000)
tab(60000)

print(T(RQ=getRQ(8,3),work=105000,Q=3))
print(T(RQ=getRQ(8,3),work=31000,Q=3))
