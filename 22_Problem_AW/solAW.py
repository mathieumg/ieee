from sys import stdin
from collections import defaultdict
#input=stdin.read()
input='''2  
case  
simple  
  
AJWHPU GXAU'''
lines = input.splitlines()
cypher = lines[-1].split()
decrypted = {}
tableWordSize = defaultdict(list)
cypherWordSize = defaultdict(list)
answerDict = {}

for line in lines[1:-2] :
    line = line[:-2]
    tableWordSize[len(line)].append(line)

for word in cypher :
    cypherWordSize[len(word)].append(word)

# check for words with only 1 occurence
for cypherW in cypherWordSize :
    if len(cypherWordSize[cypherW]) == 1 :
       # found match
       charFound = cypherWordSize.get(cypherW)

       for i in range(cypherW) :
           answerDict[charFound[0][i]] = tableWordSize.get(cypherW)[0][i]

#replace found letters in decrypted
decrypted = cypher
lsDec = list(decrypted)
finalOutput=""
for wordIdx in range(len(cypher)) :
    lsDec = list(decrypted[wordIdx])
    for letterIdx in range(len(cypher[wordIdx])) :
        lsDec[letterIdx] = answerDict[cypher[wordIdx][letterIdx]]
    if wordIdx != 0 :
        finalOutput = finalOutput + " "
    finalOutput = finalOutput + "".join(lsDec)

print(finalOutput.upper())