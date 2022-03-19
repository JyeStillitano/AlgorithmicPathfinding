using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAI_Assignment1
{
    public class Environment
    {
        //  Example

        //    01234567890  X
        //    ------------
        //  0|  XX   GX X|
        //  1|R XX    X  |
        //  2|           |
        //  3|  X      XG|
        //  4|  XXXX  XX |
        //  Y ------------


        // Size, N * M
        int N;
        int M;

        // List of cells as a 2D array
        Cell[,] cells;

        // Start Coordinate
        int StartX;
        int StartY;

        // One or more Goal Coordinates
        List<Cell> goals;

        public Environment(string filepath)
        {
            // Setup stream reader.

            // Environment Size [N,M] | [Y,X]

            // Agent State Coordinate (x,y)

            // Goal State Coordinates (x,y) | (x,y)

            // Wall Coordinates (x,y,width,depth)
        }
    }
}
