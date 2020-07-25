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

            // Get the user input for the size of the graph and the number of occupied houses.
            int[] firstInput;
            int edgeLength = firstInput[0];
            int nodeCount = firstInput[1];

            HexTessGraph graph = new HexTessGraph(9);
        }

        // ===== User Input =====
        private static int[] getIntegersInput(){
            Console.ReadLine("");
        }
    }

    // ===== Heaxagonal Tessalation Graph =====
    // Graph to store the graph of tessalated hexagons.
    class HexTessGraph{
        // ===== Instance Variables =====
        private List<Node> nodes;
        private int edgeLength;

        // ===== Constructor =====
        public HexTessGraph(int edgeLength){
            this.edgeLength = edgeLength;

            // Add all the nodes.
            addSetupNodes(edgeLength);
        }
        
        // ===== Setter Functions =====


        // ===== Graph Construction =====
        private void addSetupNodes(int edgeLength){
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
                    currentNode ++;
                }
                if(i < edgeLength-1){
                    rowLength += 2;
                }else{
                    rowLength += -2;
                }
            }

            //int width = edgeLength + (edgeLength - 1) * 2;

            // Add all the nodes to the system.

        }
    }

    // ===== Node Class =====
    // To map out all the nodes and edges on the graph.
    class Node{
        // ===== Instance variables =====
        private int number;
        private List<int> edges;
        private Boolean isEdge;
        private Boolean isHouse;

        // ===== Constructor =====
        public Node(int number, List<int> edges, Boolean isEdge, Boolean isHouse){
            this.number = number;
            this.edges = edges;
            this.isEdge = isEdge;
            this.isHouse = isHouse;
        }

        public Node(int number, List<int> edges, Boolean isEdge){
            new Node(number, edges, isEdge, false);
        }

        // ===== Getter Functions =====
        public int getNumber(){
            return number;
        }

        public List<int> getEdges(){
            return edges;
        }

        public int[] getEdgesArray(){
            return getEdges().ToArray();
        }

        public String ToString(){
            if(isHouse){
                return "⬢";
            }else{
                return "⬡";
            }
        }

        // ===== Setter Functions =====
        // Private
        private void setEdges(List<int> edges){
            this.edges = edges;
        }

        // Public
        public void addEdge(int edge){
            edges.Add(edge);
        }

        public Boolean removeEdge(int edge){
            // Get all the edges.
            List<int> edges = getEdges();

            // Check if the edge can be removed.
            if(edges.Contains(edge)){
                edges.Remove(edge);
                return true;
            }else{
                return false;
            }
        }
    }
}
