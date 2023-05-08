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
    /// Interaction logic for SmallChangesSimulationStepPage.xaml
    /// </summary>
    public partial class SmallChangesSimulationStepPage : Page
    {
        public SmallChangesSimulationStepPage()
        {
            InitializeComponent();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SmallChangesSimulationPage((int) (slider.Value + 0.5)));
        }
    }
}
