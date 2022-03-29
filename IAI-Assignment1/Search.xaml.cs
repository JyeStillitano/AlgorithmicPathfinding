using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IAI_Assignment1
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        public Search(SearchAlgorithmTypes type)
        {
            InitializeComponent();

            switch (type) 
            {
                case SearchAlgorithmTypes.DFS:
                    
                    break;
                case SearchAlgorithmTypes.BFS:
                    BFS();
                    break;
                case SearchAlgorithmTypes.Greedy:
                    
                    break;
                case SearchAlgorithmTypes.AStar:
                    
                    break;
            }

            
        }

        private void BFS()
        {
            Environment env = new Environment("C:/Users/jstillitano/Desktop/IAI-Assignment1/TestEnvironment.txt");

            foreach (Cell goal in env.goals)
            {
                env.currentGoal = goal;
                SearchAlgorithms search = new SearchAlgorithms();

                search.BreadthFirstSearch(env);
                search.DebugResults();
            }
        }
    }
}
