using System;
using System.Collections.Generic;
using System.IO;



public class DataContainer
{
    private string _StringBuffer;
    private Stack<string> _StringStack = new Stack<string>();
    private int _MaxMessageSize;

    public DataContainer(int maxMessageSize)
    {
        _MaxMessageSize = maxMessageSize;
        _StringBuffer = "";
        _MaxMessageSize = maxMessageSize;
    }

    public string GetStringData()
    {
        string data = _StringBuffer;
        while(_StringStack.Count > 0)
        {
            data = _StringStack.Pop() + data;
        }
        return data;
    }

    // Will add the most string to the stack. Returns false if there is no way that the string is in the dictionary 
    public bool AddString(string str)
    {
        _StringBuffer += str;
        return StackData();
    }

    public bool Finalize()
    {
        bool retVal=true;
        while (retVal&&_StringBuffer.Length>_MaxMessageSize)
        {
            retVal=StackData();
        }
        return retVal;
    }

    private bool StackData()
    {
        if (_StringBuffer.Length < _MaxMessageSize)
        {
            return true;
        }

        if (!StackPartial(_MaxMessageSize))
        {
            // Not found, maybe there was an error, we need to unstack
            if (_StringStack.Count == 0)
            {
                // There was an error because we unstacked to the root. The string must not be contained in the dict
                return false;
            }
            else
            {
                String unstacked = _StringStack.Pop();
                _StringBuffer=unstacked+_StringBuffer;

                if (!StackPartial(unstacked.Length-1))
                {
                    return false;
                }
                else
                {
                    // Restack as much as possible
                    return Finalize();
                }
            }
        }
        return true;
    }

    private bool StackPartial(int maxLength)
    {
        bool foundInDict=false;
        // Try to stack, if there is an error, unstack and use smaller size
        for (int i=maxLength;i>0;i--)
        {
            string subStr=_StringBuffer.Substring(0, i);
            if (IsInDict(subStr))
            {
                foundInDict=true;
                _StringBuffer=_StringBuffer.Substring(i);
                _StringStack.Push(subStr);
                break;
            }
        }
        return foundInDict;
    }


    private bool IsInDict(string str)
    {
        return Solution.Dict.ContainsKey(str);
    }
}


class Solution
{

    static Char AChar='A';

    static string ShiftString(string str, int delta)
    {
        string outStr = "";
        for (int i=0; i<str.Length; i++)
        {
            Char c = str[i];
            int d = c-AChar;
            d = d-delta;
            while (d<0)
            {
                d = d+26;
            }
            Char c2 = (char)(d+AChar);
            outStr += c2;
        }
        return outStr;
    }


    public static Dictionary<String, String> Dict = new Dictionary<String, String>();


    static void Main(String[] args)
    {
        
        using (TextReader tr=System.Console.In)
        {
            String codedString = tr.ReadLine();
            codedString = codedString.ToUpper();
            codedString = codedString.Trim();
            tr.ReadLine(); // Blank line wtf

            String nbDictStr = tr.ReadLine();
            int nbDict = Convert.ToInt32(nbDictStr);

            String fullDict = tr.ReadLine();
            fullDict = fullDict.Trim();

            String[] dictContent = fullDict.Split(' ');
            int longWordMax = 0;
            
            foreach (String d in dictContent)
            {
                String d2 = d.Trim();
                if (d2.Length > 0)
                {
                    if (d2.Length>longWordMax)
                    {
                        longWordMax = d2.Length;
                    }
                    Solution.Dict[d2] = d2;
                }
            }

            int blockSize=10;
            for (;blockSize>0;blockSize--)
            {
                List<string> blocks = new List<string>();
                double l=(double)codedString.Length;
                double bs=(double)blockSize;
                int nbBlocks = (int) Math.Ceiling(l/bs);
                int n = 0;
                for (n=0;n<(nbBlocks-1);n++)
                {
                    blocks.Add(codedString.Substring(n*blockSize, blockSize));
                }
                blocks.Add(codedString.Substring(n*blockSize));

                int[] k = new int[4];
                for (int bob = 0; bob < 4; bob++)
                {
                    k[bob] = 0;
                }

                for (k[1]=0;k[1]<26;k[1]++)
                {
                    for (k[2]=0;k[2]<26;k[2]++)
                    {
                        for (k[3]=0;k[3]<26;k[3]++)
                        {
                            DataContainer dc = new DataContainer(longWordMax);

                            for (int c=0;c<nbBlocks;c++)
                            {
                                int kIndex = (c%3) + 1;
                                int delta=k[kIndex];

                                string val = ShiftString(blocks[c], delta);
                                if (!dc.AddString(val))
                                {
                                    break;
                                }

                                if (c==nbBlocks-1)
                                {
                                    if (dc.Finalize())
                                    {
                                        // Done
                                        string finalMassage = dc.GetStringData();
                                        Console.Out.WriteLine(blockSize);
                                        Console.Out.WriteLine(k[1]);
                                        Console.Out.WriteLine(k[2]);
                                        Console.Out.WriteLine(k[3]);
                                        Console.Out.WriteLine(finalMassage);
                                        Environment.Exit(0);
                                    }
                                    else
                                    {
                                        // Error
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}