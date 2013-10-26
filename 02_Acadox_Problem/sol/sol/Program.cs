using System;
using System.Collections.Generic;
using System.IO;

public abstract class Operator
{
    public virtual int DoWork(int val1, int val2)
    {
        return FormatOverflowUnderflow(DoWorkActual(val1, val2));
    }

    public abstract int DoWorkActual(int val1, int val2);
    public virtual int GetNumArgs()
    {
        return 2;
    }

    public int FormatOverflowUnderflow(int val)
    {
        if (val > 0xFFFF)
        {
            return 0xFFFF;
        }
        else if (val < 0x0000)
        {
            return 0x0000;
        }
        return val;
    }
}

public class Plus : Operator
{
    public override int DoWorkActual(int val1, int val2)
    {
        return val1+val2;
    }
}

public class Minus : Operator
{
    public override int DoWorkActual(int val1, int val2)
    {
        return val1-val2;
    }
}

public class LogicalAnd : Operator
{
    public override int DoWorkActual(int val1, int val2)
    {

        return (val1&0xFFFF)&(val2&0xFFFF);
    }
}

public class LogicalOr : Operator
{
    public override int DoWorkActual(int val1, int val2)
    {
        return (val1&0xFFFF)|(val2&0xFFFF);
    }
}

public class LogicalNot : Operator
{
    public override int DoWorkActual(int val1, int val2)
    {
        return ~(val1&0xFFFF);
    }

    public override int GetNumArgs()
    {
        return 1;
    }
}

public class LogicalExclusiveOr : Operator
{
    public override int DoWorkActual(int val1, int val2)
    {
        return (val1&0xFFFF)^(val2&0xFFFF);
    }
}


class Solution
{

    static void PrintError()
    {
        System.Console.Out.WriteLine("ERROR");
        Environment.Exit(0);
    }

    static void PrintSolution(int val)
    {
        string outVal = val.ToString("X");
        outVal = outVal.ToUpper();
        outVal = outVal.PadLeft(4, '0');
        System.Console.Out.WriteLine(outVal);
    }

    static Operator GetOperator(string str)
    {
        switch (str)
        {
            case "+":
                return new Plus();
            case "-":
                return new Minus();
            case "&":
                return new LogicalAnd();
            case "|":
                return new LogicalOr();
            case "~":
                return new LogicalNot();
            case "X":
                return new LogicalExclusiveOr();
        }
        return null;
    }


    static void Main(String[] args)
    {
        System.Collections.Stack stack = new System.Collections.Stack();
        using (TextReader tr = System.Console.In)
        {
                try
                {
                    stack.Clear();
                    string line=tr.ReadLine();
                    line=line.Trim();
                    line=line.ToUpper();
                    string[] tokensStr=line.Split(' ');

                    if (tokensStr.Length>20||line.Length == 0)
                    {
                        Solution.PrintError();
                    }

                    foreach (string str in tokensStr)
                    {
                        Operator op=Solution.GetOperator(str);

                        if (op==null)
                        {
                            if (str.Length > 4)
                            {
                                Solution.PrintError();
                            }
                            int val = (int)Convert.ToUInt32(str, 16);
                            stack.Push(val);
                        }
                        else
                        {
                            int nbOperandsNecessary=op.GetNumArgs();
                            if (stack.Count<nbOperandsNecessary)
                            {
                                Solution.PrintError();
                                break;
                            }
                            else
                            {
                                int[] vals;
                                vals=new int[2];
                                vals[0]=0;
                                vals[1]=0;

                                for (int i=0;i<nbOperandsNecessary;i++)
                                {
                                    try
                                    {
                                        vals[1-i]=(int) stack.Pop();
                                    }
                                    catch (Exception)
                                    {
                                        Solution.PrintError();
                                        break;
                                    }
                                }

                                int retVal=op.DoWork(vals[0], vals[1]);
                                stack.Push(retVal);
                            }
                        }
                    }

                    if (stack.Count!=1)
                    {
                        Solution.PrintError();
                    }
                    else
                    {
                        int lastVal=(int)stack.Pop();
                        Solution.PrintSolution(lastVal);
                    }
                }
                catch (Exception)
                {
                    PrintError();
                }
        }
    }
}