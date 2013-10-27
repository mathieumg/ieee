from sys import stdin
from collections import defaultdict
from string import maketrans
import sys
#input=stdin.read()
input='''5  
ball  
belongs  
red  
the  
to  
  
SJI XIL TEMM TIMUBPK SU ETI'''
lines = input.splitlines()

cypher = lines[-1].split()

tableWordSize = defaultdict(list)
cypherWordSize = defaultdict(list)

answerDict = {}
decrypted = {}

unMappedYet=list(str.upper("abcdefghijklmnopqrstuvwxyz"))
mapped = list()
