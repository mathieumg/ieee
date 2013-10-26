rawInput = input() #seed for the LCG
splitedInput = rawInput.split(",")

a = int(splitedInput[0])
b = int(splitedInput[1])
counter = 0

if a==b :
    binNb = bin(a)
    binNb = binNb[2:]
    # binList = [int(x) for x in list('{0:0b}'.format(8))]
    if str(binNb) == str(binNb)[::-1] :
        counter = counter + 1
else :
    for i in range(a,b):
        binNb = bin(i)
        binNb = binNb[2:]
        # binList = [int(x) for x in list('{0:0b}'.format(8))]
        if str(binNb) == str(binNb)[::-1] :
            counter = counter + 1

print(counter)