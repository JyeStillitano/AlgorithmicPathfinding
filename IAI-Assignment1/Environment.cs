using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace IAI_Assignment1
{
    public class Cell
    {
        // A Cell will have
        // Coordinates
        // Wall or Space.

        public Cell(int x, int y, bool isWall = false)
        {
            X = x;
            Y = y;
            IsWall = isWall;
        }
        public int X { get; }
        public int Y { get; }
        public bool IsWall { get; set; }
    }
    
    public class Environment
    {
        //  Example

        //    01234567890  X/M
        //    ------------
        //  0|  XX   GX X|
        //  1|R XX    X  |
        //  2|           |
        //  3|  X      XG|
        //  4|  XXXX  XX |
        // Y/N------------


        // Size, N * M | ROWS * COLUMNS
        int N;
        int M;

        // List of cells as a 2D array
        public Cell[,] cells;
        public List<Cell> goals = new List<Cell>();
        public Cell currentGoal;
        private List<Cell> walls = new List<Cell>();

        // Frontier
        Queue<State> frontier = new Queue<State>();
        List<State> visitedStates = new List<State> ();

        // Start
        int StartX;
        int StartY;
        public State StartState { get; }
        public Environment(string filepath)
        {
            // If file successfully processed continue setup.
            if (ProcessInput(filepath))
            {
                SetupCells();
                SetupWalls();

                StartState = new State(cells[StartX, StartY], null, 0);
                frontier.Enqueue(StartState);
                visitedStates.Add(StartState);
            }
        }

        // Initialise all cells in 2D array.
        private void SetupCells()
        {
            // M      N
            // 0: 11, 1: 5
            for (int i = 0; i < cells.GetLength(0); i++) 
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    cells[i, j] = new Cell(i, j);
                }
            }
        }
        
        // Setup appropriate cells as walls.
        private void SetupWalls()
        {
            foreach (Cell cell in walls) 
            {
                cells[cell.X, cell.Y].IsWall = true;
            }
        }

        // Process input environment file.
        private bool ProcessInput(string filepath) 
        {
            // Setup stream reader.
            StreamReader sr = new StreamReader(filepath);
            try
            {
                if (sr != null)
                {
                    // Environment Size [N,M] | [Y,X]
                    string[] envSize = sr.ReadLine().Split(new char[] { '[', ',', ']' }, StringSplitOptions.RemoveEmptyEntries);
                    N = int.Parse(envSize[0]);
                    M = int.Parse(envSize[1]);
                    cells = new Cell[M, N]; // ORDER SHOULD UPHOLD X,Y ACCESS

                    // Agent State Coordinate (x,y)
                    string[] startCoord = sr.ReadLine().Split(new char[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
                    StartX = int.Parse(startCoord[0]);
                    StartY = int.Parse(startCoord[1]);

                    // Goal State Coordinates (x,y) | (x,y)
                    string[] goalStates = sr.ReadLine().Split('|');
                    foreach (string s in goalStates)
                    {
                        string[] coord = s.Split(new char[] { '(', ',', ')', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        int goalX = int.Parse(coord[0]);
                        int goalY = int.Parse(coord[1]);
                        goals.Add(new Cell(goalX, goalY));
                    }

                    // Wall Coordinates (x,y,width,depth)
                    while (!sr.EndOfStream)
                    {
                        string[] wallString = sr.ReadLine().Split(new char[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
                        int inX = int.Parse(wallString[0]);
                        int inY = int.Parse(wallString[1]);

                        for (int xi = 0; xi < int.Parse(wallString[2]); xi++)
                        {
                            for (int yi = 0; yi < int.Parse(wallString[3]); yi++)
                            {
                                walls.Add(new Cell(inX + xi, inY + yi, true));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("File processing failed with the following error: " + ex.Message);
                return false;
            }
            return true;
        }

        public List<Cell> AvailableMoves(int x, int y)
        {
            List<Cell> result = new List<Cell>();
            // Check moving in each direction won't exceed boundaries or move into wall. ORDER: UP, LEFT, DOWN, RIGHT
            // Up
            if (y > 0)
            {
                if (!cells[x, y - 1].IsWall) { result.Add(cells[x, y - 1]); }
            }
            // Left
            if (x > 0)
            {
                if (!cells[x - 1, y].IsWall) { result.Add(cells[x - 1, y]); }
            }
            // Down
            if (y < N - 1)
            {
                if (!cells[x, y + 1].IsWall) { result.Add(cells[x, y + 1]); }
            }
            // Right
            if (x < M - 1)
            {
                if (!cells[x + 1, y].IsWall) { result.Add(cells[x + 1, y]); }
            }
            return result;
        }

        public bool AtGoalState(int x, int y)
        {
            if (x == currentGoal.X && y == currentGoal.Y) return true;
            return false;
        }
    }
}
