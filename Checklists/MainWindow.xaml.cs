﻿using Checklists.Models;
using Checklists.Windows;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Checklists
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string APPLICATION_FOLDER = "Checklists";
        private static readonly string TEMPLATES_FOLDER = "Templates";
        private static readonly string TEMPLATE_EXTENSION = ".json";

        private ObservableCollection<ChecklistTemplate> checklistTemplates = new();

        public MainWindow()
        {
            InitializeComponent();

            LoadAllTemplates();

            templatesListView.ItemsSource = checklistTemplates;
        }

        private void LoadAllTemplates()
        {
            checklistTemplates.Clear();

            string templatesDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), APPLICATION_FOLDER, TEMPLATES_FOLDER);
            if (Directory.Exists(templatesDirectory))
            {
                string[] templateFiles = Directory.GetFiles(templatesDirectory, $"*{TEMPLATE_EXTENSION}");
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

        private void AddNewTemplate(object sender, RoutedEventArgs e)
        {
            TemplateWindow templateWindow = new();
            templateWindow.Owner = this;
            if (templateWindow.ShowDialog() == true)
            {
                checklistTemplates.Add(templateWindow.GetTemplate());
            }
        }

        private void SaveTemplatesOnClosing(object sender, System.ComponentModel.CancelEventArgs e)
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
            string templatePath = Path.Combine(rootPath, $"{template.FileName}{TEMPLATE_EXTENSION}");

            string json = JsonConvert.SerializeObject(template, Formatting.Indented);
            File.WriteAllText(templatePath, json);
        }

        private void CreateChecklist(object sender, RoutedEventArgs e)
        {
            if (templatesListView.SelectedItem is ChecklistTemplate selectedTemplate)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Title = "Save Checklist File";
                dlg.Filter = "Json Files(*.json)|*.json";
                //dlg.FileName = "hotspots";

                if (dlg.ShowDialog() == true)
                {
                    ChecklistName nameWindow = new();
                    nameWindow.Owner = this;
                    if (nameWindow.ShowDialog() == true)
                    {
                        Checklist checklist = new Checklist(nameWindow.Data, selectedTemplate);
                        string json = JsonConvert.SerializeObject(checklist, Formatting.Indented);
                        File.WriteAllText(dlg.FileName, json);
                    }
                }
            }
        }

        private void LoadChecklist(object sender, RoutedEventArgs e)
        {
            FileDialog dlg = new OpenFileDialog();
            dlg.Title = "Load Checklist";
            dlg.Filter = "Json Files(*.json)|*.json";
            if (dlg.ShowDialog() == true)
            {
                Checklist checklist = JsonConvert.DeserializeObject<Checklist>(File.ReadAllText(dlg.FileName));
                ChecklistWindow checklistWindow = new(checklist, dlg.FileName);
                checklistWindow.Owner = this;
                if (checklistWindow.ShowDialog() == true)
                {

                }
            }
        }

        private void DeleteTemplate(object sender, RoutedEventArgs e)
        {
            if (templatesListView.SelectedItem is ChecklistTemplate selectedTemplate)
            {
                checklistTemplates.Remove(selectedTemplate);
                string templatePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), APPLICATION_FOLDER, TEMPLATES_FOLDER, $"{selectedTemplate.FileName}{TEMPLATE_EXTENSION}");
                File.Delete(templatePath);
            }
        }
    }
}