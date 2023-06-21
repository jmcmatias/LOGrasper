using LOGrasper.Commands;
using LOGrasper.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Diagnostics;
using System.Windows.Input;
using System;
using System.Linq;

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
    private string _systemInfo;

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
            _messageDispenser = "";
            _messageDispenser = value;
            OnPropertyChanged(nameof(MessageDispenser));
        }
    }

    public string SystemInfo
    {
        get => _systemInfo;
        set
        {
            _systemInfo = value;
            OnPropertyChanged(nameof(SystemInfo));
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
        
        SearchButton = "SEARCH";
        SearchButtonColor = "#6CCCEA";

        if(!OutputWindowViewModel.FoundInFiles.Any())
        {
            
            MessageDispenser = "Search Completed in " + StopwatchString + " => NO MATCHES WHERE FOUND";
        }
        else
        {
            MessageDispenser = "Search Completed in " + StopwatchString;
        }
        SystemInfo = "Average SearchTasks/Second: " + Math.Round(go.GetTotalSearchTasks() / stopwatch.Elapsed.TotalSeconds, 2);
    }

    public SearchViewViewModel()
    {
        _systemInfo = GetSystemInfo();
        _messageDispenser = string.Empty;
        _stopwatch = string.Empty;
        stopwatch = new();
        RootFolderBrowseViewModel = new RootFolderBrowseViewModel(this);
        KeywordListViewModel = new KeywordListViewModel(this);
        OutputWindowViewModel = new OutputWindowViewModel(this, RootFolderBrowseViewModel);
        SearchCommand = new SearchCommand(this);

    }

    public void GetDirectoryStatistics()
    {
        MessageDispenser = "You Picked a total of " + Math.Round(RootFolderBrowseViewModel.TotalSizeMB, 2).ToString() + " MB " + " from a total of " + RootFolderBrowseViewModel.folderCount + " folders and " + RootFolderBrowseViewModel.fileCount + " files";
    }

    public string GetSystemInfo()
    {
        string processors = string.Empty;
        int coreCount = 0;
        foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get())
        {
            processors = "Number Of Physical Processors: " + item["NumberOfProcessors"];
        }


        foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
        {
            if (item["NumberOfCores"] != null)
                coreCount += int.Parse(item["NumberOfCores"].ToString());
        }

        return processors + " | Number of Cores: " + coreCount + " | Number Of Logical Processors: " + Environment.ProcessorCount + " | Maximum Search Tasks Selected " + Environment.ProcessorCount; 
    }
}