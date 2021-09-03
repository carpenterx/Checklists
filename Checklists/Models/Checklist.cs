using System.Collections.ObjectModel;

namespace Checklists.Models
{
    public class Checklist
    {
        public string Name { get; set; }

        public string TemplateFileName { get; set; }
        public ObservableCollection<ChecklistStep> ChecklistSteps { get; set; }

        public Checklist(string name, ChecklistTemplate template)
        {
            Name = name;
            TemplateFileName = template.FileName;
            ChecklistSteps = template.ChecklistSteps;
        }

        public Checklist()
        {
            Name = string.Empty;
            TemplateFileName = string.Empty;
            ChecklistSteps = new ObservableCollection<ChecklistStep>();
        }
    }
}
