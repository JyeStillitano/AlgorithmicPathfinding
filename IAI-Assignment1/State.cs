﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAI_Assignment1
{
    public class State
    {
        public State(Cell cell, State? parent, double currentCost)
        {
            Cell = cell;
            Parent = parent;
            CurrentCost = currentCost;
        }

        public Cell Cell { get; }
        public State Parent { get; }
        public double CurrentCost { get; set; }
    }
}
