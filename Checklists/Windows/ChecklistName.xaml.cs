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
using System.Windows.Shapes;

namespace Checklists.Windows
{
    /// <summary>
    /// Interaction logic for ChecklistName.xaml
    /// </summary>
    public partial class ChecklistName : Window
    {
        public string Data
        {
            get => DataTextBox.Text;
            set => DataTextBox.Text = value;
        }

        public ChecklistName()
        {
            InitializeComponent();
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
