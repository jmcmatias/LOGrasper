using LOGrasper.Commands;
using LOGrasper.Models;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Input;
using System;
using System.Linq;
using Xceed.Wpf.Toolkit;
using System.Threading;

namespace LOGrasper.ViewModels;

public class SearchViewViewModel : ViewModelBase
{
    // ViewModels for RootFolderBrowse, KeywordList, and OutputWindow
    public RootFolderBrowseViewModel RootFolderBrowseViewModel { get; set; }
    public KeywordListViewModel KeywordListViewModel { get; set; }
    public OutputWindowViewModel OutputWindowViewModel { get; set; }

    // SearchObject containing the search parameters
    public SearchObject SearchObject { get; set; }

    // Fields and properties related to the UI
    private string _searchButton = "SEARCH";
    private string _searchButtonColor = "#6CCCEA";
    private bool _cancellationFlag = false;
    private string _messageDispenser;
    private string _systemInfo;
    private string _stopwatch;
    private bool _hasKeywordList = false;
    private bool _hasRootFolder = false;
    // Stopwatch
    public Stopwatch stopwatch;

    // Task/Multitask Related fields
    private static int _numberOfTasks = Environment.ProcessorCount;
    private static SemaphoreSlim _semaphore = new SemaphoreSlim(Environment.ProcessorCount);
    private static int _semaphoreUpdate = _numberOfTasks;
    public SemaphoreSlim Semaphore { get { return _semaphore; } }

    // Commands
    public ICommand SearchCommand { get; }
    public ICommand SetNumberOfTasksCommand { get; }

    // Constructor
    public SearchViewViewModel()
    {
        GetSystemInfo();
        _messageDispenser = string.Empty;
        _stopwatch = string.Empty;
        stopwatch = new();
        RootFolderBrowseViewModel = new RootFolderBrowseViewModel(this);
        KeywordListViewModel = new KeywordListViewModel(this);
        OutputWindowViewModel = new OutputWindowViewModel(this, RootFolderBrowseViewModel);
        SearchCommand = new SearchCommand(this);
        SetNumberOfTasksCommand = new SetNumberOfTasksCommand(this);
    }

    // Property for the number of parallel tasks
    public int NumberOfTasks
    {
        get { return _numberOfTasks; }
        set
        {
            _numberOfTasks = value;
            OnPropertyChanged(nameof(NumberOfTasks));
        }
    }

    // Property for updating the semaphore count
    public int SemaphoreUpdate
    {
        get { return _semaphoreUpdate; }
        set
        {
            _semaphoreUpdate = value;
            if (_semaphoreUpdate <= 0)
            {
                NumberOfTasks = 1;
                _semaphoreUpdate = 1;
                MessageDispenser = "You cannot choose 0 Parallel SearchTasks, minimum is 1";
                _semaphore = new SemaphoreSlim(_semaphoreUpdate);
            }
            else
            {
                _semaphore = new SemaphoreSlim(_semaphoreUpdate);
                MessageDispenser = "You have chosen " + _semaphoreUpdate + " Parallel Search Tasks";
            }

            OnPropertyChanged(nameof(SemaphoreUpdate));
        }
    }

    // Properties for UI state
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

    // Property for displaying messages in the UI
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

    // Property for displaying system information in the UI
    public string SystemInfo
    {
        get => _systemInfo;
        set
        {
            _systemInfo = value;
            OnPropertyChanged(nameof(SystemInfo));
        }
    }

    // Property for the search button text in the UI
    public string SearchButton
    {
        get => _searchButton;
        set
        {
            _searchButton = value;
            OnPropertyChanged(nameof(SearchButton));
        }
    }

    // Property for the search button color in the UI
    public string SearchButtonColor
    {
        get => _searchButtonColor;
        set
        {
            _searchButtonColor = value;
            OnPropertyChanged(nameof(SearchButtonColor));
        }
    }

    // Property for the cancellation flag
    public bool CancellationFlag
    {
        get => _cancellationFlag;
        set
        {
            _cancellationFlag = value;
            OnPropertyChanged(nameof(CancellationFlag));
        }
    }

    // Property for displaying the stopwatch time in the UI
    public string StopwatchString
    {
        get => _stopwatch;
        set
        {
            _stopwatch = value;
            OnPropertyChanged(nameof(StopwatchString));
        }
    }

    // Method for initiating the asynchronous search
    public async Task InitiateAsyncSearch(RootFolderBrowseViewModel rootFolderBrowseViewModel, KeywordListViewModel keywordListViewModel)
    {
        // Set the message to indicate that the search has started
        MessageDispenser = "Search Started";    // Pratically unseen because of the speed

        // Create a new SearchObject with the root folder path and keyword list
        SearchObject = new SearchObject(rootFolderBrowseViewModel.RootFolderPath, keywordListViewModel._keywordList);

        // Create a new instance of the SearchEngine class, passing the SearchObject and the current ViewModel instance
        SearchEngine go = new SearchEngine(SearchObject, this);

        // Start a new task to execute the go.SearchAC() method on a background thread
        Task search = Task.Run(() => go.SearchAC());

        // Await the completion of the search task
        await search;

        // Update the SearchButton and SearchButtonColor properties to their default values
        SearchButton = "SEARCH";
        SearchButtonColor = "#6CCCEA";

        // Check if any files were found during the search
        if (!OutputWindowViewModel.FoundInFiles.Any())
        {
            // Update the message to indicate that no matches were found
            MessageDispenser = "Search Completed in " + StopwatchString + " => NO MATCHES WERE FOUND";
        }
        else
        {
            // Update the message to indicate that the search was completed
            MessageDispenser = "Search Completed in " + StopwatchString;
        }

        // Calculate the average number of completed search tasks per second
        double averageSearchTasksPerSecond = go.GetTotalSearchTasks() / stopwatch.Elapsed.TotalSeconds;

        // Update the SystemInfo property with the average search tasks per second
        SystemInfo = "Average Completed SearchTasks/Second: " + Math.Round(averageSearchTasksPerSecond, 2);
    }

    // Method for getting directory statistics
    public void GetDirectoryStatistics()
    {
        MessageDispenser = "You Picked a total of " + Math.Round(RootFolderBrowseViewModel.TotalSizeMB, 2).ToString() + " MB " + " from a total of " + RootFolderBrowseViewModel.folderCount + " folders and " + RootFolderBrowseViewModel.fileCount + " files";
    }

    // Method for getting system information
    public void GetSystemInfo()
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

        SystemInfo = processors + " | Number of Cores: " + coreCount + " | Number Of Logical Processors: " + Environment.ProcessorCount + " | Maximum Search Tasks Selected " + this.SemaphoreUpdate;
    } 
}