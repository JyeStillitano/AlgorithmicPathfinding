namespace AlgorithmicPathfinding
{
    public class State
    {
        public State(Cell cell, State parent, double currentCost, MoveDirection direction)
        {
            Cell = cell;
            Parent = parent;
            CurrentCost = currentCost;
            Direction = direction;
        }

        public Cell Cell { get; }
        public State Parent { get; }
        public double CurrentCost { get; set; }
        public MoveDirection Direction { get; }
    }
}
