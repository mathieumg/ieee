from sys import stdin
#input=stdin.read()
input='''7 7
2 2 2 1 2 3 4 
2 2 1 2 1 2 5
4 1 1 2 1 1 5 
1 2 2 3 3 4 1 
4 1 1 2 2 1 4
3 4 1 4 1 2 1
1 2 1 1 1 2 1'''
lines = input.splitlines()
M,N= map(int,lines[0].split())
mat = [list(map(int, line.split())) for line in lines[1:]]

#for line in mat:
#    print(line)

curLine = 0

