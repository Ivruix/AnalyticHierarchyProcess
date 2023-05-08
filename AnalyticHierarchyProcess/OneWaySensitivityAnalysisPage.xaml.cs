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
    /// Interaction logic for OneWaySensitivityAnalysisPage.xaml
    /// </summary>
    public partial class OneWaySensitivityAnalysisPage : Page
    {
        public OneWaySensitivityAnalysisPage(Node node, int index1, int index2)
        {
            InitializeComponent();
            comparisonTitle.Content = $"Comparison based on \"{node.Name}\":";
            leftCandidate.Content = node.Children[index1].Name;
            rightCandidate.Content = node.Children[index2].Name;

            double originalValue = node.Matrix[index1, index2];

            double[] positions = new double[17];
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = i - 8;
            }
            string[] labels = {"9", "8", "7", "6", "5", "4", "3", "2", "1", "2", "3", "4", "5", "6", "7", "8", "9"};


            List<double[]> values = new();
            for (int i = 0; i < Globals.AHP.Alternatives.Count; i++)
            {
                values.Add(new double[17]);
            }

            for (int i = 0; i < 17; i++)
            {
                double convertedValue = convertValue(i - 8);
                node.Matrix[index1, index2] = convertedValue;
                node.Matrix[index2, index1] = 1.0 / convertedValue;

                Globals.AHP.UpdateAllPriorities();
                for (int j = 0; j < Globals.AHP.Alternatives.Count; j++)
                {
                    values[j][i] = Globals.AHP.Alternatives[j].Priority;  
                }
            }

            for (int i = 0; i < Globals.AHP.Alternatives.Count; i++)
            {
                var plot = WpfPlot.Plot.AddScatter(positions, values[i], label: Globals.AHP.Alternatives[i].Name);
                plot.Smooth = true;
            }

            WpfPlot.Plot.AddVerticalLine(convertBack(originalValue), color: System.Drawing.Color.Gray);

            WpfPlot.Plot.XTicks(positions, labels);

            WpfPlot.Plot.Style(figureBackground: System.Drawing.Color.FromArgb(245, 246, 250));

            WpfPlot.Plot.Legend(location: Alignment.UpperRight);

            WpfPlot.Plot.XAxis.TickLabelStyle(fontSize: 20);

            WpfPlot.Refresh();

            node.Matrix[index1, index2] = originalValue;
            node.Matrix[index2, index1] = 1.0 / originalValue;
            Globals.AHP.UpdateAllPriorities();
        }

        private static double convertValue(int value)
        {
            if (value > 0)
            {
                return 1.0 / (value + 1.0);
            }
            return 1.0 - value;
        }

        private static double convertBack(double value)
        {
            if (value >= 1.0)
            {
                return (int) ((1.0 - value) - 0.5);
            }
            return (int) (((1.0 / value) - 1.0) + 0.5);
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
