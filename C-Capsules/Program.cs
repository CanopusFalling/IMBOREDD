using System;

using System.Collections.Generic;

namespace C_Capsules
{
    class Program
    {
        static void Main(string[] args)
        {
            // ===== User Data Ingestion =====
            // Get dimensions of the grid.
            int[] gridDims = getIntegersInput(2);

            // Get the numbers in the grid. 0 represents a blank tile.
            int[][] gridContents = getGridContents(gridDims[0], gridDims[1]);

            // Get the number of capsules.
            int capsuleCount = getIntegersInput(1)[0];

            // Get the capsules.
            List<Capsule> capsules = getCapsules(capsuleCount);

            // ===== Grid Setup =====
            CapsuleGrid grid = new CapsuleGrid(gridContents, capsules);

            grid.solve();

            Console.WriteLine(grid);
        }

        // Get grid contents.
        private static int[][] getGridContents(int height, int width)
        {
            int[][] result = new int[height][];

            for (int i = 0; i < height; i++)
            {
                result[i] = getIntegersInput(width);
            }

            return result;
        }

        // Get input of an array of integers.
        private static int[] getIntegersInput(int length)
        {
            // Get string input and split it down.
            String userInput = Console.ReadLine();
            String[] stringNumbers = userInput.Split(" ");

            // Convert the numbers to an integer array.
            int[] result = new int[length];
            for (int i = 0; i < length; i++)
            {
                try
                {
                    // Try to parse the string to an int.
                    result[i] = int.Parse(stringNumbers[i]);
                }
                catch (System.FormatException)
                {
                    // Set the index to 0.
                    result[i] = 0;
                }
            }

            return result;
        }

        // Get all the capsules.
        private static List<Capsule> getCapsules(int count)
        {
            List<Capsule> result = new List<Capsule>();
            for (int i = 0; i < count; i++)
            {
                // Add new capsule.
                result.Add(new Capsule(Console.ReadLine()));
            }
            return result;
        }
    }

    class CapsuleGrid
    {
        // ===== InstanceVariables =====
        int[][] gridState;
        Capsule[] capsules;

        // ===== Constructor =====
        public CapsuleGrid(int[][] grid, List<Capsule> capsules)
        {
            this.gridState = grid;
            this.capsules = capsules.ToArray();
        }

        public CapsuleGrid(int[][] grid, Capsule[] capsules)
        {
            this.gridState = grid;
            this.capsules = capsules;
        }

        // ===== Accessor Functions =====
        public int[][] getGridState()
        {
            return this.gridState;
        }

        override public String ToString()
        {
            String result = "";

            foreach (int[] row in this.gridState)
            {
                foreach (int cell in row)
                {
                    result += cell + " ";
                }
                result += "\n";
            }

            return result;
        }

