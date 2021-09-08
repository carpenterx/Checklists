using Checklists.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Checklists.Windows
{
    /// <summary>
    /// Interaction logic for ChecklistName.xaml
    /// </summary>
    public partial class ChecklistName : Window
    {
        public string Name
        {
            get => NameTextBox.Text;
            set => NameTextBox.Text = value;
        }

        public ObservableCollection<ChecklistVariable> checklistVariables = new();

        public ChecklistName(ChecklistTemplate template)
        {
            InitializeComponent();

            ExtractVariables(template);
        }

        private void ExtractVariables(ChecklistTemplate template)
        {
            List<string> stringsList = new();
            stringsList.Add(template.Name);
            foreach (ChecklistStep checklistStep in template.ChecklistSteps)
            {
                stringsList.Add(checklistStep.Text);
            }
            
            List<string> matches = GetMatchesList(stringsList);

            foreach (string match in matches)
            {
                checklistVariables.Add(new ChecklistVariable(match));
            }
            variablesListView.ItemsSource = checklistVariables;
        }

        private List<string> GetMatchesList(List<string> strings)
        {
            List<string> matchesList = new List<string>();
            Regex variablePattern = new Regex(@"\[(\w+?)\]");
            foreach (string stringItem in strings)
            {
                MatchCollection matches = variablePattern.Matches(stringItem);
                foreach (Match match in matches)
                {
                    matchesList.Add(match.Groups[1].Value);
                }
            }
            return matchesList.Distinct().ToList();
        }

        private void SetDataClick(object sender, RoutedEventArgs e)
        {
            GetWindow(this).DialogResult = true;
            GetWindow(this).Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            GetWindow(this).Close();
        }

        private void SelectAllText(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox inputText)
            {
                inputText.SelectAll();
            }
        }
    }
}
