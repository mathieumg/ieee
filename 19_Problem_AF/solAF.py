from sys import stdin
import sys

def contains_dig(d):
    return any(int(char)==7 for char in str(d))

def isDivisible(d):
    return d%7==0

#input=stdin.read()
input='''2
10
17'''

lines = input.splitlines()
nbLines = lines[0].split() 


yellCounter = 1
position = 1
inc = 1

for favNbIdx in range(1,len(lines)):
    yellCounter = 1
    position = 1
    inc = 1
    while yellCounter !=int(lines[favNbIdx]) :
        if contains_dig(yellCounter) or isDivisible(yellCounter) :
            inc = inc * -1
        yellCounter = yellCounter + 1
        if position == 1 and inc == -1:
            position = 1337
        elif position == 1337 and inc == 1:
            position = 1
        else :
            position = position + inc
    print(position)


    

    