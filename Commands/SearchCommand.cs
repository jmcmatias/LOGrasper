using LOGrasper.ViewModels;
using System.ComponentModel;

namespace LOGrasper.Commands
{
    internal class SearchCommand : CommandBase
    {
        private readonly SearchViewViewModel _searchViewViewModel; // Reference to the SearchViewViewModel
        private readonly RootFolderBrowseViewModel _rootFolderBrowseViewModel; // Reference to the RootFolderBrowseViewModel
        private readonly KeywordListViewModel _keywordListViewModel; // Reference to the KeywordListViewModel

        public SearchCommand(SearchViewViewModel searchViewViewModel)
        {
            _searchViewViewModel = searchViewViewModel; // Assign the SearchViewViewModel reference
            _rootFolderBrowseViewModel = _searchViewViewModel.RootFolderBrowseViewModel; // Assign the RootFolderBrowseViewModel reference from the SearchViewViewModel
            _keywordListViewModel = _searchViewViewModel.KeywordListViewModel; // Assign the KeywordListViewModel reference from the SearchViewViewModel

            _searchViewViewModel.PropertyChanged += _searchViewViewmodel_PropertyChanged; // Subscribe to the PropertyChanged event of the SearchViewViewModel
        }

        // Event handler for property changed events in the SearchViewViewModel
        private void _searchViewViewmodel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // Checks if the properties HasKeywordList or HasRootFolder have changed
            if (e.PropertyName == nameof(_searchViewViewModel.HasKeywordList) || e.PropertyName == nameof(_searchViewViewModel.HasRootFolder))
            {
                OnCanExecuteChanged(); // Invoke the OnCanExecuteChanged method to update the command's executable state
            }
        }

        // Determines whether the command can be executed
        public override bool CanExecute(object? parameter)
        {
            return _searchViewViewModel.HasKeywordList && _searchViewViewModel.HasRootFolder; // Command can execute if both HasKeywordList and HasRootFolder are true
        }

        // Executes the command
        public override void Execute(object? parameter)
        {
            if (_searchViewViewModel.SearchButton == "SEARCH")
            {
                _searchViewViewModel.OutputWindowViewModel.ClearOutput(); // Clear the output window
                _searchViewViewModel.CancellationFlag = false; // Reset the cancellation flag
                _ = _searchViewViewModel.InitiateAsyncSearch(_rootFolderBrowseViewModel, _keywordListViewModel); // Start the asynchronous search process
                _searchViewViewModel.SearchButton = "STOP SEARCH"; // Change the text of the search button to "STOP SEARCH"
                _searchViewViewModel.SearchButtonColor = "#fc7474"; // Change the color of the search button to indicate it's active
            }
            else if (_searchViewViewModel.SearchButton == "STOP SEARCH")
            {
                _searchViewViewModel.CancellationFlag = true; // Set the cancellation flag to stop the search process
            }
        }
    }
}
