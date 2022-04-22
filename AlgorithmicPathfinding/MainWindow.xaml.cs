using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AlgorithmicPathfinding
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string[] args = System.Environment.GetCommandLineArgs();
            
            // Setup Environment.
            Environment env = new Environment(args[1]);
            string searchMethod = args[2];

            // Run the search.
            SearchAlgorithms search = RunSearch(env, searchMethod);

            // Display the results in the GUI.
            DisplayEnvironment(env);
            DisplayResults(env, search);
        }

        /// <summary>
        /// Call the necessary search algorithm based on the method argument.
        /// </summary>
        /// <param name="env">Environment to be searched.</param>
        /// <param name="searchMethod">Shortened string representation of search method.</param>
        /// <returns>Search algorithm for reading results.</returns>
        private SearchAlgorithms RunSearch(Environment env, string searchMethod)
        {
            SearchAlgorithms search = new SearchAlgorithms();

            switch (searchMethod)
            {
                case ("DFS"):
                    search.DepthFirstSearch(env);
                    break;
                case ("BFS"):
                    search.BreadthFirstSearch(env);
                    break;
                case ("GBFS"):
                    search.GreedySearch(env);
                    break;
                case ("AS"):
                    search.AStarSearch(env);
                    break;
                case ("IDAS"):
                    search.IterativeDepthAStarSearch(env);
                    break;
                case ("RW"):
                    search.RandomWalk(env);
                    break;
                default:
                    MessageBox.Show("ERROR: Method Command Line Argument Failure");
                    break;
            }

            return search;
        }

        /// <summary>
        /// Draws the environment grid, walls, goals and start cell using Shapes.
        /// </summary>
        /// <param name="env">The environment to be displayed.</param>
        private void DisplayEnvironment(Environment env)
        {
            // Environment Data
            Cell[,] cells = env.GetAllCells();
            List<Cell> walls = env.GetWalls();
            List<Cell> goals = env.GetGoals();

            // Draw Environment Grid
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    AddCellToDisplay(i, j, Brushes.Black, Brushes.Transparent);
                }
            }

            // Draw Walls in Black
            foreach (Cell wall in walls)
            {
                AddCellToDisplay(wall.X, wall.Y, Brushes.Transparent, Brushes.Black);
            }

            // Draw Goals in Green
            foreach (Cell goal in goals)
            {
                AddCellToDisplay(goal.X, goal.Y, Brushes.Transparent, Brushes.Green);
            }

            // Draw start location in Red
            AddCellToDisplay(env.StartState.Cell.X, env.StartState.Cell.Y, Brushes.Transparent, Brushes.Red);
        }

        /// <summary>
        /// Draws the solution path using Shapes.
        /// </summary>
        /// <param name="env">The environment to be displayed.</param>
        /// <param name="search">The SearchAlgorithm object for retrieving search results.</param>
        private async void DisplayResults(Environment env, SearchAlgorithms search)
        {
            // Search Result Data
            string environmentFilepath = System.Environment.GetCommandLineArgs()[1];
            string algorithm = search.GetLastAlgorithm();
            List<State> visited = search.GetVisitedStates();
            Stack<State> solution = search.GetResults();

            // Output Results to Command Line
            System.Console.WriteLine(environmentFilepath);
            System.Console.WriteLine(algorithm);
            System.Console.WriteLine(solution.Count);

            // Fill out results information.
            EnvironmentValue.Text = environmentFilepath;
            AlgorithmValue.Content = algorithm;
            TraversedValue.Content = solution.Count;
            VisitedValue.Content = visited.Count;

            // Draw the solution path.
            if (solution.Count > 0)
            {
                // Pop each state off the top until none are remaining.
                while (solution.Count > 0)
                {
                    await Task.Delay(600);

                    State state = solution.Pop();
                    if (env.AtGoalState(state.Cell.X, state.Cell.Y) || env.AtStartState(state.Cell.X, state.Cell.Y))
                    {
                        AddCellToDisplay(state.Cell.X, state.Cell.Y, Brushes.Orange, Brushes.Transparent, 4);
                    }
                    else { AddCellToDisplay(state.Cell.X, state.Cell.Y, Brushes.Transparent, Brushes.Orange); }
                    
                    if(!(state.Direction == MoveDirection.None)) 
                    {
                        System.Console.WriteLine(state.Direction.ToString() + ";");
                        SolutionPathValues.Content += state.Direction + ";\r\n"; 
                    }
                }
            }
            else
            {
                System.Console.WriteLine("No solution found.");
                SolutionPathValues.Content = "Search failure. \r\nNo Solution found.";
            }
        }

        /// <summary>
        /// Adds a cell to the display using Square shapes.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="border">The border brush colour.</param>
        /// <param name="fill">The fill brush colour.</param>
        /// <param name="borderThickness">The border thickness. Defaults to 1.</param>
        private void AddCellToDisplay(int x, int y, Brush border, Brush fill, int borderThickness = 1)
        {
            Rectangle newRect = new Rectangle();
            newRect.Stroke = border;
            newRect.StrokeThickness = borderThickness;
            newRect.Fill = fill;
            newRect.Height = 100;
            newRect.Width = 100;
            newRect.VerticalAlignment = VerticalAlignment.Top;
            newRect.HorizontalAlignment = HorizontalAlignment.Left;
            newRect.Margin = new Thickness(x * 100, y * 100, 0, 0);
            MainCanvas.Children.Add(newRect);
        }
    }
}
