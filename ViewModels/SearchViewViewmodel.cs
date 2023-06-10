using LOGrasper.Commands;
using LOGrasper.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Diagnostics;
using System.Windows.Input;

namespace LOGrasper.ViewModels;

public class SearchViewViewModel : ViewModelBase
{
    public RootFolderBrowseViewModel RootFolderBrowseViewModel { get; set; }
    public KeywordListViewModel KeywordListViewModel { get; set; }
    public OutputWindowViewModel OutputWindowViewModel { get; set; }

    

    public SearchObject SearchObject { get; set; }


    private string _messageDispenser;

    private string _stopwatch;

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

    public string Stopwatch
    {
        get=>_stopwatch;
        set
        {
            _stopwatch = value;
            OnPropertyChanged(nameof(Stopwatch));
        }
    }
        

    public void InitiateSearch(RootFolderBrowseViewModel rootFolderBrowseViewModel, KeywordListViewModel keywordListViewModel)
    {
        get => _messageDispenser;
        set
        {
            _messageDispenser = value;
            OnPropertyChanged(nameof(MessageDispenser));
        }
    }
    public async Task InitiateSearch(RootFolderBrowseViewModel rootFolderBrowseViewModel, KeywordListViewModel keywordListViewModel)
    {
        MessageDispenser = "Search Started";
        SearchObject = new SearchObject(rootFolderBrowseViewModel.RootFolderPath, keywordListViewModel._keywordList);

        SearchEngine go = new(SearchObject, OutputWindowViewModel, this);
        Stopwatch stopwatch = new();
        stopwatch.Start();
        Task search = Task.Run(() => go.SearchAC());
        MessageDispenser = stopwatch.Elapsed.ToString();
        stopwatch.Stop();
        Stopwatch = stopwatch.Elapsed.ToString();


        await search;
        MessageDispenser = "Search Completed";
    }

    public SearchViewViewModel()
    {
        MessageDispenser = "TESTE";
        RootFolderBrowseViewModel = new RootFolderBrowseViewModel(this);
        KeywordListViewModel = new KeywordListViewModel(this);

        OutputWindowViewModel = new OutputWindowViewModel(this, RootFolderBrowseViewModel);
        MessageDispenser = "TESTE";

        SearchCommand = new SearchCommand(this);
        // CanSearch(RootFolderBrowseViewModel, KeywordListViewModel);
    }
}