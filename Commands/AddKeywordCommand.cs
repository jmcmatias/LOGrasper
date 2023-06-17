
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
                int index = _keywordListViewModel._keywordList.IndexOf(_keywordListViewModel.SelectedKeyword);
                _keywordListViewModel._keywordList.Remove(_keywordListViewModel.SelectedKeyword);
                _keywordListViewModel._keywordList.Insert(index, new KeywordViewModel(_keywordListViewModel.NewKeyword));
                _keywordListViewModel.IsEditing = false;
                _keywordListViewModel.AddingButton();
            }
            else
            {
                _keywordListViewModel._keywordList.Add(new KeywordViewModel(_keywordListViewModel.NewKeyword));
            }

                if (_keywordListViewModel.KeywordList.Any())
                {
                    _searchViewViewmodel.HasKeywordList = true;
                }
            _keywordListViewModel.NewKeyword = "";
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_keywordListViewModel.NewKeyword) && base.CanExecute(parameter);
        }

        private void keywordListViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(_keywordListViewModel.NewKeyword)) 
            {
                OnCanExecuteChanged();
            }
        }

    }
}
