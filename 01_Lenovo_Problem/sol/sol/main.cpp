#include <cmath>
#include <cstdio>
#include <vector>
#include <iostream>
#include <algorithm>
#include <string>
#include <sstream>
#include <assert.h>

using namespace std;


void printError()
{
    cout << "ERROR" << endl;
    assert(false);
    exit(-1);
}

bool validResidue(const char& c)
{
    return (c == 'A') || (c == 'C') || (c == 'G') || (c == 'T');
}

bool validSequence(const string& str)
{
    for (int i=0; i<str.length(); i++)
    {
        if (!validResidue(str[i]))
        {
            return false;
        }
    }
    return true;
}

int main() {

    int n = -1;

    string nStr;
    getline(cin, nStr);

    stringstream convert(nStr);
    if ( !(convert >> n) )
    {
        n = -1;
    }

    // We need to manages the case of n==0 (no sequences and output the no alignment string)
    if (n < 0 || n > 5)
    {
        printError();
        assert(false);
    }


    vector<string> sequences(n);
    for (int i=0; i<n; i++)
    {
        string sequenceStr;
        getline(cin, sequenceStr);
        transform(sequenceStr.begin(), sequenceStr.end(),sequenceStr.begin(), ::toupper);
        if (!validSequence(sequenceStr))
        {
            printError();
        }
        sequences.push_back(sequenceStr);
    }

    int k = -1;
    string kStr;
    getline(cin, kStr);
    stringstream convert2(nStr);
    if ( !(convert2 >> k) )
    {
        k = -1;
    }

    if (k == -1)
    {
        printError();
        assert(false);
    }

    vector<int> posAlignmentsOutput(k);

    for(int i=0; i<k; i++)
    {
        // We already do the conversion from 1 based to 0 based.
        // So at output, use the index directly (no index-1) !!!!

        string indexStr1Based;
        getline(cin, indexStr1Based);
        int index1Based = -1;

        stringstream convert3(indexStr1Based);
        if ( !(convert3 >> index1Based) )
        {
            index1Based = -1;
        }

        assert(index1Based-1 >= 0);
        posAlignmentsOutput.push_back(index1Based - 1);
    }













    return 0;
}
