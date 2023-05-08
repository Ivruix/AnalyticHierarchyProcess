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
    /// Interaction logic for CurrentOptionsPage.xaml
    /// </summary>
    public partial class CurrentOptionsPage : Page
    {
        public CurrentOptionsPage()
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
            foreach (var alternative in Globals.AHP.GetAlternativeNames())
            {
                ListBoxItem listBoxItem = new ListBoxItem();
                listBoxItem.Content = $"{index}. {alternative}";
                listBoxItem.FontWeight = FontWeights.Bold;
                listBox.Items.Add(listBoxItem);
                index++;
            }
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            Globals.AHP.FinalizeStructure();
            NavigationService.Navigate(new FullComparisonPage());
        }

        private void AddAnother(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddAnotherOptionPage());
        }
    }
}
