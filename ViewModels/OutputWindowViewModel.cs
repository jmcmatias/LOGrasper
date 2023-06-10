using LOGrasper.Commands;
using LOGrasper.Models;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Linq;
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
    private bool _foundInFilesEmpty = true;

    private readonly SearchViewViewModel _searchViewViewModel;
    private readonly KeywordListViewModel _keywordListViewModel;
    private readonly RootFolderBrowseViewModel _rootFolderBrowseViewModel;

    public IEnumerable<FoundInFileViewModel> FoundInFiles
    {
        get
        {
            return _foundInFiles;
        }
    }

    public bool FoundInFilesEmpty
    {
        get { return _foundInFilesEmpty; }
        set
        {
            _foundInFilesEmpty = value;
            OnPropertyChanged(nameof(FoundInFiles));
        }
    }

    public ICommand ClearOutputCommand { get; }
    public ICommand SaveOutputCommand { get; }


    public OutputWindowViewModel(SearchViewViewModel searchViewViewModel, RootFolderBrowseViewModel rootFolderBrowseViewModel)
    {
        _searchViewViewModel = searchViewViewModel;
        _rootFolderBrowseViewModel = rootFolderBrowseViewModel;
        ClearOutputCommand = new ClearOutputCommand(this);
        SaveOutputCommand = new SaveOutputCommand(this, searchViewViewModel);
    }

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
    public void UpdateOutput(OutputObject outputObject)
    {
        _foundInFiles.Clear();
        if (outputObject._ouputObject.Count != 0)
        {
            FoundInFilesEmpty = false;
            foreach (var foundInFile in outputObject._ouputObject)
            {
                _foundInFiles.Add(new FoundInFileViewModel(foundInFile));
                OnPropertyChanged(nameof(_foundInFiles));
            }

        }
    }
    public void ClearOutput()
    {
        _foundInFiles.Clear();
        FoundInFilesEmpty = true;
    }

    public void SaveOutput(string stopwatch)
    {
        VistaSaveFileDialog dialog = new VistaSaveFileDialog()
        {
            Filter = "txt|*.txt",
            AddExtension = true,
            OverwritePrompt = true,
            DefaultExt = ".txt",
            FileName = "SearchResult"
        };

        dialog.ShowDialog();

        string filename = dialog.FileName;
        ;

        if (filename != null)
        {
            TextWriter tw = new StreamWriter(filename);

            tw.WriteLine("Search took " + stopwatch + " to search the keywords:");
            foreach (string kw in _searchViewViewModel.SearchObject._keywordList)
            {
                tw.WriteLine("-"+kw);
            }

            tw.WriteLine("From a total of " + Math.Ceiling(_rootFolderBrowseViewModel.TotalSizeMB) + " MB");
            
        foreach (var file in _foundInFiles)
            {
                tw.Write("\n" + file.LinesFound.Count() + " Lines found @File -> ");
                tw.WriteLine(file.FileName);
                foreach (var line in file.LinesFound)
                {
                    tw.WriteLine("    " + line.DisplayLine);
                }
                tw.WriteLine();
            }
            tw.Close();
        }
        

    }


}