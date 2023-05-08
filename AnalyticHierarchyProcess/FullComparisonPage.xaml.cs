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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnalyticHierarchyProcess
{
    /// <summary>
    /// Interaction logic for FullComparisonPage.xaml
    /// </summary>
    public partial class FullComparisonPage : Page
    {
        private readonly Queue<Tuple<Node, int, int>> comparisons = new();
        Tuple<Node, int, int> currentComparison;

        public FullComparisonPage()
        {
            InitializeComponent();

            foreach (Node subcriterion in Globals.AHP.Subcriteria)
            {
                AddNodeComparisons(subcriterion);
            }
            foreach (Node criterion in Globals.AHP.Criteria)
            {
                AddNodeComparisons(criterion);
            }
            AddNodeComparisons(Globals.AHP.Goal);
        }

        private void AddNodeComparisons(Node node)
        {
            for (int i = 0; i < node.Children.Count; i++)
            {
                for (int j = i + 1; j < node.Children.Count; j++)
                {
                    comparisons.Enqueue(new(node, i, j));
                }
            }
        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
            NextComparison();
        }

        private void NextComparison()
        {
            if (comparisons.Count > 0)
            {
                currentComparison = comparisons.Dequeue();
                NavigationService.Navigate(new ComparisonPage(currentComparison.Item1, currentComparison.Item2, currentComparison.Item3));
            }
            else
            {
                Globals.AHP.UpdateAllPriorities();
                NavigationService.Navigate(new CurrentProjectPage());
            }
        }
    }
}
