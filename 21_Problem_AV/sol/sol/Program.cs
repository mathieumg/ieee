using System;
using System.Collections.Generic;
using System.IO;


public class Node
{
    private int _x;
    private int _y;

    private List<Node> _neighbors;
    private List<double> _costs;


    public Node(int x, int y)
    {
        _x = x;
        _y = y;
        _neighbors = new List<Node>();
        _costs = new List<double>();
    }

    public int X
    {
        get { return _x;}
    }

    public int Y
    {
        get { return _y; }
    }

    public void AddNeihbor(Node n, double c)
    {
        _neighbors.Add(n);
        _costs.Add(c);
    }

    public double GetHeuristic(Node n2)
    {
        return Math.Sqrt((n2.X-X)^2+(n2.Y-Y)^2);
    }

}

public class Desert : Node 
{

    public Desert(int x, int y)
        : base(x, y)
    {

    }
}

public class StartingPoint : Node
{
    public StartingPoint(int x, int y)
        : base(x, y)
    {

    }
}

public class Oasis : Node
{
    public Oasis(int x, int y)
        : base(x, y)
    {

    }
}

public class EndingPoint : Node
{
    public EndingPoint(int x, int y)
        : base(x, y)
    {

    }
}


class Solution
{
    public static List<Node> AStar(Node start, Node end)
    {
        List<Node> fullpath = new List<Node>();
        List<Node> closedSet = new List<Node>();
        List<Node> openSet = new List<Node>();
        openSet.Add(start);


        while (openSet.Count != 0)
        {




        }










        return fullpath;
    }



    public static Node BuildNode(Char c, int x, int y)
    {
        switch (c)
        {
        case 'D':
            return new Desert(x, y);
        case 'S':
            Node n = new StartingPoint(x, y);
            Solution.SP = n;
            return n;
        case '+':
            return new Oasis(x, y);
        case 'E':
            Node n2 = new EndingPoint(x, y);
            Solution.EP = n2;
            return n2;
        }
        return null;
    }

    public static Node[][] GenerateMap(List<string> rows)
    {
        Node[][] nodes = new Node[rows.Count][];
        for (int i=0; i<rows.Count; i++)
        {
            nodes[i] = new Node[rows[i].Length];
            for (int j=0; j<rows[i].Length; j++)
            {
                nodes[i][j] = BuildNode(rows[i][j], i, j);
            }
        }

        for (int i=0; i<rows.Count; i++)
        {
            for (int j=0; j<rows[i].Length; j++)
            {
                Node curNode = nodes[i][j];
                int xPos = i;
                int yPos = j;
                for (int d=-1; d<=1;d++)
                {
                    for(int d2 = -1; d2<=1; d2++)
                    {
                        int xPosN = xPos+d;
                        int yPosN = yPos+d2;
                        if (xPosN > 0 && xPosN < rows.Count-1 && yPosN > 0 && yPosN < rows[i].Length-1)
                        {
                            int absSum = Math.Abs(xPosN) + Math.Abs(yPosN);
                            if (absSum > 1)
                            {
                                curNode.AddNeihbor(nodes[xPosN][yPosN], 1.5);
                            }
                            else
                            {
                                curNode.AddNeihbor(nodes[xPosN][yPosN], 1);
                            }
                        }
                    }
                }
            }
        }
        return nodes;
    }


    public static void PrintNoPath()
    {
        Console.Out.WriteLine("IMPOSSIBLE");
    }

    public static void PrintPath(List<Node> path)
    {

    }



    public static Node[][] Map = null;
    public static Node SP = null;
    public static Node EP = null;


    static void Main(String[] args)
    {

        // There are 4 types of squares in the grid, desert (D), starting point (S), oasis (+), and ending point (E)

        PrintNoPath();
        Environment.Exit(0);

        string line1 = Console.In.ReadLine();
        line1 = line1.Trim();

        string[] line1Arr = line1.Split(' ');

        int nbRows=Convert.ToInt32(line1Arr[0]);
        int nbCols=Convert.ToInt32(line1Arr[1]);

        List<string> rows = new List<string>();
        for (int i=0; i<nbRows; i++)
        {
            string lineR = Console.In.ReadLine();
            lineR = lineR.Trim();
            lineR = lineR.ToUpper();
            rows.Add(lineR);
        }

        Solution.Map = GenerateMap(rows);


        List<Node> path = Solution.AStar(SP, EP);

        if (path.Count == 0)
        {
            PrintNoPath();
        }
        else
        {
            PrintPath(path);
        }


    }
}












