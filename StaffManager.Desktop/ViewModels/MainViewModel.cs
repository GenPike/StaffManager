using Microsoft.Win32;
using StaffManager.Desktop.Commands;
using System.Windows.Input;

namespace StaffManager.Desktop.ViewModels;

internal sealed class MainViewModel
{
    public ICommand PickOrderFileCommand { get; }

    public ICommand PickTemplateFileCommand { get; }
    
    public ICommand SelectTargetFolderCommand { get; }

    public string OrderFilePath { get; set; }

    public string TemplateFilePath { get; set; }

    public string TargetFolderPath { get; set; }

    public MainViewModel()
    {
        PickOrderFileCommand = new RelayCommand<object>(PickOrderFile);
        PickTemplateFileCommand = new RelayCommand<object>(PickTemplateFile);
        SelectTargetFolderCommand = new RelayCommand<object>(SelectTargetFolder);
    }

    private void PickOrderFile(object _)
    {
        OrderFilePath = PickWordFileDialog();
    }

    private void PickTemplateFile(object _)
    {
        TemplateFilePath = PickWordFileDialog();
    }

    private void SelectTargetFolder(object _)
    {
        TargetFolderPath = SelectFolderDialog();
    }

    private static string PickWordFileDialog()
    {
        var fileDialog = new OpenFileDialog
        {
            Title = "Оберіть файл...",
            Filter = "Word files (*.docx)|*.docx"
        };

        if (fileDialog.ShowDialog() is null or false)
            return null;

        return fileDialog.FileName;
    }

    private static string SelectFolderDialog()
    {
        var openFolderDialog = new OpenFolderDialog
        {
            Title = "Оберіть папку...",
        };

        if (openFolderDialog.ShowDialog() is null or false)
            return null;

        return openFolderDialog.FolderName;
    }
}
