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
    /// Interaction logic for AddSubcriterionPage.xaml
    /// </summary>
    public partial class AddSubcriterionPage : Page
    {
        private string _criterion;
        public AddSubcriterionPage(string criterion)
        {
            InitializeComponent();
            _criterion = criterion;
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            if (textBox.Text.Length == 0)
            {
                return;
            }
            Globals.AHP.AddSubcriterion(_criterion, textBox.Text);
            NavigationService.GoBack();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
