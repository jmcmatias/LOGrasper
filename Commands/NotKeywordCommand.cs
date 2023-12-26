using LOGrasper.ViewModels;
using System.Windows.Media;

namespace LOGrasper.Commands
{
    internal class NotKeywordCommand : CommandBase
    {
        private readonly KeywordListViewModel _keywordListViewModel; // Reference to the KeywordListViewModel
        private readonly SearchViewViewModel _searchViewViewModel; // Reference to the SearchViewViewModel

        private bool _hasSelection; // Flag to track if there is a selected keyword

        public bool HasSelection
        {
            get { return _hasSelection; }
            set { _hasSelection = value; }
        }

        public NotKeywordCommand(KeywordListViewModel keywordListViewModel, SearchViewViewModel searchViewViewModel)
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
                if (!_keywordListViewModel.SelectedKeyword.IsNot)
                {
                    _keywordListViewModel.SelectedKeyword.IsNot=true;
                    _keywordListViewModel.SelectedKeyword.KeywordColor = _keywordListViewModel._HasNotClauseColor;
                    _searchViewViewModel.MessageDispenser = "Not Clause select - Keyword will invalidate lines";  // Update the MessageDispenser property of the SearchViewViewModel with a message indicating the not Clause
                }
                else
                {
                    _keywordListViewModel.SelectedKeyword.IsNot = false;
                    _keywordListViewModel.SelectedKeyword.KeywordColor = _keywordListViewModel._StandardKeywordColor; ;
                    _searchViewViewModel.MessageDispenser = "Not Clause unselected - Keyword will validate lines";  // Update the MessageDispenser property of the SearchViewViewModel with a message indicating the not Clause

                }
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return HasSelection; // The command can be executed if there is a selected keyword
        }
    }
}
