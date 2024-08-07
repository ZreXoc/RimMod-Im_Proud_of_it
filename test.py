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

u = lambda QI,work,Q: (K1*A**QI + K2*np.log(work)/np.log(S)+CM + (Q-4 if Q >= 5 else 0)) if QI > 0 else 0
T = lambda QI,work,Q: (K3*work/S * QI+CT+(Q-4 if Q >= 5 else 0)) if QI > 0 else 0

def getQI(l,Q):
    for i in range(len(L)):
        if(L[i]>l):
            return Q - (i-1)

def tab(s):
    print("s = ", s)
    mood_buffs = [[ u(QI=getQI(l,Q),work=s,Q=Q) for Q in range(0,7) ] for l in range(0,21)]
    times = [[ T(QI=getQI(l,Q),work=s,Q=Q) for Q in range(0,7) ] for l in range(0,21)]

    # P = 0
    mood_buffs[0][4] = 0
    mood_buffs[0][5] = 0
    mood_buffs[1][5] = 0
    mood_buffs[2][5] = 0
    mood_buffs[3][5] = 0

    times[0][4] = 0
    times[0][5] = 0
    times[1][5] = 0
    times[2][5] = 0
    times[3][5] = 0


    t = [ [f"{round(mood_buffs[i][j],1)}x{round(times[i][j],1)}" for j in range(7)] for i in range(21) ]

    df = pd.DataFrame(t)

    print(df)

tab(31000)
tab(60000)

print(T(QI=getQI(8,3),work=105000,Q=3))
print(T(QI=getQI(8,3),work=31000,Q=3))