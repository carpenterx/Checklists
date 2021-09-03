namespace Checklists.Models
{
    public class ChecklistStep
    {
        public string Text { get; set; }
        public bool IsDone { get; set; }

        public ChecklistStep(string text)
        {
            Text = text;
        }

        public ChecklistStep()
        {
            Text = string.Empty;
            //IsDone = false;
        }
    }
}
