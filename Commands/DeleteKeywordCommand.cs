using LOGrasper.ViewModels;
using System.Linq;

namespace LOGrasper.Commands
{
    internal class DeleteKeywordCommand : CommandBase
    {
        private readonly KeywordListViewModel _keywordListViewModel; // Reference to the KeywordListViewModel
        private readonly SearchViewViewModel _searchViewViewModel; // Reference to the SearchViewViewModel
        private bool _hasSelection; // Flag to track if there is a selected keyword

        public bool HasSelection
        {
            get { return _hasSelection; }
            set { _hasSelection = value; }
        }

        public DeleteKeywordCommand(KeywordListViewModel keywordListViewModel, SearchViewViewModel searchViewViewModel)
        {
            _keywordListViewModel = keywordListViewModel; // Assign the KeywordListViewModel reference
            _searchViewViewModel = searchViewViewModel; // Assign the SearchViewViewModel reference

            _keywordListViewModel.PropertyChanged += keywordListViewModel_PropertyChanged; // Subscribe to the PropertyChanged event of the KeywordListViewModel

        }

        // Event handler for the SelectedKeyword property changed event
        private void keywordListViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_keywordListViewModel.SelectedKeyword))
            {
                if (_keywordListViewModel.SelectedKeyword != null)
                {
                    HasSelection = true; // Set the HasSelection flag to true if a keyword is selected
                    OnCanExecuteChanged();
                }
                else
                {
                    HasSelection = false; // Set the HasSelection flag to false if no keyword is selected
                    OnCanExecuteChanged();
                }
            }
        }

        // Determines if the command can be executed
        public override bool CanExecute(object? parameter)
        {
            return HasSelection; // The command can be executed if there is a selected keyword
        }

        // Executes the command
        public override void Execute(object? parameter)
        {
            _keywordListViewModel._keywordList.Remove((_keywordListViewModel.SelectedKeyword));          // Remove the selected keyword from the keyword list

            // Check if the keyword list is empty and update the HasKeywordList property of the SearchViewViewModel
            if (!_keywordListViewModel._keywordList.Any())
            {
                _searchViewViewModel.HasKeywordList = false;
            }

            // Update the MessageDispenser property of the SearchViewViewModel with a message indicating the keyword deletion
            _searchViewViewModel.MessageDispenser = "";
            _searchViewViewModel.MessageDispenser = "Keyword Deleted";
            
        }
    }
}
