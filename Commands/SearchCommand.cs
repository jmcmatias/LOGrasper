using LOGrasper.ViewModels;
using System.ComponentModel;

namespace LOGrasper.Commands
{
    internal class SearchCommand : CommandBase
    {



        private readonly SearchViewViewModel _searchViewViewModel;
        private readonly RootFolderBrowseViewModel _rootFolderBrowseViewModel;
        private readonly KeywordListViewModel _keywordListViewModel;

        public SearchCommand(SearchViewViewModel searchViewViewModel) 
        {
            _searchViewViewModel = searchViewViewModel;
            _rootFolderBrowseViewModel = _searchViewViewModel.RootFolderBrowseViewModel;
            _keywordListViewModel = _searchViewViewModel.KeywordListViewModel;

            _searchViewViewModel.PropertyChanged += _searchViewViewmodel_PropertyChanged;

        }

        private void _searchViewViewmodel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_searchViewViewModel.HasKeywordList) || e.PropertyName == nameof(_searchViewViewModel.HasRootFolder))
            {
                OnCanExecuteChanged();
            }          
        }

        public override bool CanExecute(object? parameter)
        {
            return _searchViewViewModel.HasKeywordList && _searchViewViewModel.HasRootFolder;
        }

        public override void Execute(object? parameter)
        {
            _searchViewViewModel.OutputWindowViewModel.ClearOutput();
            
            if (_searchViewViewModel.SearchButton == "SEARCH")
            {
                _searchViewViewModel.CancellationFlag = false;
                _ = _searchViewViewModel.InitiateAsyncSearch(_rootFolderBrowseViewModel, _keywordListViewModel);
                _searchViewViewModel.SearchButton = "STOP SEARCH";
                _searchViewViewModel.SearchButtonColor = "#fc7474";
            } else if (_searchViewViewModel.SearchButton == "STOP SEARCH")
            {
                _searchViewViewModel.CancellationFlag = true;
            }
        }
    }
}
