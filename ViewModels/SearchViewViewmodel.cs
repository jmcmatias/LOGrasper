using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using LOGrasper.Commands;
using LOGrasper.Models;
using LOGrasper.ViewModels;

namespace LOGrasper.ViewModels;

public class SearchViewViewmodel : ViewModelBase
{
    public RootFolderBrowseViewModel RootFolderBrowseViewModel { get; set; }
    public KeywordListViewModel KeywordListViewModel { get; set; }
    public OutputWindowViewModel OutputWindowViewModel { get; set; }

    public SearchObject _searchObject { get; set; }

   
    private string? _messageDispenser;

    private bool _canSearch = true;

    public ICommand SearchCommand { get; }

    public bool CanSearch
    {

        get { return _canSearch; }

        set
        {
            _canSearch = value;
            OnPropertyChanged(nameof(CanSearch));
        }
    }
    public string MessageDispenser { set => _messageDispenser = value; }

    public KeywordListViewModel GetKeywordListViewModel()
    {
        return KeywordListViewModel;
    }

    public void InitiateSearch(RootFolderBrowseViewModel rootFolderBrowseViewModel, KeywordListViewModel keywordListViewModel)
    {
        _searchObject = new SearchObject(rootFolderBrowseViewModel.RootFolderPath, keywordListViewModel._keywordList);

    }

    public SearchViewViewmodel()
    {
        RootFolderBrowseViewModel = new RootFolderBrowseViewModel();
        KeywordListViewModel = new KeywordListViewModel();
        OutputWindowViewModel = new OutputWindowViewModel();
        MessageDispenser = "TESTE";
        SearchCommand = new SearchCommand(this, RootFolderBrowseViewModel, KeywordListViewModel);
        // CanSearch(RootFolderBrowseViewModel, KeywordListViewModel);
    }

  

 


}