using Checklists.Models;
using Newtonsoft.Json;
using System.IO;
using System.Windows;

namespace Checklists.Windows
{
    /// <summary>
    /// Interaction logic for ChecklistWindow.xaml
    /// </summary>
    public partial class ChecklistWindow : Window
    {
        public Checklist Checklist { get; set; }
        private string filePath;
        public ChecklistWindow(Checklist checklist, string path)
        {
            InitializeComponent();

            filePath = path;
            Checklist = checklist;
            DataContext = Checklist;

            Title = Path.GetFileName(filePath);
        }

        private void SaveChecklist(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string json = JsonConvert.SerializeObject(Checklist, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}
