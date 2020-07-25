using System;

using System.Collections.Generic;

namespace B_Bee_House_Perimeter
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set the console to use UTF8 (8-bit unicode.)
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Get the user input for the size of the graph and 
            // the number of occupied nodes.
            int[] firstInput = getIntegersInput();
            int edgeLength = firstInput[0];
            int nodeCount = firstInput[1];

            // Get the locations of the occupied nodes.
            int[] occupiedNodes = getIntegersInput();

            HexTessGraph graph = new HexTessGraph(edgeLength);
            Console.WriteLine(graph);
        }

        // ===== User Input =====
        private static int[] getIntegersInput()
        {
            // Get string input and split it down.
            String userInput = Console.ReadLine();
            String[] stringNumbers = userInput.Split(" ");
            int numberOfItems = stringNumbers.Length;

            // Convert the numbers to an integer array.
            int[] result = new int[numberOfItems];
            for (int i = 0; i < numberOfItems; i++)
            {
                try
                {
                    // Try to parse the string to an int.
                    result[i] = int.Parse(stringNumbers[i]);
                }
                catch (System.FormatException e)
                {
                    // Check if the format exception is expected or not.
                    if (i != numberOfItems - 1)
                    {
                        throw new System.FormatException(e.ToString());
                    }
                }
            }

            return result;
        }
    }

    // ===== Hexagonal Tessalation Graph =====
    // Graph to store the graph of tessalated hexagons.
    class HexTessGraph
    {
        // ===== Instance Variables =====
        private List<Node> nodes;
        private int edgeLength;

        // ===== Constructor =====
        public HexTessGraph(int edgeLength)
        {
            this.edgeLength = edgeLength;
            nodes = new List<Node>();

            // Add all the nodes.
            addSetupNodes(edgeLength);
        }

        // ===== Setter Functions =====

        // ===== Getter Functions =====
        override public String ToString(){
            String result = "";
            foreach (var item in nodes)
            {
                result += item;
            }
            return result;
        }

        // ===== Graph Construction =====
        private void addSetupNodes(int edgeLength)
        {
            // Get information about the graph size.
            int height = (edgeLength * 2) - 1;

            // Generate numbering array.
            int[][] numberingArray = new int[height][];
            int rowLength = edgeLength;
            int currentNode = 1;

            for (int i = 0; i < height; i++)
            {
                numberingArray[i] = new int[rowLength];
                for (int j = 0; j < numberingArray[i].Length; j++)
                {
                    numberingArray[i][j] = currentNode;
                    currentNode++;
                }
                if (i < edgeLength - 1)
                {
                    rowLength += 2;
                }
                else
                {
                    rowLength += -2;
                }
            }

            //int width = edgeLength + (edgeLength - 1) * 2;

            // Add all the nodes to the system.
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < numberingArray[i].Length; j++)
                {
                    int selectedNode = numberingArray[i][j];
                    List<int> edges = getNodeNeighbors(numberingArray, i, j);
                    Boolean isEdge = false;
                    if(edges.Count <= 4){
                        isEdge = true;
                    }

                    Node newNode = new Node(selectedNode, edges, isEdge, false);
                    this.nodes.Add(newNode);
                }
            }
        }

        private List<int> getNodeNeighbors(int[][] numberingArray, int x, int y)
        {
            List<int> neighbors = new List<int>();

            // Check the nodes to either side.
            int[] nodeRow = numberingArray[x];
            Console.WriteLine(nodeRow.Length);
            if (y - 1 >= 0)
            {
                neighbors.Add(nodeRow[y-1]);
            }
            if (y + 1 < nodeRow.Length)
            {
                neighbors.Add(nodeRow[y+1]);
            }

            // Check the nodes above.
            if (x + 1 < numberingArray.Length)
            {
                int[] aboveNodeRow = numberingArray[x + 1];
                int modifier = -1;
                if(aboveNodeRow.Length > nodeRow.Length){modifier = 1;}
                if (y + modifier >= 0 && y + modifier < numberingArray.Length)
                {
                    neighbors.Add(aboveNodeRow[y+modifier]);
                }
                if (y < nodeRow.Length)
                {
                    neighbors.Add(aboveNodeRow[y]);
                }
            }

            // Check the nodes below.
            if (x - 1 >= 0)
            {
                int[] belowNodeRow = numberingArray[x - 1];
                int modifier = -1;
                if(belowNodeRow.Length > nodeRow.Length){modifier = 1;}
                if (((y + modifier) >= 0) && ((y + modifier) < belowNodeRow.Length));
                {
                    Console.WriteLine(y+modifier + " : " + belowNodeRow.Length + " : " + ((y + modifier) >= 0));
                    neighbors.Add(belowNodeRow[y+modifier]);
                }
                if (y < nodeRow.Length)
                {
                    neighbors.Add(belowNodeRow[y]);
                }
            }

            return neighbors;
        }
    }

    // ===== Node Class =====
    // To map out all the nodes and edges on the graph.
    class Node
    {
        // ===== Instance variables =====
        private int number;
        private List<int> edges;
        private Boolean isEdge;
        private Boolean isHouse;

        // ===== Constructor =====
        public Node(int number, List<int> edges, Boolean isEdge, Boolean isHouse)
        {
            this.number = number;
            this.edges = edges;
            this.isEdge = isEdge;
            this.isHouse = isHouse;
        }

        public Node(int number, List<int> edges, Boolean isEdge)
        {
            new Node(number, edges, isEdge, false);
        }

        // ===== Getter Functions =====
        public int getNumber()
        {
            return number;
        }

        public List<int> getEdges()
        {
            return edges;
        }

        public int[] getEdgesArray()
        {
            return getEdges().ToArray();
        }

        override public String ToString()
        {
            if (this.isEdge)
            {
                return "⬢";
            }
            else
            {
                return "⬡";
            }
        }

        // ===== Setter Functions =====
        // Private
        private void setEdges(List<int> edges)
        {
            this.edges = edges;
        }

        // Public
        public void addEdge(int edge)
        {
            edges.Add(edge);
        }

        public Boolean removeEdge(int edge)
        {
            // Get all the edges.
            List<int> edges = getEdges();

            // Check if the edge can be removed.
            if (edges.Contains(edge))
            {
                edges.Remove(edge);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
