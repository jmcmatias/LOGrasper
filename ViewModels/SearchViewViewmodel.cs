using System;
using System.Windows.Controls;
using System.Windows.Input;
using LOGrasper.ViewModels;

namespace LOGrasper.ViewModels;

public class SearchViewViewmodel : ViewModelBase
{
    public KeywordListViewModel KeywordListViewModel { get; set; }
    public OutputWindowViewModel OutputWindowViewModel { get; set; }

    public String RootFolder = "teste";

    public SearchViewViewmodel()
    {
        KeywordListViewModel = new KeywordListViewModel();
        OutputWindowViewModel = new OutputWindowViewModel();
    }

    public ICommand BrowseRootFolderCommand { get; set; }
    public ICommand SearchCommand { get; set; }

    public bool CanSearch
    {
        get
        {
            if (KeywordListViewModel.CanEdit && RootFolder != "") return true;
            else return false;
        }
    }
}