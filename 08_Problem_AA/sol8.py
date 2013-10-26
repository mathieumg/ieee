def o(s):
    l=len(s)
    return set([a+b+c
                    for a in s for b in s for c in s]).__len__()==l*(l+1)*(l+2)//6
M=int(input())
N=3**M
i=1
s=M*[i]
while i:
    if s[i]-N:
        s[i]=s[i]+1
        if o(s[:i+1]):
            if i<M-1:
                i=i+1
                s[i]=s[i-1]
            else:
                N=s[-1]
    else:
        i=i-1
print(N)