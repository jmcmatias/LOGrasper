
using LOGrasper.ViewModels;
using System.Linq;


namespace LOGrasper.Commands
{
    internal class AddKeywordCommand : CommandBase
    {
        // private readonly KeywordListViewModel _keywordListViewModel; // Viewmodel instance

        private readonly KeywordListViewModel _keywordListViewModel;
        private readonly SearchViewViewModel _searchViewViewmodel;


        public AddKeywordCommand(KeywordListViewModel KeywordListViewModel, SearchViewViewModel searchViewViewmodel)
        {
            _keywordListViewModel = KeywordListViewModel;
            _searchViewViewmodel = searchViewViewmodel;
            _keywordListViewModel.PropertyChanged += keywordListViewModel_PropertyChanged;

        }



        public override void Execute(object? parameter)
        {
            if (_keywordListViewModel.IsEditing)
            {
                // If the new keyword doesnt exit or if the new keyword is the same as the selected one
                if (!_keywordListViewModel.KeywordExists(_keywordListViewModel.NewKeyword) || (_keywordListViewModel.NewKeyword == _keywordListViewModel.SelectedKeyword.Keyword.ToString()))
                {
                    int index = _keywordListViewModel._keywordList.IndexOf(_keywordListViewModel.SelectedKeyword);
                    _keywordListViewModel._keywordList.Remove(_keywordListViewModel.SelectedKeyword);
                    _keywordListViewModel._keywordList.Insert(index, new KeywordViewModel(_keywordListViewModel.NewKeyword));
                    _keywordListViewModel.IsEditing = false;
                    _keywordListViewModel.SelectKeywordUnlock = !_keywordListViewModel.IsEditing;
                    _keywordListViewModel.AddingButton();
                    _keywordListViewModel.NewKeyword = "";
                    _searchViewViewmodel.MessageDispenser = "";
                    _searchViewViewmodel.MessageDispenser = "Keyword Successfully Edited";
                }
                else
                {
                    _searchViewViewmodel.MessageDispenser = "";
                    _searchViewViewmodel.MessageDispenser = "Keyword already exists in the list, please change your edited keyword";
                }
            }
            else
            {
                if (!_keywordListViewModel.KeywordExists(_keywordListViewModel.NewKeyword))
                {
                    _keywordListViewModel._keywordList.Add(new KeywordViewModel(_keywordListViewModel.NewKeyword));
                    _keywordListViewModel.NewKeyword = "";
                    _searchViewViewmodel.MessageDispenser = "";
                    _searchViewViewmodel.MessageDispenser = "Keyword added";
                }
                else
                {
                    _searchViewViewmodel.MessageDispenser = "";
                    _searchViewViewmodel.MessageDispenser = "Keyword already exists in the list, please choose another keyword";
                }
            }
            if (_keywordListViewModel.KeywordList.Any())
            {
                _searchViewViewmodel.HasKeywordList = true;
            }
        }



        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_keywordListViewModel.NewKeyword) && base.CanExecute(parameter);
        }

        private void keywordListViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_keywordListViewModel.NewKeyword))
            {
                OnCanExecuteChanged();
            }
        }

    }
}
