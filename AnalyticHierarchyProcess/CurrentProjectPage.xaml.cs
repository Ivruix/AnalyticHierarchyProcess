using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
    /// Interaction logic for CurrenProjectPage.xaml
    /// </summary>
    public partial class CurrentProjectPage : Page
    {
        public CurrentProjectPage()
        {
            InitializeComponent();
            listBoxGoal.SelectionChanged += (s, e) =>
            {
                if (listBoxGoal.SelectedIndex != -1)
                {
                    listBoxGoal.SelectedIndex = -1;
                }
            };
            listBoxCriteriaAndSubcriteria.SelectionChanged += (s, e) =>
            {
                if (listBoxCriteriaAndSubcriteria.SelectedIndex != -1)
                {
                    listBoxCriteriaAndSubcriteria.SelectedIndex = -1;
                }
            };
            listBoxAlternatives.SelectionChanged += (s, e) =>
            {
                if (listBoxAlternatives.SelectedIndex != -1)
                {
                    listBoxAlternatives.SelectedIndex = -1;
                }
            };
        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            UpdateGoal();
            UpdateCriteriaAndSubcriteria();
            UpdateAlternatives();
        }

        private Grid CreateItem(Node node, string index, bool bold = true)
        {
            Grid grid = new();

            var columnDefinitonLabel = new ColumnDefinition();
            columnDefinitonLabel.Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions.Add(columnDefinitonLabel);

            var columnDefinitonSettings = new ColumnDefinition();
            columnDefinitonSettings.Width = new GridLength(60, GridUnitType.Pixel);
            grid.ColumnDefinitions.Add(columnDefinitonSettings);

            var columnDefinitonPriority = new ColumnDefinition();
            columnDefinitonPriority.Width = new GridLength(75, GridUnitType.Pixel);
            grid.ColumnDefinitions.Add(columnDefinitonPriority);

            var labelName = new Label();

            labelName.Content = $"{index} {node.Name}";
            
            labelName.FontSize = 25;
            labelName.Margin = new Thickness(0, 0, 0, 0);
            labelName.Foreground = new BrushConverter().ConvertFrom("#FF353B48") as SolidColorBrush;
            labelName.VerticalContentAlignment = VerticalAlignment.Center;
            labelName.HorizontalContentAlignment = HorizontalAlignment.Left;

            if (bold)
            {
                labelName.FontWeight = FontWeights.Bold;
            }
            

            Grid.SetRow(labelName, 0);
            Grid.SetColumn(labelName, 0);

            grid.Children.Add(labelName);

            var labelPrioriy = new Label();
            labelPrioriy.Content = $"{node.Priority:0.000}";

            labelPrioriy.FontSize = 25;
            labelPrioriy.Margin = new Thickness(0, 0, 0, 0);
            labelPrioriy.Foreground = new BrushConverter().ConvertFrom("#FF353B48") as SolidColorBrush;
            labelPrioriy.VerticalContentAlignment = VerticalAlignment.Center;
            labelPrioriy.HorizontalContentAlignment = HorizontalAlignment.Right;

            if (bold)
            {
                labelPrioriy.FontWeight = FontWeights.Bold;
            }

            Grid.SetRow(labelPrioriy, 0);
            Grid.SetColumn(labelPrioriy, 2);

            grid.Children.Add(labelPrioriy);

            if (node.Children.Count != 0)
            {
                Image image = new Image();
                image.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/configuration-icon.png"));

                RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);

                image.Width = 25;
                image.Height = 25;

                image.HorizontalAlignment = HorizontalAlignment.Right;
                image.VerticalAlignment = VerticalAlignment.Center;

                Grid.SetRow(image, 0);
                Grid.SetColumn(image, 1);

                grid.Children.Add(image);

                image.Cursor = Cursors.Hand;
                image.PreviewMouseLeftButtonDown += (s, e) =>
                {
                    NavigationService.Navigate(new MatrixPage(node, canEdit: true, showConsistency: true));
                };
            }

            return grid;
        }

        private void UpdateGoal()
        {
            listBoxGoal.Items.Clear();
            listBoxGoal.Items.Add(CreateItem(Globals.AHP.Goal, "1."));
        }

        private void UpdateCriteriaAndSubcriteria()
        {
            listBoxCriteriaAndSubcriteria.Items.Clear();
            ulong index = 1;
            foreach (Node criterion in Globals.AHP.Criteria)
            {
                listBoxCriteriaAndSubcriteria.Items.Add(CreateItem(criterion, $"{index}."));
                index++;

                if (Globals.AHP.HasSubcriteria(criterion))
                {
                    ulong subindex = 0;
                    foreach (Node subcriterion in criterion.Children)
                    {
                        listBoxCriteriaAndSubcriteria.Items.Add(CreateItem(subcriterion, $"    {(char)('a' + subindex)}.", false));
                        subindex++;
                    }
                }
            }
        }

        private void UpdateAlternatives()
        {
            listBoxAlternatives.Items.Clear();
            ulong index = 1;
            List<Node> alternatives = new(Globals.AHP.Alternatives);
            alternatives.Sort((x, y) => y.Priority.CompareTo(x.Priority));
            foreach (Node alternative in alternatives)
            {
                listBoxAlternatives.Items.Add(CreateItem(alternative, $"{index}."));
                index++;
            }
        }

        private void CompareAgain(object sender, RoutedEventArgs e)
        {
            Globals.AHP.ResetNodes();
            NavigationService.Navigate(new FullComparisonPage());
        }

        private void SensitivityAnalysis(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SensitivityAnalysisTypePage());
        }


        private void Save(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "AHP project file (*.ahpproject)|*.ahpproject";
            if (saveFileDialog.ShowDialog() == true)
            {
                var formatter = new BinaryFormatter();
                using var fs = new FileStream(saveFileDialog.FileName, FileMode.Create);
                formatter.Serialize(fs, Globals.AHP);
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Globals.AHP = null;
            while (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void Export(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();

            System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
            {
                foreach (Node node in Globals.AHP.GetAllNodes())
                {
                    if (node.Children.Count == 0)
                    {
                        continue;
                    }

                    List<string> childrenNames = new();
                    foreach (Node child in node.Children)
                    {
                        childrenNames.Add(child.Name);
                    }

                    var csv = new StringBuilder();

                    csv.Append(';');
                    csv.AppendLine(string.Join(";", childrenNames));

                    for (int i = 0; i < node.Matrix.Size; i++)
                    {
                        csv.Append(childrenNames[i]);
                        for (int j = 0; j < node.Matrix.Size; j++)
                        {
                            csv.Append(';');
                            csv.Append(node.Matrix[i, j]);
                        }
                        if (i != node.Matrix.Size - 1)
                        {
                            csv.AppendLine();
                        }
                    }
                    csv.AppendLine();
                    csv.Append($"Weight:;{node.Priority}");
                    
                    string pathToNewFile = System.IO.Path.Combine(folderBrowserDialog.SelectedPath, $"{node.Name}.csv");
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(pathToNewFile));
                    File.WriteAllText(pathToNewFile, csv.ToString());
                }
            }
        }
    }
}
