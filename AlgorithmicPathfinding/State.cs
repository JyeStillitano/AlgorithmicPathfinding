namespace AlgorithmicPathfinding
{
    public class State
    {
        public State(Cell cell, State parent, double currentCost)
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
