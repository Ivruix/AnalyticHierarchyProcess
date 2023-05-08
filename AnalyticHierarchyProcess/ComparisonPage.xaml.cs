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
    /// Interaction logic for ComparisonPage.xaml
    /// </summary>
    public partial class ComparisonPage : Page
    {
        private Node node;
        private int index1, index2;

        public ComparisonPage(Node node, int index1, int index2)
        {
            InitializeComponent();
            this.node = node;
            this.index1 = index1;
            this.index2 = index2;

            comparisonTitle.Content = $"Compare based on \"{node.Name}\":";
            leftCandidate.Content = node.Children[index1].Name;
            rightCandidate.Content = node.Children[index2].Name;
            slider.Value = 0.0;
        }

        private double GetComparisonValue()
        {
            double value = slider.Value;
            if (value > 0.0)
            {
                return 1.0 / (value + 1.0);
            }
            return 1.0 - value;
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            double comparisonValue = GetComparisonValue();
            node.Matrix[index1, index2] = comparisonValue;
            node.Matrix[index2, index1] = 1.0 / comparisonValue;
            NavigationService.GoBack();
        }

        private void ViewMatrix(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MatrixPage(node));
        }
    }
}
