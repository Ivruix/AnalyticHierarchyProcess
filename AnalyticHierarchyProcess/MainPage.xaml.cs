using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Help(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HelpPage());
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void NewProject(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NewProjectPage());
        }

        private void OpenProject(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "AHP project file (*.ahpproject)|*.ahpproject";
            if (openFileDialog.ShowDialog() == true)
            {
                var formatter = new BinaryFormatter();
                using var fs = new FileStream(openFileDialog.FileName, FileMode.Open);
                Globals.AHP = (AHP) formatter.Deserialize(fs);
                NavigationService.Navigate(new CurrentProjectPage());
            }
        }
    }
}
