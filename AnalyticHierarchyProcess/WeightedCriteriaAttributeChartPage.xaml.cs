using ScottPlot;
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
using static ScottPlot.Plottable.PopulationPlot;

namespace AnalyticHierarchyProcess
{
    /// <summary>
    /// Interaction logic for WeightedCriteriaAttributeChartPage.xaml
    /// </summary>
    public partial class WeightedCriteriaAttributeChartPage : Page
    {
        public WeightedCriteriaAttributeChartPage()
        {
            InitializeComponent();

            List<double[]> values = new();
            List<Node> nodes = new();

            foreach (Node criterion in Globals.AHP.Criteria)
            {
                if (!Globals.AHP.HasSubcriteria(criterion))
                {
                    nodes.Add(criterion);
                }
            }

            foreach (Node subcriterion in Globals.AHP.Subcriteria)
            {
                nodes.Add(subcriterion);
            }

            foreach (Node node in nodes) { 
                values.Add(node.GetPropagatedPriorities());
            }

            for (int i = 1; i < values.Count; i++)
            {
                for (int j = 0; j < values[i].Length; j++)
                {
                    values[i][j] += values[i - 1][j];
                }
            }

            for (int i = values.Count - 1; i >= 0; i--)
            {
                var bar = WpfPlot.Plot.AddBar(values[i]);
                bar.Label = nodes[i].Name;
            }

            double[] positions = new double[Globals.AHP.Alternatives.Count];
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = i;
            }
            string[] labels = Globals.AHP.GetAlternativeNames().ToArray();
            WpfPlot.Plot.XTicks(positions, labels);
            WpfPlot.Plot.Style(figureBackground: System.Drawing.Color.FromArgb(245, 246, 250));
            WpfPlot.Plot.SetAxisLimits(yMin: 0);
            WpfPlot.Configuration.LockHorizontalAxis = true;
            WpfPlot.Configuration.LockVerticalAxis = true;
            WpfPlot.Plot.Legend(location: Alignment.UpperRight);
            WpfPlot.Plot.XAxis.TickLabelStyle(fontSize: 20);

            WpfPlot.Refresh();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
