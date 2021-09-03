using System.Collections.ObjectModel;

namespace Checklists.Models
{
    public class ChecklistTemplate
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public ObservableCollection<ChecklistStep> ChecklistSteps { get; set; }

        public ChecklistTemplate()
        {
            ChecklistSteps = new ObservableCollection<ChecklistStep>();
        }
    }
}
