using LOGrasper.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO.Packaging;
using System.Printing;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using static LOGrasper.Models.OutputObject;

namespace LOGrasper.ViewModels;

public class OutputWindowViewModel : ViewModelBase
{
    private readonly ObservableCollection<FoundInFileViewModel> _foundInFiles = new();
    private readonly OutputObject _outputObject = new();

    public IEnumerable<FoundInFileViewModel> FoundInFiles
    {
        get
        {
            return _foundInFiles;
        }
    }


    public ICommand ClearOutputCommand { get; }
    public ICommand SaveOutputCommand { get; }
    
    
    public OutputWindowViewModel()
    {
        
      
        /*
        ObservableCollection<FoundInFile.LineInfo> _lines = new();
        ObservableCollection<FoundInFile.LineInfo> _lines2 = new();

        _lines.Add(new FoundInFile.LineInfo(1, "teste1"));
        _lines.Add(new FoundInFile.LineInfo(2, "teste2"));
        _lines.Add(new FoundInFile.LineInfo(3, "teste3"));

        _lines2.Add(new FoundInFile.LineInfo(4, "teste4"));
        _lines2.Add(new FoundInFile.LineInfo(5, "teste5"));
        _lines2.Add(new FoundInFile.LineInfo(6, "teste6"));
        

        _foundInFiles.Add(new FoundInFileViewModel(new FoundInFile("Path1", "FileName1", _lines)));
        _foundInFiles.Add(new FoundInFileViewModel(new FoundInFile("Path2", "FileName2", _lines2)));
        */
       // UpdateOutput(_outputObject);
        int debug = 1;

        
    }






    public void UpdateOutput(OutputObject outputObject)
    {
        _foundInFiles.Clear();
        foreach ( var foundInFile in outputObject._ouputObject)
        {
            _foundInFiles.Add(new FoundInFileViewModel(foundInFile));
            OnPropertyChanged(nameof(_foundInFiles));
        }      
       
    }
    
    /*
    public OutputWindowViewModel()
    {
        
        ObservableCollection<FoundInFile.LineInfo> _lines = new();
        ObservableCollection<FoundInFile.LineInfo> _lines2 = new();

        _lines.Add(new FoundInFile.LineInfo(1, "teste1"));
        _lines.Add(new FoundInFile.LineInfo(2, "teste2"));
        _lines.Add(new FoundInFile.LineInfo(3, "teste3"));

        _lines2.Add(new FoundInFile.LineInfo(4, "teste4"));
        _lines2.Add(new FoundInFile.LineInfo(5, "teste5"));
        _lines2.Add(new FoundInFile.LineInfo(6, "teste6"));


        _foundInFiles.Add(new FoundInFileViewModel(new FoundInFile("Path1", "FileName1", _lines)));
        _foundInFiles.Add(new FoundInFileViewModel(new FoundInFile("Path2", "FileName2", _lines2)));
    }
    */
    public OutputWindowViewModel(ICommand clearOutputCommand, ICommand saveOutputCommand)
    {
        ClearOutputCommand = clearOutputCommand;
        SaveOutputCommand = saveOutputCommand;
    }

    public ObservableCollection<FoundInFileViewModel> ConvertOutputObjectToViewmodel(OutputObject outputObject)
    {
        ObservableCollection<FoundInFileViewModel> foundInFileViewModels = new();

        foreach (var file in outputObject._ouputObject)
        {
            foundInFileViewModels.Add(new FoundInFileViewModel(file));
        }

        return foundInFileViewModels;
    }


    public void ClearOutput()
    {
        _foundInFiles.Clear();
      

    }
}