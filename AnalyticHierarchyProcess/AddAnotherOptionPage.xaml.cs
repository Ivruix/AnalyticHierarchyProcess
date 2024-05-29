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
    /// Interaction logic for AddAnotherOptionPage.xaml
    /// </summary>
    public partial class AddAnotherOptionPage : Page
    {
        public AddAnotherOptionPage()
        {
            InitializeComponent();
            textBox.Focus();
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            if (textBox.Text.Length == 0)
            {
                return;
            }
            Globals.AHP.AddAlternative(textBox.Text);
            NavigationService.GoBack();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
