using LOGrasper.Commands;
using LOGrasper.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Diagnostics;
using System.Windows.Input;
using System;

namespace LOGrasper.ViewModels;

public class SearchViewViewModel : ViewModelBase
{
    public RootFolderBrowseViewModel RootFolderBrowseViewModel { get; set; }
    public KeywordListViewModel KeywordListViewModel { get; set; }
    public OutputWindowViewModel OutputWindowViewModel { get; set; }
    
    public SearchObject SearchObject { get; set; }

    private string _searchButton = "SEARCH";
    private string _searchButtonColor = "#6CCCEA";

    private bool _cancellationFlag = false;

    private string _messageDispenser;

    private string _stopwatch;

    public Stopwatch stopwatch;

    private bool _hasKeywordList = false;
    private bool _hasRootFolder = false;

    public ICommand SearchCommand { get; }

    public bool HasKeywordList
    {
        get { return _hasKeywordList; }
        set
        {
            _hasKeywordList = value;
            OnPropertyChanged(nameof(HasKeywordList));
        }
    }

    public bool HasRootFolder
    {
        get { return _hasRootFolder; }
        set
        {
            _hasRootFolder = value;
            OnPropertyChanged(nameof(HasRootFolder));
        }
    }
    public string MessageDispenser
    {
        get => _messageDispenser;
        set
        {
            _messageDispenser = value;
            OnPropertyChanged(nameof(MessageDispenser));
        }
    }

    public string SearchButton
    {
        get => _searchButton;
        set
        {
            _searchButton = value;
            OnPropertyChanged(nameof(SearchButton));
        }
    }

    public string SearchButtonColor
    {
        get => _searchButtonColor;
        set
        {
            _searchButtonColor = value;
            OnPropertyChanged(nameof(SearchButtonColor));
        }
    }

    public bool CancellationFlag
    {
        get => _cancellationFlag;
        set
        {
            _cancellationFlag = value;
            OnPropertyChanged(nameof(CancellationFlag));
        }
    }

    public string StopwatchString
    {
        get=> _stopwatch;
        set
        {
            _stopwatch = value;
            OnPropertyChanged(nameof(StopwatchString));
        }
    }
        

    public async Task InitiateAsyncSearch(RootFolderBrowseViewModel rootFolderBrowseViewModel, KeywordListViewModel keywordListViewModel)
    {
        MessageDispenser = "Search Started";
        SearchObject = new SearchObject(rootFolderBrowseViewModel.RootFolderPath, keywordListViewModel._keywordList);


        SearchEngine go = new(SearchObject, this);
                      
        Task search = Task.Run(() => go.SearchAC(CancellationFlag));
        
        await search;
        MessageDispenser = "Search Completed in " + StopwatchString;
        SearchButton = "SEARCH";
        SearchButtonColor = "#6CCCEA";
    }

    public SearchViewViewModel()
    {

        RootFolderBrowseViewModel = new RootFolderBrowseViewModel(this);
        KeywordListViewModel = new KeywordListViewModel(this);
        OutputWindowViewModel = new OutputWindowViewModel(this, RootFolderBrowseViewModel);
        SearchCommand = new SearchCommand(this);

    }

    public void GetDirectoryStatistics()
    {
        MessageDispenser = "You Picked a total of " + Math.Round(RootFolderBrowseViewModel.TotalSizeMB, 2).ToString() + " MB " + " from a total of " + RootFolderBrowseViewModel.folderCount + " folders and " + RootFolderBrowseViewModel.fileCount + " files";
    }
}