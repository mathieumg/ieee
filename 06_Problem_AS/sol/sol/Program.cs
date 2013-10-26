using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;


public class Node
{
    private Char _Identifier;
    private List<Node> _Neighbours;

    public static String BuildStringPath(List<Node> path)
    {
        String output="";
        foreach (Node node in path)
        {
            output+=node.Identifier;
        }
        return output;
    }

    public static String BuildStringPathWithSpaces(List<Node> path)
    {
        String output="";
        foreach (Node node in path)
        {
            output+=(node.Identifier+" ");
        }
        output = output.Substring(0, output.Length-1);
        return output;
    }

    public static List<Node> FindBestPath(List<List<Node>> paths)
    {
        List<Node> best = paths[0];

        foreach (List<Node> list in paths)
        {
            best = Node.GetBestPath(best, list);
        }

        return best;
    }

    public static List<Node> GetBestPath(List<Node> p1, List<Node> p2)
    {
        if (p1[p1.Count-1].Identifier != Solution.Destination)
        {
            return p2;
        }
        else if (p2[p2.Count-1].Identifier!=Solution.Destination)
        {
            return p1;
        }

        if (p1.Count < p2.Count)
        {
            return p1;
        }
        else if (p2.Count < p1.Count)
        {
            return p2;
        }
        else
        {
            String str1=Node.BuildStringPath(p1);
            String str2=Node.BuildStringPath(p2);
            int val = String.Compare(str1, str2);

            if (val < 0)
            {
                return p1;
            }
            else if (val > 0)
            {
                return p2;
            }
            else
            {
                return p1;
            }
        }
    }

    public Node(Char identifier)
    {
        _Identifier = identifier;
        _Neighbours = new List<Node>();
    }

    public override string ToString()
    {
        return _Identifier.ToString();
    }

    public override bool Equals(object obj)
    {
        Node n2 = obj as Node;
        if (n2 == null)
        {
            return false;
        }
        else
        {
            return this.Identifier == n2.Identifier;
        }
    }

    public void AttachNode(Node neighbour)
    {
        if (!_Neighbours.Contains(neighbour) && neighbour != this)
        {
            _Neighbours.Add(neighbour);
        }
    }

    public void Travel(List<Node> path)
    {
        if (_Identifier == Solution.Destination)
        {
            List<Node> newPath=new List<Node>(path);
            newPath.Add(this);
            Solution.Paths.Add(newPath);
        }
        else if (path.Contains(this))
        {
            return;
        }
        else
        {
            foreach (Node node in _Neighbours)
            {
                List<Node> newPath=new List<Node>(path);
                newPath.Add(this);
                node.Travel(newPath);
            }
        }
    }

    public Char Identifier
    {
        get { return _Identifier;}
    }

}









class Solution
{
    public static Char Destination = 'A';

    public static List<List<Node>> Paths = new List<List<Node>>();

    private static Dictionary<Char, Node> Nodes = new Dictionary<Char, Node>();

    public static Node GetOrCreateNode(Char identifier)
    {
        Node node;
        if (!Nodes.TryGetValue(identifier, out node))
        {
            node = new Node(identifier);
            Nodes[identifier] = node;
        }
        return node;
    }


    static void PrintNoRoute()
    {
        String outVal=String.Format("No Route Available from F to {0}", Solution.Destination);
        Console.Out.WriteLine(outVal);
    }

    static void PrintArrived(int routesCount, int shortestDistance, string path)
    {
        String outVal=String.Format("Total Routes: {0}", routesCount);
        Console.Out.WriteLine(outVal);
        outVal=String.Format("Shortest Route Length: {0}", shortestDistance);
        Console.Out.WriteLine(outVal);
        outVal=String.Format("Shortest Route after Sorting of Routes of length {0}: {1}", shortestDistance, path);
        Console.Out.WriteLine(outVal);
    }


    static void Main(String[] args)
    {
        //using (StringReader tr=new StringReader("K\r\nF G\r\nF H\r\nH I\r\nH J\r\nI K\r\nJ K\r\nG H\r\nG I\r\nA A\r\n"))
        //using (StringReader tr=new StringReader("K\nG F\nH F\nI H\nJ H\nK I\nK J\nH G\nI G\nA A\n"))
        //using (StringReader tr=new StringReader("Z\nF M\nS T\nU V\nW X\nY Z\nJ K\nN O\nA A\n"))
        //using (StringReader tr=new StringReader("H\nH G\nF G\nA A\n"))

        using (TextReader tr=System.Console.In)
        {
            String firstLine = tr.ReadLine();
            Char firstChar = firstLine[0];

            if (firstChar == 'F')
            {
                // If already at destination
                Solution.PrintArrived(1, 0, "F");
            }

            Solution.Destination = firstChar;
            Solution.GetOrCreateNode(firstChar);

            while (true)
            {
                String input = tr.ReadLine();
                input = input.ToUpper();

                if (input == "A A")
                {
                    break;
                }

                String[] points = input.Split(' ');
                List<String> pointsList = new List<String>();
                if (points.Length != 2)
                {
                    break;
                }

                for (int i=0; i<points.Length; i++)
                {
                    if (points[i].Length > 0)
                    {
                        pointsList.Add(points[i].Trim());
                    }
                }

                Node n1=Solution.GetOrCreateNode(pointsList[0][0]);
                Node n2=Solution.GetOrCreateNode(pointsList[1][0]);

                n1.AttachNode(n2);
                n2.AttachNode(n1);

            }

            // Travel!
            List<Node> path = new List<Node>();

            Node rootNode;
            if (Solution.Nodes.TryGetValue('F', out rootNode))
            {
                rootNode.Travel(path);
                if (Solution.Paths.Count == 0)
                {
                    Solution.PrintNoRoute();
                }
                else
                {
                    List<Node> bestPath = Node.FindBestPath(Solution.Paths);
                    Solution.PrintArrived(Solution.Paths.Count, bestPath.Count, Node.BuildStringPathWithSpaces(bestPath).Trim());
                }
            }
            else
            {
                Solution.PrintNoRoute();
            }

        }
        //System.Console.In.Read();
    }
}