        // Check if grid is following the defined patterns.
        public Boolean isGridCorrect()
        {
            Boolean result = true;

            // Check each cell of the grid state.
            int[][] grid = this.gridState;
            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[0].Length; x++)
                {
                    if (!checkNeighbors(x, y) && grid[y][x] != 0)
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        // Check a cells neighbors.
        public Boolean checkNeighbors(int x, int y)
        {
            int[][] grid = this.gridState;
            Boolean result = true;
            for (int yMod = -1; yMod < 1; yMod++)
            {
                for (int xMod = -1; xMod < 1; xMod++)
                {
                    if (!(xMod == 0 && yMod == 0))
                    {
                        try
                        {
                            if (grid[y][x] == grid[y + yMod][x + xMod])
                            {
                                result = false;
                            }
                        }
                        catch (System.IndexOutOfRangeException) { }
                    }
                }
            }
            return result;
        }

        public int[] findPossibilities(int[] cell, Capsule capsule)
        {
            int[][] grid = this.gridState;
            List<int> possibilities = new List<int>();
            List<int[]> capsuleCells = capsule.getCells();

            for (int i = 0; i < capsuleCells.Count; i++)
            {
                possibilities.Add(i + 1);
            }

            for (int yMod = -1; yMod < 1; yMod++)
            {
                for (int xMod = -1; xMod < 1; xMod++)
                {
                    if (!(xMod == 0 && yMod == 0))
                    {
                        try
                        {
                            foreach (int possibility in possibilities)
                            {
                                if (grid[cell[0] + yMod][cell[1] + xMod] == possibility)
                                {

                                }
                            }
                        }
                        catch (System.IndexOutOfRangeException) { }
                    }
                }
            }

            return possibilities.ToArray();
        }

        public Boolean isSolved(){
            Boolean result = true;
            foreach (Capsule capsule in this.capsules)
            {
                foreach (int[] currentCell in capsule.getCells())
                {
                    if(this.gridState[currentCell[0]-1][currentCell[1]-1] == 0){
                        result = false;
                    }
                }
            }

            return result;
        }

        // ===== Mutator Functions ======
        // Function to solve the grid.
        public void solve()
        {
            int[][] grid = this.gridState;
            int[][][] possibleStates = new int[grid.Length][][];
            for (int i = 0; i < grid.Length; i++)
            {
                possibleStates[i] = new int[grid[0].Length][];
            }

            // Iterate through each capsule.
            foreach (Capsule capsule in this.capsules)
            {
                foreach (int[] currentCell in capsule.getCells())
                {
                    if (grid[currentCell[0] - 1][currentCell[1] - 1] == 0)
                    {
                        possibleStates[currentCell[0] - 1][currentCell[1] - 1] = findPossibilities(currentCell, capsule);
                    }else{
                        possibleStates[currentCell[0] - 1][currentCell[1] - 1] = new int[100];
                    }
                }
            }

            // Chose the cell with the smallest number of possibilities.
            int[] possibleContents = possibleStates[0][0];
            int[] cell = new int[] { 0, 0 };
            foreach (Capsule capsule in this.capsules)
            {
                foreach (int[] currentCell in capsule.getCells())
                {
                    int[] currentStates = possibleStates[currentCell[0] - 1][currentCell[1] - 1];
                    if (currentStates.Length < possibleContents.Length)
                    {
                        cell = currentCell;
                        possibleContents = currentStates;
                    }
                }
            }

            // Change the possibility then recurse.
            foreach (int possibility in possibleContents)
            {
                int[][] newGridState = (int[][])this.gridState.Clone();

                newGridState[cell[0]][cell[1]] = possibility;

                CapsuleGrid newGrid = new CapsuleGrid(newGridState, this.capsules);

                if(newGrid.isSolved()){
                    Console.WriteLine("Break");
                    break;
                }

                newGrid.solve();
                if (newGrid.isGridCorrect())
                {
                    this.gridState = newGrid.getGridState();
                    break;
                }
            }
        }

        // Function to sort all the capsules by length.
        private void sortCapsules()
        {
            List<Capsule> sortedList = new List<Capsule>();

            // Work out the maximum number of cells.
            int maxCells = 0;
            foreach (Capsule capsule in this.capsules)
            {
                int capsuleSize = capsule.getSize();
                if (capsuleSize > maxCells)
                {
                    maxCells = capsuleSize;
                }
            }

            // Add each capsule to the list in ascending size order.
            for (int i = 1; i <= maxCells; i++)
            {
                foreach (Capsule capsule in this.capsules)
                {
                    if (capsule.getSize() == i)
                    {
                        sortedList.Add(capsule);
                    }
                }
            }

            this.capsules = sortedList.ToArray();
        }
    }

    class Capsule
    {
        // ===== Instance Variables =====
        List<int[]> cells;

        // ===== Constructor =====
        private Capsule(List<int[]> cells)
        {
            this.cells = cells;
        }
        public Capsule(String input)
        {
            this.cells = getCords(input);
        }

        // Methods to assist the constructor.
        private static List<int[]> getCords(String input)
        {
            String[] splitInput = input.Split(" ");

            List<int[]> result = new List<int[]>();
            for (int i = 1; i < splitInput.Length; i++)
            {
                String currentItem = splitInput[i];
                // Remove brackets from the string.
                currentItem = String.Join("", currentItem.Split('(', ')'));
                // Split the string into coordinates.
                String[] stringCords = currentItem.Split(",");

                //Convert coordinates to integer array.
                int[] coords = new int[2];
                for (int j = 0; j < coords.Length; j++)
                {
                    coords[j] = int.Parse(stringCords[j]);
                }
                result.Add(coords);
            }

            return result;
        }

        // ===== Access Functions =====
        public List<int[]> getCells()
        {
            return this.cells;
        }

        public int getSize()
        {
            return this.cells.Count;
        }
    }
}
