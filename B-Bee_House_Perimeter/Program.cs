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
            graph.setHouse(occupiedNodes);
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

        // ===== Getter Functions =====
        override public String ToString()
        {
            String result = "";
            foreach (var item in nodes)
            {
                result += item;
            }
            return result;
        }

        // ===== Setter Functions =====
        public void setHouse(int[] houseTiles){
            foreach (Node node in this.nodes)
            {
                foreach (int house in houseTiles)
                {
                    if(node.getNumber() == house){
                        node.setHouse(true);
                    }
                }
            }
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
                    rowLength += 1;
                }
                else
                {
                    rowLength += -1;
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
                    if (edges.Count <= 4)
                    {
                        isEdge = true;
                    }

                    Node newNode = new Node(selectedNode, edges, isEdge, false);
                    this.nodes.Add(newNode);
                }
            }
        }

        private List<int> getNodeNeighbors(int[][] numberingArray, int x, int y)
        {
            List<int> edges = new List<int>();

            List<int[]> relativeConnections = new List<int[]>();
            int[] currentRow = numberingArray[x];

            // Add the nodes either side.
            relativeConnections.Add(new int[] { 0, 1 });
            relativeConnections.Add(new int[] { 0, -1 });

            // Add nodes that are above and below.
            if (x > 0)
            {
                int[] belowRow = numberingArray[x - 1];
                int modifier = 1;
                if (belowRow.Length < currentRow.Length) { modifier = -1; };

                relativeConnections.Add(new int[] { -1, 0 });
                relativeConnections.Add(new int[] { -1, modifier });
            }

            if ((x + 1) < numberingArray.Length)
            {
                int[] aboveRow = numberingArray[x + 1];
                int modifier = 1;
                if (aboveRow.Length < currentRow.Length) { modifier = -1; };

                relativeConnections.Add(new int[] { 1, 0 });
                relativeConnections.Add(new int[] { 1, modifier });
            }

            // Iterate over all the relative positions and add edges were possible.
            foreach (var item in relativeConnections)
            {
                try
                {
                    edges.Add(numberingArray[item[0] + x][item[1] + y]);
                }
                catch (System.IndexOutOfRangeException) { }
            }

            return edges;
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
            String result = ""/*getNumber().ToString()*/;
            if (this.isHouse)
            {
                result += " ⬢ ";
            }
            else
            {
                result += " ⬡ ";
            }
            return result;
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

        public void setHouse(Boolean isHouse){
            this.isHouse = isHouse;
        }
    }
}
