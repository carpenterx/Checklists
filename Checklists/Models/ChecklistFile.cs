namespace Checklists.Models
{
    public class ChecklistFile
    {
        public string ChecklistName { get; set; }
        public string FilePath { get; set; }

        public ChecklistFile(string checklistName, string filePath)
        {
            ChecklistName = checklistName;
            FilePath = filePath;
        }
    }
}
