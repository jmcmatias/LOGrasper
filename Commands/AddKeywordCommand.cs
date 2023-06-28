using LOGrasper.ViewModels;
using System.Linq;

namespace LOGrasper.Commands
{
    internal class AddKeywordCommand : CommandBase
    {
        private readonly KeywordListViewModel _keywordListViewModel; // Reference to the KeywordListViewModel
        private readonly SearchViewViewModel _searchViewViewmodel; // Reference to the SearchViewViewModel

        public AddKeywordCommand(KeywordListViewModel keywordListViewModel, SearchViewViewModel searchViewViewmodel)
        {
            _keywordListViewModel = keywordListViewModel; // Assign the KeywordListViewModel reference
            _searchViewViewmodel = searchViewViewmodel; // Assign the SearchViewViewModel reference
            _keywordListViewModel.PropertyChanged += keywordListViewModel_PropertyChanged;
            // Subscribe to the PropertyChanged event of the KeywordListViewModel
        }

        public override void Execute(object? parameter)
        {
            // If the user is Editing...
            if (_keywordListViewModel.IsEditing)
            {
                // Check if the new keyword doesn't exist or if it's the same as the selected keyword
                if (!_keywordListViewModel.KeywordExists(_keywordListViewModel.NewKeyword) ||
                    (_keywordListViewModel.NewKeyword == _keywordListViewModel.SelectedKeyword.Keyword.ToString()))
                {
                    // Save the index of the selected keyword so that it may be inserted in the same order
                    int index = _keywordListViewModel._keywordList.IndexOf(_keywordListViewModel.SelectedKeyword);
                    _keywordListViewModel._keywordList.Remove(_keywordListViewModel.SelectedKeyword);           // Remove the selected keyword
                    _keywordListViewModel._keywordList.Insert(index, new KeywordViewModel(_keywordListViewModel.NewKeyword));  // Insert the new keyword in the saved index
                    _keywordListViewModel.IsEditing = false;            // Stop editing bool
                    _keywordListViewModel.SelectKeywordUnlock = !_keywordListViewModel.IsEditing; // Unlock the List
                    _keywordListViewModel.AddingButton();       // transform the button back into Add Button
                    _keywordListViewModel.NewKeyword = "";      // clear the new keyword form
                    _searchViewViewmodel.MessageDispenser = ""; // clear and send message to message Dispenser.
                    _searchViewViewmodel.MessageDispenser = "Keyword Successfully Edited";
                }
                else
                {
                    // if not send message that the keyword already exists, and ask to insert a diferent one
                    _searchViewViewmodel.MessageDispenser = "";
                    _searchViewViewmodel.MessageDispenser = "Keyword already exists in the list, please change your edited keyword";
                }
            }
            else
            {
                // If not editing add new keyword if the NewKeyword doesnt exist.
                if (!_keywordListViewModel.KeywordExists(_keywordListViewModel.NewKeyword))
                {
                    _keywordListViewModel._keywordList.Add(new KeywordViewModel(_keywordListViewModel.NewKeyword));
                    _keywordListViewModel.NewKeyword = "";
                    _searchViewViewmodel.MessageDispenser = "";
                    _searchViewViewmodel.MessageDispenser = "Keyword added";
                }
                // If the new keyword exists then send message informing that.
                else
                {
                    _searchViewViewmodel.MessageDispenser = "";
                    _searchViewViewmodel.MessageDispenser = "Keyword already exists in the list, please choose another keyword";
                }
            }
            // If the list has keywords
            if (_keywordListViewModel.KeywordList.Any())
            {
                _searchViewViewmodel.HasKeywordList = true; // Set to true 
            }
        }

        public override bool CanExecute(object? parameter)
        {
            // The command can be executed if the new keyword is not empty, the base CanExecute returns true,
            // and the new keyword is not "Add Keywords" (to keep user from adding the placeholder text)
            return !string.IsNullOrEmpty(_keywordListViewModel.NewKeyword) && base.CanExecute(parameter) && _keywordListViewModel.NewKeyword != "Add Keywords";
        }

        private void keywordListViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // if the property changed was NewKeyword the Button can execute.
            if (e.PropertyName == nameof(_keywordListViewModel.NewKeyword))
            {
                OnCanExecuteChanged(); // Invoke the base class OnCanExecuteChanged method when the NewKeyword property changes
            }
        }
    }
}
