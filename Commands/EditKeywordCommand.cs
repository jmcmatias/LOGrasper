using LOGrasper.ViewModels;

namespace LOGrasper.Commands
{
    internal class EditKeywordCommand : CommandBase
    {
        private readonly KeywordListViewModel _keywordListViewModel; // Reference to the KeywordListViewModel
        private readonly SearchViewViewModel _searchViewViewModel; // Reference to the SearchViewViewModel

        private bool _hasSelection; // Flag to track if there is a selected keyword

        public bool HasSelection
        {
            get { return _hasSelection; }
            set { _hasSelection = value; }
        }

        public EditKeywordCommand(KeywordListViewModel keywordListViewModel, SearchViewViewModel searchViewViewModel)
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

        public override void Execute(object? parameter)
        {
            if (_keywordListViewModel.SelectedKeyword != null)
            {
                _keywordListViewModel.NewKeyword = _keywordListViewModel.SelectedKeyword.Keyword.ToString(); // Set the NewKeyword property of the KeywordListViewModel to the selected keyword
                _keywordListViewModel.IsEditing = true; // Set the IsEditing property of the KeywordListViewModel to true to indicate that the keyword is being edited
                _keywordListViewModel.SelectKeywordUnlock = !_keywordListViewModel.IsEditing; // Update the SelectKeywordUnlock property of the KeywordListViewModel based on the IsEditing flag
                _keywordListViewModel.EditingButton(); // Call the EditingButton method of the KeywordListViewModel to update the state of editing buttons
                _searchViewViewModel.MessageDispenser = "Entered Keyword Edit Mode, Click Editing Button to Finish Editing";  // Update the MessageDispenser property of the SearchViewViewModel with a message indicating the edit mode
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return HasSelection; // The command can be executed if there is a selected keyword
        }
    }
}
