using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAI_Assignment1
{
    internal class Cell
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
        Cell[,] cells;

        // Start Coordinate
        int StartX;
        int StartY;

        // One or more Goal Coordinates
        List<Cell> goals = new List<Cell>();

        List<Cell> walls = new List<Cell>();

        public Environment(string filepath)
        {
            ProcessInput(filepath);
        }

        void ProcessInput(string filepath) 
        {
            // Setup stream reader.
            StreamReader sr = new StreamReader(filepath);

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
    }
}
