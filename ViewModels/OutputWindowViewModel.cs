using LOGrasper.Commands;
using LOGrasper.Models;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LOGrasper.ViewModels;

public class OutputWindowViewModel : ViewModelBase
{
    private readonly ObservableCollection<FoundInFileViewModel> _foundInFiles = new();
    private readonly OutputObject _outputObject = new();
    private bool _foundInFilesEmpty = true;

    private static SearchViewViewModel _searchViewViewModel;
    private readonly RootFolderBrowseViewModel _rootFolderBrowseViewModel;

    public SearchViewViewModel MainViewModel 
    {
        get { return _searchViewViewModel; }
        set
        {
            _searchViewViewModel = value;
        } 
    }

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
        ClearOutputCommand = new ClearOutputCommand(this, _searchViewViewModel);
        SaveOutputCommand = new SaveOutputCommand(this, searchViewViewModel);
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
    public Task UpdateOutput(OutputObject outputObject)
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

        return Task.CompletedTask;
    }

    public void ClearOutput()
    {
        _foundInFiles.Clear();
        FoundInFilesEmpty = true;
        _searchViewViewModel.MessageDispenser = "Output Cleared";
    }

    public void SaveOutput(string stopwatch)
    {
        try
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

            if (filename != null)
            {
                TextWriter tw = new StreamWriter(filename);

                tw.WriteLine("Search took " + stopwatch + " to search the keywords:");
                foreach (KeywordViewModel kw in _searchViewViewModel.SearchObject._keywordList)
                {                    
                    tw.Write(" - " + kw.Keyword);
                    if (kw.IsNot)
                    {
                        tw.Write(" =>[NOT]");
                    }
                    tw.WriteLine();
                }

                tw.WriteLine("From a total of " + Math.Ceiling(_rootFolderBrowseViewModel.TotalSizeMB) + " MB");

                foreach (var file in _foundInFiles)
                {
                    tw.Write("\n" + file.LinesFound.Count() + " Lines found @File -> ");
                    tw.WriteLine(file.FileName);
                    foreach (var line in file.LinesFound)
                    {
                        tw.WriteLine(line.DisplayLine);
                    }
                    tw.WriteLine();
                }
                tw.Close();
            }
       

        if (File.Exists(filename))
        {
                try
                {
                    // Start the process for the specific file using the default application
                    ProcessStartInfo processStartInfo = new(filename)
                    {
                        UseShellExecute = true          // Uses the windows shell to execute the default application for the extension
                    };
                    Process.Start(processStartInfo);
                }
                catch (Exception ex)
                {
                    _searchViewViewModel.MessageDispenser = ex.Message;
                }
        }
        else
        {
            _searchViewViewModel.MessageDispenser = "File was not created successfully, please try again.";
        }
        }
        catch (Exception ex)
        {
            _searchViewViewModel.MessageDispenser = ex.Message;
        }
    }
}