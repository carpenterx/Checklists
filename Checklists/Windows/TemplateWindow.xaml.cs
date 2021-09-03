using Checklists.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace Checklists.Windows
{
    /// <summary>
    /// Interaction logic for TemplateWindow.xaml
    /// </summary>
    public partial class TemplateWindow : Window
    {
        private ChecklistTemplate checklistTemplate = new();

        //private ObservableCollection<ChecklistStep> checklistSteps = new();
        public TemplateWindow()
        {
            InitializeComponent();

            DataContext = checklistTemplate;
            stepsListView.ItemsSource = checklistTemplate.ChecklistSteps;
        }

        private void AddTemplateClick(object sender, RoutedEventArgs e)
        {
            GetWindow(this).DialogResult = true;
            GetWindow(this).Close();
        }

        public ChecklistTemplate GetTemplate()
        {
            return checklistTemplate;
        }

        private void AddActionClick(object sender, RoutedEventArgs e)
        {
            if (stepTxt.Text != string.Empty)
            {
                checklistTemplate.ChecklistSteps.Add(new ChecklistStep(stepTxt.Text));
            }
        }
    }
}
