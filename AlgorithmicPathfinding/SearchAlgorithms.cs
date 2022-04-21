using System;
using System.Collections.Generic;

namespace AlgorithmicPathfinding
{
    public class SearchAlgorithms
    {
        private List<State> visitedStates = new List<State>();
        private PriorityQueue<State, double> priorityFrontier = new PriorityQueue<State, double>();
        private Queue<State> frontier = new Queue<State>();
        private Stack<State> results = new Stack<State>();
        private string lastAlgorithm = "Null";

        // General Functions --------------------------------------------------------------------------------

        /// <summary>
        /// Retrieves a list of states visited throughout search.
        /// </summary>
        /// <returns>List of visited states.</returns>
        public List<State> GetVisitedStates() { return visitedStates; }
        /// <summary>
        /// Retrieves a stack of states which form the solution path when popped off.
        /// </summary>
        /// <returns>Stack of states forming the solution path.</returns>
        public Stack<State> GetResults() { return results; }
        /// <summary>
        /// Returns the name of the last algorithm run using the current SearchAlgorithm object.
        /// </summary>
        /// <returns>Name of the algorithm run most recently.</returns>
        public string GetLastAlgorithm() { return lastAlgorithm; }

        /// <summary>
        /// Check if the given state has been explored throughout the run.
        /// </summary>
        /// <param name="state">The state to check.</param>
        /// <returns>If the given state has been explored, returns true. Else, false.</returns>
        private bool StateVisited(State state)
        {
            foreach (State visited in visitedStates)
            {
                if (state.Cell.X == visited.Cell.X && state.Cell.Y == visited.Cell.Y) return true;
            }
            return false;
        }

        // Uninformed Search Algorithms ---------------------------------------------------------------------

        /// <summary>
        /// Attempts to locate the given environments current goal state using a depth-first search algorithm.
        /// </summary>
        /// <param name="env">The environment to be traversed.</param>
        public void DepthFirstSearch(Environment env)
        {
            lastAlgorithm = "Depth First Search";
            State state = env.StartState;
            visitedStates.Add(state);

            DFSRecursive(env, state);
        }

