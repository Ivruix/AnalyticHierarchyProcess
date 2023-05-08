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
using System.Xml.Linq;

namespace AnalyticHierarchyProcess
{
    /// <summary>
    /// Interaction logic for MatrixPage.xaml
    /// </summary>
    public partial class MatrixPage : Page
    {
        private readonly double[] randomIndexes = { 0, 0, 0, 0.58, 0.9, 1.12, 1.24, 1.32, 1.41, 1.45,
            1.49, 1.51, 1.48, 1.56, 1.57, 1.59, 1.605, 1.61, 1.6151, 1.62, 1.625 };

        private Node node;
        private bool canEdit, canAnalyse, showConsistency;

        public MatrixPage(Node node, bool canEdit = false, bool canAnalyse = false, bool showConsistency = false)
        {
            InitializeComponent();

            this.node = node;
            this.canEdit = canEdit;
            this.canAnalyse = canAnalyse;
            this.showConsistency = showConsistency;

            label.Content = $"Comparison matrix based on \"{node.Name}\":";

            for (int i = 0; i < node.Children.Count + 1; i++)
            {
                var columnDefiniton = new ColumnDefinition();
                columnDefiniton.Width = new GridLength(1, GridUnitType.Star);
                matrixGrid.ColumnDefinitions.Add(columnDefiniton);
            }

            for (int j = 0; j < node.Children.Count + 1; j++)
            {
                var rowDefiniton = new RowDefinition();
                rowDefiniton.Height = new GridLength(1, GridUnitType.Star);
                matrixGrid.RowDefinitions.Add(rowDefiniton);
            }
        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void Done(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Update()
        {
            matrixGrid.Children.Clear();
            Globals.AHP.UpdateAllPriorities();

            for (int i = 0; i < node.Children.Count; i++)
            {
                var label = new Label();
                label.Content = node.Children[i].Name;
                label.FontSize = 25;
                label.Margin = new Thickness(0, 0, 0, 0);
                label.Foreground = new BrushConverter().ConvertFrom("#FF353B48") as SolidColorBrush;
                label.VerticalContentAlignment = VerticalAlignment.Center;
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                label.FontWeight = FontWeights.Bold;

                Grid.SetRow(label, 0);
                Grid.SetColumn(label, i + 1);

                matrixGrid.Children.Add(label);
            }

            for (int i = 0; i < node.Children.Count; i++)
            {
                var label = new Label();
                label.Content = node.Children[i].Name;
                label.FontSize = 25;
                label.Margin = new Thickness(0, 0, 0, 0);
                label.Foreground = new BrushConverter().ConvertFrom("#FF353B48") as SolidColorBrush;
                label.VerticalContentAlignment = VerticalAlignment.Center;
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                label.FontWeight = FontWeights.Bold;

                Grid.SetRow(label, i + 1);
                Grid.SetColumn(label, 0);

                matrixGrid.Children.Add(label);
            }

            for (int i = 0; i < node.Children.Count; i++)
            {
                for (int j = 0; j < node.Children.Count; j++)
                {
                    var label = new Label();
                    label.Foreground = new BrushConverter().ConvertFrom("#FF353B48") as SolidColorBrush;

                    if (node.Matrix[i, j] == 0.0)
                    {
                        label.Content = "?";
                        label.Foreground = Brushes.Red;
                    }
                    else
                    {
                        if (node.Matrix[i, j] < 1.0)
                        {

                            label.Content = $"1/{1 / node.Matrix[i, j]:0}";
                        }
                        else
                        {
                            label.Content = Math.Round(node.Matrix[i, j]);
                        }

                    }

                    label.FontSize = 25;
                    label.Margin = new Thickness(0, 0, 0, 0);

                    label.VerticalContentAlignment = VerticalAlignment.Center;
                    label.HorizontalContentAlignment = HorizontalAlignment.Center;

                    if (canEdit)
                    {
                        if (i != j)
                        {
                            label.Cursor = Cursors.Hand;
                            int index1 = i;
                            int index2 = j;
                            label.PreviewMouseDown += (sender, e) =>
                            {
                                NavigationService.Navigate(new ComparisonPage(node, index1, index2));
                            };
                        }
                    } else if (canAnalyse)
                    {
                        if (i != j)
                        {
                            label.Cursor = Cursors.Hand;
                            int index1 = i;
                            int index2 = j;
                            label.PreviewMouseDown += (sender, e) =>
                            {
                                NavigationService.Navigate(new OneWaySensitivityAnalysisPage(node, index1, index2));
                            };
                        }
                    }

                    Grid.SetRow(label, i + 1);
                    Grid.SetColumn(label, j + 1);

                    if (i == j)
                    {
                        label.FontWeight = FontWeights.Light;
                    }

                    matrixGrid.Children.Add(label);
                }
            }


            if (showConsistency)
            {
                double eigenvalue = node.Matrix.GetApproximatePrincipalEigenvalue();
                double consistencyIndex = (eigenvalue - node.Children.Count) / (node.Children.Count - 1);
                if (randomIndexes[node.Children.Count] == 0)
                {
                    bottomLabel.Content = "Consistency rate: 0%";
                }
                else
                {
                    double consistencyRate = consistencyIndex / randomIndexes[node.Children.Count];
                    bottomLabel.Content = $"Consistency rate: {consistencyRate:P}";
                    if (consistencyRate > 0.1)
                    {
                        bottomLabel.Foreground = Brushes.Red;
                    }
                    else if (consistencyRate < 0.08)
                    {
                        bottomLabel.Foreground = Brushes.Green;
                    }
                    else
                    {
                        bottomLabel.Foreground = Brushes.Orange;
                    }
                }

                if (double.IsNaN(consistencyIndex))
                {
                    additionalInfoLabel.Content = $"Principal eigenvalue: {eigenvalue:0.0000}";
                } else
                {
                    additionalInfoLabel.Content = $"Principal eigenvalue: {eigenvalue:0.0000}    Consisteny index: {consistencyIndex:0.0000}";
                }
            } else if (canAnalyse)
            {
                bottomLabel.Content = "Select one of the comparisons to analyse";
            }
        }
    }
}
