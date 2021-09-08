using System;
using System.Collections.ObjectModel;

namespace Checklists.Models
{
    public class Checklist
    {
        public string Name { get; set; }

        public string TemplateFileName { get; set; }
        public ObservableCollection<ChecklistStep> ChecklistSteps { get; set; }

        public Checklist()
        {
            Name = string.Empty;
            TemplateFileName = string.Empty;
            ChecklistSteps = new ObservableCollection<ChecklistStep>();
        }

        public Checklist(string name, ChecklistTemplate template)
        {
            Name = name;
            TemplateFileName = template.FileName;
            ChecklistSteps = template.ChecklistSteps;
        }

        public Checklist(string name, ChecklistTemplate template, ObservableCollection<ChecklistVariable> checklistVariables)
        {
            Name = ReplaceVariables(template.Name, checklistVariables);
            TemplateFileName = template.FileName;
            ChecklistSteps = GetReplacedSteps(template.ChecklistSteps, checklistVariables);
        }

        private string ReplaceVariables(string original, ObservableCollection<ChecklistVariable> checklistVariables)
        {
            string replacement = original;
            foreach (ChecklistVariable checklistVariable in checklistVariables)
            {
                replacement = replacement.Replace(checklistVariable.Label, checklistVariable.Value);
            }
            return replacement;
        }

        private ObservableCollection<ChecklistStep> GetReplacedSteps(ObservableCollection<ChecklistStep> checklistSteps, ObservableCollection<ChecklistVariable> checklistVariables)
        {
            ObservableCollection<ChecklistStep> replacedSteps = new();
            foreach (ChecklistStep step in checklistSteps)
            {
                replacedSteps.Add(new ChecklistStep(ReplaceVariables(step.Text, checklistVariables)));
            }
            return replacedSteps;
        }
    }
}
