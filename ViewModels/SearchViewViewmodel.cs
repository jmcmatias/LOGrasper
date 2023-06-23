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

    private static int _numberOfTasks = Environment.ProcessorCount;
    private static SemaphoreSlim _semaphore = new SemaphoreSlim(Environment.ProcessorCount);
    private static int _semaphoreUpdate = _numberOfTasks;
    public SemaphoreSlim Semaphore { get { return _semaphore; } }

    public ICommand SearchCommand { get; }

    public ICommand SetNumberOfTasksCommand { get; }

    public int NumberOfTasks
    {
        get { return _numberOfTasks; }
        set
        {
            _numberOfTasks = value;
            OnPropertyChanged(nameof(NumberOfTasks));
        }
    }
    
    public int SemaphoreUpdate
    {
        get { return _semaphoreUpdate; }
        set
        {   
            _semaphoreUpdate = value;
            _semaphore = new SemaphoreSlim(_semaphoreUpdate);
            OnPropertyChanged(nameof(SemaphoreUpdate));        
        }
    }

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
        get => _stopwatch;
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

        if (!OutputWindowViewModel.FoundInFiles.Any())
        {

            MessageDispenser = "Search Completed in " + StopwatchString + " => NO MATCHES WHERE FOUND";
        }
        else
        {
            MessageDispenser = "Search Completed in " + StopwatchString;
        }
        SystemInfo = "Average Completed SearchTasks/Second: " + Math.Round(go.GetTotalSearchTasks() / stopwatch.Elapsed.TotalSeconds, 2);
    }

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

    public void GetDirectoryStatistics()
    {
        MessageDispenser = "You Picked a total of " + Math.Round(RootFolderBrowseViewModel.TotalSizeMB, 2).ToString() + " MB " + " from a total of " + RootFolderBrowseViewModel.folderCount + " folders and " + RootFolderBrowseViewModel.fileCount + " files";
    }

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

    public class CustomIntegerUpDown : IntegerUpDown
    {
        protected override int DecrementValue(int value, int increment)
        {
            if (value - increment < 1)
            {
                return value;
            }
            return value - increment;
        }
        private void IntegerUpDown_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _) || int.Parse(e.Text) < 1)
            {
                e.Handled = true;
            }
        }
    }
}