using System.Windows;
using System.Windows.Controls;

namespace IAI_Assignment1
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        Frame Main;
        public Home(Frame main)
        {
            Main = main;
            InitializeComponent();

        }

        private void DFSButtonOnClick(object sender, RoutedEventArgs e)
        {
            Environment env = new Environment("C:/Users/jyest/Desktop/IAI - Assignment1/IAI-Assignment1/TestEnvironment.txt");

            foreach (Cell goal in env.goals)
            {
                env.currentGoal = goal;
                SearchAlgorithms search = new SearchAlgorithms();

                search.DepthFirstSearch(env);
                search.DebugResults();
            }
            Main.Content = new Results(env);
        }

        private void BFSButtonOnClick(object sender, RoutedEventArgs e)
        {
            Environment env = new Environment("C:/Users/jyest/Desktop/IAI - Assignment1/IAI-Assignment1/TestEnvironment.txt");

            foreach (Cell goal in env.goals)
            {
                env.currentGoal = goal;
                SearchAlgorithms search = new SearchAlgorithms();

                search.BreadthFirstSearch(env);
                search.DebugResults();
            }
            Main.Content = new Results(env);
        }

        private void GreedyButtonOnClick(object sender, RoutedEventArgs e)
        {
            Environment env = new Environment("C:/Users/jyest/Desktop/IAI - Assignment1/IAI-Assignment1/TestEnvironment.txt");

            foreach (Cell goal in env.goals)
            {
                env.currentGoal = goal;
                SearchAlgorithms search = new SearchAlgorithms();

                search.GreedySearch(env);
                search.DebugResults();
            }
            Main.Content = new Results(env);
        }

        private void AStarButtonOnClick(object sender, RoutedEventArgs e)
        {
            Environment env = new Environment("C:/Users/jyest/Desktop/IAI - Assignment1/IAI-Assignment1/TestEnvironment.txt");

            foreach (Cell goal in env.goals)
            {
                env.currentGoal = goal;
                SearchAlgorithms search = new SearchAlgorithms();

                search.AStarSearch(env);
                search.DebugResults();
            }
            Main.Content = new Results(env);
        }
    }
}
