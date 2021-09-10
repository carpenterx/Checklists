using Checklists.Models;
using Checklists.Properties;
using Checklists.Windows;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace Checklists
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string APPLICATION_FOLDER = "Checklists";
        private const string TEMPLATES_FOLDER = "Templates";
        //private const string TEMPLATE_EXTENSION = ".json";
        //private const string CHECKLIST_EXTENSION = ".chek";
        //private const string CHECKLIST_FILE = "Checklist";

        private const string CHECKLISTS_HISTORY = "Checklists.json";

        private ObservableCollection<ChecklistTemplate> checklistTemplates = new();
        private ObservableCollection<ChecklistFile> checklistFiles = new();

        public MainWindow()
        {
            InitializeApplication();
        }

        public MainWindow(string checklistPath)
        {
            InitializeApplication();

            Checklist checklist = JsonConvert.DeserializeObject<Checklist>(File.ReadAllText(checklistPath));
            ChecklistWindow checklistWindow = new(checklist, checklistPath);
            //checklistWindow.Owner = this;
            if (checklistWindow.ShowDialog() == true)
            {

            }
        }

        private void InitializeApplication()
        {
            InitializeComponent();

            LoadAllTemplates();

            LoadChecklistsHistory();

            templatesListView.ItemsSource = checklistTemplates;
            checklistsListView.ItemsSource = checklistFiles;
        }

        private void LoadAllTemplates()
        {
            checklistTemplates.Clear();

            string templatesDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), APPLICATION_FOLDER, TEMPLATES_FOLDER);
            if (Directory.Exists(templatesDirectory))
            {
                string[] templateFiles = Directory.GetFiles(templatesDirectory, $"*{Settings.Default.TemplateExtension}");
                foreach (string templateFile in templateFiles)
                {
                    checklistTemplates.Add(GetChecklistTemplate(templateFile));
                }
            }
        }

        private ChecklistTemplate GetChecklistTemplate(string filePath)
        {
            return JsonConvert.DeserializeObject<ChecklistTemplate>(File.ReadAllText(filePath));
        }

        private void LoadChecklistsHistory()
        {
            string checklistsHistoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), APPLICATION_FOLDER, CHECKLISTS_HISTORY);
            if (File.Exists(checklistsHistoryPath))
            {
                checklistFiles = JsonConvert.DeserializeObject<ObservableCollection<ChecklistFile>>(File.ReadAllText(checklistsHistoryPath));
            }
        }

        private void AddNewTemplate(object sender, RoutedEventArgs e)
        {
            TemplateWindow templateWindow = new();
            templateWindow.Owner = this;
            if (templateWindow.ShowDialog() == true)
            {
                checklistTemplates.Add(templateWindow.GetTemplate());
            }
        }

        private void SaveDataOnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveTemplates();

            SaveChecklistsHistory();
        }

        private void SaveTemplates()
        {
            string templatesDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), APPLICATION_FOLDER, TEMPLATES_FOLDER);
            if (!Directory.Exists(templatesDirectory))
            {
                Directory.CreateDirectory(templatesDirectory);
            }
            foreach (ChecklistTemplate template in checklistTemplates)
            {
                SaveTemplate(template, templatesDirectory);
            }
        }

        private void SaveTemplate(ChecklistTemplate template, string rootPath)
        {
            string templatePath = Path.Combine(rootPath, $"{template.FileName}{Settings.Default.TemplateExtension}");

            string json = JsonConvert.SerializeObject(template, Formatting.Indented);
            File.WriteAllText(templatePath, json);
        }

        private void SaveChecklistsHistory()
        {
            string checklistsHistoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), APPLICATION_FOLDER, CHECKLISTS_HISTORY);
            string json = JsonConvert.SerializeObject(checklistFiles, Formatting.Indented);
            File.WriteAllText(checklistsHistoryPath, json);
        }

        private void CreateChecklist(object sender, RoutedEventArgs e)
        {
            if (templatesListView.SelectedItem is ChecklistTemplate selectedTemplate)
            {
                ChecklistName nameWindow = new(selectedTemplate);
                nameWindow.Owner = this;
                if (nameWindow.ShowDialog() == true)
                {
                    Checklist checklist;
                    if (nameWindow.checklistVariables.Count > 0)
                    {
                        checklist = new Checklist(nameWindow.Name, selectedTemplate, nameWindow.checklistVariables);
                    }
                    else
                    {
                        checklist = new Checklist(nameWindow.Name, selectedTemplate);
                    }

                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.Title = $"Save {Settings.Default.ChecklistFile} File";
                    dlg.FileName = checklist.Name;
                    dlg.Filter = $"{Settings.Default.ChecklistFile} Files(*{Settings.Default.ChecklistExtension})|*{Settings.Default.ChecklistExtension}";

                    if (dlg.ShowDialog() == true)
                    {
                        string json = JsonConvert.SerializeObject(checklist, Formatting.Indented);
                        File.WriteAllText(dlg.FileName, json);
                        checklistFiles.Add(new ChecklistFile(checklist.Name, dlg.FileName));
                    }
                }
            }
        }

        private void LoadChecklist(object sender, RoutedEventArgs e)
        {
            if (checklistsListView.SelectedItem is ChecklistFile selectedChecklistFile)
            {
                LoadChecklistFile(selectedChecklistFile.FilePath);
            }
            else
            {
                FileDialog dlg = new OpenFileDialog();
                dlg.Title = $"Load {Settings.Default.ChecklistFile}";
                dlg.Filter = $"{Settings.Default.ChecklistFile} Files(*{Settings.Default.ChecklistExtension})|*{Settings.Default.ChecklistExtension}";
                if (dlg.ShowDialog() == true)
                {
                    LoadAndAddChecklistFile(dlg.FileName);
                }
            }
        }

        private void LoadChecklistFile(string checklistPath)
        {
            if (File.Exists(checklistPath))
            {
                Checklist checklist = JsonConvert.DeserializeObject<Checklist>(File.ReadAllText(checklistPath));
                ChecklistWindow checklistWindow = new(checklist, checklistPath);
                checklistWindow.Owner = this;
                if (checklistWindow.ShowDialog() == true)
                {

                }
            }
            else
            {
                ShowFileNotFoundError(checklistPath);
            }
        }

        private void LoadAndAddChecklistFile(string checklistPath)
        {
            if (File.Exists(checklistPath))
            {
                Checklist checklist = JsonConvert.DeserializeObject<Checklist>(File.ReadAllText(checklistPath));
                checklistFiles.Add(new ChecklistFile(checklist.Name, checklistPath));
                ChecklistWindow checklistWindow = new(checklist, checklistPath);
                checklistWindow.Owner = this;
                if (checklistWindow.ShowDialog() == true)
                {

                }
            }
            else
            {
                ShowFileNotFoundError(checklistPath);
            }
        }

        private void ShowFileNotFoundError(string path)
        {
            MessageBox.Show($"Could not find checklist file at: {path}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void DeleteTemplate(object sender, RoutedEventArgs e)
        {
            if (templatesListView.SelectedItem is ChecklistTemplate selectedTemplate)
            {
                checklistTemplates.Remove(selectedTemplate);
                string templatePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), APPLICATION_FOLDER, TEMPLATES_FOLDER, $"{selectedTemplate.FileName}{Settings.Default.TemplateExtension}");
                File.Delete(templatePath);
            }
        }

        private void EditTemplate(object sender, RoutedEventArgs e)
        {
            if (templatesListView.SelectedItem is ChecklistTemplate selectedTemplate)
            {
                TemplateWindow templateWindow = new(selectedTemplate);
                templateWindow.Owner = this;
                if (templateWindow.ShowDialog() == true)
                {
                    selectedTemplate = templateWindow.GetTemplate();
                }
            }
        }

        private void DeleteChecklist(object sender, RoutedEventArgs e)
        {
            if (checklistsListView.SelectedItem is ChecklistFile selectedChecklistFile)
            {
                checklistFiles.Remove(selectedChecklistFile);
            }
        }
    }
}