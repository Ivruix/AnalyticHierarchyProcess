using ScottPlot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Navigation;

namespace AnalyticHierarchyProcess
{
    /// <summary>
    /// Interaction logic for SmallChangesSimulationPage.xaml
    /// </summary>
    public partial class SmallChangesSimulationPage : Page
    {
        const double tieThreshold = 1e-10;
        const int simulationsPerPieChartUpdate = 5;

        private readonly int step;
        private readonly int[] values;
        private readonly string[] labels;
        private readonly List<AHP>[] simulations;
        private bool simulationRunning = false;
        private readonly System.Windows.Threading.DispatcherTimer dispatcherTimer = new();

        public SmallChangesSimulationPage(int step)
        {
            InitializeComponent();
            this.step = step;
            values = new int[Globals.AHP.Alternatives.Count];
            labels = Globals.AHP.GetAlternativeNames().ToArray();
            simulations = new List<AHP>[Globals.AHP.Alternatives.Count];
            for (int i = 0; i < simulations.Length; i++)
            {
                simulations[i] = new List<AHP>();
            }

            dispatcherTimer.Tick += DispatcherTimer_Tick;
        }

        private AHP CloneAHP()
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, Globals.AHP);
                ms.Position = 0;

                return (AHP)formatter.Deserialize(ms);
            }
        }

        private void Simulate()
        {
            AHP cloned = CloneAHP();
            foreach (Node node in cloned.GetAllNodes())
            {
                ModifyMatrix(node.Matrix);
            }
            cloned.UpdateAllPriorities();

            double maxPriority = cloned.Alternatives[0].Priority;
            int index = 0;
            bool tie = false;

            for (int i = 1; i < values.Length; i++)
            {
                if (Math.Abs(cloned.Alternatives[i].Priority - maxPriority) < tieThreshold)
                {
                    tie = true;
                    continue;
                }

                if (cloned.Alternatives[i].Priority > maxPriority)
                {
                    maxPriority = cloned.Alternatives[i].Priority;
                    index = i;
                    tie = false;
                }
            }

            if (!tie)
            {
                values[index]++;
                simulations[index].Add(cloned);
            }
        }

        private void ModifyMatrix(SquareMatrix matrix)
        {
            Random rng = new();
            for (int i = 0; i < matrix.Size; i++)
            {
                for (int j = i + 1; j < matrix.Size; j++)
                {
                    double value = matrix[i, j];
                    value = convertValue(value);
                    value += rng.Next(-1, 2) * step;
                    value = convertBack(value);

                    matrix[i, j] = value;
                    matrix[j, i] = 1 / value;
                }
            }
        }

        private static double convertValue(double value)
        {
            if (value >= 1.0)
            {
                return (int)((1.0 - value) - 0.5);
            }
            return (int)(((1.0 / value) - 1.0) + 0.5);
        }

        private static double convertBack(double value)
        {
            if (value > 0)
            {
                return 1.0 / (value + 1.0);
            }
            return 1.0 - value;
        }

        public void UpdatePieChart()
        {
            WpfPlot.Visibility = Visibility.Visible;
            WpfPlot.Plot.Clear();

            double sum = 0;
            for (int i = 0; i < values.Length; i++)
            {
                sum += values[i];
            }
            List<ScottPlot.Plottable.Bar> bars = new();
            for (int i = 0; i < values.Length; i++)
            {
                ScottPlot.Plottable.Bar bar = new()
                {
                    Value = values[i],
                    Position = i,
                    FillColor = Palette.Category10.GetColor(i),
                    Label = $"{values[i] / sum:P}"
                };
                bars.Add(bar);
            };
            WpfPlot.Plot.AddBarSeries(bars);
            double[] positions = new double[values.Length];
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = i;
            }
            WpfPlot.Plot.XTicks(positions, labels);
            WpfPlot.Plot.SetAxisLimits(yMin: 0);
            WpfPlot.Plot.Style(figureBackground: System.Drawing.Color.FromArgb(245, 246, 250),
                dataBackground: System.Drawing.Color.FromArgb(245, 246, 250));
            WpfPlot.Plot.Legend(location: Alignment.UpperRight);
            WpfPlot.Configuration.LockHorizontalAxis = true;
            WpfPlot.Configuration.LockVerticalAxis = true;
            WpfPlot.Plot.XAxis.TickLabelStyle(fontSize: 20);

            WpfPlot.Refresh();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            if (simulationRunning)
            {
                dispatcherTimer.Stop();
                toggleButton.Content = "Start simulation";
                simulationRunning = false;
            }
            dispatcherTimer.Stop();
            NavigationService.GoBack();
        }

        private void ToggleSimulation(object sender, RoutedEventArgs e)
        {
            if (simulationRunning)
            {
                dispatcherTimer.Stop();
                toggleButton.Content = "Start simulation";
            }
            else
            {
                dispatcherTimer.Start();
                toggleButton.Content = "Stop simulation";
            }
            simulationRunning = !simulationRunning;
        }

        private void SaveResults(object sender, RoutedEventArgs e)
        {
            if (simulationRunning)
            {
                dispatcherTimer.Stop();
                toggleButton.Content = "Start simulation";
                simulationRunning = false;
            }

            var folderBrowserDialog = new FolderBrowserDialog();

            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
            {
                for (int i = 0; i < simulations.Length; i++)
                {
                    for (int j = 0; j < simulations[i].Count; j++)
                    {
                        var formatter = new BinaryFormatter();
                        string pathToNewFile = Path.Combine(folderBrowserDialog.SelectedPath, labels[i], $"{j + 1}.ahpproject");
                        Directory.CreateDirectory(Path.GetDirectoryName(pathToNewFile));
                        using var fs = new FileStream(pathToNewFile, FileMode.Create);
                        formatter.Serialize(fs, simulations[i][j]);
                    }
                }
            }
        }

        private void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            for (int i = 0; i < simulationsPerPieChartUpdate; i++)
            {
                Simulate();
            }
            UpdatePieChart();
        }
    }
}
