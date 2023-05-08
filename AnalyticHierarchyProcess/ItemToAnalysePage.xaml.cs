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
    /// Interaction logic for ItemToAnalysePage.xaml
    /// </summary>
    public partial class ItemToAnalysePage : Page
    {
        public ItemToAnalysePage()
        {
            InitializeComponent();
        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            UpdateGoal();
            UpdateCriteriaAndSubcriteria();
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

            Image image = new Image();
            image.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/settings-gear-icon.png"));

            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);

            image.Width = 33;
            image.Height = 25;

            image.HorizontalAlignment = HorizontalAlignment.Right;
            image.VerticalAlignment = VerticalAlignment.Center;

            Grid.SetRow(image, 0);
            Grid.SetColumn(image, 1);

            grid.Children.Add(image);

            image.Cursor = Cursors.Hand;
            image.PreviewMouseLeftButtonDown += (s, e) =>
            {
                NavigationService.Navigate(new MatrixPage(node, canAnalyse: true));
            };

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

        private void Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
