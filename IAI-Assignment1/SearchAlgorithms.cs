using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace IAI_Assignment1
{
    public enum SearchAlgorithmTypes
    {
        BFS,
        DFS,
        Greedy,
        AStar
    }

    public class SearchAlgorithms
    {
        List<State> visitedStates = new List<State>();
        PriorityQueue<State, int> frontier = new PriorityQueue<State, int>();
        Stack<State> results = new Stack<State>();

        public void DebugResults()
        {
            if (results.Count > 0)
            {
                Debug.WriteLine("Search success! Goal state found through...");
                while (results.Count > 0)
                {
                    State state = results.Pop();
                    Debug.WriteLine(state.Cell.X + ", " + state.Cell.Y);
                }
            } else
            {
                Debug.WriteLine("Search failure. No results found.");
            }
        }

        /// <summary>
        /// Check if the given state has been explored throughout the run.
        /// </summary>
        /// <param name="state">The state to check.</param>
        /// <returns>If the given state has been explored, returns true. Else, false.</returns>
        public bool StateVisited(State state)
        {
            foreach (State visited in visitedStates)
            {
                if (state.Cell.X == visited.Cell.X && state.Cell.Y == visited.Cell.Y) return true;
            }
            return false;
        }

        // Uninformed Search Algorithms ------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public void DepthFirstSearch(Environment env)
        {
            frontier.Enqueue(env.StartState, 1);
            visitedStates.Add(env.StartState);


        }

        /// <summary>
        /// Attempts to locate the given environments current goal state using a breadth-first search algorithm.
        /// </summary>
        /// <param name="env">The environment to be traversed.</param>
        public void BreadthFirstSearch(Environment env)
        {
            frontier.Enqueue(env.StartState, 1);
            visitedStates.Add(env.StartState);

            while (frontier.Count > 0)
            {
                State state = frontier.Dequeue();

                if (!env.AtGoalState(state.Cell.X, state.Cell.Y))
                {
                    foreach (Cell childCell in env.AvailableMoves(state.Cell.X, state.Cell.Y))
                    {
                        State childState = new State(childCell, state, state.CurrentCost + 1);
                        if (!StateVisited(childState))
                        {
                            frontier.Enqueue(childState, 1);
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

        // Informed Search Algorithms - Best First Search ---------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public void GreedySearch(Environment env)
        {
            frontier.Enqueue(env.StartState, 0);
            visitedStates.Add(env.StartState);

            while (frontier.Count > 0)
            {
                State state = frontier.Dequeue();

                if (!env.AtGoalState(state.Cell.X, state.Cell.Y))
                {
                    foreach (Cell childCell in env.AvailableMoves(state.Cell.X, state.Cell.Y))
                    {
                        // TODO CHANGE COST AND PRIORITY
                        State childState = new State(childCell, state, state.CurrentCost + 1);
                        //int SLD = env.GetSLD(r.Destination, destination);
                        //priorityFrontier.Enqueue(childState, SLD);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public void AStarSearch(Environment env)
        {
            frontier.Enqueue(env.StartState, 0);
            visitedStates.Add(env.StartState);

            while (frontier.Count > 0)
            {
                State state = frontier.Dequeue();

                if (!env.AtGoalState(state.Cell.X, state.Cell.Y))
                {
                    foreach (Cell childCell in env.AvailableMoves(state.Cell.X, state.Cell.Y))
                    {
                        // TODO: Queueing
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

        // Two Custom Search Strategies (one informed, one uninformed) --------------------------------------
    }
}