        /// <summary>
        /// Internal recursive function for completing a Depth First Search.
        /// </summary>
        /// <param name="env">The environment to be traversed.</param>
        /// <param name="state">The current state.</param>
        /// <returns></returns>
        private bool DFSRecursive(Environment env, State state)
        {
            if (!env.AtGoalState(state.Cell.X, state.Cell.Y))
            {
                foreach (MoveDirection direction in env.AvailableMoves(state.Cell.X, state.Cell.Y))
                {
                    Cell childCell = env.GetCellInDirection(state.Cell.X, state.Cell.Y, direction);
                    State childState = new State(childCell, state, state.CurrentCost + 1);
                    if (!StateVisited(childState))
                    {
                        visitedStates.Add(childState);
                        if (DFSRecursive(env, childState)) return true;
                    }
                }
            }
            else
            {
                while (state.Parent != null)
                {
                    results.Push(state);
                    state = state.Parent;
                }
                results.Push(state);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to locate the given environments current goal state using a breadth-first search algorithm.
        /// </summary>
        /// <param name="env">The environment to be traversed.</param>
        public void BreadthFirstSearch(Environment env)
        {
            lastAlgorithm = "Breadth First Search";

            frontier.Enqueue(env.StartState);
            visitedStates.Add(env.StartState);

            while (frontier.Count > 0)
            {
                State state = frontier.Dequeue();

                if (!env.AtGoalState(state.Cell.X, state.Cell.Y))
                {
                    foreach (MoveDirection direction in env.AvailableMoves(state.Cell.X, state.Cell.Y))
                    {
                        Cell childCell = env.GetCellInDirection(state.Cell.X, state.Cell.Y, direction);
                        State childState = new State(childCell, state, state.CurrentCost + 1);
                        if (!StateVisited(childState))
                        {
                            frontier.Enqueue(childState);
                            visitedStates.Add(childState);
                        }
                    }
                }
                else
                {
                    while (state.Parent != null)
                    {
                        results.Push(state);
                        state = state.Parent;
                    }
                    results.Push(state);
                    return;
                }
            }
        }

        // Informed Search Algorithms - Best First Search ---------------------------------------------------

        /// <summary>
        /// Attempts to locate the given environments current goal state using a greedy best-first search algorithm.
        /// </summary>
        /// <param name="env">The environment to be traversed.</param>
        public void GreedySearch(Environment env)
        {
            lastAlgorithm = "Greedy Best First Search";

            priorityFrontier.Enqueue(env.StartState, 0);
            visitedStates.Add(env.StartState);

            while (priorityFrontier.Count > 0)
            {
                State parentState = priorityFrontier.Dequeue();

                if (!env.AtGoalState(parentState.Cell.X, parentState.Cell.Y))
                {
                    foreach (MoveDirection direction in env.AvailableMoves(parentState.Cell.X, parentState.Cell.Y))
                    {
                        double directionPriority = 0.0;
                        switch (direction)
                        {
                            case MoveDirection.Up:
                                directionPriority = 0.1;
                                break;
                            case MoveDirection.Left:
                                directionPriority = 0.2;
                                break;
                            case MoveDirection.Down:
                                directionPriority = 0.3;
                                break;
                            case MoveDirection.Right:
                                directionPriority = 0.4;
                                break;
                        }

                        Cell childCell = env.GetCellInDirection(parentState.Cell.X, parentState.Cell.Y, direction);
                        State childState = new State(childCell, parentState, env.GetManhattanDistance(childCell.X, childCell.Y) + directionPriority);
                        if (!StateVisited(childState))
                        {
                            priorityFrontier.Enqueue(childState, childState.CurrentCost);
                            visitedStates.Add(childState);
                        }
                    }
                }
                else
                {
                    while (parentState.Parent != null)
                    {
                        results.Push(parentState);
                        parentState = parentState.Parent;
                    }
                    results.Push(parentState);
                    return;
                }
            }
        }

        /// <summary>
        /// Attempts to locate the given environments current goal state using an A* search algorithm.
        /// </summary>
        /// <param name="env">The environment to be traversed.</param>
        public void AStarSearch(Environment env)
        {
            lastAlgorithm = "A* Search";

            priorityFrontier.Enqueue(env.StartState, 0);
            visitedStates.Add(env.StartState);

            while (priorityFrontier.Count > 0)
            {
                State parentState = priorityFrontier.Dequeue();

                if (!env.AtGoalState(parentState.Cell.X, parentState.Cell.Y))
                {
                    foreach (MoveDirection direction in env.AvailableMoves(parentState.Cell.X, parentState.Cell.Y))
                    {
                        double directionPriority = 0.0;
                        switch (direction)
                        {
                            case MoveDirection.Up:
                                directionPriority = 0.1;
                                break;
                            case MoveDirection.Left:
                                directionPriority = 0.2;
                                break;
                            case MoveDirection.Down:
                                directionPriority = 0.3;
                                break;
                            case MoveDirection.Right:
                                directionPriority = 0.4;
                                break;
                        }

                        Cell childCell = env.GetCellInDirection(parentState.Cell.X, parentState.Cell.Y, direction);
                        State childState = new State(childCell, parentState, parentState.CurrentCost + 1);
                        if (!StateVisited(childState))
                        {
                            priorityFrontier.Enqueue(childState, childState.CurrentCost + env.GetManhattanDistance(childCell.X, childCell.Y) + directionPriority);
                            visitedStates.Add(childState);
                        }
                    }
                }
                else
                {
                    while (parentState.Parent != null)
                    {
                        results.Push(parentState);
                        parentState = parentState.Parent;
                    }
                    results.Push(parentState);
                    return;
                }
            }
        }

        // Two Custom Search Strategies (one informed, one uninformed) --------------------------------------

        /// <summary>
        /// Attempts to locate the given environments current goal state using an Iterative Depth A* search algorithm.
        /// </summary>
        /// <param name="env">The environment to be traversed.</param>
        public void IterativeDepthAStarSearch(Environment env)
        {
            lastAlgorithm = "Iterative Depth A* Search";

            int depth = 1;
            bool success = false;
            do
            {
                success = IDAS(env, depth);
                if (!success) visitedStates.Clear();
                depth++;
            } while (!success && depth < 1000000);
        }

        /// <summary>
        /// Internal function for performing Iterative Depth A Star search.
        /// </summary>
        /// <param name="env">The environment to be searching.</param>
        /// <param name="depth">The limited depth to search.</param>
        /// <returns>Whether search at the given depth was successful.</returns>
        private bool IDAS(Environment env, int depth)
        {
            priorityFrontier.Enqueue(env.StartState, 0);
            visitedStates.Add(env.StartState);

            while (priorityFrontier.Count > 0)
            {
                State parentState = priorityFrontier.Dequeue();

                if (!env.AtGoalState(parentState.Cell.X, parentState.Cell.Y))
                {
                    foreach (MoveDirection direction in env.AvailableMoves(parentState.Cell.X, parentState.Cell.Y))
                    {
                        double directionPriority = 0.0;
                        switch (direction)
                        {
                            case MoveDirection.Up:
                                directionPriority = 0.1;
                                break;
                            case MoveDirection.Left:
                                directionPriority = 0.2;
                                break;
                            case MoveDirection.Down:
                                directionPriority = 0.3;
                                break;
                            case MoveDirection.Right:
                                directionPriority = 0.4;
                                break;
                        }

                        Cell childCell = env.GetCellInDirection(parentState.Cell.X, parentState.Cell.Y, direction);
                        State childState = new State(childCell, parentState, parentState.CurrentCost + 1);
                        if (!StateVisited(childState))
                        {
                            double depthCost = childState.CurrentCost + env.GetManhattanDistance(childCell.X, childCell.Y);
                            if (depthCost < depth) 
                            { 
                                priorityFrontier.Enqueue(childState, depthCost + directionPriority);
                                visitedStates.Add(childState);
                            }
                        }
                    }
                }
                else
                {
                    while (parentState.Parent != null)
                    {
                        results.Push(parentState);
                        parentState = parentState.Parent;
                    }
                    results.Push(parentState);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Attempts to locate the given environments current goal state using a Random Walk algorithm.
        /// </summary>
        /// <param name="env">The environment to be traversed.</param>
        /// <returns>Success or failure.</returns>
        public bool RandomWalk(Environment env)
        {
            lastAlgorithm = "Random Walk Search";
            int maxWalkLength = 1000;
            State state = env.StartState;
            results.Push(state);

            while (maxWalkLength > 0)
            {
                // If at goal state, log moves.
                if (env.AtGoalState(state.Cell.X, state.Cell.Y)) return true;

                // Determine Next Move
                List<MoveDirection> availableMoves = env.AvailableMoves(state.Cell.X, state.Cell.Y);
                MoveDirection move = availableMoves[new Random().Next(0, availableMoves.Count)];

                // Move
                Cell childCell = env.GetCellInDirection(state.Cell.X, state.Cell.Y, move);
                State childState = new State(childCell, state, state.CurrentCost + 1);
                results.Push(childState);
                state = childState;

                // Decrement remaining steps.
                maxWalkLength--;
            }

            results.Clear();
            return false;
        }
    }
}