using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace AlgorithmicPathfinding
{
    public enum MoveDirection
    {
        Up,
        Left,
        Down,
        Right
    }

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
        // Size, N * M | ROWS * COLUMNS
        int N;
        int M;

        // List of cells as a 2D array
        Cell[,] cells = new Cell[0,0];
        private List<Cell> goals = new List<Cell>();
        public Cell CurrentGoal { get; set; }
        private List<Cell> walls = new List<Cell>();

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
            }
        }

        // SETUP ----------------------------------------------------------------------------

        /// <summary>
        /// Processes the environment information provided.
        /// </summary>
        /// <param name="filepath">The location of the environment setup text file.</param>
        /// <returns>Whether the processing was successful.</returns>
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

        /// <summary>
        /// Initialise all cells in a 2D array.
        /// </summary>
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

        /// <summary>
        /// Internal wall setup function.
        /// </summary>
        private void SetupWalls()
        {
            foreach (Cell cell in walls)
            {
                cells[cell.X, cell.Y].IsWall = true;
            }
        }

        // GENERAL FUNCTIONS ----------------------------------------------------------------

        /// <summary>
        /// Retrieve the cell at a given coordinate.
        /// </summary>
        /// <param name="x">X Coordinate</param>
        /// <param name="y">Y Coordinate</param>
        /// <returns>Cell at given coordinate.</returns>
        public Cell GetCell(int x, int y) { return cells[x, y]; }

        /// <summary>
        /// Retrieve 2D cell array.
        /// </summary>
        /// <returns>Returns cells 2D Array.</returns>
        public Cell[,] GetAllCells() { return cells; }

        /// <summary>
        /// Retrieve all wall cells.
        /// </summary>
        /// <returns>Returns the walls as a list of cells.</returns>
        public List<Cell> GetWalls() { return walls; }

        /// <summary>
        /// Retrieve all goal cells.
        /// </summary>
        /// <returns>Goal state cells.</returns>
        public List<Cell> GetGoals() { return goals; }

        /// <summary>
        /// Retrieve the cell one move in a given direction from the given location.
        /// </summary>
        /// <param name="X">X Coordinate</param>
        /// <param name="Y">Y Coordinate</param>
        /// <param name="dir">The MoveDirection of the cell.</param>
        /// <returns>Cell at location after move.</returns>
        public Cell GetCellInDirection(int X, int Y, MoveDirection dir)
        {
            switch (dir)
            {
                case MoveDirection.Up:
                    return GetCell(X, Y - 1);
                case MoveDirection.Left:
                    return GetCell(X - 1, Y);
                case MoveDirection.Down:
                    return GetCell(X, Y + 1);
                case MoveDirection.Right:
                    return GetCell(X + 1, Y);
                default:
                    return GetCell(X, Y);
            }
        }

        /// <summary>
        /// Retrieves all legal moves from a given location.
        /// </summary>
        /// <param name="x">X Coordinate</param>
        /// <param name="y">Y Coordinate</param>
        /// <returns>Returns a list of legal MoveDirections.</returns>
        public List<MoveDirection> AvailableMoves(int x, int y)
        {
            List<MoveDirection> result = new List<MoveDirection>();
            // Check moving in each direction won't exceed boundaries or move into wall. ORDER: UP, LEFT, DOWN, RIGHT
            // Up
            if (y > 0)
            {
                if (!cells[x, y - 1].IsWall) { result.Add(MoveDirection.Up); }
            }
            // Left
            if (x > 0)
            {
                if (!cells[x - 1, y].IsWall) { result.Add(MoveDirection.Left); }
            }
            // Down
            if (y < N - 1)
            {
                if (!cells[x, y + 1].IsWall) { result.Add(MoveDirection.Down); }
            }
            // Right
            if (x < M - 1)
            {
                if (!cells[x + 1, y].IsWall) { result.Add(MoveDirection.Right); }
            }
            return result;
        }

        /// <summary>
        /// Retrieves the manhattan distance from the given cell to the current goal cell, ignoring walls. 
        /// </summary>
        /// <param name="x">X Coordinate</param>
        /// <param name="y">Y Coordinate</param>
        /// <returns>Returns the manhattan distance to the current goal ignoring walls.</returns>
        public int GetManhattanDistance(int x, int y)
        {
            return Math.Abs(x - CurrentGoal.X) + Math.Abs(y - CurrentGoal.Y);
        }

        /// <summary>
        /// Determines whether the given coordinate is at the goal state.
        /// </summary>
        /// <param name="x">X Coordinate</param>
        /// <param name="y">Y Coordinate</param>
        /// <returns>If at the goal state returns true, else false.</returns>
        public bool AtGoalState(int x, int y)
        {
            if (x == CurrentGoal.X && y == CurrentGoal.Y) return true;
            return false;
        }

        /// <summary>
        /// Determines whether the given coordinate is at the start state.
        /// </summary>
        /// <param name="x">X Coordinate</param>
        /// <param name="y">Y Coordinate</param>
        /// <returns>If at the start state returns true, else false.</returns>
        public bool AtStartState(int x, int y)
        {
            if (x == StartState.Cell.X && y == StartState.Cell.Y) return true;
            return false;
        }
    }
}
