#include <cmath>
#include <cstdio>
#include <vector>
#include <iostream>
#include <algorithm>
#include <string>
#include <sstream>
#include <assert.h>
#include <unordered_set>

using namespace std;


void printError()
{
    cout << "ERROR" << endl;
    assert(false);
    exit(-1);
}

bool validResidue(const char& c)
{
    return (c == 'A') || (c == 'C') || (c == 'G') || (c == 'T') || (c == '-');
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



// Recursive calls adding one "-" at every position every time
void generateAlignments(int size, const string& sequence, vector<string>& outAlignments)
{
    int seqLength = sequence.length();

    if (seqLength == size)
    {
        return;
    }

    for (int i=0; i<seqLength+1; i++)
    {
        string newSeq = sequence;
        newSeq = newSeq.insert(i, 1, '-');
        int newSeqL = newSeq.length();
        if (newSeqL == 0)
        {
            int bob = 0;
        }
        outAlignments.push_back(newSeq);
        generateAlignments(size, newSeq, outAlignments);
    }
}


void generateAlignmentsUnique(int size, const string& sequence, unordered_set<string>& outAlignments)
{
    vector<string> outValsNonUnique;
    outValsNonUnique.push_back(sequence);
    generateAlignments(size, sequence, outValsNonUnique);

    outAlignments.reserve(outValsNonUnique.size());
    for (auto it = outValsNonUnique.begin(); it!=outValsNonUnique.end(); it++)
    {
        outAlignments.insert(*it);
    }
}



bool keepAlignment(const vector<const string*>& alignments)
{
    int nbAlignments = alignments.size();
    if (nbAlignments < 2)
    {
        return true;
    }

    int l = alignments[0]->length();
    for (int i=1; i<alignments.size(); i++)
    {
        if (alignments[i]->length()!=l)
        {
            return false;
        }
        l = alignments[i]->length();
    }

    
    bool goodValues = false;
    for (int i=0; i<l; i++)
    {
        char charAlignment = '-';
        bool goodColumn = true;
        for (int j=0; j<nbAlignments; j++)
        {
            char charAt = alignments[j]->at(i);
            if (charAlignment == '-')
            {
                charAlignment = alignments[j]->at(i);
                continue;
            }
            if (charAt != '-' && charAlignment != alignments[j]->at(i))
            {
                goodColumn = false;
            }
        }

        // If the value is still '-', there was an entire column with '-'. We must then ignore it
        if (charAlignment == '-')
        {
            return false;
        }

        goodValues |= goodColumn;
    }

    

    return goodValues;
}


int operatorCharSmaller(const char& c1, const char& c2)
{
    char toCompare1 = c1;
    char toCompare2 = c2;

    if (toCompare1 == '-')
    {
        toCompare1  = 'Z';
    }

    if (toCompare2 == '-')
    {
        toCompare2 = 'Z';
    }

    bool eq = toCompare1 == toCompare2;
    if (eq)
    {
        return 0;
    }
    else if (toCompare1 < toCompare2)
    {
        return -1;
    }
    else
    {
        return 1;
    }

}


struct compareFunction
{
    bool operator()(const vector<const string*>& a, const vector<const string*>& b) const
    {
        int l1 = a[0]->length();
        int l2 = b[0]->length();

        bool eq = l1 == l2;
        if (!eq)
        {
            return l1 < l2;
        }

        for (int i=0; i<a.size(); i++)
        {
            const char* s1 = a[i]->c_str();
            const char* s2 = b[i]->c_str();
            int len = strlen(s1);

            for (int iChar=0; iChar < len; iChar++)
            {
                int v = operatorCharSmaller(s1[iChar], s2[iChar]);
                if (v!=0)
                {
                    return v < 0;
                }
            }
        }
        return false;
    }
};


void addValues(const vector<const string*>& arr, vector<vector<const string*>>& filteredAlignments)
{
    if (keepAlignment(arr))
    {
        filteredAlignments.push_back(arr);
    }
}


void printPossibilityCount(int count)
{
    cout << "PossibleAlignments: " << count << endl;
}

void printNoAlignment(int pos)
{
    cout << "There is no alignment at position: " << pos << endl;
}

void printAlignmentPosition(int pos, const vector<const string*>& alignment )
{
    cout << "Alignment at Position: " << pos << endl;
    for (auto it=alignment.begin(); it!=alignment.end(); it++)
    {
        cout << *(*it) << endl;
    }
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


    vector<string> sequences;
    vector<int> sequencesLength;
    int maxSequenceLength = 0;
    int secondMaxSequenceLength = 0;
    for (int i=0; i<n; i++)
    {
        string sequenceStr;
        getline(cin, sequenceStr);
        remove(sequenceStr.begin(), sequenceStr.end(), ' ');
        transform(sequenceStr.begin(), sequenceStr.end(),sequenceStr.begin(), ::toupper);
        if (!validSequence(sequenceStr))
        {
            printError();
        }
        sequences.push_back(sequenceStr);
        int seqLength = sequenceStr.length();
        if (seqLength > maxSequenceLength)
        {
            secondMaxSequenceLength = maxSequenceLength;
            maxSequenceLength = seqLength;
        }
        else if (seqLength > secondMaxSequenceLength)
        {
            secondMaxSequenceLength = seqLength;
        }
        sequencesLength.push_back(seqLength);
    }


    int k = -1;
    string kStr;
    getline(cin, kStr);
    stringstream convert2(kStr);
    if ( !(convert2 >> k) )
    {
        k = -1;
    }

    if (k == -1)
    {
        printError();
        assert(false);
    }

    vector<int> posAlignmentsOutput;
    posAlignmentsOutput.reserve(k);

    for(int i=0; i<k; i++)
    {

        string indexStr1Based;
        getline(cin, indexStr1Based);
        int index1Based = -1;

        stringstream convert3(indexStr1Based);
        if ( !(convert3 >> index1Based) )
        {
            index1Based = -1;
        }

        posAlignmentsOutput.push_back(index1Based);
    }

    // End Input management


    //////////////////////////////////////////////////////////////////////////////////

    if (n >= 2)
    {
        // This is the maximum length that the sequences will have
        int maxLengthWithPadding = maxSequenceLength+secondMaxSequenceLength;

        vector<unordered_set<string>> allPossibleAlignments(sequences.size());

        for (int i=0; i<sequences.size(); i++)
        {
            generateAlignmentsUnique(maxLengthWithPadding, sequences[i], allPossibleAlignments[i]);
        }

        vector<vector<const string*>> filteredAlignments;
        int t = 0;
        vector<const string*> testVector;
        for (unordered_set<string>::iterator it0 = allPossibleAlignments[0].begin(); it0!=allPossibleAlignments[0].end(); it0++)
        {
            testVector.push_back(&(*it0));
            for (auto it1 = allPossibleAlignments[1].begin(); it1!=allPossibleAlignments[1].end(); it1++)
            {
                testVector.push_back(&(*it1));
                if (n >=3)
                {
                    for (auto it2 = allPossibleAlignments[2].begin(); it2!=allPossibleAlignments[2].end(); it2++)
                    {
                        testVector.push_back(&(*it2));
                        if (n >=4)
                        {
                            for (auto it3 = allPossibleAlignments[3].begin(); it3!=allPossibleAlignments[3].end(); it3++)
                            {
                                testVector.push_back(&(*it3));
                                if (n >=5)
                                {
                                    for (auto it4 = allPossibleAlignments[4].begin(); it4!=allPossibleAlignments[4].end(); it4++)
                                    {
                                        testVector.push_back(&(*it4));

                                        addValues(testVector, filteredAlignments);

                                        testVector.pop_back();
                                    }
                                }
                                else
                                {
                                    addValues(testVector, filteredAlignments);
                                }
                                testVector.pop_back();
                            }
                        }
                        else
                        {
                            addValues(testVector, filteredAlignments);
                        }
                        testVector.pop_back();
                    }
                }
                else
                {
                    addValues(testVector, filteredAlignments);
                }
                testVector.pop_back();
            }
            testVector.pop_back();
        }

        sort(filteredAlignments.begin(), filteredAlignments.end(), compareFunction());
        

        printPossibilityCount(filteredAlignments.size());
        for (int p=0; p<k; p++)
        {
            int pos = posAlignmentsOutput[p];
            if (pos >= 1 && pos <= filteredAlignments.size())
            {
                printAlignmentPosition(pos, filteredAlignments[pos-1]);
            }
            else
            {
                printNoAlignment(pos);
            }
        }
    }
    else
    {
        // In that case, no matches because there is 0 or 1 sequences
        // The out put must be something like: "There is no alignment at position: X"
        printPossibilityCount(0);
        for (int p=0; p<k; p++)
        {
            printNoAlignment(posAlignmentsOutput[p]);
        }
    }


    system("pause");
    return 0;
}
