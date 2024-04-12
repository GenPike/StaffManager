using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using StaffManager.Core.BusinessTrip;
using StaffManager.Core.Order;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace StaffManager.Desktop.ViewModels;

internal sealed class MainViewModel : ObservableObject
{
    private bool _isProduceAvailable = true;

    private string _orderFilePath;
    private string _templateFilePath;
    private string _targetFolderPath;

    public ICommand PickOrderFileCommand { get; }

    public ICommand PickTemplateFileCommand { get; }
    
    public ICommand SelectTargetFolderCommand { get; }

    public ICommand ProduceCommand { get; }

    public string OrderFilePath
    { 
        get => _orderFilePath; 
        set => SetProperty(ref _orderFilePath, value); 
    }

    public string TemplateFilePath 
    { 
        get => _templateFilePath;
        set => SetProperty(ref _templateFilePath, value);
    }

    public string TargetFolderPath 
    { 
        get => _targetFolderPath;
        set => SetProperty(ref _targetFolderPath, value);
    }

    public MainViewModel()
    {
        PickOrderFileCommand = new RelayCommand(PickOrderFile);
        PickTemplateFileCommand = new RelayCommand(PickTemplateFile);
        SelectTargetFolderCommand = new RelayCommand(SelectTargetFolder);
        ProduceCommand = new RelayCommand(ProduceCertifs, () => _isProduceAvailable);
    }

    private void ProduceCertifs()
    {
        // TODO: possibly show gif animation

        _isProduceAvailable = false;

        try
        {
            var orderParser = new OrderParser(OrderFilePath);
            var order = orderParser.Parse();

            var businessTripGenerator = new BusinessTripCertifGenerator(TemplateFilePath, TargetFolderPath);
            businessTripGenerator.Produce(order);

            MessageBox.Show("Створено!");
        }
        catch (IOException ex)
        {
            MessageBox.Show(ex.Message);
        }
        catch
        {
            MessageBox.Show("Помилка");
        }
        finally
        {
            _isProduceAvailable = true;
        }
    }

    private void PickOrderFile()
    {
        OrderFilePath = PickWordFileDialog();
    }

    private void PickTemplateFile()
    {
        TemplateFilePath = PickWordFileDialog();
    }

    private void SelectTargetFolder()
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
