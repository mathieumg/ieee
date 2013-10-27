from sys import stdin
import functools
#input=stdin.read()
input='''How Hfv
are aid
you yft
.'''
lines = input.splitlines()
mat = [list(map(str, line)) for line in lines]

a = [ord(x.lower())-96 for x in mat[0]]
dict = {'a': 1,'b': 2, 'Cecil': '3258'}
for line in mat:
    print([ord(char) - 96 for char in str(line).lower()])
