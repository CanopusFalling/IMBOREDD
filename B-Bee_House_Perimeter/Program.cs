using System;

using System.Collections.Generic;

namespace B_Bee_House_Perimeter
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set the console to use UTF8 (8-bit unicode.)
            //Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Get the user input for the size of the graph and
            // the number of occupied nodes.
            int[] firstInput = getIntegersInput();
            int edgeLength = firstInput[0];
            //int nodeCount = firstInput[1];

            // Get the locations of the occupied nodes.
            int[] occupiedNodes = getIntegersInput();

            HexTessGraph graph = new HexTessGraph(edgeLength);
            graph.setHouse(occupiedNodes);
            //Console.WriteLine(graph);

            /*int perimeter = (edgeLength - 1) * 6;

            List<int[]> loops = graph.getLoops(6);

            // For each loop find any enclosed areas.
            foreach (int[] loop in loops)
            {
                graph.fillLoop(loop);
            }*/

            graph.FillNonConnected();

            //Console.WriteLine(graph);

            // Compute the number of borders.
            Console.WriteLine(graph.getBorderCount());
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
            int height = (edgeLength * 2) - 1;
            int currentRowLength = edgeLength;
            int currentNode = 0;
            for (int i = 0; i < height; i++)
            {
                for (int k = 0; k < (height - currentRowLength); k++)
                {
                    result += " ";
                }
                for (int j = 0; j < currentRowLength; j++)
                {
                    result += this.nodes[currentNode];
                    currentNode += 1;
                }
                if (i < edgeLength - 1)
                {
                    currentRowLength += 1;
                }
                else
                {
                    currentRowLength += -1;
                }
                result += "\n";
            }
            return result;
        }

        // Get all nodes in a 2D list.
        public List<List<Node>> To2DList()
        {
            List<List<Node>> result = new List<List<Node>>();
            int height = (edgeLength * 2) - 1;
            int currentRowLength = edgeLength;
            int currentNode = 0;
            for (int i = 0; i < height; i++)
            {
                result.Add(new List<Node>());
                for (int j = 0; j < currentRowLength; j++)
                {
                    result[i].Add(this.nodes[currentNode]);
                    currentNode += 1;
                }
                if (i < edgeLength - 1)
                {
                    currentRowLength += 1;
                }
                else
                {
                    currentRowLength += -1;
                }
            }
            return result;
        }

        // Get all non-house nodes connected to the edge of the graph.
        public List<Node> getEdgeConnectedNodes()
        {
            List<Node> edgeTiles = getEdgeTiles();
            List<Node> edgeConnectedTiles = new List<Node>();

            foreach (Node edge in edgeTiles)
            {
                if (!edge.isHouseTile())
                {
                    edgeConnectedTiles.Add(edge);
                }
            }

            int lastLength = 0;

            while (lastLength != edgeConnectedTiles.Count)
            {
                lastLength = edgeConnectedTiles.Count;
                //Console.WriteLine(lastLength);

                for(int i = 0; i < edgeConnectedTiles.Count; i++)
                {
                    List<int> connections = edgeConnectedTiles[i].getEdges();
                    //Console.WriteLine(connections.Count);

                    foreach (int nodeID in connections)
                    {
                        //Console.WriteLine(nodeID-1 + " : " + this.nodes.Count);
                        Node currentNode = this.nodes[nodeID-1];
                        if ((!currentNode.isHouseTile()) && (!edgeConnectedTiles.Contains(currentNode)))
                        {
                            edgeConnectedTiles.Add(currentNode);
                        }
                    }
                }
            }

            return edgeConnectedTiles;
        }


        // Get all edge nodes.
        public List<Node> getEdgeTiles()
        {
            List<Node> result = new List<Node>();
            foreach (var node in this.nodes)
            {
                if (node.isEdgeTile())
                {
                    result.Add(node);
                }
            }
            return result;
        }

        // Get a list of all the house nodes.
        public List<Node> getHouseTiles()
        {
            List<Node> result = new List<Node>();
            foreach (var node in this.nodes)
            {
                if (node.isHouseTile())
                {
                    result.Add(node);
                }
            }
            return result;
        }

        // Get a list of all the specified nodes.
        public List<Node> getNodes(List<int> ids, List<Node> space)
        {
            List<Node> result = new List<Node>();
            foreach (Node node in space)
            {
                foreach (int id in ids)
                {
                    if (node.getNumber() == id)
                    {
                        result.Add(node);
                    }
                }
            }
            return result;
        }

        // Get all the loops in the graph over a certain length.
        public List<int[]> getLoops(int minLength)
        {
            List<Node> houseTiles = getHouseTiles();
            List<Node> parents = new List<Node>();
            parents.Add(houseTiles[0]);

            return dfsLoops(houseTiles, parents, minLength);
        }

        // DFS on a graph with a list of nodes.
        private List<int[]> dfsLoops(List<Node> nodes, List<Node> parents, int minLength)
        {
            List<int[]> loops = new List<int[]>();

            Node currentNode = parents[parents.Count - 1];
            List<Node> children = getNodes(currentNode.getEdges(), nodes);

            foreach (Node child in children)
            {
                // Check if node has been previously visited.
                int loopLength = -1;
                foreach (Node parent in parents)
                {
                    if (parent.Equals(child))
                    {
                        int parentIndex = parents.IndexOf(parent);
                        loopLength = parents.Count - parentIndex;
                    }
                }

                if (loopLength == -1)
                {
                    // Recurse if node hasn't been visited.
                    parents.Add(child);
                    loops = dfsLoops(nodes, parents, minLength);
                    parents.Remove(child);
                }
                else if (loopLength >= minLength)
                {
                    // Add Loop if node has been visited.
                    int[] loop = new int[loopLength];
                    for (int i = 0; i < loopLength; i++)
                    {
                        loop[i] = parents[parents.Count - loopLength + i].getNumber();
                    }
                    loops.Add(loop);
                }
            }

            return loops;
        }

        public int getBorderCount()
        {
            int count = 0;
            List<Node> houseTiles = getHouseTiles();

            // Iterate over each tile.
            foreach (Node tile in houseTiles)
            {
                List<int> edges = tile.getEdges();
                // Add one for each edge that borders the perimiter.
                count += 6 - edges.Count;
                // Iterate over all the children.
                foreach (int edge in edges)
                {
                    if (!nodes[edge - 1].isHouseTile())
                    {
                        count += 1;
                    }
                }
            }

            return count;
        }

        // ===== Setter Functions =====
        public void setHouse(int[] houseTiles)
        {
            foreach (Node node in this.nodes)
            {
                foreach (int house in houseTiles)
                {
                    if (node.getNumber() == house)
                    {
                        node.setHouse(true);
                    }
                }
            }
        }

        public void fillLoop(int[] loop)
        {
            int height = (this.edgeLength * 2) - 1;
            int currentRowLength = this.edgeLength;
            int currentNode = 1;
            for (int i = 0; i < height; i++)
            {
                // Check if any 2 of the nodes are on the same line.
                List<int> nodesOnLine = new List<int>();
                for (int j = 0; j < currentRowLength; j++)
                {
                    foreach (int loopItem in loop)
                    {
                        if (currentNode == loopItem)
                        {
                            nodesOnLine.Add(loopItem);
                        }
                    }
                    currentNode += 1;
                }

                // Fill in the line between those nodes.
                if (nodesOnLine.Count > 1)
                {
                    fillLine(nodesOnLine[0], nodesOnLine[nodesOnLine.Count - 1]);
                }

                if (i < edgeLength - 1)
                {
                    currentRowLength += 1;
                }
                else
                {
                    currentRowLength += -1;
                }
            }
        }

        private void fillLine(int start, int end)
        {
            //Console.WriteLine(start + " : " + end);
            for (int i = start; i < end; i++)
            {
                //Console.WriteLine(this.nodes[i].getNumber());
                this.nodes[i].setHouse(true);
            }
        }

        public void FillNonConnected(){
            List<Node> connected = getEdgeConnectedNodes();

            // Set entire graph as part of the house.
            foreach (Node node in this.nodes)
            {
                node.setHouse(true);
            }

            // Set all the connected edge nodes to not
            // part of the house.
            foreach (Node node in connected)
            {
                node.setHouse(false);
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
                result += "⬢ ";
            }
            else
            {
                result += "⬡ ";
            }
            return result;
        }

        public Boolean isHouseTile()
        {
            return this.isHouse;
        }

        public Boolean isEdgeTile()
        {
            return this.isEdge;
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

        public void setHouse(Boolean isHouse)
        {
            this.isHouse = isHouse;
        }
    }
}
