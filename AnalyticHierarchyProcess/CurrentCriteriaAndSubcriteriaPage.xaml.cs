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
    /// Interaction logic for CurrentCriteriaAndSubcriteriaPage.xaml
    /// </summary>
    public partial class CurrentCriteriaAndSubcriteriaPage : Page
    {
        public CurrentCriteriaAndSubcriteriaPage()
        {
            InitializeComponent();
            listBox.SelectionChanged += (s, e) =>
            {
                if (listBox.SelectedIndex != -1)
                {
                    listBox.SelectedIndex = -1;
                }
            };
        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            listBox.Items.Clear();
            ulong index = 1;
            foreach (var criterion in Globals.AHP.GetCriterionNames())
            {
                ListBoxItem listBoxItem = new ListBoxItem();
                listBoxItem.Content = $"{index}. {criterion}";
                listBoxItem.FontWeight = FontWeights.Bold;
                listBoxItem.PreviewMouseLeftButtonDown += (s, e) =>
                {
                    NavigationService.Navigate(new AddSubcriterionPage(criterion));
                };
                listBox.Items.Add(listBoxItem);
                index++;

                ulong subindex = 0;
                foreach (var subcriterion in Globals.AHP.GetSubcriterionNames(criterion))
                {
                    ListBoxItem otherListBoxItem = new ListBoxItem();
                    otherListBoxItem.Content = $"    {(char)('a' + subindex)}. {subcriterion}";
                    listBox.Items.Add(otherListBoxItem);
                    subindex++;
                }
            }
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FirstOptionPage());
        }
    }
}
